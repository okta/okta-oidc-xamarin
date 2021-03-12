#!/bin/bash

if [[ !(-z ${BUILT_IOS}) ]]; then
    echo "iOS already built: BUILT_IOS = ${BUILT_IOS}"
else
    source ./configure.sh
    ./build.sh --target=Clean
    ./build.sh --target=iOSTarget
    ./build.sh --target=Build-UITests
    mkdir -p ./apparchives/iOS
    mv ./artifacts/iOS/ipa/ ./apparchives/iOS/ipa
    mv ./artifacts/UITest/ ./UITest

    export BUILT_IOS=`date`
fi
