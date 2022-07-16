using NewsApp.Models;
using System.Threading.Tasks;

namespace NewsApp.Interfaces
{
    public interface IContactRepository : IGenericRepository<Contact>
    {
        public Task<Contact> GetById(int? id);
    }
}
