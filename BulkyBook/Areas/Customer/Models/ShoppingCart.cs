using System.ComponentModel.DataAnnotations;
using BulkyBook.Areas.Admin.Models;

namespace BulkyBook.Areas.Customer.Models
{

    public class ShoppingCart
    {
        public Product Product { get; set; }
        [Range(1,1000 , ErrorMessage = "please enter a number btween 1 and 1000")]
        public  int count { get; set; }

    }
}