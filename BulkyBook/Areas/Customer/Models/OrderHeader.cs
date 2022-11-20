using BulkyBook.PublicModels;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulkyBook.Areas.Customer.Models
{
    public class OrderHeader
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
 
        public int id { get; set; }
        [Required]
        public string applecationUserId { get; set; }
        [ForeignKey("applecationUserId")]
        [ValidateNever]
        public ApplecationUser applecationUser { get; set; }

        [Required]
        public DateTime orderDate { get; set; }
        public DateTime shoppingDate { get; set; }
        public double orderTotal { get; set; }
        public string? orderStatus { get; set; }
        public string? paymentStatus { get; set; }
        public string trackingNumber { get; set; }
        public string? carrier { get; set; }
        public DateTime paymentDate { get; set; }
        public DateTime paymentDueDate { get; set; }

        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }

        [Required]
        public string name { get; set; }
        [Required]
        public string city { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string streetAddress { get; set; }
        [Required]
        public string postalCode { get; set; }
        [Required]
        public string State { get; set; }
    }
}
