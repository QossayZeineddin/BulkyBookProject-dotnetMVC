using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulkyBook.Areas.Admin.Models
{
    public class Categery
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Categery ID")]
        public int id { get; set; }

        [Required]
        [Display(Name = "Categery Name")]
        [StringLength(60, MinimumLength = 3)]
        public string name { get; set; }

        [Range(1, 1000, ErrorMessage = "more than number!")]
        [Display(Name = "Categery Order")]
        public string displyOrder { get; set; }

        [Required]
        [Display(Name = "Categery Created Time")]
        public DateTime createdDateTime { get; set; } = DateTime.Now;
    }
}