Action<string> write = (msg) =>
{
    System.Console.WriteLine(msg);
};
// Arguments.
var target = Argument("target", "DefaultTarget");
var configuration = Argument("configuration", "Debug");

// Variables
var repositoryUrl = "https://github.com/okta/okta-oidc-xamarin.git";
var gitCommit = System.IO.File.ReadAllText("./commit");

// Define directories.
var solutionFile = GetFiles("./Okta.Xamarin/*.sln").First();

// Common, Android and iOS.
write("Getting common project");
var commonProject = GetFiles("./Okta.Xamarin/Okta.Xamarin/Okta.Xamarin.csproj").First();
write("Getting android project");
var androidProject = GetFiles("./Okta.Xamarin/Okta.Xamarin.Android/Okta.Xamarin.Android.csproj").First();
write("Getting ios project");
var iOSProject = GetFiles("./Okta.Xamarin/Okta.Xamarin.iOS/Okta.Xamarin.iOS.csproj").First();

write("Setting output folders");
// Output folders.
var artifactsDirectory = Directory(System.IO.Path.Combine(Environment.CurrentDirectory, "artifacts"));
var appArchivesDirectory = Directory(System.IO.Path.Combine(Environment.CurrentDirectory, "apparchives"));
var solutionOutputDirectory = Directory(System.IO.Path.Combine(artifactsDirectory, "SolutionOutput")); 
var uiTestOutputDirectory = Directory(System.IO.Path.Combine(artifactsDirectory, "UITest"));
var androidUITestOutputDirectory = Directory(System.IO.Path.Combine(uiTestOutputDirectory, "UITest.Android"));
var iOSUITestOutputDirectory = Directory(System.IO.Path.Combine(uiTestOutputDirectory, "UITest.iOS"));
var commonOutputDirectory = Directory(System.IO.Path.Combine(artifactsDirectory, "Common"));
var androidOutputDirectory = Directory(System.IO.Path.Combine(artifactsDirectory, "Android"));
var adroidApkOutputDirectory = Directory(System.IO.Path.Combine(androidOutputDirectory, "apk"));
var iOSOutputDirectory = Directory(System.IO.Path.Combine(artifactsDirectory, "iOS"));
var iOSIpaOutputDirectory = Directory(System.IO.Path.Combine(iOSOutputDirectory, "ipa"));
var iPhoneSimulatorIpaOutputDirectory = Directory(System.IO.Path.Combine(iOSIpaOutputDirectory, "iPhoneSimulator"));

var testProject = GetFiles("./Okta.Xamarin/Tests/Okta.Xamarin.Test.Okta.Xamarin.Test.csproj").First();

// Common Nuget functions
Func<string> getCommonVersion = () =>
{
    var version = System.IO.File.ReadAllText("./nuget/Common/version");
    if(configuration.Equals("Debug"))
    {
        version += $"-{gitCommit}";
    }
    return version;
};
Func<string[]> getCommonReleaseNotes = () =>
{
    // TODO: define a way to get release notes from the file system and/or git commits
    return new string[]{"", ""}; 
};
Func<string[]> getCommonTags = () =>
{
    // TODO: define a way to get tags from the file system and/or git commits
    return new string[]{"okta", "token", "authentication", "authorization", "oauth", "sso", "oidc"}; 
};
Func<NuSpecContent[]> getCommonFiles = ()=>
{
    // TODO: define algorithm to build NuSpecContent array from convention based relative path from artifacts dir
    return new [] { 
        new NuSpecContent {Source = "artifacts/Common/Okta.Xamarin.dll", Target = "lib/netstandard2.0"} 
    };
};
Func<NuSpecDependency[]> getCommonDependencies = () =>
{
    // TODO: define a way to determine dependencies programmatically
    return new [] {
        new NuSpecDependency { Id = "Newtonsoft.Json", Version = "12.0.3" },
        new NuSpecDependency { Id = "System.Net.Http", Version = "4.3.4" },
        new NuSpecDependency { Id = "Xamarin.Essentials", Version = "1.6.1" },
        new NuSpecDependency { Id = "Xamarin.Forms", Version = "5.0.0.2012" }
    };    
};
// ---
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
Func<string[]> getAndroidReleaseNotes = () =>
{
    // TODO: define a way to get release notes from the file system and/or git commits
    return new string[]{"", ""}; 
};
Func<string[]> getAndroidTags = () =>
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
Func<NuSpecDependency[]> getAndroidDependencies = () =>
{
    // TODO: define a way to determine dependencies programmatically
    return new [] {
        new NuSpecDependency { Id = "Okta.Xamarin", Version = "1.0.0" },
        new NuSpecDependency { Id = "System.Net.Http", Version = "4.3.4" },
        new NuSpecDependency { Id = "Xamarin.Essentials", Version = "1.6.1" },
        new NuSpecDependency { Id = "Xamarin.Forms", Version = "5.0.0.2012" },
        new NuSpecDependency { Id = "Xamarin.Android.Support.CustomTabs", Version = "28.0.0.3" }
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
Func<string[]> getiOSReleaseNotes = () =>
{
    // TODO: define a way to get release notes from the file system and/or git commits
    return new string[]{"", ""}; 
};
Func<string[]> getiOSTags = () =>
{
    // TODO: define a way to get tags from the file system and/or git commits
    return new string[]{"okta", "token", "authentication", "authorization", "oauth", "sso", "oidc"}; 
};
Func<NuSpecContent[]> getiOSFiles = () =>
{
    // TODO: define algorithm to build NuSpecContent array from convention based relative path from artifacts dir
    return new [] { 
        new NuSpecContent {Source = "artifacts/iOS/Okta.Xamarin.iOS.dll", Target = "lib/Xamarin.iOS10"} 
    };
};
Func<NuSpecDependency[]> getiOSDependencies = () =>
{
    // TODO: define way to determine dependencies programmatically
    return new []{
        new NuSpecDependency { Id = "Okta.Xamarin", Version = "1.0.0" },
        new NuSpecDependency { Id = "System.Net.Http", Version = "4.3.4" },
        new NuSpecDependency { Id = "Xamarin.Essentials", Version = "1.6.1" },
        new NuSpecDependency { Id = "Xamarin.Forms", Version = "5.0.0.2012" }
    };
};

Task("Clean")
    .Does(() => 
    {
        try
        {
            CleanDirectory(artifactsDirectory);
            CleanDirectory(appArchivesDirectory);

            MSBuild(solutionFile, settings => settings
                .SetConfiguration(configuration)
                .WithTarget("Clean")
                .SetVerbosity(Verbosity.Minimal)); 
        }
        catch (Exception ex)
        {
            write($"Error cleaning artifacts and app archive directories: {ex.Message}");
        }
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

Task("NugetPack-Common")
    .IsDependentOn("Build-Common")
    .Does(() =>
    {
                var nuGetPackSettings   = new NuGetPackSettings { 
                                Id                      = "Okta.Xamarin", 
                                Version                 = getCommonVersion(), 
                                Authors                 = new[] {"Okta, Inc."},
                                Owners                  = new[] {"Okta, Inc."},
                                Description             = "Official Okta OIDC SDK for Xamarin applications.",
                                ProjectUrl              = new Uri("https://github.com/okta/okta-oidc-xamarin"),
                                IconUrl                 = new Uri("https://raw.githubusercontent.com/okta/okta-sdk-dotnet/master/icon.png"),
                                LicenseUrl              = new Uri("https://github.com/okta/okta-oidc-dotnet/blob/master/LICENSE.md"),
                                Repository              = new NuGetRepository{Url=repositoryUrl, Type="Git"},
                                Copyright               = "(c) 2020 Okta, Inc.",
                                ReleaseNotes            = getCommonReleaseNotes(), 
                                Tags                    = getCommonTags(), 
                                RequireLicenseAcceptance= false, 
                                Symbols                 = false, 
                                NoPackageAnalysis       = true, 
                                Files                   = getCommonFiles(),
                                Dependencies            = getCommonDependencies(),
                                BasePath                = ".", 
                                OutputDirectory         = commonOutputDirectory 
                            }; 

        NuGetPack(nuGetPackSettings);
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

Task("NugetPack-Android")
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
                                Dependencies            = getAndroidDependencies(),
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
                .SetConfiguration("Release")
                .WithTarget("Build")
                .WithProperty("Platform", "iPhone")
                .WithProperty("OutputPath", iOSOutputDirectory)
                .WithProperty("TreatWarningsAsErrors", "false")
                .SetVerbosity(Verbosity.Minimal));

        MSBuild(iOSProject, settings => 
            settings
                .SetConfiguration("Debug")
                .WithTarget("Build")
                .WithProperty("Platform", "iPhoneSimulator")
                .WithProperty("OutputPath", iOSOutputDirectory)
                .WithProperty("TreatWarningsAsErrors", "false")
                .SetVerbosity(Verbosity.Minimal));
    });

Task("NugetPack-iOS")
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
                                Dependencies            = getiOSDependencies(),
                                BasePath                = ".", 
                                OutputDirectory         = iOSOutputDirectory 
                            }; 

        NuGetPack(nuGetPackSettings);
    });

Task("Run-Tests")
    .IsDependentOn("Restore-Packages")
    .Does(() =>
    {		
        DotNetCoreTest(testProject.FullPath, 
            new DotNetCoreTestSettings()
            {
                Configuration = configuration
                //NoBuild = true // Running tests will build the test project first, uncomment this line if this behavior should change
            });
    });

Task("AndroidTarget")
    .IsDependentOn("Build-Android")
    .IsDependentOn("NugetPack-Android");

Task("iOSTarget")
    .IsDependentOn("Build-iOS")
    .IsDependentOn("NugetPack-iOS");

Task("NugetTarget")
    .IsDependentOn("Clean")
    .IsDependentOn("Build-Common")
    .IsDependentOn("NugetPack-Common")
    .IsDependentOn("Build-Android")
    .IsDependentOn("NugetPack-Android")
    .IsDependentOn("Build-iOS")
    .IsDependentOn("NugetPack-iOS");

Task("AzureBuildTarget")
    .IsDependentOn("Clean")
    .IsDependentOn("Run-Tests")
    .IsDependentOn("Build-Android")
    .IsDependentOn("NugetPack-Android")
    .IsDependentOn("Build-iOS")
    .IsDependentOn("NugetPack-iOS");

Task("DefaultTarget")
    .IsDependentOn("AzureBuildTarget");

Console.WriteLine("Cake target is " + target);
RunTarget(target);
