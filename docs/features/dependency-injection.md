---
title: Dependency Injection
description: Learn how to inject services into FFlow steps using property and constructor injection with the built-in .NET DI container.
---

# Dependency Injection

FFlow supports multiple ways to inject dependencies into your workflows and steps using standard .NET dependency injection (DI). This allows you to fully integrate application services like logging, configuration, database access, and more into your automation flows.

> [!NOTE]
> To properly use dependency injection in FFlow, use the `FFlow.Extensions.Microsoft.DependencyInjection` package, which provides the necessary extensions to register and resolve services from the built-in .NET DI container.

Consider the following service definition:

---

## Sample Service

```csharp
public interface IMessageService
{
    string GetMessage();
}

public class MessageService : IMessageService
{
    public string GetMessage() => "Hello from DI!";
}

public class DiStep : IFlowStep
{
    private readonly IMessageService _messageService;

    // Constructor injection
    public DiStep(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public async Task ExecuteAsync(IFlowContext context)
    {
        // Use the injected service
        var message = _messageService.GetMessage();
        Console.WriteLine(message);
        await Task.CompletedTask; // Simulate async work
    }
}
```

## Registering Services
Register your services in the DI container as usual:

```csharp
var services = new ServiceCollection();
services.AddFlow(); // Registers any IFlowSteps and IWorkflowDefinitions
services.AddSingleton<IMessageService, MessageService>();
var serviceProvider = services.BuildServiceProvider();
```
## Injecting Services into Steps
You can inject services into your steps using property injection or constructor injection. For DI to work, you need to use the `FFlowBuilder` which resolves dependencies from the provided `IServiceProvider`.

> [!WARNING]
> Constructor DI only works if the steps are defined types (e.g. `DiStep`) and not steps defined by lambdas.

```csharp
var workflow = new FFlowBuilder(serviceProvider)
    .StartWith<DiStep>()
    .Then<AnotherStep>()
    .Build();
```

### Using Workflow Definitions with Constructor Injection

You can also define workflows using the `IWorkflowDefinition` interface, which allows you to encapsulate the workflow logic and dependencies in a reusable manner. Here's how you can do it:

```csharp
public class HelloWorkflow : IWorkflowDefinition
{
    private readonly IServiceProvider _serviceProvider;
    public HelloWorkflow(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }
    public IWorkflow Build()
    {
        return new FFlowBuilder(_serviceProvider)
            .StartWith<DiStep>()
            .Build();
    }
}
```