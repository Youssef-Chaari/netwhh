using System;
using System.Collections.Generic;

namespace BiProject.Api.Data.DataWarehouse.Entities;

public partial class FactInternetSales
{
    public long FactSalesId { get; set; }

    public int DateSk { get; set; }

    public int CustomerSk { get; set; }

    public int ProductSk { get; set; }

    public int GeographySk { get; set; }

    public int SalesTerritorySk { get; set; }

    public string SalesOrderNumber { get; set; } = null!;

    public int SalesOrderLineNumber { get; set; }

    public int OrderQuantity { get; set; }

    public decimal SalesAmount { get; set; }

    public DateTime LoadDate { get; set; }

    public decimal? UnitPrice { get; set; }

    public double? UnitPriceDiscount { get; set; }

    public decimal? GrossAmount { get; set; }

    public decimal? DiscountAmount { get; set; }

    public decimal? NetUnitPrice { get; set; }

    public decimal? StandardCost { get; set; }

    public decimal? TotalCost { get; set; }

    public decimal? GrossMargin { get; set; }

    public double? MarginRate { get; set; }

    public DateTime? OrderDateValue { get; set; }

    public DateTime? DueDateValue { get; set; }

    public DateTime? ShipDateValue { get; set; }

    public bool? OnlineOrderFlag { get; set; }

    // Navigation properties for EF Core Includes
    public virtual DimCustomer? Customer { get; set; }
    public virtual DimDate? OrderDate { get; set; }
    public virtual DimProduct? Product { get; set; }
    public virtual DimGeography? Geography { get; set; }
    public virtual DimSalesTerritory? SalesTerritory { get; set; }
}
