#!/bin/bash

if [[ -z ${GITCOMMIT} || -z ${BUILD_OS} ]]; then
    source ./configure.sh
fi

if [[ ${BUILD_OS} != "mac" ]]; then
    echo "This script must be run from a Mac"
    exit 1
fi

mkdir -p ./nuget/packages/
mkdir -p ./apparchives/iOS/

if [ -d "./artifacts/iOS" ]; then
    for NUGETPACKAGE in $(ls ./artifacts/iOS/*.nupkg)
    do
        echo "copying ${NUGETPACKAGE} to ./nuget/packages/"
        cp ${NUGETPACKAGE} ./nuget/packages/
    done
    if [ -d "./artifacts/iOS/ipa" ]; then
        echo "moving iOS ipa artifacts to './apparchives' folder"
        mv ./artifacts/iOS/ipa/ ./apparchives/iOS/ipa
    else
        echo "./artifacts/iOS/ipa: iOS ipa artifacts NOT FOUND"
    fi
else
    echo "./artifacts/iOS: iOS packages NOT FOUND"; 
fi

if [ -d "./artifacts/UITest" ]; then
    echo "moving ./artifacts/UITest to root of repo for commit"
    mv ./artifacts/UITest/ ./UITest
fi

BRANCH_NAME=integration-${GITCOMMIT}
git checkout -b ${BRANCH_NAME}
git add ./nuget/packages/*.nupkg -f
git add ./apparchives/* -f
git add ./UITest/* -f
git commit -m "MAC-iOS-Build for ${GITCOMMIT}"
git push -u origin ${BRANCH_NAME} -f
