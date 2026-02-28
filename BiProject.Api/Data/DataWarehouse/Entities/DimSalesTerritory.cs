using System;
using System.Collections.Generic;

namespace BiProject.Api.Data.DataWarehouse.Entities;

public partial class DimSalesTerritory
{
    public int SalesTerritorySk { get; set; }

    public int? SalesTerritoryKey { get; set; }

    public string? TerritoryName { get; set; }

    public string? CountryRegion { get; set; }

    public string? TerritoryGroup { get; set; }

    public decimal? SalesYtd { get; set; }

    public decimal? SalesLastYear { get; set; }

    public decimal? CostYtd { get; set; }
    
    // Navigation property
    public virtual ICollection<FactInternetSales> FactInternetSales { get; set; } = new List<FactInternetSales>();
}
