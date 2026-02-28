using System;
using System.Collections.Generic;

namespace BiProject.Api.Data.DataWarehouse.Entities;

public partial class DimDate
{
    public int DateSk { get; set; }

    public int? DateKey { get; set; }

    public DateOnly? FullDate { get; set; }

    public int? CalendarYear { get; set; }

    public int? MonthNumberOfYear { get; set; }

    public string? EnglishMonthName { get; set; }

    public int? DayNumberOfMonth { get; set; }

    public string? DayNameOfWeek { get; set; }

    public bool? IsWeekend { get; set; }

    public int? CalendarQuarter { get; set; }

    // Navigation property
    public virtual ICollection<FactInternetSales> FactInternetSales { get; set; } = new List<FactInternetSales>();
}
