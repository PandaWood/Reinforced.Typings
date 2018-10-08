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

const string RELEASE = "Release";
const string NETCORE21 = "netcoreapp2.1";
const string NETCORE20 = "netcoreapp2.0";
const string NETCORE1 = "netcoreapp1.0";
const string NET461 = "net461";
const string NET45 = "net45";

const string CliNetCoreProject = "../Reinforced.Typings.Cli/Reinforced.Typings.Cli.NETCore.csproj";

const string toolsPath = "../package/tools";
const string contentPath = "../package/content";
const string buildPath = "../package/build";
const string multiTargetPath = "../package/buildMultiTargeting";
const string libPath = "../package/lib";
const string buildNet45 = "../package/build/net45";
const string buildNet16 = "../package/build/netstandard1.6";

Task("Build")
  .IsDependentOn("Clean")
  .IsDependentOn("PackageClean")
  .Does(() =>
{
  // Mono gives me error for this -> MSB3644: The reference assemblies for framework ".NETFramework,Version=v4.5" were not found
  DotNetCoreBuild("../Reinforced.Typings.Integrate/Reinforced.Typings.Integrate.NETCore.csproj", new DotNetCoreBuildSettings
  {
    Verbosity = DotNetCoreVerbosity.Minimal,
    Configuration = "Release",
  });
  
  DotNetCorePublish(CliNetCoreProject, new DotNetCorePublishSettings {  
    Configuration = RELEASE, 
    Framework = NETCORE21,
    OutputDirectory = System.IO.Path.Combine(toolsPath, NETCORE21)
  });
  DotNetCorePublish(CliNetCoreProject, new DotNetCorePublishSettings {  
    Configuration = RELEASE, 
    Framework = NETCORE20,
    OutputDirectory = System.IO.Path.Combine(toolsPath, NETCORE20)
  });
  DotNetCorePublish(CliNetCoreProject, new DotNetCorePublishSettings {  
    Configuration = RELEASE, 
    Framework = NETCORE1,
    OutputDirectory = System.IO.Path.Combine(toolsPath, NETCORE1)
  });
  DotNetCorePublish(CliNetCoreProject, new DotNetCorePublishSettings {  
    Configuration = RELEASE, 
    Framework = NET461,
    OutputDirectory = System.IO.Path.Combine(toolsPath, NET461)
  });
  DotNetCorePublish(CliNetCoreProject, new DotNetCorePublishSettings {  
    Configuration = RELEASE, 
    Framework = NET45,
    OutputDirectory = System.IO.Path.Combine(toolsPath, NET45)
  });

  EnsureDirectoryExists(contentPath);
  EnsureDirectoryExists(buildPath);
  EnsureDirectoryExists(multiTargetPath);
  EnsureDirectoryExists(libPath);
  EnsureDirectoryExists(buildNet45);
  EnsureDirectoryExists(buildNet16);

  CopyFileToDirectory("../xmls/Reinforced.Typings.settings.xml", contentPath);
  CopyFileToDirectory("../xmls/Reinforced.Typings.targets", buildPath);
  CopyFileToDirectory("../xmls/Reinforced.Typings.Multi.targets", multiTargetPath);
  CopyFileToDirectory("../xmls/Reinforced.Typings.props", buildPath);
  CopyFileToDirectory("../xmls/Reinforced.Typings.props", multiTargetPath);
  CopyFiles("../Reinforced.Typings/bin/Release/*.*", libPath);
  CopyFileToDirectory("../Reinforced.Typings.Integrate/bin/Release/net45/Reinforced.Typings.Integrate.dll", buildNet45);
  CopyFiles("../Reinforced.Typings.Integrate/bin/Release/netstandard1.6/*.*", buildNet16);

  NuGetPack("../package/Reinforced.Typings.nuspec", new NuGetPackSettings {
    BasePath = "../package"
  });

  Information("Build completed");
});

RunTarget(target);