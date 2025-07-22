using FFlow;
using FFlow.Core;
using FFlow.Demo;
using FFlow.Extensions;
using FFlow.Steps.SFTP;
using FFlow.Steps.DotNet;

var builder = new FFlowBuilder()
        .StartWith<HelloStep>().Then<HelloStep>().Then<HelloStep>()
        .Switch(cb =>
        {
                cb.Case("Foo", _ => false).Then<HelloStep>().Then<HelloStep>();
                cb.Case("Bar", _ => false).Then<HelloStep>().Then<HelloStep>();
                cb.Case("Fizz", _ => false).Then<HelloStep>().Then<HelloStep>();
                cb.Case("Buzz", _ => false).Then<HelloStep>().Then<HelloStep>();
        })
        .Then<HelloStep>();

Console.WriteLine(builder.Describe().ToMermaid());


