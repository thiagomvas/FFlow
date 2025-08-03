using FFlow.Core;
using FFlow.Exceptions;
using FFlow.Extensions.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Workflow.Tests.Shared;

namespace FFlow.Tests;

public class DITests
{
    [Test]
    public void StepCreation_ShouldThrow_WhenNoDiResolutionOrParameterlessConstructor()
    {
        var serviceCollection = new ServiceCollection();
        
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        Assert.Throws<StepCreationException>(() => new FFlowBuilder(serviceProvider)
            .Then<DiStep>()
            .Build(), "Should throw StepCreationException when no DI resolution or parameterless constructor is available.");
    }
    
    [Test]
    public void Then_ShouldInjectDependencies()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddFFlow().AddTransient<TestService>();
        
        
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        var workflow = new FFlowBuilder(serviceProvider)
            .Then<DiStep>()
            .Build();
        
        Assert.DoesNotThrowAsync(async () =>
        {
            await workflow.RunAsync(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);
        }, "Workflow should execute without throwing an exception when dependencies are injected.");
    }

    [Test]
    public void StartWith_ShouldInjectDependencies()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddFFlow().AddTransient<TestService>();
        
        
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        var workflow = new FFlowBuilder(serviceProvider)
            .StartWith<DiStep>()
            .Build();
        
        Assert.DoesNotThrowAsync(async () =>
        {
            await workflow.RunAsync(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);
        }, "Workflow should execute without throwing an exception when dependencies are injected.");
    }
    
    [Test]
    public void Finally_ShouldInjectDependencies()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddFFlow().AddTransient<TestService>();
        
        
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        var workflow = new FFlowBuilder(serviceProvider)
            .Then<DiStep>()
            .Build();
        
        Assert.DoesNotThrowAsync(async () =>
        {
            await workflow.RunAsync(new CancellationTokenSource(TimeSpan.FromMilliseconds(500)).Token);
        }, "Workflow should execute without throwing an exception when dependencies are injected.");
    }

    [Test]
    public void If_ShouldInjectDependencies()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddFFlow().AddTransient<TestService>();
        
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        Assert.DoesNotThrow(() => new FFlowBuilder(serviceProvider)
            .If<DiStep, DiStep>(ctx => true)
            .Build(), "If<TTrue,TFalse> should inject the services");
        
        Assert.DoesNotThrow(() => new FFlowBuilder(serviceProvider)
            .If<DiStep>(ctx => true)
            .Build(), "If<TTrue> should inject the services");
        
        
        Assert.DoesNotThrow(() => new FFlowBuilder(serviceProvider)
            .If(ctx => true, () => new FFlowBuilder(serviceProvider).StartWith<DiStep>(), () => new FFlowBuilder(serviceProvider).StartWith<DiStep>())
            .Build(), "If(Builder) should inject the services");
    }

    [Test]
    public void ForEach_ShouldInjectDependencies()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddFFlow().AddTransient<TestService>();
        
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        Assert.DoesNotThrow(() => new FFlowBuilder(serviceProvider)
            .ForEach<DiStep>(_ => []));
        
        Assert.DoesNotThrow(() => new FFlowBuilder(serviceProvider)
            .ForEach<DiStep, int>(_ => [1, 2, 3]));
        
        Assert.DoesNotThrow(() => new FFlowBuilder(serviceProvider)
            .ForEach<object>(_ => [], () => new FFlowBuilder(serviceProvider).StartWith<DiStep>()));
        
        Assert.DoesNotThrow(() => new FFlowBuilder(serviceProvider)
            .ForEach<int>(_ => [1, 2, 3], () => new FFlowBuilder(serviceProvider).StartWith<DiStep>()));
    }
    
    [Test]
    public void Fork_ShouldInjectDependencies()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddFFlow().AddTransient<TestService>();
        
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        Assert.DoesNotThrow(() => new FFlowBuilder(serviceProvider)
            .Fork(ForkStrategy.FireAndForget, () => new FFlowBuilder(serviceProvider).StartWith<DiStep>()));
        
        Assert.DoesNotThrow(() => new FFlowBuilder(serviceProvider)
            .Fork(ForkStrategy.FireAndForget, () => new FFlowBuilder(serviceProvider).StartWith<DiStep>(),
                () => new FFlowBuilder(serviceProvider).StartWith<DiStep>()));
    }
}