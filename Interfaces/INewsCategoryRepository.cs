using NewsApp.Models;
using System.Threading.Tasks;

namespace NewsApp.Interfaces
{
    public interface INewsCategoryRepository : IGenericRepository<NewsCategory>
    {
        public Task<NewsCategory> GetById(string articleID, string CategoryID);
        public Task<bool> Add(Article article, Category category);
    }
}
