using FFlow.Core;

namespace FFlow;

public static class RetryPolicies
{
    public static IRetryPolicy FixedDelay(int maxRetries, TimeSpan delay) =>
        new FixedDelayRetryPolicy(maxRetries, delay);

    public static IRetryPolicy ExceptionType(int maxRetries, TimeSpan delay, params Type[] retryOnExceptions) =>
        new ExceptionTypeRetryPolicy(maxRetries, delay, retryOnExceptions);

    public static IRetryPolicy ExponentialBackoff(int maxRetries, TimeSpan initialDelay) =>
        new ExponentialBackoffRetryPolicy(maxRetries, initialDelay);

    public class FixedDelayRetryPolicy : IRetryPolicy
    {
        private readonly int _maxRetries;
        private readonly TimeSpan _delay;

        public FixedDelayRetryPolicy(int maxRetries, TimeSpan delay)
        {
            _maxRetries = maxRetries;
            _delay = delay;
        }

        public async Task ExecuteAsync(Func<Task> action, CancellationToken cancellationToken = default)
        {
            for (int i = 0; i < _maxRetries; i++)
            {
                try
                {
                    await action();
                    return;
                }
                catch
                {
                    if (i == _maxRetries - 1) throw;
                    await Task.Delay(_delay, cancellationToken);
                }
            }
        }
    }
    
    public class ExceptionTypeRetryPolicy : IRetryPolicy
    {
        private readonly int _maxRetries;
        private readonly TimeSpan _delay;
        private readonly Type[] _retryOnExceptions;

        public ExceptionTypeRetryPolicy(int maxRetries, TimeSpan delay, params Type[] retryOnExceptions)
        {
            _maxRetries = maxRetries;
            _delay = delay;
            _retryOnExceptions = retryOnExceptions;
        }

        public async Task ExecuteAsync(Func<Task> action, CancellationToken cancellationToken = default)
        {
            for (int i = 0; i < _maxRetries; i++)
            {
                try
                {
                    await action();
                    return;
                }
                catch (Exception ex) when (Array.Exists(_retryOnExceptions, t => t.IsInstanceOfType(ex)))
                {
                    if (i == _maxRetries - 1) throw;
                    await Task.Delay(_delay, cancellationToken);
                }
            }
        }
    }
    
    public class ExponentialBackoffRetryPolicy : IRetryPolicy
    {
        private readonly int _maxRetries;
        private readonly TimeSpan _initialDelay;

        public ExponentialBackoffRetryPolicy(int maxRetries, TimeSpan initialDelay)
        {
            _maxRetries = maxRetries;
            _initialDelay = initialDelay;
        }

        public async Task ExecuteAsync(Func<Task> action, CancellationToken cancellationToken = default)
        {
            for (int i = 0; i < _maxRetries; i++)
            {
                try
                {
                    await action();
                    return;
                }
                catch
                {
                    if (i == _maxRetries - 1) throw;
                    var delay = TimeSpan.FromMilliseconds(_initialDelay.TotalMilliseconds * Math.Pow(2, i));
                    await Task.Delay(delay, cancellationToken);
                }
            }
        }
    }
}