using FFlow.Core;

namespace FFlow.Steps.Shell;
internal static class Internals
{
    public static string InjectContext(string original, IFlowContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
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
