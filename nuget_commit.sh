#!/bin/bash

# This script is used to commit nuget packages to the 'integration' branch
# for bacon to pick up and push to artifactory.  Packages should be built
# prior to running this script.

mkdir -p ./nuget/packages/

if [ -d "./artifacts/Android" ]; then
    for NUGETPACKAGE in $(ls ./artifacts/Android/*.nupkg)
    do
    echo "copying ${NUGETPACKAGE} to ./nuget/packages/"
        cp ${NUGETPACKAGE} ./nuget/packages/
    done
else
    echo "./artifacts/Android: No Android artifacts found";
fi

if [ -d "./artifacts/iOS" ]; then
    for NUGETPACKAGE in $(ls ./artifacts/iOS/*.nupkg)
    do
        echo "copying ${NUGETPACKAGE} to ./nuget/packages/"
        cp ${NUGETPACKAGE} ./nuget/packages/
    done
else
    echo "./artifacts/iOS: No iOS artifacts found"; 
fi

CURRENTBRANCH=`git branch --show-current`
if [[ -z ${CURRENTBRANCH} ]]; then
    CURRENTBRANCH=${GITCOMMIT}
fi

echo "Current branch/commit is: ${CURRENTBRANCH}"
if [[ ${CURRENTBRANCH} != "integration" ]]; then
    echo "Fetching branches..."
    git fetch --all    
    echo "Merging 'integration' branch..."
    git merge origin/integration
    git checkout integration
    git merge ${CURRENTBRANCH}
else
    git pull
fi

echo 'ls ./nuget/packages/'
ls ./nuget/packages/
git add ./nuget/packages/*.nupkg -f
git commit --author="azure-ci <noreply@okta.com>" -m "CI: 'integration' nuget packages for ${GITCOMMIT}"
git push -u origin integration -f
git push -u https://${GITHUB_USERNAME}:${GITHUB_ACCESS_TOKEN}@github.com/okta/okta-oidc-xamarin.git integration -f
