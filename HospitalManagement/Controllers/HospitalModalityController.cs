using Hospital.Core.Service;
using Hospital.Model.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HospitalModalityController : ControllerBase
    {
        private readonly IHospitalModalityService _service;

        public HospitalModalityController(IHospitalModalityService service)
        {
            _service = service;
        }
        
        [HttpGet("hospitals")]
        public async Task<IActionResult> GetHospitals()
        {
            var hospitals = await _service.GetHospitalsAsync();
            return Ok(hospitals);
        }
        
        [HttpPost("hospitals")]
        public async Task<IActionResult> CreateHospital([FromBody] HospitalDto hospitalDto)
        {
            await _service.AddHospitalAsync(hospitalDto);
            return Ok("Hospital created successfully.");
        }

        [HttpDelete("hospitals/{id}")]
        public async Task<IActionResult> DeleteHospital(int id)
        {
            await _service.SoftDeleteHospitalAsync(id);
            return Ok("Hospital deleted and affected patients updated.");
        }
        
        [HttpGet("modalities")]
        public async Task<IActionResult> GetModalities()
        {
            var modalities = await _service.GetModalitiesAsync();
            return Ok(modalities);
        }
        
        [HttpPost("modalities")]
        public async Task<IActionResult> CreateModality([FromBody] ModalityDto modalityDto)
        {
            await _service.AddModalityAsync(modalityDto);
            return Ok("Modality created successfully.");
        }
        
        [HttpDelete("modalities/{id}")]
        public async Task<IActionResult> DeleteModality(int id)
        {
            await _service.SoftDeleteModalityAsync(id);
            return Ok("Modality deleted and affected patients updated.");
        }
    }
}
