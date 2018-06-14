using AnousithExpress.Data.Implementation;
using AnousithExpress.Data.SingleViewModels;
using System.Web.Mvc;

namespace AnusithExpress.web.mvc.Controllers
{
    public class DeliveryController : Controller
    {
        // GET: Delivery
        private ItemService itemService;
        private CustService custService;
        public DeliveryController(ItemService items, CustService cust)
        {
            itemService = items;
            custService = cust;
        }
        public ActionResult Index()
        {
            if (Session["Role"] != null)
            {
                int role = (int)Session["Role"];
                if (role == 2)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("ULogin", "Account");
                }
            }
            else
            {
                return RedirectToAction("ULogin", "Account");
            }

        }
        public ActionResult ViewItemToGet()
        {
            if (Session["Role"] != null)
            {
                int role = (int)Session["Role"];
                if (role == 2)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("ULogin", "Account");
                }
            }
            else
            {
                return RedirectToAction("ULogin", "Account");
            }
        }

        [HttpGet]
        public ActionResult ViewItemToGet(int customerId = 0)
        {
            if (Session["Role"] != null)
            {
                int role = (int)Session["Role"];
                if (role == 2)
                {
                    var model = itemService.GetConfirmItems(customerId);
                    return View(model);
                }
                else
                {
                    return RedirectToAction("ULogin", "Account");
                }
            }

            else
            {
                return RedirectToAction("ULogin", "Account");
            }
        }


        public ActionResult ViewItemToSend()
        {
            if (Session["Role"] != null)
            {
                int role = (int)Session["Role"];
                if (role == 2)
                {
                    RouteTimeDropdownlist();
                    return View();
                }
                else
                {
                    return RedirectToAction("ULogin", "Account");
                }
            }
            else
            {
                return RedirectToAction("ULogin", "Account");
            }

        }

        private void RouteTimeDropdownlist()
        {
            var route = itemService.GetRoute();
            var time = itemService.GetTime();
            ViewBag.route = new SelectList(route, "Id", "Route");
            ViewBag.time = new SelectList(time, "Id", "Time");
        }

        [HttpGet]
        public ActionResult ViewItemToSend(int routeId = 0, int timeId = 0)
        {
            if (Session["Role"] != null)
            {
                int role = (int)Session["Role"];
                if (role == 2)
                {
                    RouteTimeDropdownlist();
                    var model = itemService.GetToSendItems(routeId, timeId);
                    return View(model);
                }
                else
                {
                    return RedirectToAction("ULogin", "Account");
                }
            }
            else
            {
                return RedirectToAction("ULogin", "Account");
            }


        }

        public ActionResult RecievedItem(int[] itemsId, int? customerId)
        {
            if (itemsId != null && customerId != null)
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

        [HttpPost]
        public ActionResult CreateItem(ItemSingleModel model)
        {
            if (ModelState.IsValid)
            {
                if (custService.CheckExistingCustomer(model.CustomerPhonenumber))
                {
                    var item = itemService.CreateItemForDelivery(model);
                    return RedirectToAction("ShowItem", "Delivery", new { id = item.Id });
                }
                else
                {
                    ViewBag.Message = "ເບີໂທດັ່ງກ່າວຍັງບໍ່ທັນໄດ້ລົງທະບຽນ";
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
        public ActionResult ShowItem(int id)
        {
            if (id > 0)
            {
                var model = itemService.GetSingle(id);

                return View(model);
            }
            else
            {
                return View();
            }
        }
        public ActionResult BackButton()
        {
            return Redirect(Request.UrlReferrer.ToString());
        }

    }
}