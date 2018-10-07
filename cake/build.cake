var target = Argument("target", "Build");

Task("Clean")
  .Does(() =>
{
  CleanDirectories("../Reinforced.Typings*/**/bin");
  CleanDirectories("../Reinforced.Typings*/**/obj");

  Information("Clean completed");
});
Task("PackageClean")
  .Does(() =>
{
  CleanDirectories("../package"); 

  Information("PackageClean completed");
});

var buildSettings = new DotNetCoreBuildSettings
{
    // Framework = "netcoreapp2.0",
    Verbosity = DotNetCoreVerbosity.Minimal,
    Configuration = "Release"
};

Task("Build")
  .IsDependentOn("Clean")
  .IsDependentOn("PackageClean")
  .Does(() =>
{
  NuGetRestore("../Reinforced.Typings.Integrate/Reinforced.Typings.Integrate.NETCore.csproj");
  DotNetCoreBuild("../Reinforced.Typings.Integrate/Reinforced.Typings.Integrate.NETCore.csproj", buildSettings);

  Information("Build completed");
});

RunTarget(target);