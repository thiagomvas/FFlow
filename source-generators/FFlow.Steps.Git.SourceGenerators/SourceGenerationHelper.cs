namespace FFlow.Steps.Git.SourceGenerators;

public static class SourceGenerationHelper
{
    public const string MarkerAttribute = @"
using System;

namespace FFlow.Steps.Git
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class GitStepAttribute : Attribute
    {
        /// <summary>
        /// Specify pairs of parameter/property names: param1, property1, param2, property2, ...
        /// </summary>
        public GitStepAttribute(params string[] paramPropertyPairs)
        {
            if (paramPropertyPairs.Length % 2 != 0)
                throw new ArgumentException(""Must provide an even number of strings representing param/property pairs."");

            ParamPropertyPairs = new (string Param, string Property)[paramPropertyPairs.Length / 2];
            for (int i = 0; i < paramPropertyPairs.Length; i += 2)
            {
                ParamPropertyPairs[i / 2] = (paramPropertyPairs[i], paramPropertyPairs[i + 1]);
            }
        }

        /// <summary>
        /// Array of parameter/property pairs
        /// </summary>
        public (string Param, string Property)[] ParamPropertyPairs { get; }
    }
}

";
}