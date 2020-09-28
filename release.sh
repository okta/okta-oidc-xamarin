#!/bin/bash

# This script must be run from an integration branch.
# It creates a new branch called release-<GITCOMMIT> where <GITCOMMIT> is
# the commit to be released (which is the current commit)

VERSION_COMPONENT=$1 # may specify 'major', 'minor' or 'patch' default is 'patch'

if [[ -z ${VERSION_COMPONENT} ]]; then
    VERSION_COMPONENT="patch"
fi

source ./configure.sh

CURRENTBRANCH=`git branch --show-current`
if [[ ${CURRENTBRANCH} != integration* ]]; then
    echo "Release script must be run from an integration branch."
    exit 1
fi

pushd ./nuget/semver
echo "Incrementing '${VERSION_COMPONENT}' version"
./version.sh ${VERSION_COMPONENT} release
RELEASE_VERSION=`./version.sh current`
echo "Release version is ${RELEASE_VERSION}"
git add .
popd

BRANCH_NAME=release-v${RELEASE_VERSION}
git checkout -b ${BRANCH_NAME}
echo `date` > ./release
git add release
git commit -m "CI: added release file for ${GITCOMMIT}, release version = ${RELEASE_VERSION}"
echo "pushing '${BRANCH_NAME}' branch to azure to begin release build" # ideally this would be run from bacon task but there is currently no easy way to hide azure credentials in bacon
git push -u azure ${BRANCH_NAME}
git checkout ${CURRENTBRANCH}
git branch -D ${BRANCH_NAME}
