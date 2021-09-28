#!/bin/bash

source ./.okta/functions.sh

configureEnvironment

cd ./Okta.Xamarin/Tests/Okta.Xamarin.Test
dotnet test