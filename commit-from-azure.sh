#!/bin/bash

# This script is used to commit nuget packages to the 'integration' branch
# for bacon to pick up and push to artifactory.  Packages should be built
# prior to running this script.

mkdir -p ./nuget/packages/
mkdir -p ./apparchives/Android/
mkdir -p ./apparchives/iOS/

if [ -d "./artifacts/Android" ]; then
    for NUGETPACKAGE in $(ls ./artifacts/Android/*.nupkg)
    do
        echo "copying ${NUGETPACKAGE} to ./nuget/packages/"
        cp ${NUGETPACKAGE} ./nuget/packages/
    done
    if [ -d "./artifacts/Android/apk" ]; then
        echo "moving ANDROID apk artifacts to './apparchives' folder"
        mv ./artifacts/Android/apk/ ./apparchives/Android/apk
    else
        echo "./artifacts/Android/apk: ANDROID apk artifacts NOT FOUND"
    fi
else
    echo "./artifacts/Android: Android packages NOT FOUND";
fi

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

CURRENTBRANCH=`git branch --show-current`
if [[ -z ${CURRENTBRANCH} ]]; then
    CURRENTBRANCH=${GITCOMMIT}
fi

echo 'ls ./nuget/packages/'
ls ./nuget/packages/

BRANCH_PREFIX="integration"
BRANCH_SUFFIX=${GITCOMMIT} # GITCOMMIT is set by configure.sh called by build-from-azure.sh which calls this script
if [[ -f ./release ]]; then
    BRANCH_PREFIX="release"
    pushd ./nuget/semver
    VERSION=`./version.sh current`
    BRANCH_SUFFIX="v${VERSION}"
    popd
fi

if [[ $BUILD_ENVIRONMENT == "AZURE" ]]; then
    BRANCH_NAME=${BRANCH_PREFIX}-${BRANCH_SUFFIX}
    git checkout -b ${BRANCH_NAME}
    git add ./nuget/packages/*.nupkg -f
    git add ./apparchives/* -f
    git add ./UITest/* -f
    git commit --author="azure-ci <noreply@okta.com>" -m "CI: '${BRANCH_PREFIX}' nuget packages for ${BRANCH_SUFFIX}"
    git push -u https://${GITHUB_USERNAME}:${GITHUB_ACCESS_TOKEN}@github.com/okta/okta-oidc-xamarin.git ${BRANCH_NAME} -f
else
    echo "BUILD_ENVIRONMENT is ${BUILD_ENVIRONMENT}, will not automatically push to github."
fi
