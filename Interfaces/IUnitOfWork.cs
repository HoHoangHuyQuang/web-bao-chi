using System;
using System.Threading.Tasks;

namespace NewsApp.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountRepository AccountRepo { get; }
        IArticleRepository ArticleRepo { get; }
        ICategoryRepository CategoryRepo { get; }
        INewsCategoryRepository NewsCategoryRepo { get; }
        IRoleRepository RoleRepo { get; }
        IContactRepository ContactRepo { get; }
        Task CompleteAsync();
    }
}
