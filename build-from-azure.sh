#!/bin/bash

CONFIGURATION="Debug"

if [[ -f "./release" ]]; then
    CONFIGURATION="Release"
fi

source ./configure.sh

./build.sh --target=NugetTarget --configuration=${CONFIGURATION}
./commit-from-azure.sh