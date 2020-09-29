#!/bin/bash

if [[ -z ${INSTALLED_IOS} ]]; then
    if [[ -z ${iOS_DEVICE_ID} ]]; then
        xcrun simctl list

        echo "Please enter the device id to use (copy from the list above)."
        read iOS_DEVICE_ID
    fi
    echo "Setting current simulator device id: '/Applications/Xcode.app/Contents/Developer/Applications/Simulator.app/Contents/MacOS/Simulator -CurrentDeviceUDID ${iOS_DEVICE_ID}'"
    /Applications/Xcode.app/Contents/Developer/Applications/Simulator.app/Contents/MacOS/Simulator -CurrentDeviceUDID ${iOS_DEVICE_ID} &
    echo "Installing app onto device: 'xcrun simctl install ${iOS_DEVICE_ID} ${OKTA_XAMARIN_HOME}/apparchives/iOS/ipa/iPhoneSimulatorOkta.Xamarin.iOS.app'"
    xcrun simctl install ${iOS_DEVICE_ID} ${OKTA_XAMARIN_HOME}/apparchives/iOS/ipa/iPhoneSimulatorOkta.Xamarin.iOS.app &

    export INSTALLED_IOS=`date`
fi
