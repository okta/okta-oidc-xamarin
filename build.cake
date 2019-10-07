#addin "Cake.Xamarin"

//#tool "xunit.runner.console"
#tool "nuget:?package=Microsoft.TestPlatform&version=15.7.0"


// Arguments.
var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

// Define directories.
var solutionFile = GetFiles("./Okta.Xamarin/*.sln").First();

// Android and iOS.
var androidProject = GetFiles("./Okta.Xamarin/Okta.Xamarin.Android/*.csproj").First();
var iOSProject = GetFiles("./Okta.Xamarin/Okta.Xamarin.iOS/*.csproj").First();

// Tests.
var testsProject = GetFiles("./Okta.Xamarin/Okta.Xamarin.Test/*.csproj").First();
// NOTE: (assumes Tests projects end with .Tests).
var testsDllPath = string.Format("./Okta.Xamarin/Okta.Xamarin.Test/bin/{0}/*/*.Test.dll", configuration);

// Output folders.
var artifactsDirectory = Directory("./artifacts");
var iOSOutputDirectory = "bin/iPhoneSimulator";

Task("Clean")
	.Does(() => 
	{
		CleanDirectory(artifactsDirectory);

		// There are some files created after iOSBuild that don't clean from the iOSOutputDirectory, force to clean here.
		CleanDirectories("./**/" + iOSOutputDirectory);

		DotNetBuild(solutionFile, settings => settings
			.SetConfiguration(configuration)
			.WithTarget("Clean")
			.SetVerbosity(Verbosity.Normal));
	});

Task("Restore-Packages")
	.Does(() => 
	{
		NuGetRestore(solutionFile);
	});

Task("Prepare-Build")
	.IsDependentOn("Clean")
	.IsDependentOn("Restore-Packages")
		.Does (() => {});

Task("Run-Tests")
	// Allows the build process to continue even if there Tests aren't passing.
	.ContinueOnError()
	.IsDependentOn("Prepare-Build")
	.Does(() =>
	{	
		DotNetBuild(testsProject.FullPath, settings => settings
			.SetConfiguration(configuration)
			.WithTarget("Build")
			.SetVerbosity(Verbosity.Normal));			

		var testSettings = new VSTestSettings{
			ToolPath        = Context.Tools.Resolve("vstest.console.exe"),

			// use the Trx Logger and a deterministic output file name
			// to be able to import test results into a build orchestration tool (VSTS, Teamcity etc.).
			ArgumentCustomization = arg => arg.Append("/logger:trx;LogFileName=VsTestResults.xml")
		};
		
		// the test file pattern will obviously depend on the project.
		VSTest(testsDllPath, testSettings);


	//	DotNetCoreTool(
	//		projectPath: testsDllPath,
	//		command: "xunit", 
	//        arguments: $"-configuration {configuration} --no-build");
	});



Task("Build-Android")
	.IsDependentOn("Prepare-Build")
	.Does(() =>
	{
		DotNetBuild(androidProject, settings =>
			settings.SetConfiguration(configuration)           
			.WithProperty("DebugSymbols", "false")
			.WithProperty("TreatWarningsAsErrors", "false")
			.SetVerbosity(Verbosity.Normal));
	});

Task("Build-iOS")
	.IsDependentOn("Prepare-Build")
	.Does (() =>
	{
			DotNetBuild(iOSProject, settings => 
			settings.SetConfiguration(configuration)   
			.WithTarget("Build")
			.WithProperty("Platform", "iPhoneSimulator")
			.WithProperty("OutputPath", iOSOutputDirectory)
			.WithProperty("TreatWarningsAsErrors", "false")
			.SetVerbosity(Verbosity.Normal));
	});

Task("Default")
	.IsDependentOn("Build-Android")
	.IsDependentOn("Build-iOS")
	.IsDependentOn("Run-Tests");

RunTarget(target);