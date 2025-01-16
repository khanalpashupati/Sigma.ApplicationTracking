using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sigma.ApplicationTracking.Application.DTOs
{
    public class CreateOrUpdateApplicantDTO
    {
        [Required(ErrorMessage = "Email cannot be empty, it is required.")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters.")]
        [RegularExpression(@"^[^\s@]+@[^\s@]+\.[^\s@]+$", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name cannot be empty, it is required.")]
        [StringLength(100, ErrorMessage = "First name cannot exceed 100 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name cannot be empty, it is required.")]
        [StringLength(100, ErrorMessage = "Last name cannot exceed 100 characters.")]
        public string LastName { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Phone number cannot exceed 100 characters.")]
        public string? PhoneNumber { get; set; } = string.Empty;

        public DateTime? PreferredTime { get; set; }

        [StringLength(255, ErrorMessage = "LinkedIn profile Url cannot exceed 255 characters.")]
        public string? LinkedInProfileUrl { get; set; } = string.Empty;

        [Required(ErrorMessage ="Comment is a required field.")]
        public string Comment { get; set; } = string.Empty;

        [JsonIgnore]
        public bool? IsNewApplicant { get; set; }
    }
}
