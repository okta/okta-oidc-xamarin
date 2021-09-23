#!/bin/bash

# nuget source OKTA_NUGET_TOPIC_REPO and api key ${ARTIFACTORY_NUGET_APIKEY} are set by eng-productivity scripts run in bacon
echo "setting nuget source to artifactory"
NUGET_SOURCE="OKTA_NUGET_TOPIC_REPO"
NUGET_API_KEY=${ARTIFACTORY_NUGET_APIKEY}

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
