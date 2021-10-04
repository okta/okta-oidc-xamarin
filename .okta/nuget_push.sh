#!/bin/bash

source ./.okta/functions.sh
# nuget source OKTA_NUGET_TOPIC_REPO and api key ${ARTIFACTORY_NUGET_APIKEY} are set by eng-productivity scripts run in bacon
echo "setting nuget source to artifactory"
NUGET_SOURCE="OKTA_NUGET_TOPIC_REPO"
NUGET_API_KEY=${ARTIFACTORY_NUGET_APIKEY}

configureEnvironment

pushNugetsToArtifactory
