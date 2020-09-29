#!/bin/bash

TARGET=$1 # Android or iOS
CHECKOUT=$2 # the git commit to checkout or blank to run against the current branch

function checkout(){
    GITCOMMIT=$1 # used to determine the name of the integration branch to test
    if [[ -z ${GITCOMMIT} ]]; then
        GITCOMMIT=`git log --pretty='format:%H' -1`
    fi

    if [[ -z ${OKTA_XAMARIN_HOME} ]]; then
        CURDIR=`pwd`
        echo "setting OKTA_XAMARIN_HOME to ${CURDIR}"
        OKTA_XAMARIN_HOME=${CURDIR}
    fi

    GITCOMMIT=${GITCOMMIT:0:7}
    GITBRANCH="integration-${GITCOMMIT}"
    echo "looking for branch: ${GITBRANCH}"
    IS_LOCAL=`git branch --list ${GITBRANCH}`
    IS_REMOTE=`git ls-remote --heads origin ${GITBRANCH}`
    if [[ !(-z $IS_LOCAL) ]]; then
        git checkout ${GITBRANCH}
    elif [[ !(-z $IS_REMOTE ) ]]; then
        git fetch --all
        git checkout --track github/${GITBRANCH}
    else
        echo "Unable to find branch ${GITBRANCH}, make sure that the specified commit has been built by azure."
    fi
}

function build(){
    TARGET=$1
    if [[ -z ${TARGET} ]]; then
        echo "build: target not specified"
    else
        source UITest-build-${TARGET}.sh
    fi
}

function start(){
    TARGET=$1
    if [[ -z ${TARGET} ]]; then
        echo "start: target not specified"
    else
        source UITest-start-${TARGET}.sh
    fi
}

function run(){
    TARGET=$1
    if [[ -z ${TARGET} ]]; then
        echo "run: target not specified"
    else    
        # build
        echo "Building..."
        build ${TARGET}
        # install
        echo "Installing..."
        start ${TARGET}
        # run
        echo "Running..."
        ./UITest-${TARGET}.sh    
    fi
}

function Android(){
    echo "Android"
    run "Android"
}

function iOS(){
    echo "iOS"
    run "iOS"
}

if [[ !(-z ${CHECKOUT}) ]]; then
    if [[ ${CHECKOUT} = "current" ]]; then
        CHECKOUT=`git log --pretty='format:%H' -1`
    fi
    checkout ${CHECKOUT}
fi

${TARGET}
