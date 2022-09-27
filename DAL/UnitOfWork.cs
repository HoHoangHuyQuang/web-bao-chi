using NewsApp.Interfaces;
using System;
using System.Threading.Tasks;

namespace NewsApp.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        private IAccountRepository _account;
        private IArticleRepository _article;
        private ICategoryRepository _category;
        private INewsCategoryRepository _newsCategory;
        private IRoleRepository _role;
        private IContactRepository _contact;

        public IAccountRepository AccountRepo
        {
            get
            {
                if (this._account == null)
                {
                    this._account = new AccountRepository(_context);
                }
                return _account;
            }
        }
        public IArticleRepository ArticleRepo
        {
            get
            {
                if (this._article == null)
                {
                    this._article = new ArticleRepository(_context);
                }
                return _article;
            }
        }
        public ICategoryRepository CategoryRepo
        {
            get
            {
                if (this._category == null)
                {
                    this._category = new CategoryRepository(_context);
                }
                return _category;
            }
        }
        public INewsCategoryRepository NewsCategoryRepo
        {
            get
            {
                if (this._newsCategory == null)
                {
                    this._newsCategory = new NewsCategoryRepository(_context);
                }
                return _newsCategory;
            }
        }
        public IRoleRepository RoleRepo {
            get
            {
                if (this._role == null)
                {
                    this._role = new RoleRepository(_context);
                }
                return _role;
            }
        }
        public IContactRepository ContactRepo {
            get
            {
                if (this._contact == null)
                {
                    this._contact = new ContactRepository(_context);
                }
                return _contact;
            }
        }





        public UnitOfWork(AppDbContext context)
        {
            _context = context;

        }


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
