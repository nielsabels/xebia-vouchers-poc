#addin "Cake.Docker"
var target = Argument("target", "Default");
var solutionFile = GetFiles("**/*.sln").First().ToString();
var verbosity = Argument<DotNetCoreVerbosity>("verbosity", DotNetCoreVerbosity.Quiet);
var dotNetCoreVerbosity = verbosity;
var configuration = Argument("configuration", "Release");
var testUnitProjectPattern = "./**/*.Tests.Unit.csproj";
var testAcceptanceProjectPattern = "./**/*.Tests.Acceptance.csproj";
var sourceDir = Directory("./");
var artifactsDir = Directory("./artifacts");

var publishDir = Directory("./publish/");
var publishProjects = new []
{
    sourceDir.Path + "Xebia.Vouchers.API/Xebia.Vouchers.API.csproj"
};

var dockerImageName = "nabels/xebia-voucher-api";
var dockerContainerName = "xebia-voucher-api";
var dockerContainerId = "UNKNOWN";

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

Task("Test-Acceptance")
  .Does(() => {
    RunTests(testAcceptanceProjectPattern);
});

Task("Publish")
  .Does(() => {
    foreach(var project in publishProjects)
    {
        var projectName =  project
            .Split(new [] {'/'}, StringSplitOptions.RemoveEmptyEntries)
            .Last()
            .Replace(".csproj", string.Empty);

        var outputDirectory = System.IO.Path.Combine(publishDir, projectName);

        var msBuildSettings = new DotNetCoreMSBuildSettings 
        {
            TreatAllWarningsAs = MSBuildTreatAllWarningsAs.Error,
            Verbosity = dotNetCoreVerbosity
        };

        var settings = new DotNetCorePublishSettings
        {
            Configuration = configuration,
            MSBuildSettings = msBuildSettings,
            NoRestore = true,
            OutputDirectory = outputDirectory,
            Verbosity = dotNetCoreVerbosity
        };

        Information("Publishing '{0}'...", projectName);
        DotNetCorePublish(project, settings); 
        Information("'{0}' has been published.", projectName);
    }
});

Task("Create-Container")
	.Description("Creates a Docker container for the Vouchers API.")
	.Does(() => 
    {
        string outputDirectory = System.IO.Path.Combine(publishDir, "Xebia.Vouchers.API");

        var settings = new DockerImageBuildSettings
        {
            Tag = new[] { dockerImageName },
            ForceRm = true,
            Pull = true
        };
        
        Information("Building the container '{0}'.", dockerImageName);
        DockerBuild(settings, outputDirectory);
        Information("Container '{0}' has been built.", dockerImageName);
    });

Task("Start-Container")
	.Description("Starts a Docker container for the Vouchers API.")
	.Does(() => 
    {
        Information("Starting the container '{0}'.", dockerImageName);
        dockerContainerId = DockerCreate(new DockerContainerCreateSettings() { Name = dockerContainerName}, dockerImageName, "");
        Information("Container '{0}' started ({1}).", dockerImageName, dockerContainerId);
    });

Task("Stop-Container")
	.Description("Starts a Docker container for the Vouchers API.")
	.Does(() => 
    {
        Information("Stopping the container '{0} ({1})'.", dockerImageName, dockerContainerId);
        DockerStop(new DockerContainerStopSettings(), new[] {dockerContainerId});
        Information("Container '{0}' stopped.", dockerImageName);
    });


Task("Remove-Container")
	.Description("Removes the Docker container image.")
	.Does(() => 
    {
        Information("Removing the container '{0}'.", dockerContainerId);
        DockerRm(new [] {dockerContainerId});
        Information("Container '{0}' has been removed.", dockerContainerId);
    });    

Task("Remove-Container-Image")
	.Description("Removes the Docker container image.")
	.Does(() => 
    {
        Information("Removing the container image '{0}'.", dockerImageName);
        DockerRmi(dockerImageName);
        Information("Container image '{0}' has been removed.", dockerImageName);
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

Task("Test-Local")
    .Description("Builds the solution, runs test targets, deploys the API and runs acceptance tests.")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("Build")
    .IsDependentOn("Test-Unit")
    .IsDependentOn("Publish")
    // .IsDependentOn("Pack")
    // .IsDependentOn("Deploy-Local")
    .IsDependentOn("Create-Container")
    .IsDependentOn("Start-Container")
    .IsDependentOn("Stop-Container")
    .IsDependentOn("Remove-Container")
    
    // .IsDependentOn("Create-Network")
    .IsDependentOn("Test-Acceptance")
    // .IsDependentOn("Remove-Network")
    .IsDependentOn("Remove-Container-Image");

Task("Default")
	.IsDependentOn("Clean")
	.IsDependentOn("Restore")
	.IsDependentOn("Build")
	.IsDependentOn("Test-Unit");

RunTarget(target);
