using AnousithExpress.DataEntry.Implimentation;
using System;
using System.Web.Mvc;

namespace AnousithExpress.web.mvc.Controllers
{
    public class DeliveryController : Controller
    {
        // GET: Delivery
        private ProductService _product;
        public DeliveryController(ProductService product)
        {
            _product = product;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ItemsToPickUp(int customerId = 0)
        {
            var model = _product.GetProductToPickUpByCustomerId(customerId);
            return View(model);
        }

        public ActionResult ItemsToSent(int routeId = 0, int timeId = 0, DateTime? dateToDeliver = null)
        {
            var route = _product.GetRoute();
            var time = _product.GetTime();
            ViewBag.Routes = new SelectList(route, "Id", "Route");
            ViewBag.Times = new SelectList(time, "Id", "Time");
            var model = _product.GetProductToSend(routeId, timeId, dateToDeliver);
            return View(model);
        }

        public ActionResult PickUp(int[] itemsId)
        {
            bool result = _product.PickUp(itemsId);
            if (result == true)
            {
                TempData["Message"] = "ສຳເລັດ";
            }
            else
            {
                TempData["Message"] = "ບໍ່ສຳເລັດ";
            }
            return RedirectToAction("ItemsToPickUp", "Delivery");
        }

        public ActionResult ItemDetail()
        {
            return View();
        }

    }
}