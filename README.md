<h1 align="center">
  FFlow - Code first automations built fluently
  <br>
  <a href="https://github.com/thiagomvas/FFlow/actions/workflows/ci-tests.yml">
    <img src="https://github.com/thiagomvas/FFlow/actions/workflows/ci-tests.yml/badge.svg">
  </a>

  <a href="https://github.com/thiagomvas/FFlow/actions/workflows/cicd.yml">
    <img src="https://github.com/thiagomvas/FFlow/actions/workflows/cicd.yml/badge.svg">
  </a>
  <a href="https://thiagomvas.dev/FFlow/">
    <img src="https://img.shields.io/badge/Docs-Available-limegreen?style=flat&logo=github">
  </a>
  <a href="https://github.com/thiagomvas/fflow/labels/good%20first%20issue">
    <img src="https://img.shields.io/github/issues/thiagomvas/fflow/good%20first%20issue?style=flat&color=%24EC820&label=good%20first%20issue">
  </a>
  <a href="https://github.com/thiagomvas/fflow/labels/help%20wanted">
    <img src="https://img.shields.io/github/issues/thiagomvas/fflow/help%20wanted?style=flat&color=%24EC820&label=help%20wanted">
  </a>
</h1>
<p align="center">
  <b>FFlow</b> is a fluent, code-first workflow automation library for .NET. It enables developers to orchestrate automation logic, business rules, and CI/CD pipelines in a <b>fully testable</b> and <b>extensible</b> way.
</p>

## Table of Contents
- [Table of Contents](#table-of-contents)
- [Installation](#installation)
- [Quickstart](#quickstart)
- [Features at a glance](#features-at-a-glance)
- [Why it exists](#why-it-exists)
- [Using with Dependency Injection](#using-with-dependency-injection)
- [Testing](#testing)
- [Contributing](#contributing)
- [License](#license)

## Installation
You can install FFlow via the Nuget Package Manager or by using the .NET CLI
```bash
dotnet add package FFlow
```

## Quickstart
Getting started with FFlow is easy. Here's an example of how to create a custom step that says 'Hello, FFlow!'. This example includes building a workflow with the builder, configuring it and executing it with an input.
```csharp
 public class HelloStep : FlowStep
 {
     protected override Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken = default)
     {
         var input = context.GetInput<string>();
         Console.WriteLine($"Hello, {input}!");
         return Task.CompletedTask;
     }
 }

var workflow = new FFlowBuilder()
    .StartWith<HelloStep>()
    .Build();

await workflow.RunAsync("FFlow");
```

## Features at a glance
- **Fluent syntax for flow control:** `StartWith()`, `Then()`, `Finally()`, `If()`, `Fork()`, and more
- **Dependency Injection support:** Inject services into steps via constructor injection
- **Validation utilities:** Write step context validation attributes or steps with the help of `FFlow.Validation`
- **Branching and parallelism:** Run parts of the workflow concurrently with different dispatch strategies
- **Context-aware:** Pass and retrieve dynamic data throughout the workflow
- **Reusable definitions:** Encapsulate workflows into a `IWorkflowDefinition` for factory-like behaviours
- **Lifecycle hooks:** Plug into events like step start, step failure or workflow completion.

## Why it exists
Writing and testing CI/CD pipelines has always been frustrating. It usually went from "waiting to compile" to "waiting for CI/CD", just to realize you missed something, fix it, and rerun the whole thing again. And again.

The feedback loop was too long. Small mistakes led to wasted time, and workflows often lived outside the codebase in YAML files or GUI editors that were hard to test, debug, or reuse.

FFlow was born out of this frustration. **It came from the idea that automation should feel like regular code.** Something you can write fluently, test locally, and plug into your existing services just like anything else in your app.

Tools like `Cake` or `Nuke` solve part of the problem, but I wanted something more structured and flexible. Less about running scripts. More about building flows.

## Using with Dependency Injection
FFlow integrates cleanly with Microsoft.Extensions.DependencyInjection. To enable DI:
```csharp
var services = new ServiceCollection();
services.AddFlow(); // Registers IFlowSteps and IWorkflowDefinitions
services.AddSingleton<IMyService, MyService>();

var provider = services.BuildServiceProvider();

var workflow = new FFlowBuilder(provider)
    .StartWith<StepThatUsesIMyService>()
    .Build();
```
Steps can receive services through construction injection:
```csharp
public class StepThatUsesIMyService : FlowStep
{
    private readonly IMyService _service;

    public StepThatUsesIMyService(IMyService service)
    {
        _service = service;
    }

    protected override Task ExecuteAsync(IFlowContext context, CancellationToken ct)
    {
        var result = _service.DoSomething();
        return Task.CompletedTask;
    }
}
```

## Testing
FFlow includes unit tests covering key features such as step execution, context flow, branching, validation, and DI integration. All you need to do is run
```
dotnet test
```

## Contributing

Contributions to FFlow are welcome and appreciated. Whether it’s fixing a bug, suggesting an improvement, writing documentation, or proposing a new feature.

If you’ve worked with automation, DevOps, or pipeline tools and thought “this could be easier in code”, you’re in the right place. FFlow is still growing, and there’s a lot of room to help shape what it becomes.

You don’t need to understand the entire codebase to contribute. Most improvements are isolated and straightforward. It's designs allow you to add new steps, small helpers, validation logic, or workflow patterns without knowing everything about the project.

If you have an idea or just want to help, feel free to open an issue, start a discussion, or jump into the code.

## License

FFlow is **free software** and **always will be**, released under the MIT License.  
See the [LICENSE](./LICENSE) file for more details.
