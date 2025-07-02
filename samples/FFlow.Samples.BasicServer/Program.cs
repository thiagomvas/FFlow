using FFlow.Extensions.Microsoft.DependencyInjection;
using FFlow.Observability.Listeners;
using FFlow.Observability.Metrics;
using FFlow.Samples.BasicServer.Workflows;
using FFlow.Steps.DotNet;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddConsole(options =>
{
    options.IncludeScopes = true;
});

builder.Logging.SetMinimumLevel(LogLevel.Debug);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddFFlow(typeof(BuildAndTestWorkflow).Assembly);
builder.Services.AddSingleton<InMemoryMetricsSink>()
    .AddSingleton<MetricTrackingListener<InMemoryMetricsSink>>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapPost("/webhooks/build-and-test", async (BuildAndTestWorkflow def) =>
{
    var workflow = def.Build();
    var ctx = await workflow.RunAsync("");
    var result = ctx.GetOutputFor<DotnetTestStep, DotnetTestResult>();
    return Results.Ok(new
    {
        Passed = result.Passed,
        Failed = result.Failed,
        Skipped = result.Skipped,
    });
});

app.MapPost("/webhooks/build-artifacts", async (BuildArtifactsWorkflow def) =>
{
    var workflow = def.Build();
    var ctx = await workflow.RunAsync("");
    var artifactsPath = ctx.GetValue<string>("artifactsPath");
    return Results.File(artifactsPath, "application/zip", "fflow.zip");
});

app.MapPost("/webhooks/example", async (ExampleWebhook def) =>
{
    var workflow = def.Build();
    var ctx = await workflow.RunAsync("");
    var message = ctx.GetValue<string>("message");
    return Results.Ok(new { Message = message });
});

app.MapGet("/metrics", (InMemoryMetricsSink metricsSink) =>
{
    var metrics = metricsSink.Flush();
    return Results.Ok(metrics);
});

app.Run();