using Microsoft.EntityFrameworkCore;
using NewsApp.Interfaces;
using NewsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NewsApp.DAL
{
    public class ArticleRepository : GenericRepository<Article>, IArticleRepository
    {
        public ArticleRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Article>> GetAll()
        {
            var articles = dbSet.Include(a => a.NewsCategories)
                                .ThenInclude(n => n.Category)
                                .OrderByDescending(a => a.PublishTime);

            return await articles.ToListAsync();
        }

        public override async Task<Article> GetById(string id)
        {
            var article = await dbSet
                                 .Include(a => a.NewsCategories)
                                 .ThenInclude(n => n.Category)
                                 .Where(a => a.ArticleID.Equals(id))
                                 .FirstOrDefaultAsync();

            return article;
        }
        public async Task<IEnumerable<Article>> GetAllByCategory(string cateid)
        {
            var articles = _context.NewsCategories
                            .Include(a => a.Article)
                            .Where(nc => nc.CategoryID.Equals(cateid))
                            .Select(nc => nc.Article)
                            .OrderByDescending(a => a.PublishTime);


            return await articles.ToListAsync();
        }
        public async Task<Category> GetArticleTag(string? id)
        {
            var cate = _context.NewsCategories
                            .Where(a => a.ArticleID.Equals(id))
                            .Where(nc => !nc.CategoryID.Equals("C0001"))
                            .Select(c => c.Category);


            return await cate.FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<Article>> GetRelatedArticle(string baseid)
        {
            var cate = _context.NewsCategories
                            .Where(a => a.ArticleID.Equals(baseid))
                            .Where(nc => !nc.CategoryID.Equals("C0001"))
                            .Select(c => c.Category).FirstOrDefault();

            var related = dbSet.Include(a => a.NewsCategories)
                               .Where(e => e.NewsCategories.Any(nc => nc.CategoryID.Equals(cate.CategoryID)))
                               .Where(e => e.ArticleID != baseid);

            return await related.ToListAsync();
        }

        public async Task<IEnumerable<Article>> GetMostRead(string? categoryid)
        {
            var articles = _context.NewsCategories
                            .Include(a => a.Article)
                            .Where(nc => nc.CategoryID.Equals(categoryid))
                            .Select(nc => nc.Article)
                            .OrderByDescending(a => a.ViewsCount);

            return await articles.ToListAsync();
        }
        public async Task<IEnumerable<Article>> GetHighLightByCategory(string cateid)
        {
            List<string> combine = new List<string> { "C0001", cateid };

            var articles = dbSet.Include(a => a.NewsCategories)
                               .Where(e => e.NewsCategories.All(nc => combine.Contains(nc.CategoryID)))
                               .OrderByDescending(a => a.PublishTime);


            return await articles.ToListAsync();
        }
        public string GenerateID()
        {
            var articles = dbSet.ToList();
            Regex regex = new Regex(@"\d+", RegexOptions.Compiled);
            Int64 max = 0;
            foreach (var article in articles)
            {
                Match match = regex.Match(article.ArticleID);
                if (match.Value != "")
                {
                    Int64 id = int.Parse(match.Value);
                    if (id > max)
                    {
                        max = id;
                    }
                }
            }
            max += 1;
            return "ART" + max.ToString().PadLeft(7, '0');
        }
    }
}

