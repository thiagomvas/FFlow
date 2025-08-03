using FFlow;
using FFlow.Core;
using FFlow.Demo;
using FFlow.Extensions;
using FFlow.Steps.SFTP;
using FFlow.Steps.DotNet;

var builder = new FFlowBuilder()
    .DotnetBuild(".");

Console.WriteLine(builder.Describe().ToDot());


