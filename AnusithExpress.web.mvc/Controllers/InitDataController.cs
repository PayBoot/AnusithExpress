using AnousithExpress.Data.Implementation;
using System.Web.Mvc;

namespace AnusithExpress.web.mvc.Controllers
{
    public class InitDataController : Controller
    {
        // GET: InitData
        private InitiatingService _service;
        public InitDataController(InitiatingService service)
        {
            _service = service;
        }
        public ActionResult Database()
        {
            bool result = _service.InitiateData();
            ViewBag.Result = result;
            return View();
        }
    }
}