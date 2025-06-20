# Getting Started

To get started with FFlow, follow these steps:
1. **Install the FFlow NuGet package**:
   You can install FFlow via NuGet Package Manager or by using the .NET CLI:
   ```bash
   dotnet add package FFlow
   ```
2. **Create a new step**:
   Define a step by implementing the `IFlowStep` interface. For example:
   ```csharp
    public class HelloStep : IFlowStep
    {
        public Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
        {
            var input = context.GetInput<string>();
            Console.WriteLine($"Hello, {input}!");
            return Task.CompletedTask;
        }
    }
   ```

3. **Build a workflow**:
    Use the `FFlowBuilder` to create a workflow and add steps:
    ```csharp
    var workflow = new FFlowBuilder()
         .StartWith<HelloStep>()
         .Build();
    ```

4. **Run the workflow**:
    Execute the workflow using the `RunAsync` method and passing the input:
    ```csharp
    await workflow.RunAsync("John Doe");
    ```

The output will be:
```
Hello, John Doe!
```