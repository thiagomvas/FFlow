using System.Text;
using Spectre.Console;

namespace FFlow.Cli.Commands;

public class InitCommand : ICommand
{
    public string Name => "init";

    public string Description =>
        "Interactively initializes a new FFlow workflow project.";

    public List<ICommand> Subcommands => new(); // No subcommands for now

    public void Configure(CommandBuilder builder)
    {
    }

    public int Execute(List<string> positionalArgs, Dictionary<string, string> options)
    {
        if (options.ContainsKey("i") || options.ContainsKey("interactive"))
        {
            InteractiveInit();
            return 0;
        }

        string name;
        if (!string.IsNullOrWhiteSpace(options.GetValueOrDefault("n")))
            name = options["n"];
        else if (!string.IsNullOrWhiteSpace(options.GetValueOrDefault("name")))
            name = options["name"];
        else
            name = "MyWorkflow";

        string path;
        if (!string.IsNullOrWhiteSpace(options.GetValueOrDefault("p")))
            path = options["p"];
        else if (!string.IsNullOrWhiteSpace(options.GetValueOrDefault("path")))
            path = options["path"];
        else
            path = Directory.GetCurrentDirectory();

        AnsiConsole.MarkupLine($"[yellow]Initializing project '{name}' at '{path}'...[/]");

        var template = Templates.Basic;
        var filePath = Path.Combine(path, $"{name}.cs");
        File.WriteAllText(filePath, template.Content);
        AnsiConsole.MarkupLine($"[green]✔ Workflow '{name}' initialized successfully at '{filePath}'[/]");
        return 0;
    }

    private static void InteractiveInit()
    {
        StringBuilder sb = new();

        AnsiConsole.MarkupLine("[bold underline green]FFlow Pipeline Initialization[/]");
        AnsiConsole.WriteLine();

        var projectName = AnsiConsole.Prompt(
            new TextPrompt<string>("Workflow name:")
                .PromptStyle("green")
                .DefaultValue("MyWorkflow")
                .Validate(name =>
                    string.IsNullOrWhiteSpace(name)
                        ? ValidationResult.Error("[red]Workflow name cannot be empty[/]")
                        : ValidationResult.Success()));

        var packages = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .Title("Include any [green]step bundles[/]?")
                .NotRequired()
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
                .InstructionsText(
                    "[grey](Press [blue]<space>[/] to toggle a package, " +
                    "[green]<enter>[/] to accept)[/]")
                .AddChoices(Internals.PackageMappings.Keys));
        
        AnsiConsole.MarkupLine($"[yellow]Selected packages: {string.Join(", ", packages)}[/]");

        bool includeExampleStep = AnsiConsole.Prompt(
            new SelectionPrompt<bool>()
                .Title("Include an [green]example step[/]?")
                .AddChoices(new[] { true, false })
                .UseConverter(value => value ? "Yes" : "No"));
        
        AnsiConsole.MarkupLine($"[yellow]Include example step: {(includeExampleStep ? "Yes" : "No")}[/]");

        var outputFolder = AnsiConsole.Prompt(
            new TextPrompt<string>("Output folder:")
                .PromptStyle("green")
                .DefaultValue(Directory.GetCurrentDirectory()));
        
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[grey]Scaffolding project...[/]");

        sb.AppendLine("#:package FFlow@*"); // Use latest
        foreach (var package in packages)
        {
            if (Internals.PackageMappings.TryGetValue(package, out var packageName))
            {
                AnsiConsole.MarkupLine($"[yellow]Adding package: {packageName}[/]");
                sb.AppendLine($"#:package {packageName}@*"); // Use latest
            }
        }
        
        sb.AppendLine("using FFlow;");
        if (includeExampleStep)
            sb.AppendLine("using FFlow.Core;");
        sb.AppendLine();

        sb.AppendLine("await new FFlowBuilder()");
        if (includeExampleStep)
            sb.AppendLine("    .StartWith<HelloWorldStep>()");
        sb.AppendLine("    .Build()");
        sb.AppendLine("    .RunAsync();");
        
        if (includeExampleStep)
        {
            AnsiConsole.MarkupLine("[yellow]Adding example step...[/]");
            sb.AppendLine(Templates.ExampleStep);
        }

        Directory.CreateDirectory(outputFolder);
        
        var resultPath = Path.Combine(outputFolder, $"{projectName}.cs");
        File.WriteAllText(resultPath, sb.ToString());

        AnsiConsole.MarkupLine("[green]✔ Project initialized successfully.[/]");
    }

    public void DisplayHelp()
    {
        var options = new Dictionary<string, string>
        {
            { "help, h", "Show help information." }
        };

        HelpHelper.ShowHelp(Name, Description, null, options);
    }
}