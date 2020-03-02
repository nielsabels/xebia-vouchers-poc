var target = Argument("target", "Default");
var solutionFile = GetFiles("**/*.sln").First().ToString();
var verbosity = Argument<DotNetCoreVerbosity>("verbosity", DotNetCoreVerbosity.Quiet);
var configuration = Argument("configuration", "Release");
var testUnitProjectPattern = "./**/*.Tests.Unit.csproj";

Task("Clean")
  .Does(() => {
	var settings = new DotNetCoreCleanSettings
	{
		Configuration = configuration,
		Verbosity = verbosity
	};

	DotNetCoreClean(solutionFile, settings);
});

Task("Restore")
  .Does(() => {    
    DotNetCoreRestore(solutionFile);
});

Task("Build")
  .Does(() => {	
	var settings = new DotNetCoreBuildSettings
	{
		Configuration = configuration,
		Verbosity = verbosity,
		NoRestore = true,
	};

	DotNetCoreBuild(solutionFile, settings);
});

Task("Test-Unit")
  .Does(() => {
    RunTests(testUnitProjectPattern);
});

void RunTests(string projectGlob)
{
  var settings = new DotNetCoreTestSettings
    {
      Configuration = configuration,
      Verbosity = verbosity,
      NoRestore = true,
      NoBuild = true,
      Logger = "console;verbosity=normal",
      ArgumentCustomization = args => 
        args
        .Append("/p:CollectCoverage=true")
        .Append("/p:CoverletOutputFormat=cobertura")
		.Append("/p:Exclude=[xunit.*]*")
    };

    var testProjects = GetFiles(projectGlob);

    foreach(var project in testProjects)
    {
      DotNetCoreTest(project.ToString(), settings);
    }
}

Task("Default")
	.IsDependentOn("Clean")
	.IsDependentOn("Restore")
	.IsDependentOn("Build")
	.IsDependentOn("Test-Unit");

RunTarget(target);
