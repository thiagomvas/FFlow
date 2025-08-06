using FFlow.Core;

namespace FFlow;

/// <summary>
/// Contains various retry policies for handling transient failures in workflows.
/// </summary>
public static class RetryPolicies
{
    /// <summary>
    /// Creates a fixed delay retry policy.
    /// </summary>
    /// <param name="maxRetries">The maximum number of retry attempts.</param>
    /// <param name="delay">The delay between each retry attempt.</param>
    /// <returns>An instance of <see cref="IRetryPolicy"/> with fixed delays.</returns>
    public static IRetryPolicy FixedDelay(int maxRetries, TimeSpan delay) =>
        new FixedDelayRetryPolicy(maxRetries, delay);

    /// <summary>
    /// Creates a retry policy that retries on specific exception types with fixed delays.
    /// </summary>
    /// <param name="maxRetries">The maximum number of retry attempts.</param>
    /// <param name="delay">The delay between each retry attempt.</param>
    /// <param name="retryOnExceptions">The exception types to retry on.</param>
    /// <returns>An instance of <see cref="IRetryPolicy"/> that handles specified exceptions.</returns>
    public static IRetryPolicy ExceptionType(int maxRetries, TimeSpan delay, params Type[] retryOnExceptions) =>
        new ExceptionTypeRetryPolicy(maxRetries, delay, retryOnExceptions);

    /// <summary>
    /// Creates an exponential backoff retry policy.
    /// </summary>
    /// <param name="maxRetries">The maximum number of retry attempts.</param>
    /// <param name="initialDelay">The initial delay before the first retry. Subsequent delays increase exponentially.</param>
    /// <returns>An instance of <see cref="IRetryPolicy"/> with exponential backoff behavior.</returns>
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
                    await action().ConfigureAwait(false);
                    return;
                }
                catch
                {
                    if (i == _maxRetries - 1) throw;
                    await Task.Delay(_delay, cancellationToken).ConfigureAwait(false);
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
                    await action().ConfigureAwait(false);
                    return;
                }
                catch (Exception ex) when (Array.Exists(_retryOnExceptions, t => t.IsInstanceOfType(ex)))
                {
                    if (i == _maxRetries - 1) throw;
                    await Task.Delay(_delay, cancellationToken).ConfigureAwait(false);
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
                    await action().ConfigureAwait(false);
                    return;
                }
                catch
                {
                    if (i == _maxRetries - 1) throw;
                    var delay = TimeSpan.FromMilliseconds(_initialDelay.TotalMilliseconds * Math.Pow(2, i));
                    await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
                }
            }
        }
    }
}