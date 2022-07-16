using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsApp.Models
{

    public class Article
    {
        [Key]
        [StringLength(50)]
        public string ArticleID { get; set; }
        [Required]
        public string ArticleTitle { get; set; }
        [Column(TypeName = "ntext")]
        public string ArticleDescription { get; set; }
        [Column(TypeName = "ntext")]
        public string ArticleContent { get; set; }
        public string ImageUrl { get; set; }
        [Display(Name = "Publish Time")]
        public DateTime PublishTime { get; set; }

        [Display(Name = "Views count")]
        [Column(TypeName = "bigint")]
        [DefaultValue(0)]
        public Int64 ViewsCount { get; set; }
        public string AuthorName { get; set; }
        public Account PublishBy { get; set; }
        [Display(Name = "Tags")]
        public IList<NewsCategory> NewsCategories { get; set; }



    }
}
