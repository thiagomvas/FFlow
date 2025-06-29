namespace FFlow.Scheduling;

public interface IFflowScheduledWorkflowBuilder
{
    void RunEvery(string cron);
    void RunEvery(TimeSpan interval);
    void RunOnceAt(DateTime timeUtc);
}