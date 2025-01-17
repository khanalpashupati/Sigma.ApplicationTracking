using Sigma.ApplicationTracking.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Sigma.ApplicationTracking.Domain.Test.Entities
{
    public class ApplicantTest
    {
        [Fact]
        public void DefaultValues_Should_Be_Set_Correctly()
        {
            // Arrange
            var applicant = new Applicant();

            // Act & Assert
            Assert.Equal(string.Empty, applicant.Email);
            Assert.Equal(string.Empty, applicant.FirstName);
            Assert.Equal(string.Empty, applicant.LastName);
            Assert.Equal(string.Empty, applicant.PhoneNumber);
            Assert.Null(applicant.PreferredTime);
            Assert.Equal(string.Empty, applicant.LinkedInProfileUrl);
            Assert.Equal(string.Empty, applicant.Comment);
        }

        [Fact]
        public void LinkedInProfileUrl_Should_Accept_Null()
        {
            // Arrange
            var applicant = new Applicant();

            // Act
            applicant.LinkedInProfileUrl = null;

            // Assert
            Assert.Null(applicant.LinkedInProfileUrl);
        }
        [Fact]
        public void MissingRequiredFields_ShouldFailValidation()
        {
            var dto = new Applicant
            {
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "123-456-7890",
                PreferredTime = DateTime.Now,
                LinkedInProfileUrl = "https://www.linkedin.com/in/johndoe",
                Comment = "This is a valid comment."
            }; ;
            dto.FirstName = null;
            var validationResults = ValidateModel(dto);
            Assert.Contains(validationResults, v => v.ErrorMessage.Contains("First name cannot be empty"));
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