# Compensation & Saga Pattern
**Compensation** is a mechanism that enables FFlow to gracefully handle failures by reversing the effects of previously executed steps. It follows the **Saga Pattern**, which is commonly used in long-running or distributed workflows where traditional database transactions aren't feasible. Instead of relying on atomic rollbacks, each step defines its own logic to undo its work when a failure occurs later in the workflow. This allows your workflows to maintain consistency even in the face of partial failures.

For a step to support compensation, it must implement the `ICompensableStep` interface. This interface requires you to define a `CompensateAsync` method that will be called to undo the changes made by the step.

> [!TIP]
> If your step inherits from `FlowStep`, you don’t need to implement `ICompensableStep` manually. Just override the `CompensateAsync` method to define your compensation logic.

Compensation **should not** be used for every step in your workflow. It is best suited for steps that perform significant actions, such as database updates, external service calls, or any operation that needs to be reversed if a later step fails.

## Implementing Compensation

```csharp
public class CompensableStep : FlowStep
{
    protected override async Task ExecuteAsync(IFlowContext context)
    {
        // Perform the main action of the step
        Console.WriteLine("Executing compensable step...");
        await Task.CompletedTask; // Simulate async work
    }

    public override async Task CompensateAsync(IFlowContext context)
    {
        // Undo the changes made by this step
        Console.WriteLine("Compensating for compensable step...");
        await Task.CompletedTask; // Simulate async compensation work
    }
}
```

You can then use this step in your workflow, and if a later step fails, FFlow will automatically call the `CompensateAsync` method to roll back the changes made by the `CompensableStep`.

## Limitations
Compensation in FFlow is designed to be simple and effective, but it does have some limitations:
- When using `Fork()`, compensation **only applies to the branch that threw exceptions**, and will **not** compensate operations before the fork or in other branches.
- Steps defined by lambdas or delegates **cannot** be compensated. Define them in a class that inherits from `FlowStep` to enable compensation.
- The step that throws the exception is also compensated, so write compensation logic that can handle the state of the step after it has failed.


