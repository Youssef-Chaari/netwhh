namespace BiProject.Api.DTOs.Analytics
{
    public class SalesByCategoryDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public decimal TotalSales { get; set; }
        public int QuantitySold { get; set; }
    }
}
