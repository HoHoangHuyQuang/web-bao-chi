using NewsApp.Interfaces;
using NewsApp.Models;
using System.Threading.Tasks;

namespace NewsApp.DAL
{
    public class NewsCategoryRepository : GenericRepository<NewsCategory>, INewsCategoryRepository
    {
        public NewsCategoryRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<NewsCategory> GetById(string articleID, string CategoryID)
        {

            return await dbSet.FindAsync(articleID, CategoryID);
        }
        public async Task<bool> Add(Article article, Category category)
        {
            var newsCategory = new NewsCategory(article.ArticleID, category.CategoryID, article, category);
            await dbSet.AddAsync(newsCategory);

            return true;
        }
    }
}
