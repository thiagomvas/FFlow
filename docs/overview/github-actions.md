# Using FFlow with GitHub Actions
With the new [file-based C# apps from .NET 10](https://devblogs.microsoft.com/dotnet/announcing-dotnet-run-app/), you can run flow scripts directly from a C# file. This makes it extremelly easy to use FFlow in your GitHub Actions workflows.

> [!NOTE]
> If you do not want to use .NET 10, you can still use FFlow in your GitHub Actions workflows by running standard project-based console apps.

## Setting up the pipeline
To set up a simple pipeline, configure a workflow like you would any other FFlow workflow.

```csharp
#:package FFlow@1.* // .NET 10 file-based app

using FFlow;

await new FFlowBuilder()
    .StartWith((ctx, _) => Console.WriteLine("Starting FFlow workflow..."))
    // Add your workflow steps here
    .Build()
    .RunAsync();
```

## Running it with GitHub Actions
To run the above workflow in GitHub Actions, you can create a `.github/workflows/my-pipeline.yml` file in your repository with the following content:

```yaml
name: FFlow Pipeline
on:
  push:
    branches:
      - main

jobs:
  my-job:
    runs-on: ubuntu-latest

    steps:
      - name: Checkout code
        uses: actions/checkout@v3

      - name: Setup .NET 10
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '10.0.x'
          dotnet-quality: 'preview' # Use 'preview' for .NET 10

      - name: Run FFlow script
        run: dotnet run path/to/my/pipeline.cs --verbosity quiet
```

Configure any other dependencies like other .NET versions (in case you use Dotnet steps with a different sdk version) in the .yml file as needed.

If you want more examples, the [official repository](https://github.com/thiagomvas/FFlow) uses FFlow pipelines for testing and deploying to nuget. The scripts are saved in the `pipelines/` folder.
