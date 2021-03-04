#!/bin/bash

GITCOMMIT=$1 # used to determine the name of the integration branch to test

NUNITCONSOLERUNNERVERSION="3.11.1"
NUNITCONSOLE="nunit3-console.exe"

pushd ./tools
echo "Installing NUnit.ConsoleRunner"
nuget install NUnit.ConsoleRunner -Version ${NUNITCONSOLERUNNERVERSION}

echo "Running Okta.Xamarin.UITest"
./NUnit.ConsoleRunner/tools/nunit3-console.exe ${OKTA_XAMARIN_HOME}/UITest/UITest.Android/Okta.Xamarin.UITest.Android.dll
popd
