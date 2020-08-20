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

writeOktaConfigXml "./Okta.Xamarin/Okta.Xamarin.Android/Assets/OktaConfig.xml"
writeOktaConfigPlist "./Okta.Xamarin/Okta.Xamarin.iOS/OktaConfig.plist"
./build.sh --target=AzureBuildTarget --configuration=${CONFIGURATION}
./commit-from-azure.sh
