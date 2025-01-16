using Sigma.ApplicationTracking.Application.DTOs;

namespace Sigma.ApplicationTracking.Application.Interfaces.Services
{
    public interface IApplicantService
    {
        Task<CreateOrUpdateApplicantDTO> CreateOrEditApplicant(CreateOrUpdateApplicantDTO createOrUpdateDto);
    }
}
