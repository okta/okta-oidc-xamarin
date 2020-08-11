// Arguments.
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

// Define directories.
var solutionFile = GetFiles("./Okta.Xamarin/*.sln").First();

// Android and iOS.
var androidProject = GetFiles("./Okta.Xamarin/Okta.Xamarin.Android/*.csproj").First();
var iOSProject = GetFiles("./Okta.Xamarin/Okta.Xamarin.iOS/*.csproj").First();

// Output folders.
var artifactsDirectory = Directory(System.IO.Path.Combine(Environment.CurrentDirectory, "artifacts"));
var androidOutputDirectory = Directory(System.IO.Path.Combine(artifactsDirectory, "Android"));
var iOSOutputDirectory = Directory(System.IO.Path.Combine(artifactsDirectory, "iOS"));

// Tests.
var testsProject = GetFiles("./Okta.Xamarin/Okta.Xamarin.Test/*.csproj").First();

Task("Clean")
    .Does(() => 
    {
        Console.WriteLine("Cleaning directory {0}", artifactsDirectory);
        CleanDirectory(artifactsDirectory);

        Console.WriteLine("Calling DotNetBuild Target=Clean");
        MSBuild(solutionFile, settings => settings
            .SetConfiguration(configuration)
            .WithTarget("Clean")
            .SetVerbosity(Verbosity.Minimal)); 
    });

Task("Restore-Packages")
    .Does(() => 
    {
        NuGetRestore(solutionFile);
    });

Task("Build-Android")
    .IsDependentOn("Restore-Packages")
    .Does(() =>
    { 	
        MSBuild(androidProject, settings =>
            settings
                .SetConfiguration(configuration)  
                .WithProperty("OutputPath", androidOutputDirectory)         
                .WithProperty("DebugSymbols", "false")
                .WithProperty("TreatWarningsAsErrors", "false")
                .SetVerbosity(Verbosity.Minimal));
    });

Task("Build-iOS")
    .IsDependentOn("Restore-Packages")
    .Does (() =>
    {
        MSBuild(iOSProject, settings => 
            settings
                .SetConfiguration(configuration)   
                .WithTarget("Build")
                .WithProperty("Platform", "iPhoneSimulator")
                .WithProperty("OutputPath", iOSOutputDirectory)
                .WithProperty("TreatWarningsAsErrors", "false")
                .SetVerbosity(Verbosity.Minimal));
    });

Task("Run-Tests")
    .IsDependentOn("Restore-Packages")
    .Does(() =>
    {		
        DotNetCoreTest(testsProject.FullPath, 
            new DotNetCoreTestSettings()
            {
                Configuration = configuration
                //NoBuild = true // Running tests will build the test project first, uncomment this line if this behavior should change
            });
    });

Task("Default")
    .IsDependentOn("Clean")
    .IsDependentOn("Build-Android")
    .IsDependentOn("Build-iOS")
    .IsDependentOn("Run-Tests");

RunTarget(target);
