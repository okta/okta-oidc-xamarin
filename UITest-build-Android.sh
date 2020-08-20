#!/bin/bash

if [[ !(-z ${BUILT_ANDROID}) ]]; then
    echo "Android already built: BUILT_ANDROID = ${BUILT_ANDROID}"
else
    source ./configure.sh
    ./build.sh --target=Clean
    ./build.sh --target=AndroidTarget
    ./build.sh --target=Build-UITests
    mkdir -p ./apparchives/Android
    mv ./artifacts/Android/apk/ ./apparchives/Android/apk
    mv ./artifacts/UITest/ ./UITest

    export BUILT_ANDROID=`date`
fi
