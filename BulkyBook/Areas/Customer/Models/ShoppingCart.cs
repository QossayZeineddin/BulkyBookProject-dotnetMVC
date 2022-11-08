using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BulkyBook.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using BulkyBook.PublicModels;

namespace BulkyBook.Areas.Customer.Models
{
    public class ShoppingCart
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }

        [Range(1, 1000, ErrorMessage = "please enter a number btween 1 and 1000")]
        public int count { get; set; }

        public int productId { get; set; }

        [ForeignKey("productId")]
        [ValidateNever]
        public Product Product { get; set; }

        public string applecationUserId { get; set; }

        [ForeignKey("applecationUserId")]
        [ValidateNever]
        public ApplecationUser applecationUsers { get; set; }

        [NotMapped] public double priceTotal { get; set; }
    }
}