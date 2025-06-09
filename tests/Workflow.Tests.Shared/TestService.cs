namespace Workflow.Tests.Shared;

public class TestService
{
    public void DoSomething()
    {
        Console.WriteLine("TestService is doing something.");
    }
    
    public void ThrowException()
    {
        throw new InvalidOperationException("TestService encountered an error.");
    }
    
}