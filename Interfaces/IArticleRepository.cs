using NewsApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsApp.Interfaces
{
    public interface IArticleRepository : IGenericRepository<Article>
    {

        public Task<IEnumerable<Article>> GetAllByCategory(string cateid);
        public Task<Category> GetArticleTag(string? id);
        public Task<IEnumerable<Article>> GetMostRead(string? categoryid);
        public Task<IEnumerable<Article>> GetHighLightByCategory(string cateid);
        public Task<IEnumerable<Article>> GetRelatedArticle(string baseid);
        public string GenerateID();
    }
}
