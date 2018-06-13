using AnousithExpress.Data.Implementation;
using AnousithExpress.Data.SingleViewModels;
using System.Web.Mvc;

namespace AnusithExpress.web.mvc.Controllers
{
    public class DeliveryController : Controller
    {
        // GET: Delivery
        private ItemService itemService;
        public DeliveryController(ItemService items)
        {
            itemService = items;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewItemToGet()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ViewItemToGet(int customerId = 0)
        {
            var model = itemService.GetConfirmItems(customerId);
            return View(model);
        }

        [HttpGet]
        public ActionResult ViewItemToGetTable(int customerId)
        {
            if (Request.IsAjaxRequest())
            {
                var model = itemService.GetConfirmItems(customerId);
                return PartialView("ViewItemToGetTable", model);
            }
            return View();
        }

        public ActionResult ViewItemToSend()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ViewItemToSendTable(int routeId, int TimeId)
        {
            return View();
        }

        public ActionResult RecievedItem(int[] itemsId, int customerId)
        {
            if (itemsId != null)
            {
                bool result = itemService.ReceiveItem(itemsId);
                if (result == true)
                {
                    TempData["Message"] = "ສຳເລັດ";
                    return RedirectToAction("ViewItemToGet", "Delivery", new { customerId = customerId });
                }
                else
                {
                    TempData["Message"] = "ບໍ່ສຳເລັດ ກະລຸນາກວດໃຫມ່";
                    return RedirectToAction("ViewItemToGet", "Delivery", new { customerId = customerId });
                }
            }
            else
            {
                TempData["Message"] = "ກະລຸນາເລືອກ";
                return RedirectToAction("ViewItemToGet", "Delivery", new { customerId = customerId });
            }

        }
        public ActionResult CreateItem()
        {
            return View();
        }
        public ActionResult CreateItem(ItemSingleModel model)
        {

            return View();
        }
    }
}