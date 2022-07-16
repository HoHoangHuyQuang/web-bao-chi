using NewsApp.Interfaces;
using NewsApp.Models;
using System.Threading.Tasks;

namespace NewsApp.DAL
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Role> GetById(int? id)
        {
            return await dbSet.FindAsync(id);
        }
    }
}
