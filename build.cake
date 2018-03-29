#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "default").ToLower();

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("clean")
    .Does(() =>
{
    if (DirectoryExists("bin"))
    {
        DeleteDirectory("bin",  new DeleteDirectorySettings {
            Recursive = true,
            Force = true
        });
    }
});

Task("prepare")
    .IsDependentOn("clean")
    .Does(() =>
{
    DotNetCoreRestore("Pong.sln");
});

Task("build")
    .IsDependentOn("prepare")
    .Does(() =>
{
    System.Environment.SetEnvironmentVariable("MONO_LOG_LEVEL", "debug");
    DotNetCoreBuild("Pong.sln", new DotNetCoreBuildSettings {
        Verbosity = DotNetCoreVerbosity.Minimal,
        Configuration = "Release"
    });
});

Task("publish")
    .IsDependentOn("build")
    .Does(() =>
{
    var rids = new string[0];
    var platform = Context.Environment.Platform.Family;

    if (platform == PlatformFamily.Linux)
        rids = new[] { "linux-x64" };
    else if (platform == PlatformFamily.OSX)
        rids = new[] { "osx-x64" };
    else
        rids = new[] { "win-x86", "win-x64" };

    foreach (var rid in rids)
    {
        DotNetCorePublish("Pong.sln", new DotNetCorePublishSettings {
            Verbosity = DotNetCoreVerbosity.Minimal,
            Configuration = "Release",
            Runtime = rid
        });
    }
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("default")
    .IsDependentOn("publish");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
