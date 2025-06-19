# Validation
Validation is a crucial part of ensuring that the data you are working with is accurate and conforms to the expected formats. In this section, we will cover how to validate data using various methods.

FFlow provides validation utilites through the `FFlow.Validation` package, which includes built-in validation steps and utilities to help you validate data within your workflows.

```bash
dotnet add package FFlow.Validation
```


## Built-in Validation Steps
FFlow provides built-in validation steps that can be used to validate data within your workflow. These steps can be used to check if the data meets certain criteria before proceeding with the workflow execution.
```csharp
var workflow = new FFlowBuilder()
    .StartWith<SomeStep>()
    .RequireKeys("key1", "key2") // Validates that the context contains the specified keys
    .RequireNotNull("key1") // Validates that the specified key is not null
    .RequireRegex("key2", @"^\d{3}-\d{2}-\d{4}$") // Validates that the value matches the regex pattern
    .Then<AnotherStep>()
    .Build();
```

## Custom Validation Steps
You can also create custom validation steps by implementing the `IFlowStep` interface. This allows you to define your own validation logic and integrate it into your workflow.

```csharp
public class CustomValidationStep : IFlowStep
{
    public async Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        // Custom validation logic
        if (!context.ContainsKey("key1") || context.Get<string>("key1") == null)
        {
            throw new ValidationException("key1 is required and cannot be null.");
        }

        // Simulate async work
        await Task.CompletedTask;
    }
}

var workflow = new FFlowBuilder()
    .StartWith<SomeStep>()
    .Then<CustomValidationStep>() // Custom validation step
    .Then<AnotherStep>()
    .Build();
```

## Validation Attributes
FFlow also supports validation attributes that can be used to annotate your steps. They allow you to define validation rules directly on your step classes, making it easier to manage validation logic.

Validation attributes work by injecting steps that perform the validation before executing the step. You can use attributes like `RequireKeys`, `RequireNotNull`, and `RequireRegex` to specify validation rules.

```csharp
[RequireKeys("key1", "key2")]
public class ValidatedStep : IFlowStep
{
    public async Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        // Step logic here
        await Task.CompletedTask;
    }
}
var workflow = new FFlowBuilder()
    .UseValidators()
    .StartWith<ValidatedStep>() // This step will automatically validate the keys
    .Then<AnotherStep>()
    .Build();
```

> [!IMPORTANT]
> Make sure to call `UseValidators()` on the `FFlowBuilder` to enable validation attributes in your workflow.

## Custom Validation Attributes
You can create custom validation attributes by inheriting from `BaseFlowValidationAttribute` and implementing the `CreateValidationStep` method. 

```csharp
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public class MinLengthAttribute : BaseFlowValidationAttribute
{
    public int Length { get; }
    public string Key { get; set; }
    public MinLengthAttribute(string key, int length)
    {
        if (length < 0) throw new ArgumentOutOfRangeException(nameof(length), "Length must be non-negative.");
        Length = length;
        Key = key;
    }

    public override IFlowStep CreateValidationStep()
    {
        return new MinLengthStep(Key, Length);
    }
}

internal class MinLengthStep : IFlowStep
{
    private readonly int _minLength;
    private readonly string _key;

    public MinLengthStep(string key, int minLength)
    {
        if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Key cannot be null or whitespace.", nameof(key));
        if (minLength < 0) throw new ArgumentOutOfRangeException(nameof(minLength), "Minimum length must be non-negative.");

        _key = key;
        _minLength = minLength;
    }

    public Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        cancellationToken.ThrowIfCancellationRequested();

        if (!context.TryGet<string>(_key, out var value) || value == null)
        {
            throw new FlowValidationException($"Key '{_key}' is missing or not a string.");
        }

        if (value.Length < _minLength)
        {
            throw new FlowValidationException($"Value for key '{_key}' must be at least {_minLength} characters long.");
        }

        return Task.CompletedTask;
    }
}
```