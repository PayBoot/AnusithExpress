using AnousithExpress.Data.SingleViewModels;
using System.Web.Mvc;

namespace AnusithExpress.web.mvc.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            
            return View();
        }
        public ActionResult CreateUpdateItem()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateUpdateItem(ItemSingleModel model)
        {
            return View();
        }

        public ActionResult AllItem()
        {
            return View();
        }


        public ActionResult Customer()
        {
            //GridView
            return View();
        }

        public ActionResult Customer_Items()
        {
            //ListView
            return View();
        }
        public ActionResult PrintBarCode()
        {
            return View();
        }
        public ActionResult BarCodeRegister()
        {
            return View();
        }
        public ActionResult Consolidate()
        {
            return View();
        }

    }
}