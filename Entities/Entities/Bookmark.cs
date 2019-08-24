using System;
using System.ComponentModel.DataAnnotations;

namespace ReadLater.Entities
{
    public class Bookmark : EntityBase
    {
        [Key]
        public int ID { get; set; }

        [StringLength(maximumLength: 500)]
        public string URL { get; set; }
        [Display(Name = "Short Description")]
        public string ShortDescription { get; set; }

        [Display(Name = "Category")]
        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }

        [Display(Name = "Create Date")]
        public DateTime CreateDate { get; set; }

        public int Rank { get; set; } = 0;

        public string UserId { get; set; }
    }
}
