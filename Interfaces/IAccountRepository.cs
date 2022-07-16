using NewsApp.Models;
using System.Threading.Tasks;

namespace NewsApp.Interfaces
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        public Task<Account> LoginValidate(string Username, string Password);

    }
}
