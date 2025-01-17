using Sigma.ApplicationTracking.Application.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Sigma.ApplicationTracking.Application.Test.DTOs
{
    public class CreateOrUpdateApplicantDTOTests
    {
        private CreateOrUpdateApplicantDTO GetValidDto()
        {
            return new CreateOrUpdateApplicantDTO
            {
                Email = "pk@mail.com",
                FirstName = "Pashupati",
                LastName = "Khanal",
                PhoneNumber = "9842180466",
                PreferredTime = DateTime.Now,
                LinkedInProfileUrl = "https://www.linkedin.com/in/pk",
                Comment = "This one."
            };
        }

        [Fact]
        public void ValidDto_ShouldPassValidation()
        {
            var dto = GetValidDto();
            var validationResults = ValidateModel(dto);
            Assert.Empty(validationResults);
        }

        [Fact]
        public void InvalidEmail_ShouldFailValidation()
        {
            var dto = GetValidDto();
            dto.Email = "invalid-email";
            var validationResults = ValidateModel(dto);
            Assert.Contains(validationResults, v => v.ErrorMessage.Contains("Invalid email format"));
        }

        [Fact]
        public void MissingRequiredFields_ShouldFailValidation()
        {
            var dto = GetValidDto();
            dto.FirstName = null;
            var validationResults = ValidateModel(dto);
            Assert.Contains(validationResults, v => v.ErrorMessage.Contains("First name cannot be empty"));
        }

        [Fact]
        public void StringLengthExceeded_ShouldFailValidation()
        {
            var dto = GetValidDto();
            dto.FirstName = new string('a', 101); // 101 characters
            var validationResults = ValidateModel(dto);
            Assert.Contains(validationResults, v => v.ErrorMessage.Contains("First name cannot exceed 100 characters"));
        }

        private static ValidationResult[] ValidateModel(object model)
        {
            var validationContext = new ValidationContext(model);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(model, validationContext, results, true);
            return results.ToArray();
        }
    }
}
