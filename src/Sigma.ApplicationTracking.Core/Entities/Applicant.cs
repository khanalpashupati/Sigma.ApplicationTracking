using System.ComponentModel.DataAnnotations;

namespace Sigma.ApplicationTracking.Core.Entities
{
    public class Applicant
    {
        [Key]
        public long Id { get; set; }

        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [StringLength(50)]
        public string? PhoneNumber { get; set; } =string.Empty;

        public DateTime? PreferredTime { get; set; }

        [StringLength(255)]
        public string? LinkedInProfileUrl { get; set; } = string.Empty;

        public string Comment {  get; set; } = string.Empty;



//• * First name
//• * Last name
//• Phone number
//• * Email
//• Time interval when it’s better to call(in case a call is needed)
//• LinkedIn profile URL
//• GitHub profile URL
//• * Free text comment
    }
}
