#!/bin/bash

OUTPUT_FILE="full_project.txt"
EXCLUDE_DIRS=(".git" "bin" "obj" ".idea" "Logs")

echo "Project Structure Dump - $(date)" > "$OUTPUT_FILE"
echo "==========================================" >> "$OUTPUT_FILE"
echo "" >> "$OUTPUT_FILE"
echo "Directory Tree:" >> "$OUTPUT_FILE"

# Build the find command for directories
FIND_CMD_DIRS="find . -maxdepth 10 -not -path '*/.*'"
for dir in "${EXCLUDE_DIRS[@]}"; do
    FIND_CMD_DIRS="$FIND_CMD_DIRS -not -path './$dir*'"
done

# Use tree if available, otherwise fallback to find
if command -v tree > /dev/null; then
    tree -I "$(echo "${EXCLUDE_DIRS[@]}" | tr ' ' '|')" >> "$OUTPUT_FILE"
else
    eval "$FIND_CMD_DIRS" >> "$OUTPUT_FILE"
fi

echo "" >> "$OUTPUT_FILE"
echo "File Contents:" >> "$OUTPUT_FILE"
echo "==========================================" >> "$OUTPUT_FILE"

# Find all files, excluding specified directories and the script/output file itself
FIND_CMD_FILES="find . -type f"
for dir in "${EXCLUDE_DIRS[@]}"; do
    FIND_CMD_FILES="$FIND_CMD_FILES -not -path './$dir*'"
done
FIND_CMD_FILES="$FIND_CMD_FILES -not -name '$OUTPUT_FILE' -not -name '$(basename "$0")' -not -path './.*'"

eval "$FIND_CMD_FILES" | sort | while read -r file; do
    echo "" >> "$OUTPUT_FILE"
    echo "File: $file" >> "$OUTPUT_FILE"
    echo "-------------------------------------------------------------------------------" >> "$OUTPUT_FILE"
    cat "$file" >> "$OUTPUT_FILE"
    echo "" >> "$OUTPUT_FILE"
    echo "-------------------------------------------------------------------------------" >> "$OUTPUT_FILE"
done

echo "Project dump completed: $OUTPUT_FILE"
