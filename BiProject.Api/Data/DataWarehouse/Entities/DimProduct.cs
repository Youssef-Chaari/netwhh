using System;
using System.Collections.Generic;

namespace BiProject.Api.Data.DataWarehouse.Entities;

public partial class DimProduct
{
    public int ProductSk { get; set; }

    public int? ProductKey { get; set; }

    public string? EnglishProductName { get; set; }

    public string? ProductNumber { get; set; }

    public string? Color { get; set; }

    public decimal? StandardCost { get; set; }

    public decimal? ListPrice { get; set; }
    
    // Navigation property
    public virtual ICollection<FactInternetSales> FactInternetSales { get; set; } = new List<FactInternetSales>();
}
