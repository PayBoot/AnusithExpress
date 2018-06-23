using AnousithExpress.DataEntry.Implimentation;
using AnousithExpress.DataEntry.ViewModels.Admin;
using AnousithExpress.DataEntry.ViewModels.Customer;
using System;
using System.Web.Mvc;

namespace AnousithExpress.web.mvc.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        private ProductService _product;
        private CustomerService _customer;
        private AllocationService _allocation;
        public AdminController(ProductService product, CustomerService customer, AllocationService allocation)
        {
            _product = product;
            _customer = customer;
            _allocation = allocation;
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
            var model = _customer.CustomerList();
            return View(model);
        }

        public ActionResult CustomerItems(int customerId, int statusId = 2, DateTime? fromDate = null, DateTime? toDate = null)
        {
            ViewBag.CustomerId = customerId.ToString().PadLeft(4, '0');
            ViewData["CustomerId"] = customerId;
            var status = _product.GetStatus();
            ViewBag.Statuses = new SelectList(status, "Id", "Status", 2);
            var model = _product.GetProductByCustomerId(customerId, statusId, fromDate, toDate);
            return View(model);
        }

        public ActionResult ItemCreateUpdate(int itemId = 0)
        {
            var route = _product.GetRoute();
            var time = _product.GetTime();
            ViewBag.Routes = new SelectList(route, "Id", "Route");
            ViewBag.Times = new SelectList(time, "Id", "Time");
            var status = _product.GetStatus();
            ViewBag.Statuses = new SelectList(status, "Status", "Status");
            if (itemId > 0)
            {
                var model = _product.GetSingle(itemId);
                return View(model);
            }
            return View();

        }

        [HttpPost]
        public ActionResult ItemCreateUpdate(ItemsModel model)
        {
            var route = _product.GetRoute();
            var time = _product.GetTime();
            ViewBag.Routes = new SelectList(route, "Id", "Route");
            ViewBag.Times = new SelectList(time, "Id", "Time");
            var status = _product.GetStatus();
            ViewBag.Statuses = new SelectList(status, "Status", "Status");
            if (ModelState.IsValid)
            {
                if (model.Id > 0)
                {

                    int result = _product.Update(model);
                    if (result == 0)
                    {
                        ViewBag.Message = "ສຳເລັດ";
                    }
                    else
                    {
                        ViewBag.Message = "ບໍ່ສຳເລັດ";
                    }
                    return View(model);

                }
                else
                {
                    ViewBag.Message = "ສຳເລັດ";
                    bool result = _product.CreateByAdmin(model);
                    if (result == true)
                    {
                        ViewBag.Message = "ສຳເລັດ";
                        ModelState.Clear();
                    }
                    else
                    {
                        ViewBag.Message = "ບໍ່ສຳເລັດ";
                    }

                    return View(model);
                }
            }
            else
            {
                return View();
            }

        }

        public ActionResult Allocation(int? customerId = null, int? routeId = null, int? timeId = null, DateTime? dateToDeliverFrom = null, DateTime? dateToDeliverTo = null)
        {
            var route = _product.GetRoute();
            var time = _product.GetTime();
            ViewBag.Routes = new SelectList(route, "Id", "Route");
            ViewBag.Times = new SelectList(time, "Id", "Time");
            var model = _allocation.GetBySorting(customerId, routeId, timeId, dateToDeliverFrom, dateToDeliverTo);
            return View(model);
        }

        public ActionResult AllocateItem(int itemId)
        {

            bool result = true;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}