using System.Net;
using System.Text;
using System.Text.Json;
using FFlow.Core;

namespace FFlow.Steps.Http;

/// <summary>
/// Represents a step that performs an HTTP request using <see cref="HttpClient"/>.
/// </summary>
public class HttpRequestStep : FlowStep
{
    private static readonly HttpClient DefaultHttpClient = new HttpClient();

    /// <summary>
    /// Gets or sets the HTTP method to use for the request (e.g., GET, POST).
    /// </summary>
    public HttpMethod Method { get; set; } = HttpMethod.Get;

    /// <summary>
    /// Gets or sets the request URL. This property is required.
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// Gets or sets the request body.
    /// Can be a string, byte[], or an object to serialize.
    /// </summary>
    public object? Body { get; set; }

    /// <summary>
    /// Gets or sets the content type of the request body.
    /// If null, JSON will be used for objects, plain text for strings, and octet-stream for byte arrays.
    /// </summary>
    public string? ContentType { get; set; }

    /// <summary>
    /// Gets or sets the headers to include in the HTTP request.
    /// </summary>
    public Dictionary<string, string>? Headers { get; set; }

    /// <summary>
    /// Gets or sets the timeout for the HTTP request. Defaults to 30 seconds.
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Gets or sets the list of acceptable HTTP status codes.
    /// If specified, the response must match one of these, otherwise an exception is thrown.
    /// </summary>
    public HttpStatusCode[]? AcceptableStatusCodes { get; set; }

    /// <summary>
    /// Gets or sets the JSON serializer options to use when serializing the request body or deserializing the response.
    /// </summary>
    public JsonSerializerOptions? JsonSerializerOptions { get; set; }
    
    /// <summary>
    /// Gets the raw HTTP response returned from the request.
    /// </summary>
    public HttpResponseMessage? Response { get; private set; }

    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpRequestStep"/> class using a shared default <see cref="HttpClient"/>.
    /// </summary>
    public HttpRequestStep()
    {
        _httpClient = DefaultHttpClient;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpRequestStep"/> class using a provided <see cref="HttpClient"/>.
    /// </summary>
    /// <param name="httpClient">An externally managed <see cref="HttpClient"/> instance.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="httpClient"/> is null.</exception>
    public HttpRequestStep(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    protected override async Task ExecuteAsync(IFlowContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(Url))
            throw new InvalidOperationException("HttpRequestStep requires a valid URL.");

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(Timeout);

        var request = new HttpRequestMessage(Method, Url);

        if (Body != null)
        {
            if (Body is string s)
            {
                var mediaType = ContentType;

                if (!string.IsNullOrEmpty(mediaType) && mediaType.Contains(";"))
                {
                    // Strip charset or parameters from mediaType for StringContent ctor
                    mediaType = mediaType.Split(';')[0].Trim();
                }

                mediaType ??= "text/plain"; // default mediaType without charset

                request.Content = new StringContent(s, Encoding.UTF8, mediaType);
            }
            else if (Body is byte[] bytes)
            {
                request.Content = new ByteArrayContent(bytes);
                if (!string.IsNullOrEmpty(ContentType))
                    request.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(ContentType);
            }
            else
            {
                // Default to JSON serialization
                var json = JsonSerializer.Serialize(Body, JsonSerializerOptions);
                var mediaType = ContentType ?? "application/json";
                request.Content = new StringContent(json, Encoding.UTF8, mediaType);
            }
        }

        if (Headers != null)
        {
            foreach (var header in Headers)
            {
                // TryAddWithoutValidation to allow custom headers like Authorization, etc.
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseContentRead, cts.Token).ConfigureAwait(false);
        Response = response;

        if (AcceptableStatusCodes is not null && !AcceptableStatusCodes.Contains(response.StatusCode))
        {
            throw new HttpRequestException($"Unexpected status code: {response.StatusCode}. Expected one of: {string.Join(", ", AcceptableStatusCodes)}");
        }
        else if (AcceptableStatusCodes is null && !response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Request failed with status code: {response.StatusCode}");
        }

        context.SetOutputFor<HttpRequestStep, HttpResponseMessage>(Response);
    }
}
