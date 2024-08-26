﻿namespace ServiceMarketplace.Data.Entities;

public class BusinessHours
{
    public int Id { get; set; }

    public DayOfWeek DayOfWeek { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public bool IsDayOff { get; set; }

    public Guid ServiceId { get; set; }
}
