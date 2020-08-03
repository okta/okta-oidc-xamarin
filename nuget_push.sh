#!/bin/bash

# if run from azure, this script will push nuget packages to github packages
# if run from bacon, this script will push nuget packages to artifactory

source ./configure.sh
GITHUB_PACKAGE_SOURCE="https://nuget.pkg.github.com/okta/index.json"

if [ ${BUILD_ENVIRONMENT} == "AZURE" ]; then
    echo "adding github package source: ${GITHUB_PACKAGE_SOURCE}"
    dotnet nuget add source ${GITHUB_PACKAGE_SOURCE} -n github -u ${GITHUB_USERNAME} -p ${GITHUB_ACCESS_TOKEN} --store-password-in-clear-text
    echo "setting nuget source to github"
    NUGET_SOURCE="github"
    NUGET_API_KEY=${GITHUB_ACCESS_TOKEN}
else
    # nuget source OKTA_NUGET_TOPIC_REPO and api key ${ARTIFACTORY_NUGET_APIKEY} are set by eng-productivity scripts run in bacon
    echo "setting nuget source to artifactory"
    NUGET_SOURCE="OKTA_NUGET_TOPIC_REPO"
    NUGET_API_KEY=${ARTIFACTORY_NUGET_APIKEY}
fi

if [ -d "./nuget/packages" ]; then
    for NUGETPACKAGE in $(ls ./nuget/packages/*.nupkg)
    do
        echo "Pushing nuget package ${NUGETPACKAGE} to ${NUGET_SOURCE}"
        echo "dotnet nuget push ${NUGETPACKAGE} -k ${NUGET_API_KEY} -s ${NUGET_SOURCE}"
        dotnet nuget push ${NUGETPACKAGE} -k ${NUGET_API_KEY} -s ${NUGET_SOURCE}
    done
else
    echo "./nuget/packages: No nuget packages found";
fi
