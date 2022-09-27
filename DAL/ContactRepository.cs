using NewsApp.Interfaces;
using NewsApp.Models;
using System.Threading.Tasks;

namespace NewsApp.DAL
{
    public class ContactRepository : GenericRepository<Contact>, IContactRepository
    {
        public ContactRepository(AppDbContext context) : base(context)
        {
        }
        public async Task<Contact> GetById(int? id)
        {
            return await dbSet.FindAsync(id);
        }
    }
}
