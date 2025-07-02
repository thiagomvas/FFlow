using System.Diagnostics;
using System.Text;
using FFlow.Core;

namespace FFlow.Steps.DotNet;

/// <summary>
/// Executes the <c>dotnet nuget push</c> command to push a NuGet package to a server or local source.
/// Throws if no package path is specified or if the push fails.
/// </summary>
[StepName(".NET Push to Nuget")]
[StepTags("dotnet")]
public class DotnetNugetPushStep : IFlowStep
{
    /// <summary>
    /// The path to the package or folder to push. Required.
    /// </summary>
    public string? PackagePathOrRoot { get; set; }

    /// <summary>
    /// The API key to use when pushing the package.
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    /// The source URL or folder where the package is pushed.
    /// </summary>
    public string? Source { get; set; } = "https://api.nuget.org/v3/index.json";

    /// <summary>
    /// The symbol API key to use for symbol packages.
    /// </summary>
    public string? SymbolApiKey { get; set; }

    /// <summary>
    /// The symbol source URL or folder.
    /// </summary>
    public string? SymbolSource { get; set; }

    /// <summary>
    /// Timeout in seconds for the push command.
    /// </summary>
    public int? TimeoutSeconds { get; set; }

    /// <summary>
    /// Path to a NuGet config file.
    /// </summary>
    public string? ConfigFile { get; set; }

    /// <summary>
    /// Disables buffering for the push command.
    /// </summary>
    public bool DisableBuffering { get; set; }

    /// <summary>
    /// Forces English output.
    /// </summary>
    public bool ForceEnglishOutput { get; set; }

    /// <summary>
    /// Allows interactive command prompts.
    /// </summary>
    public bool Interactive { get; set; }

    /// <summary>
    /// Suppresses pushing of symbols packages.
    /// </summary>
    public bool NoSymbols { get; set; }

    /// <summary>
    /// Suppresses service endpoint usage.
    /// </summary>
    public bool NoServiceEndpoint { get; set; }

    /// <summary>
    /// Skips pushing duplicates.
    /// </summary>
    public bool SkipDuplicate { get; set; }
    
    /// <summary>
    /// The result of the <c>dotnet nuget push</c> command, including output and exit code.
    /// </summary>
    public DotnetNugetPushResult? Result { get; private set; }

    public async Task RunAsync(IFlowContext context, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(PackagePathOrRoot))
            throw new InvalidOperationException("PackagePathOrRoot must be specified for the nuget push step.");

        var command = BuildCommand(context);

        var (output, error, exitCode) = await Internals.RunDotnetCommandAsync(command, cancellationToken);

        if (exitCode != 0)
            throw new InvalidOperationException($"Dotnet nuget push failed with exit code {exitCode}.\nOutput: {output}\nError: {error}");

        Result = new DotnetNugetPushResult(exitCode, output, error);
        
        context.SetOutputFor<DotnetNugetPushStep, DotnetNugetPushResult>(Result);
    }

    private string BuildCommand(IFlowContext context)
    {
        var sb = new StringBuilder("dotnet nuget push");

        sb.Append(" ");
        sb.Append(QuoteArg(Internals.InjectContext(PackagePathOrRoot!, context)));

        if (DisableBuffering) sb.Append(" --disable-buffering");
        if (ForceEnglishOutput) sb.Append(" --force-english-output");
        if (Interactive) sb.Append(" --interactive");
        if (NoSymbols) sb.Append(" --no-symbols");
        if (NoServiceEndpoint) sb.Append(" --no-service-endpoint");
        if (SkipDuplicate) sb.Append(" --skip-duplicate");

        if (!string.IsNullOrEmpty(ApiKey))
        {
            sb.Append(" -k ");
            sb.Append(QuoteArg(ApiKey));
        }

        if (!string.IsNullOrEmpty(Source))
        {
            sb.Append(" -s ");
            sb.Append(QuoteArg(Internals.InjectContext(Source, context)));
        }

        if (!string.IsNullOrEmpty(SymbolApiKey))
        {
            sb.Append(" -sk ");
            sb.Append(QuoteArg(SymbolApiKey));
        }

        if (!string.IsNullOrEmpty(SymbolSource))
        {
            sb.Append(" -ss ");
            sb.Append(QuoteArg(Internals.InjectContext(SymbolSource, context)));
        }

        if (TimeoutSeconds.HasValue)
        {
            sb.Append(" -t ");
            sb.Append(TimeoutSeconds.Value);
        }

        if (!string.IsNullOrEmpty(ConfigFile))
        {
            sb.Append(" --configfile ");
            sb.Append(QuoteArg(ConfigFile));
        }

        return sb.ToString();
    }

    private static string QuoteArg(string arg)
    {
        if (string.IsNullOrEmpty(arg)) return "\"\"";
        if (arg.Contains(' ') || arg.Contains('"'))
            return $"\"{arg.Replace("\"", "\\\"")}\"";
        return arg;
    }
}
