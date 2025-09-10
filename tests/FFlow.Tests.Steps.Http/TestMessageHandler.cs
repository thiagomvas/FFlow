using System.Net;
using System.Net.Http.Json;

namespace FFlow.Tests.Steps.Http;

public class TestMessageHandler : HttpMessageHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = new HttpResponseMessage();
        foreach (var header in request.Headers)
            response.Headers.Add(header.Key, header.Value);

        var requestBody = await request.Content.ReadFromJsonAsync<TestRequest>();
        var responseContent = new TestResponse() { Method = request.Method , Value = requestBody?.Value ?? "" };
        response.Content = JsonContent.Create(responseContent);
        
        if (requestBody.ResponseStatusCode is not null)
            response.StatusCode = requestBody.ResponseStatusCode.Value;
        else
            response.StatusCode = HttpStatusCode.OK;
        
        return response;
    }
}