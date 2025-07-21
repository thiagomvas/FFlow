using FFlow;
using FFlow.Steps.SFTP;
using FFlow.Steps.DotNet;

var registry = new StepTemplateRegistry();
var flow = new FFlowBuilder(null, registry)
    .ConnectToSftp("localhost", 2222, "user", "password")
    .DownloadDirectoryViaSftp("upload", "/home/thiagomv/sftp_upload")
    .DotnetBuild(".")
    .Then((ctx, ct) => ctx.GetDotnetBuildOutput())
    .Build();

var ctx = await flow.RunAsync();

