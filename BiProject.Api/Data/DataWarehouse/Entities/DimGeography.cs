using System;
using System.Collections.Generic;

namespace BiProject.Api.Data.DataWarehouse.Entities;

public partial class DimGeography
{
    public int GeographySk { get; set; }

    public int? GeographyKey { get; set; }

    public string? City { get; set; }

    public string? StateProvinceName { get; set; }

    public string? EnglishCountryRegionName { get; set; }

    public string? PostalCode { get; set; }

    public string? StateProvinceCode { get; set; }

    public string? CountryRegionCode { get; set; }

    public string? ContinentName { get; set; }
    
    // Navigation property
    public virtual ICollection<FactInternetSales> FactInternetSales { get; set; } = new List<FactInternetSales>();
}
