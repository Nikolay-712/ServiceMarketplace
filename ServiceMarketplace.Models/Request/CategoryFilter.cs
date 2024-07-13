namespace ServiceMarketplace.Models.Request;

public class CategoryFilter : PaginationRequestModel
{
    public string? SearchTerm { get; set; }

    public OrderParameters? OrderParameters { get; set; }

    public bool Ascending { get; set; } = true;
}


public enum OrderParameters
{
    Name,
    Data,
}