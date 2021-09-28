#!/bin/bash

source ./functions.sh

configureEnvironment

cd ./Okta.Xamarin/Tests/Okta.Xamarin.Test
dotnet test