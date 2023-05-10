using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationProject.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize(Policy = "IsNotDeletedPolicy")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
