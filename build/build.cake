var target = Argument("target", "Default");

Task("Default")
.IsDependentOn("Clean")
  .Does(() =>
{
  
});

Task("Clean")
  .Does(() =>
{
  Information("Cleaning directories...");

  CleanDirectories("../Reinforced.Typings*/**/bin");
  CleanDirectories("../Reinforced.Typings*/**/obj");

  Information("Clean completed");
});

RunTarget(target);