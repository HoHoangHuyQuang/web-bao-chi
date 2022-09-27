using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NewsApp.Models
{
    public class Category
    {
        [Key]
        [StringLength(50)]
        public string CategoryID { get; set; }
        [Required]
        public string CategoryName { get; set; }
        public IList<NewsCategory> NewsCategories { get; set; }


    }
}
