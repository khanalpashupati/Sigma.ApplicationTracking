using Sigma.ApplicationTracking.Application.DTOs;
using Sigma.ApplicationTracking.Application.Interfaces.Services;
using Sigma.ApplicationTracking.Domain.Entities;
using Sigma.ApplicationTracking.Domain.Interface;


namespace Sigma.ApplicationTracking.Application.Services
{
    public class ApplicantService : IApplicantService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApplicantService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Create or update applicant
        public async Task<CreateOrUpdateApplicantDTO> CreateOrEditApplicant(CreateOrUpdateApplicantDTO createOrUpdateDto)
        {
            if (createOrUpdateDto == null)
            {
                throw new ArgumentNullException(nameof(createOrUpdateDto));
            }

            try
            {
                var applicantRepository = _unitOfWork.ApplicantRepository;

                // Check if an applicant with the given email already exists
                var applicant = await applicantRepository.FindAsync(x=>x.Email == createOrUpdateDto.Email);

                if (applicant == null)
                {
                    return await CreateNewApplicant(createOrUpdateDto, applicantRepository);
                }
                else
                {
                    return await UpdateExistingApplicant(createOrUpdateDto, applicant, applicantRepository);
                }
            }
            catch (Exception ex)
            {
                // Log exception if logging is implemented
                throw new InvalidOperationException("An error occurred while creating or updating the applicant.", ex);
            }
        }

        private async Task<CreateOrUpdateApplicantDTO> CreateNewApplicant(CreateOrUpdateApplicantDTO dto, IRepository<Applicant> repository)
        {
            dto.IsNewApplicant = true;

            var applicant = new Applicant
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                PreferredTime = dto.PreferredTime,
                LinkedInProfileUrl = dto.LinkedInProfileUrl,
                Comment = dto.Comment
            };

            await repository.InsertAsync(applicant);
            await _unitOfWork.SaveAsync();

            return dto;
        }

        private async Task<CreateOrUpdateApplicantDTO> UpdateExistingApplicant(CreateOrUpdateApplicantDTO dto, Applicant applicant, IRepository<Applicant> repository)
        {
            dto.IsNewApplicant = false;

            applicant.FirstName = dto.FirstName;
            applicant.LastName = dto.LastName;
            applicant.PhoneNumber = dto.PhoneNumber;
            applicant.PreferredTime = dto.PreferredTime;
            applicant.LinkedInProfileUrl = dto.LinkedInProfileUrl;
            applicant.Comment = dto.Comment;

            await repository.UpdateAsync(applicant);
            await _unitOfWork.SaveAsync();

            return dto;
        }

    }
}
