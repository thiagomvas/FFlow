using FFlow;
using FFlow.Steps.SFTP;

var registry = new StepTemplateRegistry();
var flow = new FFlowBuilder(null, registry)
    .ConnectToSftp("localhost", 2222, "user", "password")
    .UploadDirectoryViaSftp("/home/thiagomv/Src/temp", "upload/something")
    .Build();

var ctx = await flow.RunAsync();

