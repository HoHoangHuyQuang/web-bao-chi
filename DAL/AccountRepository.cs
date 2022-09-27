using Microsoft.EntityFrameworkCore;
using NewsApp.Interfaces;
using NewsApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace NewsApp.DAL
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(AppDbContext context) : base(context)
        {
        }
        public override async Task<Account> GetById(string id)
        {
            var account = await dbSet.Include(a => a.Role)
                                 .Where(a => a.Username.Equals(id))
                                 .FirstOrDefaultAsync();
            return account;

        }

        public async Task<Account> LoginValidate(string Username, string Password)
        {
            var result = await dbSet.Include(a => a.Role)
                                 .Where(a => a.Username.Equals(Username) && a.Password.Equals(Password))
                                 .FirstOrDefaultAsync();

            return result;
        }
    }
}
