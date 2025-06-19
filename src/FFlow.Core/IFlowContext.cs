namespace FFlow.Core;
    
    /// <summary>
    /// Represents the context for a workflow, providing methods to manage input and key-value pairs.
    /// </summary>
    public interface IFlowContext
    {
        Guid Id { get; }
        /// <summary>
        /// Retrieves the input of the specified type from the context.
        /// </summary>
        /// <typeparam name="TInput">The type of the input to retrieve.</typeparam>
        /// <returns>The input of type <typeparamref name="TInput"/>.</returns>
        TInput GetInput<TInput>();
    
        /// <summary>
        /// Sets the input of the specified type in the context.
        /// </summary>
        /// <typeparam name="TInput">The type of the input to set.</typeparam>
        /// <param name="input">The input value to set.</param>
        void SetInput<TInput>(TInput input);
    
        /// <summary>
        /// Retrieves a value of the specified type associated with the given key.
        /// </summary>
        /// <typeparam name="T">The type of the value to retrieve.</typeparam>
        /// <param name="key">The key associated with the value.</param>
        /// <returns>The value of type <typeparamref name="T"/>.</returns>
        T Get<T>(string key);
    
        /// <summary>
        /// Sets a value of the specified type associated with the given key.
        /// </summary>
        /// <typeparam name="T">The type of the value to set.</typeparam>
        /// <param name="key">The key to associate with the value.</param>
        /// <param name="value">The value to set.</param>
        void Set<T>(string key, T value);
    
        /// <summary>
        /// Attempts to retrieve a value of the specified type associated with the given key.
        /// </summary>
        /// <typeparam name="T">The type of the value to retrieve.</typeparam>
        /// <param name="key">The key associated with the value.</param>
        /// <param name="value">When this method returns, contains the value associated with the key, if found; otherwise, the default value for the type of the value parameter.</param>
        /// <returns><c>true</c> if the value was found; otherwise, <c>false</c>.</returns>
        bool TryGet<T>(string key, out T value);

        /// <summary>
        /// Clones the data in the context for use in different threads or flows.
        /// </summary>
        /// <returns>
        /// An instance of <see cref="IFlowContext"/> with the same data as the original.
        /// </returns>
        IFlowContext Fork();
        
        /// <summary>
        /// Gets all key-value pairs in the context.
        /// </summary>
        /// <returns>
        /// An enumerable collection of key-value pairs representing the context's data.
        /// </returns>
        IEnumerable<KeyValuePair<string, object>> GetAll();

        IFlowContext SetId(Guid id);
    }