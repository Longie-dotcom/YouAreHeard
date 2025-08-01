using Microsoft.AspNetCore.Mvc;
using YouAreHeard.Models.DTOs;
using YouAreHeard.Services.Interfaces;

namespace YouAreHeard.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("dashboard")]
        public IActionResult GetDashboardSummary()
        {
            AdminDashboardDTO dashboard = _adminService.GetDashboardSummary();
            return Ok(dashboard);
        }
    }
}
