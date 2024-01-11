#!/bin/bash

if [ "$#" -ne 1 ]; then
    echo "Usage: $0 <target_folder>"
    exit 1
fi

target_folder="$1"

function remove_empty_folders() {
    # Find all empty folders and their associated .meta files
    find "$target_folder" -type d -empty -print0 | while IFS= read -r -d $'\0' folder; do
        # Remove the empty folder
        rm -r "$folder"

        # Remove the associated .meta file if it exists
        meta_file="$folder.meta"
        if [ -e "$meta_file" ]; then
            rm "$meta_file"
        fi
    done
}

# Run the function until there are no more empty folders
while [ "$(find "$target_folder" -type d -empty)" ]; do
    remove_empty_folders
done

echo "Empty folders and their associated .meta files removed in $target_folder."
