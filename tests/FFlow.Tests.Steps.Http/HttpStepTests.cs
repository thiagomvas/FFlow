using System.Net;
using System.Net.Http.Json;
using FFlow.Steps.Http;

namespace FFlow.Tests.Steps.Http;

public class HttpStepTests
{
    private HttpRequestStep httpStep;

    [SetUp]
    public void SetUp()
    {
        var client = new HttpClient(new TestMessageHandler());
        httpStep = new HttpRequestStep(client)
        {
            Url = "http://www.test.com",
            Headers = new()
            {
                {"k", "v"}
            },
            Body = new TestRequest()
            {
                Value = "foo"
            }
        };
    }
    
    [Test]
    public async Task HttpRequestStep_ShouldAddHeadersCorrectly()
    {
        var ctx = await new FFlowBuilder()
            .Then(httpStep)
            .Build()
            .RunAsync();

        var response = ctx.GetLastOutput<HttpResponseMessage>();
        var headers = response.Headers.ToDictionary();
        Assert.That(headers, Has.Count.EqualTo(1));
        Assert.That(headers, Does.ContainKey("k"));
        Assert.That(headers["k"].First(), Is.EqualTo("v"));
    }

    [Test]
    public async Task HttpRequestStep_ShouldPassTheBody()
    {
        var ctx = await new FFlowBuilder()
            .Then(httpStep)
            .Build()
            .RunAsync();
        
        var response = ctx.GetLastOutput<HttpResponseMessage>();
        var body = await response.Content.ReadFromJsonAsync<TestResponse>();
        Assert.That(body, Is.Not.Null);
        Assert.That(body.Value, Is.EqualTo("foo"));
    }

    [Test]
    public async Task HttpRequestStep_ShouldUseCorrectMethod()
    {
        httpStep.Method = HttpMethod.Patch;
        
        var ctx = await new FFlowBuilder()
            .Then(httpStep)
            .Build()
            .RunAsync();
        
        var response = ctx.GetLastOutput<HttpResponseMessage>();
        var body = await response.Content.ReadFromJsonAsync<TestResponse>();
        
        Assert.That(body, Is.Not.Null);
        Assert.That(body.Method, Is.EqualTo(HttpMethod.Patch));
    }

    [Test]
    public async Task HttpRequestStep_WhenReturningUnacceptableStatusCode_ShouldThrow()
    {
        httpStep.AcceptableStatusCodes = [HttpStatusCode.Accepted];
        httpStep.Body = new TestRequest()
        {
            ResponseStatusCode = HttpStatusCode.OK
        };
        var workflow = new FFlowBuilder()
            .Then(httpStep)
            .Build();

        Assert.ThrowsAsync<HttpRequestException>(async () => await workflow.RunAsync());
    }

    [Test]
    public async Task HttpRequestStep_WhenReturningAcceptableStatusCode_ShouldNotThrow()
    {
        
        httpStep.AcceptableStatusCodes = [HttpStatusCode.Found];
        httpStep.Body = new TestRequest()
        {
            ResponseStatusCode = HttpStatusCode.Found
        };
        
        var workflow = new FFlowBuilder()
            .Then(httpStep)
            .Build();
        
        Assert.DoesNotThrowAsync(async () => await workflow.RunAsync());
    }
}

public record TestPOCO(string Name, int Age);