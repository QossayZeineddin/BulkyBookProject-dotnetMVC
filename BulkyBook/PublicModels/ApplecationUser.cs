using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BulkyBook.PublicModels
{
    public class ApplecationUser : IdentityUser
    {
        [Required]
        public string name { get; set; }
        public string city { get; set; }
        public string? streetAddress { get; set; }

        public string? postalCode { get; set; }
    }
}
