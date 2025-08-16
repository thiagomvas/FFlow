using System.Text.Json;
using FFlow;
using FFlow.Core;
using FFlow.Demo;
using FFlow.Extensions;
using FFlow.Steps.SFTP;
using FFlow.Steps.DotNet;
using FFlow.Steps.Git;
using FFlow.Steps.Http;

var builder = new FFlowBuilder()
    .GitClone("https://github.com/thiagomvas/fflow.git", "/home/thiagomv/testfflow/")
    .Then(() =>
    {
        var generatedFolder = Path.Combine(Directory.GetCurrentDirectory(), "fflow"); 
        if (Directory.Exists(generatedFolder))
        {
            Directory.Delete(generatedFolder, recursive: true);
            Console.WriteLine($"Deleted folder: {generatedFolder}");
        }
    });

await builder.Build().RunAsync();


