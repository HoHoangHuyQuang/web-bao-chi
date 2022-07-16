using Microsoft.EntityFrameworkCore;
using NewsApp.Models;

namespace NewsApp.DAL
{
    public class AppDbContext : DbContext
    {

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<NewsCategory> NewsCategories { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<NewsCategory>().HasKey(e => new { e.ArticleID, e.CategoryID });


            modelBuilder.Entity<NewsCategory>()
                .HasOne<Article>(nc => nc.Article)
                .WithMany(a => a.NewsCategories)
                .HasForeignKey(nc => nc.ArticleID);

            modelBuilder.Entity<NewsCategory>()
                .HasOne<Category>(nc => nc.Category)
                .WithMany(c => c.NewsCategories)
                .HasForeignKey(nc => nc.CategoryID);



        }
    }
}
