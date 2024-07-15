namespace ServiceMarketplace.Models.Request;

public class CityFilter : PaginationRequestModel
{
    public string SearchTerm { get; set; }
}
