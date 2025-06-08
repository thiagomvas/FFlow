using FFlow;
using FFlow.Demo;

var flow = new FFlowBuilder<object?>()
    .StartWith<HelloStep>()
    .Then(ctx => Task.Run(() => Console.WriteLine("This is a delegate step.")))
    .Build();

await flow.RunAsync(null);
    