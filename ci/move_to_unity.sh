#!/bin/bash

sourceDir="GameJolt.NET"

unitySourceDir="${sourceDir}/Unity Integration"
editorSourceDir="${unitySourceDir}/Editor"

destinationDir="Unity/Packages/se.hertzole.gamejolt.net"
runtimeDestinationDir="${destinationDir}/Runtime"
editorDestinationDir="${destinationDir}/Editor"

objDir="${sourceDir}/obj"
binDir="${sourceDir}/bin"

testsSourceDir="GameJolt.NET.Tests"

testsRuntimeDestinationDir="${destinationDir}/Tests/Runtime"

testsObjDir="${testsSourceDir}/obj"
testsBinDir="${testsSourceDir}/bin"

mkdir -p "$destinationDir"

echo "|--------------------------|"
echo "| Copying runtime files... |"
echo "|--------------------------|"

find "$sourceDir" -name "*.cs" | while IFS= read -r file; do
    if [[ $file == *"$objDir"* ]] || [[ $file == *"$binDir"* ]] || [[ $file == *"$editorSourceDir"* ]]; then
        echo "SKIPPING '$file'"
        continue
    fi

    relativePath="${file#$sourceDir}"

    targetPath="${runtimeDestinationDir}/${relativePath}"

    # Create parent directories if they don't exist
    mkdir -p "$(dirname "$targetPath")"

    cp "$file" "$targetPath"
    echo "COPIED '$file' TO '$targetPath'"
done

echo "|-------------------------|"
echo "| Copying editor files... |"
echo "|-------------------------|"

find "$editorSourceDir" -name "*.cs" | while IFS= read -r file; do

    relativePath="${file#$editorSourceDir}"

    targetPath="${editorDestinationDir}/${relativePath}"

    # Create parent directories if they don't exist
    mkdir -p "$(dirname "$targetPath")"

    cp "$file" "$targetPath"
    echo "COPIED '$file' TO '$targetPath'"
done

echo "|------------------------|"
echo "| Copying tests files... |"
echo "|------------------------|"

find "$testsSourceDir" -name "*.cs" | while IFS= read -r file; do
    if [[ $file == *"$testsObjDir"* ]] || [[ $file == *"$testsBinDir"* ]]; then
        echo "SKIPPING '$file'"
        continue
    fi

    relativePath="${file#$testsSourceDir}"

    targetPath="${testsRuntimeDestinationDir}/${relativePath}"

    # Create parent directories if they don't exist
    mkdir -p "$(dirname "$targetPath")"

    cp "$file" "$targetPath"
    echo "COPIED '$file' TO '$targetPath'"
done

echo "All .cs files copied successfully."