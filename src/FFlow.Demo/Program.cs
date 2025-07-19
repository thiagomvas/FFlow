using FFlow;
using FFlow.Steps.SFTP;

var registry = new StepTemplateRegistry();
var flow = new FFlowBuilder(null, registry)
    .ConnectToSftp("localhost", 2222, "user", "password")
    .UploadFileToSftp("/home/thiagomv/Test.txt", "/upload/testfile.txt")
    .Build();

var ctx = await flow.RunAsync();

