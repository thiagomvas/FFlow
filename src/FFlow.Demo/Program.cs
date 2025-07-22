using FFlow;
using FFlow.Core;
using FFlow.Demo;
using FFlow.Extensions;
using FFlow.Steps.SFTP;
using FFlow.Steps.DotNet;

var builder = new FFlowBuilder()
        .StartWith<HelloStep>().Then<HelloStep>().Then<HelloStep>()
        .Input<HelloStep, string>(step => step.Name, "World")
        .Delay(1000)
        .Then<GoodByeStep>()
        .Input<GoodByeStep, string>(step => step.Name, "World")
        .If(ctx => true, (_, _) => Console.WriteLine("Condition is true!"),
            (_, _) => Console.WriteLine("Condition is false!"))
        .Fork(ForkStrategy.FireAndForget,
            () => new FFlowBuilder().LogToConsole("First"),
            () => new FFlowBuilder().LogToConsole("Second"))
        .ForEach<HelloStep, string>(ctx => ["Hello", "World"])
        .ThrowIf<ArgumentOutOfRangeException>(_ => false, "Foobar")
        .Then<HelloStep>();

Console.WriteLine(builder.Describe().ToMermaid());


