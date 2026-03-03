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

typedef struct
{
    int *fd;
} pipe_t;

void close_unused_pipes(int n, int proc_index, pipe_t **pipes)
{
    for (int i = 0; i < n; i++)
    {
        if (i == (proc_index + 1) % n)
        {
            printf("[%d] Closing pipe fd (%d)...", getpid(), pipes[i]->fd[0]);
            if (close(pipes[i]->fd[0]))
                ERR("close");
        }
        else if (i == (proc_index - 1) % n)
        {
            printf("[%d] Closing pipe fd (%d)...", getpid(), pipes[i]->fd[1]);
            if (close(pipes[i]->fd[1]))
                ERR("close");
        }
        else
        {
            printf("[%d] Closing pipe fd (%d)...", getpid(), pipes[i]->fd[0]);
            if (close(pipes[i]->fd[0]))
                ERR("close");
            printf("[%d] Closing pipe fd (%d)...", getpid(), pipes[i]->fd[1]);
            if (close(pipes[i]->fd[1]))
                ERR("close");
        }
    }
}

void close_used_pipes(int n, int proc_index, pipe_t **pipes)
{
    if (close(pipes[(proc_index + 1) % n]->fd[1]))
    {
        printf("[%d] Closing pipe fd (%d)...", getpid(), pipes[(proc_index + 1) % n]->fd[0]);
        ERR("close");
    }
    if (close(pipes[(proc_index - 1) % n]->fd[0]))
    {
        printf("[%d] Closing pipe fd (%d)...", getpid(), pipes[(proc_index + 1) % n]->fd[0]);
        ERR("close");
    }
}

void create_pipe_loop(int n, pipe_t **pipes)
{
    for (int i = 0; i < n; i++)
    {
        printf("%d TEST\n", i);
        if (pipe(pipes[i]->fd) == -1)
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

    pipe_t *pipes = (pipe_t *)malloc(sizeof(pipe_t) * n);
    if (!pipes)
    {
        ERR("malloc");
    }
    for (int i = 0; i < n; i++)
    {
        if (NULL == (pipes[i].fd = (int *)malloc(sizeof(int) * 2)))
            ERR("malloc");
    }

    create_pipe_loop(n, &pipes);
    free(pipes);

    return EXIT_SUCCESS;
}
