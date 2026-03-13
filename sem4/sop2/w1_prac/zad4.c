#include <fcntl.h>
#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>
#include <sys/stat.h>
#include <sys/types.h>
#include <sys/wait.h>
#include <time.h>
#include <unistd.h>

#define PLAYING 0
#define LEAVING 1
#define CONTINUE 0
#define BROKE 1

// Wiadomość: Gracz -> Krupier (etap obstawiania)
typedef struct
{
    pid_t pid;
    int type;
    int bet_amount;
    int bet_number;
} BetMsg;

// Wiadomość: Krupier -> Gracz (etap losowania)
typedef struct
{
    int lucky_number;
} ResultMsg;

// Wiadomość: Gracz -> Krupier (potwierdzenie po rundzie)
typedef struct
{
    pid_t pid;
    int type;
} AckMsg;

// Struktura pomocnicza dla Krupiera do śledzenia graczy
typedef struct
{
    pid_t pid;
    int d2p_fd;  // Deskryptor FIFO: Krupier -> Gracz
    bool active;
} PlayerInfo;

int main(int argc, char* argv[])
{
    // 1. Walidacja argumentów
    if (argc != 3)
    {
        fprintf(stderr, "Uzycie: %s <liczba_graczy_N> <poczatkowa_kwota_M>\n", argv[0]);
        return 1;
    }
    int N = atoi(argv[1]);
    int M = atoi(argv[2]);
    if (N < 1 || M < 100)
    {
        fprintf(stderr, "Blad: N musi byc >= 1, a M musi byc >= 100.\n");
        return 1;
    }

    // 2. Tworzenie kolejek FIFO
    // Wspólne FIFO: Gracze -> Krupier
    char p2d_name[64];
    snprintf(p2d_name, sizeof(p2d_name), "/tmp/roulette_p2d_%d", getpid());
    mkfifo(p2d_name, 0666);

    // Indywidualne FIFO: Krupier -> Gracz (zapobiega to podbieraniu wiadomości przez innych graczy)
    char d2p_names[N][64];
    for (int i = 0; i < N; i++)
    {
        snprintf(d2p_names[i], sizeof(d2p_names[i]), "/tmp/roulette_d2p_%d_%d", getpid(), i);
        mkfifo(d2p_names[i], 0666);
    }

    PlayerInfo players[N];

    // 3. Tworzenie procesów graczy
    for (int i = 0; i < N; i++)
    {
        pid_t pid = fork();
        if (pid < 0)
        {
            perror("Fork failed");
            exit(1);
        }

        if (pid == 0)
        {
            // ---- KOD PROCESU GRACZA ----
            pid_t my_pid = getpid();
            int current_M = M;

            // Otwieranie FIFO: Ważna jest kolejność, aby uniknąć zakleszczenia
            int p2d_fd = open(p2d_name, O_WRONLY);
            int d2p_fd = open(d2p_names[i], O_RDONLY);

            // Unikalny seed dla losowości
            srand(time(NULL) ^ my_pid ^ (i << 16));

            printf("[%d]: I have %d and I'm going to play roulette.\n", my_pid, current_M);

            while (1)
            {
                // Szansa 10% na opuszczenie gry
                if ((rand() % 100) < 10)
                {
                    printf("[%d]: I saved %d\n", my_pid, current_M);
                    BetMsg b = {my_pid, LEAVING, 0, 0};
                    write(p2d_fd, &b, sizeof(b));
                    break;
                }

                // Generowanie zakładu
                int bet_amount = (rand() % current_M) + 1;
                int bet_number = rand() % 37;  // Zakres [0, 36]
                current_M -= bet_amount;

                // Wysyłanie zakładu do Krupiera
                BetMsg b = {my_pid, PLAYING, bet_amount, bet_number};
                write(p2d_fd, &b, sizeof(b));

                // Odbiór wylosowanego numeru
                ResultMsg r;
                read(d2p_fd, &r, sizeof(r));

                // Rozliczenie
                if (r.lucky_number == bet_number)
                {
                    int won = bet_amount * 36;  // Otrzymuje stawkę + wygraną w stosunku 35:1
                    current_M += won;
                    printf("[%d]: I won %d\n", my_pid, won);
                }

                // Sprawdzanie stanu konta i wysyłanie potwierdzenia do krupiera
                if (current_M <= 0)
                {
                    printf("[%d]: I'm broke\n", my_pid);
                    AckMsg a = {my_pid, BROKE};
                    write(p2d_fd, &a, sizeof(a));
                    break;
                }
                else
                {
                    AckMsg a = {my_pid, CONTINUE};
                    write(p2d_fd, &a, sizeof(a));
                }
            }

            close(p2d_fd);
            close(d2p_fd);
            exit(0);
        }
        else
        {
            // Zapisywanie informacji o dziecku w strukturach Krupiera
            players[i].pid = pid;
            players[i].active = true;
        }
    }

    // ---- KOD PROCESU KRUPIERA ----
    srand(time(NULL) ^ getpid());

    // Krupier otwiera połączenia (odwrotna kolejność O_RDONLY / O_WRONLY pasująca do graczy)
    int p2d_fd = open(p2d_name, O_RDONLY);
    for (int i = 0; i < N; i++)
    {
        players[i].d2p_fd = open(d2p_names[i], O_WRONLY);
    }

    int active_count = N;

    // Główna pętla gry Krupiera
    while (active_count > 0)
    {
        int playing_count = 0;

        // Faza 1: Odbieranie zakładów
        for (int i = 0; i < active_count; i++)
        {
            BetMsg msg;
            read(p2d_fd, &msg, sizeof(msg));

            // Znajdź gracza na liście
            int p_idx = -1;
            for (int j = 0; j < N; j++)
            {
                if (players[j].pid == msg.pid)
                {
                    p_idx = j;
                    break;
                }
            }

            if (msg.type == LEAVING)
            {
                players[p_idx].active = false;  // Gracz odszedł dobrowolnie
            }
            else
            {
                printf("Dealer: %d placed %d on %d\n", msg.pid, msg.bet_amount, msg.bet_number);
                playing_count++;
            }
        }

        // Jeśli wszyscy zdecydowali się odejść w tej rundzie (10% szans) i nikt nie gra
        if (playing_count == 0)
            break;

        // Faza 2: Losowanie i ogłoszenie wyniku
        int lucky = rand() % 37;
        printf("Dealer: %d is the lucky number.\n", lucky);

        // Faza 3: Wysyłanie wylosowanego numeru tylko do AKTYWNYCH, grających w tej rundzie graczy
        for (int i = 0; i < N; i++)
        {
            if (players[i].active)
            {
                ResultMsg r = {lucky};
                write(players[i].d2p_fd, &r, sizeof(r));
            }
        }

        // Faza 4: Zbieranie statusów po rundzie (żeby krupier wiedział, na kogo czekać w kolejnej)
        active_count = 0;
        for (int i = 0; i < playing_count; i++)
        {
            AckMsg a;
            read(p2d_fd, &a, sizeof(a));

            int p_idx = -1;
            for (int j = 0; j < N; j++)
            {
                if (players[j].pid == a.pid)
                {
                    p_idx = j;
                    break;
                }
            }

            if (a.type == BROKE)
            {
                players[p_idx].active = false;  // Gracz stracił pieniądze
            }
            else
            {
                active_count++;  // Gracz ma środki i gra dalej
            }
        }
    }

    printf("Dealer: Casino always wins\n");

    // 4. Sprzątanie
    // Czekamy, aż wszystkie procesy potomne zakończą działanie, by zapobiec tworzeniu "zombie"
    while (wait(NULL) > 0)
        ;

    // Zamykanie i usuwanie pików FIFO z systemu
    close(p2d_fd);
    unlink(p2d_name);
    for (int i = 0; i < N; i++)
    {
        close(players[i].d2p_fd);
        unlink(d2p_names[i]);
    }

    return 0;
}
