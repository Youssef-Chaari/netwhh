using BiProject.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BiProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [HttpGet("sales/period")]
        public async Task<IActionResult> GetSalesByPeriod()
        {
            var data = await _analyticsService.GetSalesByPeriodAsync();
            return Ok(data);
        }

        [HttpGet("sales/channel")]
        public async Task<IActionResult> GetSalesByChannel()
        {
            var data = await _analyticsService.GetSalesByChannelAsync();
            return Ok(data);
        }

        [HttpGet("sales/customer")]
        public async Task<IActionResult> GetSalesByCustomer()
        {
            var data = await _analyticsService.GetSalesByCustomerAsync();
            return Ok(data);
        }

        [HttpGet("sales/product")]
        public async Task<IActionResult> GetSalesByProduct()
        {
            var data = await _analyticsService.GetSalesByProductAsync();
            return Ok(data);
        }

        [HttpGet("sales/category")]
        public async Task<IActionResult> GetSalesByCategory()
        {
            var data = await _analyticsService.GetSalesByCategoryAsync();
            return Ok(data);
        }

        [HttpGet("kpis")]
        public async Task<IActionResult> GetKpis()
        {
            var data = await _analyticsService.GetKpisAsync();
            return Ok(data);
        }
    }
}
