# Shell Commands and Scripts
FFlow provides convenient extension methods to run shell commands and scripts as workflow steps. You can execute simple commands, inline scripts, or scripts loaded from files with optional configuration.

To use the shell extensions and steps, you need to install the `FFlow.Steps.Shell` package:

```bash
dotnet add package FFlow.Steps.Shell
```

> [!IMPORTANT]
> The steps by default use `bash`, so ensure that your environment supports it. If you need to use a different shell, you can specify the shell executable in the step configuration.

## Running Shell Commands
You can run shell commands using the `RunCommand` step. This step allows you to execute a command in the shell and capture its output.

```csharp
var workflow = new FFlowBuilder()
    .RunCommand("echo 'Hello, World!'", step => {
        step.OutputHandler = Console.WriteLine;
    })
    .Build();

await workflow.RunAsync();
```

## Running scripts
You can run scripts using the `RunScriptRaw` step. 

```csharp
var workflow = new FFlowBuilder()
    .RunScriptRaw("echo $MY_VAR", step => {
        step.OutputHandler = Console.WriteLine;
        step.EnvironmentVariables = new Dictionary<string, string>
        {
            ["MY_VAR"] = "Hello from env"
        };
    })
    .Build();
```

## Running a script from a file
You can also run scripts from a file using the `RunScriptFile` step.

```csharp
var workflow = new FFlowBuilder()
    .RunScriptFile("path/to/script.sh", step => {
        step.OutputHandler = Console.WriteLine;
        step.EnvironmentVariables = new Dictionary<string, string>
        {
            ["MY_VAR"] = "Hello from env"
        };
    })
    .Build();
```

You can specify the path to the script file, and it will be executed in the shell with the provided environment variables.

## Limitations
The shell steps do **not** support stdin input redirection. They are designed to run commands and scripts that do not require interactive input. If you need to run commands that require user input, consider using other methods or redesigning your workflow to avoid such steps.

