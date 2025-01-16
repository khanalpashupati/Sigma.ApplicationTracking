using Microsoft.AspNetCore.Mvc;
using Sigma.ApplicationTracking.Application.DTOs;
using Sigma.ApplicationTracking.Application.Interfaces.Services;

namespace Sigma.ApplicationTracking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicantController : ControllerBase
    {
        private readonly IApplicantService _applicationService;
        private readonly string currentUser = "admin";
        public ApplicantController(IApplicantService applicantService)
        {
            _applicationService = applicantService;
        }

        [HttpPost("CreateOrUpdateApplicant")]
        public async Task<IActionResult> CreateOrUpdateApplicant([FromBody] CreateOrUpdateApplicantDTO createOrUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _applicationService.CreateOrEditApplicant(createOrUpdateDto);
                if (result.IsNewApplicant??true)
                {
                    return StatusCode(StatusCodes.Status201Created, new
                    {
                        message = "Applicant created successfully.",
                        data = result
                    });
                }
                else
                {
                    return Ok(new
                    {
                        message = "Applicant updated successfully.",
                        data = result
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
