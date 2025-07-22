using FFlow;
using FFlow.Core;
using FFlow.Demo;
using FFlow.Extensions;
using FFlow.Steps.SFTP;
using FFlow.Steps.DotNet;

var builder = new FFlowBuilder()
        .StartWith<HelloStep>().Then<NoOpStep>().Then<GoodByeStep>();

Console.WriteLine(builder.Describe().ToMermaid());


