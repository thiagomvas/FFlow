# FFlow - Code first automation built fluently

## FRs

**FR1 — Workflow Definition DSL**  
The system shall provide a fluent, chainable API to define workflows programmatically with methods such as `WithName`, `StartWith<TStep>`, `Then<TStep>`, `If(predicate)`, and `ForEach(collection)`.

**FR2 — WorkflowContext**  
The system shall maintain a `WorkflowContext` object that supports setting, getting, and trying to get variables by key during workflow execution.

**FR3 — Step Execution**  
Each workflow step shall implement an interface with an asynchronous `ExecuteAsync(WorkflowContext)` method, allowing custom logic and variable manipulation.

**FR4 — Conditional Branching**  
The workflow shall support conditional branching via `If` blocks that evaluate predicates against the current `WorkflowContext` to decide which path to execute.

**FR5 — Looping**  
The workflow shall support `ForEach` looping over collections accessible in the `WorkflowContext`, applying a nested workflow body for each item.

**FR6 — Dependency Injection Support**  
Workflow steps shall support constructor injection of services, enabling integration with dependency injection containers.

**FR7 — Persistence Abstraction**  
The system shall provide an abstraction layer to support multiple workflow persistence backends (e.g., in-memory, SQLite, Redis), enabling saving and resuming workflows.

**FR8 — Extensible Step Registration**  
Users shall be able to register custom step types and extend the fluent DSL with new methods or step behaviors via extension methods.

**FR9 — Workflow Build and Execution**  
The system shall support building a workflow definition into an executable object and running it asynchronously with proper context management.

**FR10 — Error Handling and Logging**  
The system shall provide mechanisms for error handling within steps and support logging integrations for diagnostic purposes.

**FR11 — Support for CQRS Compatibility**  
The workflow shall support dispatching commands or queries inside steps, aligning with CQRS patterns.

**FR12 — Automatic DI for steps**  
The workflow shall automatically register any steps involved when registering it on the service collection.


**FR13 — Support for Multiple instances of the same workflow**  
Any workflow execution shall be independent of each other and be able to be ran asynchronously, even if it is the same workflow type (Using a workflow definition marker interface).

## Syntax
### Basic Workflow
```cs
Flow.Define("SendWelcomeFlow")
    .WithName("Send welcome email to user")
    .StartWith<ValidateUser>()
    .Then<CreateUser>()
    .Then<SendWelcomeEmail>()
    .Build();

```

### Conditional
```cs
Flow.Define("ValidateAndProcess")
    .StartWith<ValidateUser>()
    .If(ctx => ctx.Get<bool>("isValid"), then => then
        .Then<CreateUser>()
        .Then<SendWelcomeEmail>(),
        elseBranch: els => els
        .Then<LogValidationFailure>())
    .Build();
```

### Foreach
```
Flow.Define("NotifyAllUsers")
    .StartWith<GetEmails>()
    .ForEach(ctx => ctx.Get<List<string>>("emails"), each => each
        .Then<SendNotification>())
    .Build();
```

### Workflow Context
```cs
Flow.Define("CalculateTotal")
    .StartWith<LoadCart>()
    .Then<CalculateTax>()
    .Then(ctx =>
    {
        var total = ctx.Get<decimal>("subtotal") + ctx.Get<decimal>("tax");
        ctx.Set("total", total);
        return Task.CompletedTask;
    })
    .Then<ChargeCustomer>()
    .Build();
```

