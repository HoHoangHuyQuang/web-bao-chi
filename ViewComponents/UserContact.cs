using Microsoft.AspNetCore.Mvc;
using NewsApp.Interfaces;
using NewsApp.Models;
using System.Threading.Tasks;

namespace NewsApp.ViewComponents
{
    public class UserContact : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserContact(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            Contact con = new Contact();

            return await Task.Run(() => View("/Views/Shared/Components/UserContact/_FooterComp.cshtml", con));
        }

    }
}
