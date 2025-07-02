# .NET CLI Integration
FFlow provides a package for integrating with .NET CLI commands, allowing you to run .NET commands as part of your workflow. This is particularly useful for building, testing, and deploying .NET applications.

To use the .NET CLI extensions and steps, you need to install the `FFlow.Steps.Dotnet` package:

```bash
dotnet add package FFlow.Steps.Dotnet
```

Each `Dotnet___Step` defines it's output in the context as a `Dotnet___Result` (e.g. `DotnetPackStep` produces a `DotnetPackResult` obtainable through IFlowContext.GetOutputFor<DotnetPackStep, DotnetPackResult>()). 

All of the steps also have an extension method for `IFlowBuilder` to add the step directly to the workflow, with relevant overloads for simplicity.

The package includes the following commands and overloads:
- `DotnetBuildStep` with `DotnetBuild` extension method
- `DotnetTestStep` with `DotnetTest` extension method
- `DotnetPackStep` with `DotnetPack` extension method
- `DotnetPublishStep` with `DotnetPublish` extension method
- `DotnetRestoreStep` with `DotnetRestore` extension method
- `DotnetRunStep` with `DotnetRun` extension method
- `DotnetNugetPushStep` with `DotnetNugetPush` extension method

### Example Usage
```csharp
builder.StartWith<SomeSetupStep>()
    .DotnetBuild("path/to/my/project")
    .DotnetTest("path/to/my/project", step => step.NoBuild = true)
    .Finally((ctx, ct) => {
        var result = ctx.GetOutputFor<DotnetTestStep, DotnetTestResult>();
        Console.WriteLine($"Passed: {result.Passed}, Failed: {result.Failed}, Skipped: {result.Skipped}");
    });
```

