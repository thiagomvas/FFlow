namespace FFlow.Core;
    
    /// <summary>
    /// Provides methods to define cases for a switch-case structure in a workflow.
    /// </summary>
    public interface ISwitchCaseBuilder
    {
        /// <summary>
        /// Adds a case to the switch-case structure with a specified condition.
        /// </summary>
        /// <param name="condition">The condition to evaluate for this case.</param>
        /// <returns>An instance of <see cref="IWorkflowBuilder"/> to continue building the workflow.</returns>
        IWorkflowBuilder Case(Func<IFlowContext, bool> condition);


        /// <summary>
        /// Adds a named case to the switch-case structure with a specified condition and step type.
        /// </summary>
        /// <typeparam name="TStep">The type of the step to execute if the condition is true.</typeparam>
        /// <param name="Name">The name of the switch case.</param>
        /// <param name="condition">The condition to evaluate for this case.</param>
        /// <returns>An instance of <see cref="IWorkflowBuilder"/> to continue building the workflow.</returns>
        IWorkflowBuilder Case(string Name, Func<IFlowContext, bool> condition);
    
        /// <summary>
        /// Adds a case to the switch-case structure with a specified condition and step type.
        /// </summary>
        /// <typeparam name="TStep">The type of the step to execute if the condition is true.</typeparam>
        /// <param name="condition">The condition to evaluate for this case.</param>
        /// <returns>An instance of <see cref="IWorkflowBuilder"/> to continue building the workflow.</returns>
        IWorkflowBuilder Case<TStep>(Func<IFlowContext, bool> condition) where TStep : class, IFlowStep;

        /// <summary>
        /// Adds a named case to the switch-case structure with a specified condition and step type.
        /// </summary>
        /// <typeparam name="TStep">The type of the step to execute if the condition is true.</typeparam>
        /// <param name="Name">The name of the switch case.</param>
        /// <param name="condition">The condition to evaluate for this case.</param>
        /// <returns>An instance of <see cref="IWorkflowBuilder"/> to continue building the workflow.</returns>
        IWorkflowBuilder Case<TStep>(string Name, Func<IFlowContext, bool> condition) where TStep : class, IFlowStep;
    }