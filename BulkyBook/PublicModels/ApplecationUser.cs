using BulkyBook.Areas.Admin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulkyBook.PublicModels
{
    public class ApplecationUser : IdentityUser
    {
        [Required]
        public string name { get; set; }
        [Required]
        public string city { get; set; }
        public string? streetAddress { get; set; }
        public string? postalCode { get; set; }
        public int? companyId { get; set; }

        public string? State { get; set; }

        [ValidateNever]
        [ForeignKey("companyId")]
        public Company Company { get; set; }
    }
}  
