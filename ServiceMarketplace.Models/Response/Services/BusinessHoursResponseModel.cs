namespace ServiceMarketplace.Models.Response.Services;

public record BusinessHoursResponseModel(
    string DayOfWeek, 
    string StartTime, 
    string EndTime, 
    bool IsDayOf);


