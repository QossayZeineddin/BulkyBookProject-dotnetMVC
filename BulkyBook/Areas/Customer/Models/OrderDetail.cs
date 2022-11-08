using BulkyBook.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulkyBook.Areas.Customer.Models
{
    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        
        [Required]
        public int orderId { get; set; }
        [ForeignKey("orderId")]
        [ValidateNever]
        public OrderHeader orderHeader { get; set; }

        [Required]
        public int productId { get; set; }
        [ForeignKey("productId")]
        [ValidateNever]
        public Product product { get; set; }

        public int count { get; set; }
        public double price { get; set; }
    }
}
