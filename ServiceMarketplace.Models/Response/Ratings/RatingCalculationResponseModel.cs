namespace ServiceMarketplace.Models.Response.Ratings;

public record RatingCalculationResponseModel(
        int VotesCount,
        double AverageRating);