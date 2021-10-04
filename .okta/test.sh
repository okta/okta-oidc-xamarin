#!/bin/bash

function dryRun

if [ "$DRY_RUN" = true ] ; then
    echo "yes dry run"
else
    echo "no dry run"
fi