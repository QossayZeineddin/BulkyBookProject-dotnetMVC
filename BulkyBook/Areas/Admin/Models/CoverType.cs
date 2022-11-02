using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulkyBook.Areas.Admin.Models;

public class CoverType
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key] public int id { get; set; }

    [Required]
    [Display(Name = "Name Of Cover Type")]
    [MaxLength(55)]
    public string CoverName { get; set; }
}