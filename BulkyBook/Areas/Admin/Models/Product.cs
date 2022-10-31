using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulkyBook.Areas.Admin.Models;

public class Product
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key] public double Id { get; set; }

    [Required]
    [Display(Name = "Title of the Book")]
    [MaxLength(60)]
    public string Title { get; set; }

    public string Description { get; set; }

    [Required] [StringLength(13)] public string ISBN { get; set; }

    [Required]
    [MaxLength(10)]
    [MinLength(60)]
    public string Author { get; set; }

    [Range(1, 10000)] public double ListPrice { get; set; }

    [Required] [Range(1, 10000)] public double Price { get; set; }


    [Required]
    [Range(1, 10000)]
    [Display(Name = "Price of 50s Prodects")]
    public double Price50 { get; set; }

    [Required]
    [Range(1, 10000)]
    [Display(Name = "Price of 100s Prodects")]
    public double Price100 { get; set; }

    public string ImageUrl { get; set; }


    [Required] public double CategeryId { get; set; }
    [ForeignKey("CategeryId")] public Categery Categery { get; set; }
    
    [Required] public double CoverTypeId { get; set; }
    [ForeignKey("CoverTypeId")] public CoverType CoverType { get; set; }
}