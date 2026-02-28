using System;
using System.Collections.Generic;

namespace BiProject.Api.Data.DataWarehouse.Entities;

public partial class DimCustomer
{
    public int CustomerSk { get; set; }

    public int? CustomerKey { get; set; }

    public int? GeographyKey { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? EmailAddress { get; set; }

    public string? FullName { get; set; }

    public string? EmailDomain { get; set; }
    
    // Navigation property
    public virtual ICollection<FactInternetSales> FactInternetSales { get; set; } = new List<FactInternetSales>();
}
