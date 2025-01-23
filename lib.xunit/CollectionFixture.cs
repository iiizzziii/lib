namespace lib.xunit;

public abstract class HttpClientFixture(
    HttpClientHandler httpClientHandler) : IDisposable
{
    private HttpClient HttpClient { get; } = new(httpClientHandler);

    public void Dispose()
    {
        HttpClient.Dispose();
        GC.SuppressFinalize(this);
    }
}

[CollectionDefinition("HttpClientCollection")]
public class HttpClientCollection : ICollectionFixture<HttpClientFixture> { }
