#!/bin/bash

if [[ -z ${OKTA_CLIENT_ID} ]]; then
    echo "Please enter a value for OKTA_CLIENT_ID."
    read OKTA_CLIENT_ID
fi
if [[ -z ${OKTA_DOMAIN} ]]; then
    echo "Please enter a value for OKTA_DOMAIN."
    read OKTA_DOMAIN
fi

CONFIGURATION="Debug"

if [[ -f "./release" ]]; then
    CONFIGURATION="Release"
fi

source ./configure.sh

writeOktaConfigXml "./Okta.Xamarin/Okta.Xamarin.Android/Assets/OktaConfig.xml"
writeOktaConfigPlist "./Okta.Xamarin/Okta.Xamarin.iOS/OktaConfig.plist"

./build-iOS.sh
./commit-from-mac.sh
