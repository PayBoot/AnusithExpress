using AnousithExpress.Data.Implementation;
using AnousithExpress.Data.SingleViewModels;
using System.Web.Mvc;

namespace AnusithExpress.web.mvc.Controllers
{
    public class CustomerController : Controller
    {
        private ItemService _itemServ;
        public CustomerController(ItemService itemService)
        {
            _itemServ = itemService;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateItem()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateItem(ItemSingleModel model)
        {
            if (ModelState.IsValid)
            {
                _itemServ.Create(model);
                ViewBag.Message = "ເພີ່ມລາຍການສະເລັດ";
                return View();
            }
            else
            {
                return View();
            }

        }

        public ActionResult ViewItems()
        {
            int CustId = (int)Session["UserId"];
            var model = _itemServ.GetForCustomer(CustId);
            return View(model);
        }
        public ActionResult DeleteItem(int id)
        {
            ViewBag.Message = "ລືບລາຍການສຳເລັດ";
            return RedirectToAction("ViewItems");
        }
    }
}