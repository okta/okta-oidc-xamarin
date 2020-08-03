#!/bin/bash

CONFIGURATION="Debug"

if [[ -f "./release" ]]; then
    CONFIGURATION="Release"
fi

source ./configure.sh
./build.sh --target=AzureBuildTarget --configuration=${CONFIGURATION}
./nuget_commit.sh
