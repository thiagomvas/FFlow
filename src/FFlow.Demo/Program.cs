using FFlow;
using FFlow.Extensions;
using FFlow.Steps.SFTP;
using FFlow.Steps.DotNet;

var builder = (FFlowBuilder)new FFlowBuilder()
    .ConnectToSftp("localhost", 2222, "user", "password")
    .UploadFileViaSftp("/home/thiagomv/memhog.c", "upload/memhog.c")
    .ThrowIf(_ => false, "This is a test error");

Console.WriteLine(builder.Describe().ToMermaid());

await builder.Build().RunAsync();

