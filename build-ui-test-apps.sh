#!/bin/bash

if [[ -z ${OKTA_CLIENT_ID} ]]; then
    echo "OKTA_CLIENT_ID environment variable not set."
    exit 1
fi
if [[ -z ${OKTA_DOMAIN} ]]; then
    echo "OKTA_DOMAIN environment variable not set."
    exit 1
fi

CONFIGURATION="Debug"

if [[ -f "./release" ]]; then
    CONFIGURATION="Release"
fi

source ./configure.sh

function buildRelease(){
    ./build.sh --target=AzureBuildTarget --configuration=Release
    ./commit-from-azure.sh   
}

function buildDebug(){
    writeOktaConfigXml "./Okta.Xamarin/Okta.Xamarin.Android/Assets/OktaConfig.xml"
    writeOktaConfigPlist "./Okta.Xamarin/Okta.Xamarin.iOS/OktaConfig.plist"
    ./build.sh --target=AzureBuildTarget --configuration=${CONFIGURATION}
    ./commit-from-azure.sh
}

BUILD_FUNC="build${CONFIGURATION}"
$BUILD_FUNC