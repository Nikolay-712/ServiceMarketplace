namespace ServiceMarketplace.Models;

public class ModelStateErrorResponse
{
    public int StatusCode { get; set; }

    public IReadOnlyList<string> ValidationMessages { get; set; }
}
