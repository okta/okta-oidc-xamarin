#!/bin/bash

NUNITCONSOLERUNNERVERSION="3.11.1"
NUNITCONSOLE="nunit3-console.exe"

pushd ./tools
echo "Installing NUnit.ConsoleRunner"
nuget install NUnit.ConsoleRunner -Version ${NUNITCONSOLERUNNERVERSION}

echo "Running Okta.Xamarin.UITest"
exec /Library/Frameworks/Mono.framework/Versions/Current/Commands/mono ./NUnit.ConsoleRunner.${NUNITCONSOLERUNNERVERSION}/tools/nunit3-console.exe "${OKTA_XAMARIN_HOME}/UITest/UITest.iOS/Okta.Xamarin.UITest.iOS.dll"
popd
