namespace FFlow.Core;

public interface IRetryableFlowStep
{
    IRetryPolicy SetRetryPolicy(IRetryPolicy retryPolicy);
}