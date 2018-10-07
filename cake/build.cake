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

const string BUILD_OUTPUT_DIR = "../package/tools";
const string RELEASE = "Release";
const string NETCORE21 = "netcoreapp2.1";
const string NETCORE20 = "netcoreapp2.0";
const string NETCORE1 = "netcoreapp1.0";
const string NET461 = "net461";
const string NET45 = "net45";

const string CliNetCoreProject = "../Reinforced.Typings.Cli/Reinforced.Typings.Cli.NETCore.csproj";

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
    OutputDirectory = System.IO.Path.Combine(BUILD_OUTPUT_DIR, NETCORE21)
  });
  DotNetCorePublish(CliNetCoreProject, new DotNetCorePublishSettings {  
    Configuration = RELEASE, 
    Framework = NETCORE20,
    OutputDirectory = System.IO.Path.Combine(BUILD_OUTPUT_DIR, NETCORE20)
  });
  DotNetCorePublish(CliNetCoreProject, new DotNetCorePublishSettings {  
    Configuration = RELEASE, 
    Framework = NETCORE1,
    OutputDirectory = System.IO.Path.Combine(BUILD_OUTPUT_DIR, NETCORE1)
  });
  DotNetCorePublish(CliNetCoreProject, new DotNetCorePublishSettings {  
    Configuration = RELEASE, 
    Framework = NET461,
    OutputDirectory = System.IO.Path.Combine(BUILD_OUTPUT_DIR, NET461)
  });
  DotNetCorePublish(CliNetCoreProject, new DotNetCorePublishSettings {  
    Configuration = RELEASE, 
    Framework = NET45,
    OutputDirectory = System.IO.Path.Combine(BUILD_OUTPUT_DIR, NET45)
  });

  // need to test/check these cake commands create the destination if it doesn't exist
  CopyFileToDirectory("../xmls/Reinforced.Typings.settings.xml", "package/content");
  CopyFileToDirectory("../xmls/Reinforced.Typings.targets", "package/build");
  CopyFileToDirectory("../xmls/Reinforced.Typings.Multi.targets", "package/buildMultiTargeting");
  CopyFileToDirectory("../xmls/Reinforced.Typings.props", "package/build");
  CopyFileToDirectory("../xmls/Reinforced.Typings.props", "package/buildMultiTargeting");
  CopyFiles("../Reinforced.Typings/bin/Release/*.*", "package/lib");
  CopyFileToDirectory("../Reinforced.Typings.Integrate/bin/Release/net45/Reinforced.Typings.Integrate.dll", "package/build/net45");
  CopyFiles("../Reinforced.Typings.Integrate/bin/Release/netstandard1.6/*.*", "package/build/netstandard1.6");

  Pack("../package/Reinforced.Typings.nuspec", new NuGetPackSettings {
    BasePath = "../package"
  })

  // original file copy operations for reference (since we need to emulate the xcopy options)
  // xcopy xmls\Reinforced.Typings.settings.xml package\content\ /I /Y
  // xcopy xmls\Reinforced.Typings.targets package\build\ /I /Y
  // xcopy xmls\Reinforced.Typings.Multi.targets package\buildMultiTargeting\ /I /Y
  // xcopy xmls\Reinforced.Typings.props package\build\ /I /Y
  // xcopy xmls\Reinforced.Typings.props package\buildMultiTargeting\ /I /Y
  // xcopy Reinforced.Typings\bin\Release\*.* package\lib\ /E /I /Y
  // xcopy Reinforced.Typings.Integrate\bin\Release\net45\Reinforced.Typings.Integrate.dll package\build\net45\ /I /Y
  // xcopy Reinforced.Typings.Integrate\bin\Release\netstandard1.6\*.* package\build\netstandard1.6\ /I /Y
  // nuget pack package\Reinforced.Typings.nuspec -BasePath package

  Information("Build completed");
});

RunTarget(target);