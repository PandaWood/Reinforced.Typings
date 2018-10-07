var target = Argument("target", "Build");

Task("Clean")
  .Does(() =>
{
  CleanDirectories("../Reinforced.Typings*/**/bin");
  CleanDirectories("../Reinforced.Typings*/**/obj");
});
Task("PackageClean")
  .Does(() =>
{
  CleanDirectories("../package"); 
  Information("PackageClean completed");
});

Task("Build")
  .IsDependentOn("Clean")
  .IsDependentOn("PackageClean")
  .Does(() =>
{
  
  // cd package
  Information("Build completed");
});

RunTarget(target);