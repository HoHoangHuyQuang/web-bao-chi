using Microsoft.AspNetCore.Http;
using NewsApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NewsApp.Areas.Admin.Models
{
    public class ArticleViewsModel
    {
        [Required]
        public string ArticleID { get; set; }
        [Required]
        public string ArticleTitle { get; set; }
        public string ArticleDescription { get; set; }
        public string ArticleContent { get; set; }
        public string ImageUrl { get; set; }
        public IFormFile Image { get; set; }
        public DateTime PublishTime { get; set; }
        public Int64 ViewsCount { get; set; }
        public string AuthorName { get; set; }
        public IList<NewsCategory> NewsCategories { get; set; }
    }
}
