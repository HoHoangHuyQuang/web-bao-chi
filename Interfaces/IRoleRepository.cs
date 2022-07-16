using NewsApp.Models;
using System.Threading.Tasks;

namespace NewsApp.Interfaces
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        public Task<Role> GetById(int? id);


    }

}
