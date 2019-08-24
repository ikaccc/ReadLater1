using System.ComponentModel.DataAnnotations;

namespace ReadLater.Entities
{
    public class Category : EntityBase
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Category Name")]
        [StringLength(maximumLength: 50)]
        public string Name { get; set; }
    }
}
