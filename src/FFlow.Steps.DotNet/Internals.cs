using System.Diagnostics;
using FFlow.Core;

namespace FFlow.Steps.DotNet;

internal static class Internals
{
    public const string BaseNamespace = "FFlow.Steps.DotNet";
    public const string DotnetBuildConfigKey = $"{BaseNamespace}.Build.Configuration";
    public const string DotnetRestoreConfigKey = $"{BaseNamespace}.Restore.Configuration";
    public const string DotnetTestConfigKey = $"{BaseNamespace}.Test.Configuration";
    public const string DotnetPublishConfigKey = $"{BaseNamespace}.Publish.Configuration";
    public const string DotnetPackConfigKey = $"{BaseNamespace}.Pack.Configuration";
    public const string DotnetRunConfigKey = $"{BaseNamespace}.Run.Configuration";

    public static async Task<(string Output, string Error, int ExitCode)> RunDotnetCommandAsync(
        string arguments, CancellationToken cancellationToken = default)
    {
        using var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };

        process.Start();

        var outputTask = process.StandardOutput.ReadToEndAsync();
        var errorTask = process.StandardError.ReadToEndAsync();

        await Task.WhenAll(outputTask, errorTask);

        process.WaitForExit();

        return (outputTask.Result, errorTask.Result, process.ExitCode);
    }
    
    public static string InjectContext(string original, IFlowContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (string.IsNullOrEmpty(original)) return original;

        var result = new System.Text.StringBuilder(original);
        int startIndex = 0;

        while (true)
        {
            int openBrace = result.ToString().IndexOf('{', startIndex);
            if (openBrace == -1) break;
            int closeBrace = result.ToString().IndexOf('}', openBrace);
            if (closeBrace == -1) break;

            string token = result.ToString().Substring(openBrace + 1, closeBrace - openBrace - 1);

            if (token.StartsWith("context:"))
            {
                string key = token.Substring("context:".Length);
                var value = context.GetValue<string>(key, string.Empty) ?? string.Empty;

                result.Remove(openBrace, closeBrace - openBrace + 1);
                result.Insert(openBrace, value);

                startIndex = openBrace + value.Length;
            }
            else
            {
                startIndex = closeBrace + 1; // skip non-matching placeholder
            }
        }

        return result.ToString();
    }
}