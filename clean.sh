#!/bin/bash

# This script deletes the contents of the ./artifacts and ./apparchives 
# directories, then deletes all locally available branches whose name 
# starts with integration-

git checkout commit # reset the commit file
./build.sh --target=Clean

for GITBRANCH in $(git branch)
do
    if [[ $GITBRANCH == integration-* ]]; then
        echo "Deleting branch $GITBRANCH"
        git branch -D $GITBRANCH
        git push -u github :$GITBRANCH
    fi
done

rm -fr ./UITest
