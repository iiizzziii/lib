namespace lib.xunit;

public abstract class HttpClientFixture : IDisposable
{
    private HttpClient HttpClient { get; set; }

    protected HttpClientFixture()
    {
        HttpClientHandler httpClientHandler = new();
        HttpClient = new HttpClient(httpClientHandler);
    }

    public void Dispose()
    {
        HttpClient.Dispose();
        GC.SuppressFinalize(this);
    }
}

[CollectionDefinition("HttpClientCollection")]
public class HttpClientCollection : ICollectionFixture<HttpClientFixture> { }
