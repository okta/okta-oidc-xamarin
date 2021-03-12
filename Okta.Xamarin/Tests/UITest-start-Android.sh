#!/bin/bash

if [[ -z ${INSTALLED_ANDROID} ]]; then
    EXE=
    if [[ "${OSTYPE}" == "cygwin" || "${OSTYPE}" == "msys" ]]; then
        EXE=".exe"
    fi
    echo "Starting Android emulator"
    "${ANDROID_HOME}/emulator/emulator${EXE}" -avd pixel_3_pie_9_0_-_api_28 -wipe-data &
    output=''
    while [[ ${output:0:7} != 'stopped' ]]; do # make sure the emulator has started before moving on
        echo "Waiting until emulator has started..."
        output=`adb shell getprop init.svc.bootanim`        
        sleep 1
    done

    export INSTALLED_ANDROID=`date`
fi
