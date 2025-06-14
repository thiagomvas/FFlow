namespace FFlow.Exceptions;

public class StepCreationException : Exception
{
    public StepCreationException(Type stepType)
        : base($"Cannot create instance of {stepType.FullName}. No service registered and no public parameterless constructor found.")
    {
    }

    public StepCreationException(Type stepType, Exception innerException)
        : base($"Cannot create instance of {stepType.FullName}. No service registered and no public parameterless constructor found.", innerException)
    {
    }
}