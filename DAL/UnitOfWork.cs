using NewsApp.Interfaces;
using System;
using System.Threading.Tasks;

namespace NewsApp.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;


        public UnitOfWork(AppDbContext context)
        {
            _context = context;

            if (AccountRepo == null)
            {
                AccountRepo = new AccountRepository(_context);
            }
            if (RoleRepo == null)
            {
                RoleRepo = new RoleRepository(_context);
            }
            if (CategoryRepo == null)
            {
                CategoryRepo = new CategoryRepository(_context);
            }
            if (ArticleRepo == null)
            {
                ArticleRepo = new ArticleRepository(_context);
            }
            if (NewsCategoryRepo == null)
            {
                NewsCategoryRepo = new NewsCategoryRepository(_context);
            }
            if (ContactRepo == null)
            {
                ContactRepo = new ContactRepository(_context);
            }

        }

        public IAccountRepository AccountRepo { get; private set; }
        public IArticleRepository ArticleRepo { get; private set; }
        public ICategoryRepository CategoryRepo { get; private set; }
        public INewsCategoryRepository NewsCategoryRepo { get; private set; }
        public IRoleRepository RoleRepo { get; private set; }
        public IContactRepository ContactRepo { get; private set; }
        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
