// Arguments.
var target = Argument("target", "DefaultTarget");
var configuration = Argument("configuration", "Debug");

// Variables
var repositoryUrl = "https://github.com/okta/okta-oidc-xamarin.git";
var gitCommit = System.IO.File.ReadAllText("./commit");

// Define directories.
var solutionFile = GetFiles("./Okta.Xamarin/*.sln").First();

// Common, Android and iOS.
var commonProject = GetFiles("./Okta.Xamarin/Okta.Xamarin/Okta.Xamarin.csproj").First();
var androidProject = GetFiles("./Okta.Xamarin/Okta.Xamarin.Android/Okta.Xamarin.Android.csproj").First();
var iOSProject = GetFiles("./Okta.Xamarin/Okta.Xamarin.iOS/Okta.Xamarin.iOS.csproj").First();

// Output folders.
var artifactsDirectory = Directory(System.IO.Path.Combine(Environment.CurrentDirectory, "artifacts"));
var solutionOutputDirectory = Directory(System.IO.Path.Combine(artifactsDirectory, "SolutionOutput")); 
var commonOutputDirectory = Directory(System.IO.Path.Combine(artifactsDirectory, "Common"));
var androidOutputDirectory = Directory(System.IO.Path.Combine(artifactsDirectory, "Android"));
var iOSOutputDirectory = Directory(System.IO.Path.Combine(artifactsDirectory, "iOS"));

// Tests.
var testsProject = GetFiles("./Okta.Xamarin/Okta.Xamarin.Test/*.csproj").First();

// Android Nuget functions
Func<string> getAndroidVersion = () =>
{
    var version = System.IO.File.ReadAllText("./nuget/Android/version");
    if(configuration.Equals("Debug"))
    {
        version += $"-{gitCommit}";
    }
    return version;
};
Func<string[]> getAndroidReleaseNotes = ()=>
{
    // TODO: define a way to get release notes from the file system and/or git commits
    return new string[]{"", ""}; 
};
Func<string[]> getAndroidTags = ()=>
{
    // TODO: define a way to get tags from the file system and/or git commits
    return new string[]{"okta", "token", "authentication", "authorization", "oauth", "sso", "oidc"}; 
};
Func<NuSpecContent[]> getAndroidFiles = ()=>
{
    // TODO: define algorithm to build NuSpecContent array from convention based relative path from artifacts dir
    return new [] { 
        new NuSpecContent {Source = "artifacts/Android/Okta.Xamarin.Android.dll", Target = "lib/MonoAndroid10"} 
    };
};

// iOS Nuget functions
Func<string> getiOSVersion = () =>
{
    var version = System.IO.File.ReadAllText("./nuget/iOS/version");
    if(configuration.Equals("Debug"))
    {
        version += $"-{gitCommit}";
    }
    return version;
};
Func<string[]> getiOSReleaseNotes = ()=>
{
    // TODO: define a way to get release notes from the file system and/or git commits
    return new string[]{"", ""}; 
};
Func<string[]> getiOSTags = ()=>
{
    // TODO: define a way to get tags from the file system and/or git commits
    return new string[]{"okta", "token", "authentication", "authorization", "oauth", "sso", "oidc"}; 
};
Func<NuSpecContent[]> getiOSFiles = ()=>
{
    // TODO: define algorithm to build NuSpecContent array from convention based relative path from artifacts dir
    return new [] { 
        new NuSpecContent {Source = "artifacts/iOS/Okta.Xamarin.iOS.dll", Target = "lib/Xamarin.iOS10"} 
    };
};

Task("Clean")
    .Does(() => 
    {
        CleanDirectory(artifactsDirectory);

        MSBuild(solutionFile, settings => settings
            .SetConfiguration(configuration)
            .WithTarget("Clean")
            .SetVerbosity(Verbosity.Minimal)); 
    });

Task("Restore-Packages")
    .Does(() => 
    {
        NuGetRestore(solutionFile, 
            new NuGetRestoreSettings
            {
                ConfigFile = new FilePath("./NuGet.Config")
            });
    });

Task("Build-Solution")
    .IsDependentOn("Restore-Packages")
    .Does(() =>
    { 	
        MSBuild(solutionFile, settings =>
            settings
                .SetConfiguration(configuration)  
                .WithProperty("OutputPath", solutionOutputDirectory)         
                .WithProperty("DebugSymbols", "false")
                .WithProperty("TreatWarningsAsErrors", "false")
                .SetVerbosity(Verbosity.Minimal));
    });    

Task("Build-Common")
    .IsDependentOn("Restore-Packages")
    .Does(() =>
    { 	
        MSBuild(commonProject, settings =>
            settings
                .SetConfiguration(configuration)  
                .WithProperty("OutputPath", commonOutputDirectory)
                .WithProperty("DebugSymbols", "false")
                .WithProperty("TreatWarningsAsErrors", "false")
                .SetVerbosity(Verbosity.Minimal));
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

Task("Pack-Android")
    .IsDependentOn("Build-Android")
    .Does(() =>
    {
        var nuGetPackSettings   = new NuGetPackSettings { 
                                Id                      = "Okta.Xamarin.Android", 
                                Version                 = getAndroidVersion(), 
                                Authors                 = new[] {"Okta, Inc."},
                                Owners                  = new[] {"Okta, Inc."},
                                Description             = "Official Okta OIDC SDK for Xamarin Android applications.",
                                ProjectUrl              = new Uri("https://github.com/okta/okta-oidc-xamarin"),
                                IconUrl                 = new Uri("https://raw.githubusercontent.com/okta/okta-sdk-dotnet/master/icon.png"),
                                LicenseUrl              = new Uri("https://github.com/okta/okta-oidc-dotnet/blob/master/LICENSE.md"),
                                Repository              = new NuGetRepository{Url=repositoryUrl, Type="Git"},
                                Copyright               = "(c) 2020 Okta, Inc.",
                                ReleaseNotes            = getAndroidReleaseNotes(), 
                                Tags                    = getAndroidTags(), 
                                RequireLicenseAcceptance= false, 
                                Symbols                 = false, 
                                NoPackageAnalysis       = true, 
                                Files                   = getAndroidFiles(), 
                                BasePath                = ".", 
                                OutputDirectory         = androidOutputDirectory 
                            }; 

        NuGetPack(nuGetPackSettings);
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

Task("Pack-iOS")
    .IsDependentOn("Build-iOS")
    .Does(() =>
    {
        var nuGetPackSettings   = new NuGetPackSettings { 
                                Id                      = "Okta.Xamarin.iOS", 
                                Version                 = getiOSVersion(), 
                                Authors                 = new[] {"Okta, Inc."},
                                Owners                  = new[] {"Okta, Inc."},
                                Description             = "Official Okta OIDC SDK for Xamarin iOS applications.",
                                ProjectUrl              = new Uri("https://github.com/okta/okta-oidc-xamarin"),
                                IconUrl                 = new Uri("https://raw.githubusercontent.com/okta/okta-sdk-dotnet/master/icon.png"),
                                LicenseUrl              = new Uri("https://github.com/okta/okta-oidc-dotnet/blob/master/LICENSE.md"),
                                Repository              = new NuGetRepository{Url=repositoryUrl, Type="Git"},
                                Copyright               = "(c) 2020 Okta, Inc.",
                                ReleaseNotes            = getiOSReleaseNotes(), 
                                Tags                    = getiOSTags(), 
                                RequireLicenseAcceptance= false, 
                                Symbols                 = false, 
                                NoPackageAnalysis       = true, 
                                Files                   = getiOSFiles(), 
                                BasePath                = ".", 
                                OutputDirectory         = iOSOutputDirectory 
                            }; 

        NuGetPack(nuGetPackSettings);
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

Task("AndroidTarget")
    .IsDependentOn("Clean")
    .IsDependentOn("Build-Android")
    .IsDependentOn("Run-Tests")
    .IsDependentOn("Pack-Android");

Task("iOSTarget")
    .IsDependentOn("Clean")
    .IsDependentOn("Build-iOS")
    .IsDependentOn("Run-Tests")
    .IsDependentOn("Pack-iOS");

Task("AzureBuildTarget")
    .IsDependentOn("Clean")
    .IsDependentOn("Build-Android")
    .IsDependentOn("Build-iOS")
    .IsDependentOn("Run-Tests")
    .IsDependentOn("Pack-Android")
    .IsDependentOn("Pack-iOS");

Task("DefaultTarget")
    .IsDependentOn("AzureBuildTarget");

Console.WriteLine("Cake target is " + target);
RunTarget(target);
