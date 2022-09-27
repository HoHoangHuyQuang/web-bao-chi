using NewsApp.Interfaces;
using NewsApp.Models;

namespace NewsApp.DAL
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }


    }
}
