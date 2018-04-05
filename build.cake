#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "default").ToLower();
var solution = "";

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("init")
    .Does(() =>
{
    switch (Context.Environment.Platform.Family)
    {
        case PlatformFamily.Linux:
            solution = "src/Pong.Linux.sln";
            break;
        case PlatformFamily.OSX:
            solution = "src/Pong.MacOS.sln";
            break;
        default:
            solution = "src/Pong.Windows.sln";
            break;
    }
});

Task("clean")
    .IsDependentOn("init")
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
    DotNetCoreRestore(solution);
});

Task("build")
    .IsDependentOn("prepare")
    .Does(() =>
{
    System.Environment.SetEnvironmentVariable("MONO_LOG_LEVEL", "debug");
    DotNetCoreBuild(solution, new DotNetCoreBuildSettings {
        Verbosity = DotNetCoreVerbosity.Minimal,
        Configuration = "Release"
    });
});

Task("publish")
    .IsDependentOn("build")
    .Does(() =>
{
    var rids = new string[0];

    switch (Context.Environment.Platform.Family)
    {
        case PlatformFamily.Linux:
            rids = new[] { "linux-x64" };
            break;
        case PlatformFamily.OSX:
            rids = new[] { "osx-x64" };
            break;
        default:
            rids = new[] { "win-x86", "win-x64" };
            break;
    }

    foreach (var rid in rids)
    {
        DotNetCorePublish(solution, new DotNetCorePublishSettings {
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
