using FFlow;
using FFlow.Steps.SFTP;

var registry = new StepTemplateRegistry();
var flow = new FFlowBuilder(null, registry)
    .ConnectToSftp("localhost", 2222, "user", "password")
    .DownloadDirectoryViaSftp("upload", "/home/thiagomv/sftp_upload")
    .Build();

var ctx = await flow.RunAsync();

