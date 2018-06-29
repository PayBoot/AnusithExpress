using AnousithExpress.DataEntry.Implimentation;
using System.Web.Mvc;

namespace AnousithExpress.web.mvc.Controllers
{
    public class HomeController : Controller
    {
        private DatabaseInit data;
        public HomeController(DatabaseInit d)
        {
            data = d;

        }
        public ActionResult Datainit()
        {
            bool s = data.InitiateData();
            return View(s);

        }
    }
}