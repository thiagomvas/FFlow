namespace FFlow.Cli;

using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;

public static class HelpHelper
{
    public static void ShowHelp(
        string commandName,
        string description,
        Dictionary<string, string>? subcommands = null,
        Dictionary<string, string>? options = null)
    {
        int nameWidth = 0;

        if (subcommands?.Count > 0)
            nameWidth = subcommands.Keys.Max(k => k.Length);

        if (options?.Count > 0)
            nameWidth = Math.Max(nameWidth, options.Keys.Max(k => k.Length + 2)); // account for "--"

        nameWidth += 4;
        
        

        AnsiConsole.MarkupLine($"[bold]Usage:[/] fflow {commandName} {(subcommands is null ? "" : "<command>")} [[options]]\n");
        AnsiConsole.MarkupLine(description + "\n");

        if (subcommands?.Count > 0)
        {
            AnsiConsole.MarkupLine("[bold]Commands:[/]");
            foreach (var (cmd, desc) in subcommands)
            {
                AnsiConsole.MarkupLine($"  [green]{cmd.PadRight(nameWidth)}[/] {desc}");
            }
            AnsiConsole.MarkupLine("");
        }

        if (options?.Count > 0)
        {
            AnsiConsole.MarkupLine("[bold]Options:[/]");
            foreach (var (opt, desc) in options)
            {
                AnsiConsole.MarkupLine($"  --{opt.PadRight(nameWidth)} {desc}");
            }
        }
    }
}