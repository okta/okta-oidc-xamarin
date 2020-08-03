#!/bin/bash

# This script must be run from the integration branch.
# It will create a new branch called release-<GITCOMMIT> where <GITCOMMIT> is
# the commit to be released (which is the current commit)

source ./configure.sh
echo "Resetting commit file"
git checkout commit # reset the commit file to avoid confusion

CURRENTBRANCH=`git branch --show-current`
if [[ ${CURRENTBRANCH} != 'integration' ]]; then
    echo "Release script must be run from the integration branch."
    exit 1
fi

echo "Current branch is: ${CURRENTBRANCH}"
echo "Fetching branches..."
git fetch --all    
git checkout -b release-${GITCOMMIT}
echo `date` > ./release
git add release
git commit -m "CI: added release file"
echo "push 'release-${GITCOMMIT}' branch to azure to begin release build" # ideally this would be run from bacon task but there is currently no easy way to hide azure credentials in bacon
