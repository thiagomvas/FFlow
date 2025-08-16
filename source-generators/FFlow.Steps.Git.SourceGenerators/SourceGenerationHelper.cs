namespace FFlow.Steps.Git.SourceGenerators;

public static class SourceGenerationHelper
{
    public const string MarkerAttribute = @"
using System;

namespace FFlow.Steps.Git
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class GitStepAttribute : Attribute
    {
        public GitStepAttribute(string? paramName = null, string? propertyName = null)
        {
            ParamName = paramName;
            PropertyName = propertyName;
        }

        public string? ParamName { get; }
        public string? PropertyName { get; }
    }
}
";
}