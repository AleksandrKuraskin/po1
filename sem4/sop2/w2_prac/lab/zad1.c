#include <errno.h>
#include <signal.h>
#include <stdio.h>
#include <stdlib.h>

#define ERR(source)                                                            \
  (fprintf(stderr, "%s:%d\n", __FILE__, __LINE__), perror(source),             \
   kill(0, SIGKILL), exit(EXIT_FAILURE))

void usage(char *name) {
  printf(stderr, "USAGE %s: No arguments needed.", name);
  exit(EXIT_FAILURE);
}

int main(int argc, char **argv){
    
}