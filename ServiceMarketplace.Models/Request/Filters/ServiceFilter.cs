namespace ServiceMarketplace.Models.Request.Filters;

public class ServiceFilter : PaginationRequestModel
{
    public string SearchTerm { get; set; }

    public Guid CategoryId { get; set; }

    public Guid SubCategoryId { get; set; }

    public int? OfferedAtId { get; set; }

    public Guid CityId { get; set; }

    public HashSet<int> TagsId { get; set; }

    public ServiceOrderParameters OrderParameters { get; set; }
}


public enum ServiceOrderParameters
{
    Date,
    Name,
    Highest_Rating,
    Lowest_Rating,
}