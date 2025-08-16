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
    .GitClone("https://github.com/thiagomvas/fflow.git");

await builder.Build().RunAsync();


