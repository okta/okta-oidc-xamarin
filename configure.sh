#!/bin/bash

function configureWindows(){
    echo "<configure.sh.configureWindows>"
    # put windows specific configuration here

    echo "</ configure.sh.configureWindows>"
}

function configureMac(){
    echo "<configure.sh.configureMac>"
    # put mac specific configuration here

    echo "</ configure.sh.configureMac>"
}

function configureLinux(){
    echo '<configure.sh.configureLinux>'

    echo 'Assuming OS is Centos 7 (with yum)'
    yum -y update

    echo 'Installing openjdk 8'
    yum -y install java-1.8.0-openjdk
    export JAVA_HOME=/usr/java/jdk1.8.0_45
    export PATH=$PATH:$JAVA_HOME/bin
    export CLASSPATH=.:$JAVA_HOME/jre/lib:$JAVA_HOME/lib:$JAVA_HOME/lib/tools.jar
    java -version
    echo 'JAVA_HOME is'
    echo $JAVA_HOME

    echo 'Checking for unzip'
    if ! [ -x "$(command -v unzip)" ]; then
        yum -y install unzip
        echo 'Installed unzip'
    else
        echo 'unzip already installed'
    fi

    echo 'Installing Android tools'
    curl https://dl.google.com/android/repository/commandlinetools-linux-6609375_latest.zip -o android-sdk.zip
    unzip android-sdk.zip -d ./cmdline-tools/
    rm android-sdk.zip

    yes | ./cmdline-tools/tools/bin/sdkmanager tools
    ./cmdline-tools/tools/bin/sdkmanager --update
    ./cmdline-tools/tools/bin/sdkmanager --list
    yes | ./cmdline-tools/tools/bin/sdkmanager --licenses

    # ensure that mono is installed, assumes host has yum (CentOS 7)
    echo 'Checking for mono'
    if ! [ -x "$(command -v mono)" ]; then
        echo 'Mono is not installed.'
        echo 'Installing Mono...'
        rpmkeys --import 'http://pool.sks-keyservers.net/pks/lookup?op=get&search=0x3fa7e0328081bff6a14da29aa6a19b38d3d831ef'  
        su -c 'curl https://download.mono-project.com/repo/centos7-stable.repo | tee /etc/yum.repos.d/mono-centos7-stable.repo'
        yum -y install mono-complete
    fi
    mono --version

    # ensure that dotnet is installed, assumes host has yum (CentOS 7)
    echo 'Checking for dotnet'
    if ! [ -x "$(command -v dotnet)" ]; then
        rpm -Uvh https://packages.microsoft.com/config/centos/7/packages-microsoft-prod.rpm
        yum -y install dotnet-sdk-3.1
    fi
    echo 'dotnet version is:'
    dotnet --version

    echo 'Installing MonoDevelop'
    rpm --import "http://keyserver.ubuntu.com/pks/lookup?op=get&search=0x3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF"
    curl https://download.mono-project.com/repo/centos7-vs.repo | tee /etc/yum.repos.d/mono-centos7-vs.repo
    yum -y install monodevelop

    echo '</ configure.sh.configureLinux>'
}


function writeOktaConfigXml(){
    FILEPATH=$1
    echo '<?xml version="1.0" encoding="utf-8" ?>' > ${FILEPATH}
    echo '<Okta>' >> $FILEPATH
    echo $'\t'"<ClientId>${OKTA_CLIENT_ID}</ClientId>" >> ${FILEPATH}
    echo $'\t'"<OktaDomain>${OKTA_DOMAIN}</OktaDomain>" >> ${FILEPATH}
    echo $'\t'"<RedirectUri>com.okta.xamarin.android:/callback</RedirectUri>" >> ${FILEPATH}
    echo $'\t'"<PostLogoutRedirectUri>com.okta.xamarin.android:/logout</PostLogoutRedirectUri>" >> ${FILEPATH}
    echo "</Okta>" >> ${FILEPATH}
}

function writeOktaConfigPlist(){
    FILEPATH=$1
    echo '<?xml version="1.0" encoding="UTF-8"?>' > ${FILEPATH}
    echo '<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">' >> ${FILEPATH}
    echo '<plist version="1.0">' >> ${FILEPATH}
    echo '<dict>' >> ${FILEPATH}
    echo $'\t'"<key>ClientId</key>" >> ${FILEPATH}
    echo $'\t'"<string>${OKTA_CLIENT_ID}</string>" >> ${FILEPATH}
    echo $'\t'"<key>OktaDomain</key>" >> ${FILEPATH}
    echo $'\t'"<string>${OKTA_DOMAIN}</string>" >> ${FILEPATH}
    echo $'\t'"<key>RedirectUri</key>" >> ${FILEPATH}
    echo $'\t'"<string>com.okta.xamarin.ios:/callback</string>" >> ${FILEPATH}
    echo $'\t'"<key>PostLogoutRedirectUri</key>" >> ${FILEPATH}
    echo $'\t'"<string>com.okta.xamarin.ios:/logout</string>" >> ${FILEPATH}
    echo "</dict>" >> ${FILEPATH}
    echo "</plist>" >> ${FILEPATH}
}

if [[ "${OSTYPE}" == "linux-gnu" ]]; then
    export BUILD_OS=linux
    configureLinux
fi
if [[ "${OSTYPE}" == "darwin"* ]]; then
    export BUILD_OS=mac
    configureMac
fi
if [[ "${OSTYPE}" == "cygwin" ]]; then
    export BUILD_OS=windows
    configureWindows
fi
if [[ "${OSTYPE}" == "msys" ]]; then
    export BUILD_OS=windows
    configureWindows
fi
if [[ "${OSTYPE}" == "freebsd"* ]]; then
    export BUILD_OS=mac
    configureMac
fi

if [[ -z ${CONFIGURED} ]]; then
    export GITCOMMIT=`git rev-parse --short HEAD`
    echo "OSTYPE: ${OSTYPE}"
    echo "BUILD_OS: ${BUILD_OS}"
    echo "GITCOMMIT=${GITCOMMIT}"
    echo ${GITCOMMIT} > ./commit
    export CONFIGURED=${GITCOMMIT}
fi

if [[ -z ${BUILD_ENVIRONMENT} ]]; then
    export BUILD_ENVIRONMENT="BACON"
fi
