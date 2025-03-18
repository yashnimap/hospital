using Hospital.Core.Service;
using Hospital.Model.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    [Route("api/[controller]")]    
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientRepository)
        {
            _patientService = patientRepository;
        }

        //[HttpGet("{userId}")]
        //public async Task<IActionResult> GetPatients(int userId)
        //{
        //    var patients = await _patientService.GetPatientsAsync(userId);
        //    if (patients == null || !patients.Any())
        //    {
        //        return NotFound(new { message = "No patients found for the given user." });
        //    }
        //    return Ok(patients);
        //}

        [Authorize(Roles = "Admin,SuperAdmin,User")]
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<PatientDto>>> GetPatients(int userId)
        {
            var patients = await _patientService.GetPatientsAsync(userId);
            return Ok(patients);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<ActionResult<int>> CreatePatient([FromBody] CreatePatientDto patientDto)
        {
            var patientId = await _patientService.CreatePatientAsync(patientDto);
            return CreatedAtAction(nameof(GetPatients), new { userId = patientId }, patientId);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] CreatePatientDto patientDto)
        {
            var success = await _patientService.UpdatePatientAsync(id, patientDto);
            if (!success) return NotFound();
            return NoContent();
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var success = await _patientService.DeletePatientAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
