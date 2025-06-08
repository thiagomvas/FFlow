using FFlow;
using FFlow.Demo;

var flow = new FFlowBuilder<object?>()
    .StartWith(ctx => Task.Run(() => throw new ArgumentException("Simulated exception")))
    .OnAnyError<HelloStep>()
    .Build();

await flow.RunAsync(null);
    