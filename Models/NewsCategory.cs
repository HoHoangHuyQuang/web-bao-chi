using System.ComponentModel.DataAnnotations;
namespace NewsApp.Models
{

    public class NewsCategory
    {

        [StringLength(50)]
        public string ArticleID { get; set; }
        [StringLength(50)]
        public string CategoryID { get; set; }
        public Article Article { get; set; }

        public Category Category { get; set; }

        public NewsCategory()
        {
        }

        public NewsCategory(string articleID, string categoryID, Article article, Category category)
        {
            ArticleID = articleID;
            CategoryID = categoryID;
            Article = article;
            Category = category;
        }
    }
}
