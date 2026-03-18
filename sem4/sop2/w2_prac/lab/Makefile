CC=gcc
CFLAGS= -std=gnu99 -Wall


NAME=zad1

.PHONY: all clean

all: ${NAME}

${NAME}: ${NAME}.c
	$(CC) $(CFLAGS) -o ${NAME} ${NAME}.c

clean:
	rm -f ${NAME}