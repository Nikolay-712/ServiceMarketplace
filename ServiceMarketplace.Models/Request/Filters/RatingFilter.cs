namespace ServiceMarketplace.Models.Request.Filters;

public class RatingFilter : PaginationRequestModel
{
    public RatingOrderParameters OrderParameters { get; set; }
}

public enum RatingOrderParameters
{
    Newest,
    Oldest,
    Highest_Score,
    Lowest_Score,
}
