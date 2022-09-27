using Microsoft.AspNetCore.Mvc;
using NewsApp.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace NewsApp.ViewComponents
{
    public class HighLightEvent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public HighLightEvent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            var highLight_lst = await _unitOfWork.ArticleRepo.GetHighLightByCategory("C0002");

            return View("/Views/Shared/Components/HighLightEvent/_EventComp.cshtml", highLight_lst.Take(6).ToList());


        }
    }
}
