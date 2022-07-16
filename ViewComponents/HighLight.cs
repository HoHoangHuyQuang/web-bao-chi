using Microsoft.AspNetCore.Mvc;
using NewsApp.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace NewsApp.ViewComponents
{
    public class HighLight : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public HighLight(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            var highLight_lst = await _unitOfWork.ArticleRepo.GetAllByCategory("C0001");

            return View("/Views/Shared/Components/HighLight/_HighLightComp.cshtml", highLight_lst.ToList());


        }
    }
}
