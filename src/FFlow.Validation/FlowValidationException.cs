namespace FFlow.Validation;

/// <summary>
/// The exception that is thrown when a validation error occurs in a workflow.
/// </summary>
public class FlowValidationException : Exception
{
    public FlowValidationException(string message) : base(message) { }
}