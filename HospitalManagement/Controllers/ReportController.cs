using Hospital.Core.Service;
using Hospital.Model.Dto;
using Hospital.Model.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [Authorize(Roles = "User")]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetReports(int userId)
        {
            var reports = await _reportService.GetUserReportsAsync(userId);
            return Ok(reports);
        }


        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> CreateReport([FromBody] ReportCreateDto reportDto)
        {
            int reportId = await _reportService.CreateReportAsync(reportDto);
            return CreatedAtAction(nameof(GetReports), new { reportId }, "Report created successfully.");
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{reportId}/{userId}")]
        public async Task<IActionResult> UpdateReport(int reportId, int userId, [FromBody] ReportUpdateDto reportDto)
        {
            bool updated = await _reportService.UpdateReportAsync(reportId, reportDto, userId);
            return updated ? Ok("Report updated successfully.") : Forbid("You can only edit reports with status 'Write'.");
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("{reportId}/{userId}")]
        public async Task<IActionResult> DeleteReport(int reportId, int userId)
        {
            bool deleted = await _reportService.DeleteReportAsync(reportId, userId);
            return deleted ? Ok("Report deleted successfully.") : Forbid("You can only delete reports with status 'Write'.");
        }

    }
}
