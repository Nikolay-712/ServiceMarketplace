namespace ServiceMarketplace.Models;

public class ResponseContent<TResponse> : ErrorResponse
    where TResponse : class
{
    public TResponse Result { get; set; }
}

public class ResponseContent
{
    public ErrorResponse ErrorResponse { get; set; }
}
