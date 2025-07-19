using FFlow;
using FFlow.Steps.SFTP;

var registry = new StepTemplateRegistry();
var flow = new FFlowBuilder(null, registry)
    .ConnectToSftp("localhost", 2222, "user", "password")
    .DownloadFileViaSftp("upload/testfile.txt", "/home/thiagomv/sftptest/downloaded_via_sftp.txt")
    .Build();

var ctx = await flow.RunAsync();

