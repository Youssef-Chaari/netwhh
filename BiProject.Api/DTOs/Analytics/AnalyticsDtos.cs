namespace BiProject.Api.DTOs.Analytics
{
    public class SalesByPeriodDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; } = string.Empty;
        public decimal TotalSales { get; set; }
        public decimal TotalGrossMargin { get; set; }
        public decimal TotalCost { get; set; }
        public int OrderCount { get; set; }
    }

    public class SalesByChannelDto
    {
        public string Channel { get; set; } = string.Empty; // "Online" or "Reseller"
        public decimal TotalSales { get; set; }
        public int OrderCount { get; set; }
    }

    public class SalesByCustomerDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public decimal TotalSales { get; set; }
        public decimal TotalGrossMargin { get; set; }
        public int OrderCount { get; set; }
    }

    public class SalesByProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal TotalSales { get; set; }
        public decimal TotalGrossMargin { get; set; }
        public int QuantitySold { get; set; }
    }

    public class KpiDto
    {
        public decimal TotalSales { get; set; }
        public decimal TotalGrossMargin { get; set; }
        public decimal AverageMarginRate { get; set; }
        public decimal AverageOrderValue { get; set; }
        public int TotalOrders { get; set; }
        public int TotalUniqueCustomers { get; set; }
        public int TotalProductsSold { get; set; }
    }
}
