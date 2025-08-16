using System.Text.Json;
using FFlow;
using FFlow.Core;
using FFlow.Demo;
using FFlow.Extensions;
using FFlow.Steps.SFTP;
using FFlow.Steps.DotNet;
using FFlow.Steps.Git;
using FFlow.Steps.Http;

var tempPath = Path.Combine(Path.GetTempPath(), "fflow-demo");
var builder = new FFlowBuilder()
    .Then<GitCloneStep>(step =>
    {
        step.RepositoryUrl = "https://github.com/thiagomvas/fflow.git";
        step.LocalPath = tempPath;
    })
    // Log how many files are in the cloned repository
    .Then(() =>
    {
        Console.WriteLine($"Cloning repository to {tempPath}");
        var files = Directory.GetFiles(tempPath, "*", SearchOption.AllDirectories);
        Console.WriteLine($"Cloned repository contains {files.Length} files.");
    });

await builder.Build().RunAsync();


