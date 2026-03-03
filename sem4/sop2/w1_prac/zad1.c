#include <errno.h>
#include <signal.h>
#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>

#define ERR(source) \
    (fprintf(stderr, "%s:%d\n", __FILE__, __LINE__), perror(source), kill(0, SIGKILL), exit(EXIT_FAILURE))

void usage(char *func)
{
    fprintf(stderr,
            "USAGE: %s PROC_AMOUNT\nCreate a loop of PROC_AMOUNT processes, where each passes forward a random "
            "number\n\n\tPROC_AMOUNT \tNumber of processes in the loop",
            func);
    exit(EXIT_FAILURE);
}

void close_unused_pipes(int n, int proc_index, int pipes[n][2])
{
    for (int i = 0; i < n; i++)
    {
        if (i == (proc_index + 1) % n)
        {
            if (close(pipes[i][0]))
            {
                if (errno != EBADF)
                    ERR("close");
            }
            printf("[%d] Closing pipe fd (%d)...\n", getpid(), pipes[i][0]);
        }
        else if (i == (proc_index - 1) % n)
        {
            printf("[%d] Closing pipe fd (%d)...\n", getpid(), pipes[i][1]);
            if (close(pipes[i][1]))
            {
                if (errno != EBADF)
                    ERR("close");
            }
        }
        else
        {
            printf("[%d] Closing pipe fd (%d)...\n", getpid(), pipes[i][0]);
            if (close(pipes[i][0]))
            {
                if (errno != EBADF)
                    ERR("close");
            }
            printf("[%d] Closing pipe fd (%d)...\n", getpid(), pipes[i][1]);
            if (close(pipes[i][1]))
            {
                if (errno != EBADF)
                    ERR("close");
            }
        }
    }
}

void close_used_pipes(int n, int proc_index, int pipes[n][2])
{
    printf("[%d] Closing pipe fd (%d)...\n", getpid(), pipes[(proc_index + 1) % n][0]);
    if (close(pipes[(proc_index + 1) % n][1]))
    {
        if (errno != EBADF)
            ERR("close");
    }
    printf("[%d] Closing pipe fd (%d)...\n", getpid(), pipes[(proc_index + 1) % n][0]);
    if (close(pipes[(proc_index - 1) % n][0]))
    {
        if (errno != EBADF)
            ERR("close");
    }
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
                close_used_pipes(n, i, pipes);
                exit(EXIT_SUCCESS);
            case -1:
                ERR("fork");
        }
    }
    close_unused_pipes(n, 0, pipes);
    close_used_pipes(n, 0, pipes);
}

int main(int argc, char **argv)
{
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
