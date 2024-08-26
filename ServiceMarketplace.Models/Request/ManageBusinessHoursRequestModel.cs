namespace ServiceMarketplace.Models.Request;

public record ManageBusinessHoursRequestModel(
    Guid ServiceId, 
    DayOfWeek DayOfWeek,
    TimeOnly StartTime, 
    TimeOnly EndTime,
    bool IsDayOff);


