using System.Text.Json;
using FFlow;
using FFlow.Core;
using FFlow.Demo;
using FFlow.Extensions;
using FFlow.Steps.SFTP;
using FFlow.Steps.DotNet;
using FFlow.Steps.FileIO;
using FFlow.Steps.Http;

await new FFlowBuilder()
    .FileChecksum("/home/thiagomv/Src/fflow/README.md", ChecksumStep.ChecksumAlgorithm.SHA256, 
        "7346eea0c4e5fc63bb120d49768be219b05ed096dbe070717e23ad854c35e6cb")
    .Build().RunAsync();


