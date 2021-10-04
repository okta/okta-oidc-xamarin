#!/bin/bash

source ./.okta/functions.sh

configureEnvironment

downloadNugetArtifacts

pushNugetsToArtifactory
