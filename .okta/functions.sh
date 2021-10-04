#!/bin/bash

function configureEnvironment(){
    echo '<nuget_push.sh.configuringEnvironment>'

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
        yum install -y ca-certificates
        update-ca-trust force-enable
        curl -O http://ca.okta.com/Okta-Global-CA.pem
        curl -O http://ca.okta.com/Okta-Infrastructure-CA.pem
        curl -O http://ca.okta.com/Okta-Internet-CA.pem
        curl -O http://ca.okta.com/Okta-Root-CA.pem
        chmod o+r *.pem
        cp Okta-*.pem /etc/pki/ca-trust/source/anchors/
        update-ca-trust extract
        rpm -Uvh https://packages.microsoft.com/config/centos/7/packages-microsoft-prod.rpm
        yum -y install dotnet-sdk-5.0
    fi
    echo 'dotnet version is:'
    dotnet --version

    echo 'Installing MonoDevelop'
    rpm --import "http://keyserver.ubuntu.com/pks/lookup?op=get&search=0x3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF"
    curl https://download.mono-project.com/repo/centos7-vs.repo | tee /etc/yum.repos.d/mono-centos7-vs.repo
    yum -y install monodevelop

    echo '</ nuget_push.sh.configuringEnvironment>'
}

function pushNugetsToArtifactory() {
    if [ -d "./nuget/packages" ]; then
    for NUGETPACKAGE in $(ls ./nuget/packages/*.nupkg)
        do
        if [ "$DRY_RUN" = true ] ; then
            echo "export DRY_RUN=true"
            echo "echo: dotnet nuget push ${NUGETPACKAGE} -k ${NUGET_API_KEY} -s ${NUGET_SOURCE}"
        else
            echo "PUSHING nuget package ${NUGETPACKAGE} to ${NUGET_SOURCE}"
            echo "executing: dotnet nuget push ${NUGETPACKAGE} -k ${NUGET_API_KEY} -s ${NUGET_SOURCE}"
            dotnet nuget push ${NUGETPACKAGE} -k ${NUGET_API_KEY} -s ${NUGET_SOURCE}
        fi
    done
    else
        echo "./nuget/packages: No nuget packages found";
    fi
}
