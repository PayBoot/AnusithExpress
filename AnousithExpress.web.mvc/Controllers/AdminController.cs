using AnousithExpress.DataEntry.Implimentation;
using AnousithExpress.DataEntry.ViewModels.Admin;
using System.Web.Mvc;

namespace AnousithExpress.web.mvc.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        private ProductService _product;
        public AdminController(ProductService product)
        {
            _product = product;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetNotification()
        {
            double result = _product.GetNotification();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Account
        /// </summary>
        /// <returns></returns>
        public ActionResult Account()
        {
            return View();
        }
        public ActionResult AccountCreateUpdate()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AccountCreateUpdate(AccountModel model)
        {
            return View();
        }
        public ActionResult AccountDelete(int accountId)
        {
            return RedirectToAction("Account", "Admin");
        }

        ////////////////////////////////////////////////////
        //////
        /////////////// Customer

        public ActionResult Customer()
        {
            return View();
        }


    }
}