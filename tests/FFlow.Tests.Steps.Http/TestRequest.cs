using System.Net;

namespace FFlow.Tests.Steps.Http;

public class TestRequest
{
    public string Value { get; set; }
    public HttpStatusCode? ResponseStatusCode { get; set; }
}