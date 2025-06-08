using FFlow;
using FFlow.Core;
using FFlow.Demo;
using FFlow.Extensions.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

var workflow = new FFlowBuilder()
    .StartWith<HelloStep>()
    .ForEach(ctx => ctx.GetInput<Person>().Friends, () =>
    {
        return new FFlowBuilder().StartWith<HelloStep>();
    })
    .Finally<GoodByeStep>()
    .Build();

var person = new Person(1, [1,2,3,4,5]);
await workflow.RunAsync(person, new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);

record Person(int Id, int[] Friends);