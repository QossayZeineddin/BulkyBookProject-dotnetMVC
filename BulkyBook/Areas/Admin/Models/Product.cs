using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BulkyBook.Areas.Admin.Models;

public class Product
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [Display(Name = "Title of the Book")]
    [MaxLength(60)]
    public string Title { get; set; }

    public string Description { get; set; }

    [Required] public string ISBN { get; set; }

    [MaxLength(60)]
    [MinLength(10)]
    [Required]
    public string Author { get; set; }

    [Range(1, 10000)]
    [Display(Name = "List Price")]
    public double ListPrice { get; set; }

    [Required]
    [Range(1, 10000)]
    [Display(Name = "Price for 1-50 Prodects")]

    public double Price { get; set; }


    [Required]
    [Range(1, 10000)]
    [Display(Name = "Price for 50-100 Prodects")]
    public double Price50 { get; set; }

    [Required]
    [Range(1, 10000)]
    [Display(Name = "Price of 100+ Prodects")]
    public double Price100 { get; set; }

    [ValidateNever] public string ImageUrl { get; set; }


    [Required]
    [Display(Name = "Categery Id")]
    public int CategeryId { get; set; }

    [ValidateNever]
    [ForeignKey("CategeryId")]
    [Display(Name = "Categery ")]
    public Categery Categery { get; set; }

    [Required]
    [Display(Name = "Cover Type Id")]
    public int CoverTypeId { get; set; }

    [ValidateNever]
    [ForeignKey("CoverTypeId")]
    [Display(Name = "Cover Type")]
    public CoverType CoverType { get; set; }
}