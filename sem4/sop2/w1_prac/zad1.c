#include <asm-generic/errno-base.h>
#include <bits/types/sigset_t.h>
#define _GNU_SOURCE
#include <errno.h>
#include <signal.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <sys/wait.h>
#include <time.h>
#include <unistd.h>

#define ERR(source) \
    (fprintf(stderr, "%s:%d\n", __FILE__, __LINE__), perror(source), kill(0, SIGKILL), exit(EXIT_FAILURE))

volatile sig_atomic_t running = 1;
void usage(char* func)
{
    fprintf(stderr,
            "USAGE: %s PROC_AMOUNT\nCreate a loop of PROC_AMOUNT processes, where each passes forward a random "
            "number\n\n\tPROC_AMOUNT \tNumber of processes in the loop",
            func);
    exit(EXIT_FAILURE);
}

int sethandler(void (*f)(int), int sigNo)
{
    struct sigaction act;
    memset(&act, 0, sizeof(struct sigaction));
    act.sa_handler = f;
    if (-1 == sigaction(sigNo, &act, NULL))
        return -1;
    return 0;
}

void sig_handler(int sig)
{
    if (sig == SIGINT)
        running = 0;
}

void close_pipe(int fd)
{
    printf("[%d] Closing pipe fd (%d)...\n", getpid(), fd);
    if (close(fd))
    {
        if (errno != EBADF)
            ERR("close");
    }
}

int send_message(int fd, int num)
{
    if (TEMP_FAILURE_RETRY(write(fd, &num, sizeof(int))) < 0)
    {
        if (errno == EPIPE || errno == EINTR)
        {
            return 0;
        }
        ERR("write");
    }
    printf("[%d] Sending number %d to pipe...\n", getpid(), num);
    return 1;
}

int receive_message(int fd, int* num)
{
    int bytes;
    if (TEMP_FAILURE_RETRY(bytes = read(fd, num, sizeof(int))) < 1)
    {
        if (bytes == 0)
        {
            return 0;
        }
        ERR("read");
    }
    printf("[%d] Got number %d from pipe\n", getpid(), *num);
    return 1;
}

void pipe_processor(int ifd, int ofd)
{
    srand(getpid());
    int message;
    while (running)
    {
        if (!receive_message(ifd, &message))
        {
            break;
        }
        usleep(30000);
        if (!message || !running)
        {
            break;
        }
        message = message + (rand() % 21 - 10);
        if (!send_message(ofd, message))
        {
            break;
        }
    }
}

void close_unused_pipes(int n, int proc_index, int pipes[n][2])
{
    for (int i = 0; i < n; i++)
    {
        if (i == proc_index)
        {
            close_pipe(pipes[i][1]);
        }
        else if (i == (proc_index + 1) % n)
        {
            close_pipe(pipes[i][0]);
        }
        else
        {
            close_pipe(pipes[i][0]);
            close_pipe(pipes[i][1]);
        }
    }
}

void close_used_pipes(int n, int proc_index, int pipes[n][2])
{
    close_pipe(pipes[proc_index][0]);
    close_pipe(pipes[(proc_index + 1) % n][1]);
}

void create_pipe_loop(int n, int pipes[n][2])
{
    for (int i = 0; i < n; i++)
    {
        if (pipe(pipes[i]) == -1)
            ERR("pipe");
    }

    for (int i = 1; i < n; i++)
    {
        switch (fork())
        {
            case 0:
                // child process
                close_unused_pipes(n, i, pipes);

                pipe_processor(pipes[i][0], pipes[(i + 1) % n][1]);
                close_used_pipes(n, i, pipes);
                exit(EXIT_SUCCESS);
            case -1:
                ERR("fork");
        }
    }
    close_unused_pipes(n, 0, pipes);
    int message = 1;
    if (TEMP_FAILURE_RETRY(write(pipes[1][1], &message, sizeof(int))) < 0)
    {
        ERR("write");
    }
    pipe_processor(pipes[0][0], pipes[1][1]);
    close_used_pipes(n, 0, pipes);

    int s;
    while ((s = wait(NULL)) > 0)
    {
        printf("[%d] Child with PID %d died :(\n", getpid(), s);
    }
}

int main(int argc, char** argv)
{
    if (sethandler(sig_handler, SIGINT))
    {
        ERR("Couldn't set sighandler");
    }

    sigset_t sigmask;
    sigemptyset(&sigmask);
    sigaddset(&sigmask, SIGPIPE);
    if (sigprocmask(SIG_BLOCK, &sigmask, NULL))
    {
        ERR("sigprocmask");
    }

    int n = 3;
    if (argc > 1)
    {
        if ((n = atoi(argv[1])) == 0)
        {
            usage(argv[0]);
        }
    }

    int pipes[n][2];

    create_pipe_loop(n, pipes);

    return EXIT_SUCCESS;
}
