namespace FFlow.Steps.DotNet.SourceGenerators;

public static class SourceGenerationHelper
{
    public const string MarkerAttribute = @"
using System;

namespace FFlow.Steps.DotNet
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class DotnetStepAttribute : Attribute
    {
        public DotnetStepAttribute(string paramName, string propertyName)
        {
            ParamName = paramName;
            PropertyName = propertyName;
        }

        public string ParamName { get; }
        public string PropertyName { get; }
    }
}
";
}