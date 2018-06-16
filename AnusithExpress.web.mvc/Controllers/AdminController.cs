using AnousithExpress.Data.Implementation;
using AnousithExpress.Data.SingleViewModels;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AnusithExpress.web.mvc.Controllers
{

    public class AdminController : Controller
    {
        private CustomerListService customerListService;
        private ItemService itemService;
        private CustService custService;
        public AdminController(CustomerListService _customerListService, ItemService item, CustService _custSerivce)
        {
            customerListService = _customerListService;
            itemService = item;
            custService = _custSerivce;
        }
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Customer()
        {
            var model = customerListService.GetCustomerList();
            return View(model);
        }
        public ActionResult CustomerDetail(int customerId = 0, int statusId = 0)
        {
            var status = itemService.GetItemStatus();
            ViewBag.Statuses = new SelectList(status, "Id", "Status");
            ViewData["customerId"] = customerId;
            List<ItemSingleModel> model = new List<ItemSingleModel>();
            if (statusId > 0)
            {
                model = customerListService.GetItemsByStatusForCustomer(customerId, statusId);
            }
            else
            {
                model = customerListService.GetItemsForCustomer(customerId);
            }

            return View(model);
        }

        public ActionResult CreateUpdateItem(int itemId = 0)
        {
            var itemModel = itemService.GetSingle(itemId);
            return View(itemModel);
        }

        [HttpPost]
        public ActionResult CreateUpdateItem(ItemSingleModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id > 0)
                {
                    if (custService.CheckExistingCustomer(model.CustomerPhonenumber))
                    {
                        var item = itemService.Update(model);
                        return View();
                    }
                    else
                    {
                        ViewBag.Message = "ເບີໂທດັ່ງກ່າວຍັງບໍ່ທັນໄດ້ລົງທະບຽນ";
                        return View();
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(model.CustomerPhonenumber))
                    {
                        if (custService.CheckExistingCustomer(model.CustomerPhonenumber))
                        {
                            model.CustomerId = 0;
                            var item = itemService.CreateItemForDelivery(model);
                            return View();
                        }
                        else
                        {
                            ViewBag.Message = "ເບີໂທດັ່ງກ່າວຍັງບໍ່ທັນໄດ້ລົງທະບຽນ";
                            return View();
                        }
                    }
                    else if (model.CustomerId > 0)
                    {
                        if (custService.CheckExistingCustomer(model.CustomerId))
                        {
                            var item = itemService.CreateItemForDelivery(model);
                            return View();
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

            }
            else
            {
                return View();
            }
        }


        public ActionResult ItemDescription(int id)
        {
            var model = itemService.GetSingle(id);
            return PartialView("ItemDescription", model);
        }

        public ActionResult AddDescription(ItemSingleModel model)
        {
            bool result = itemService.AddDescription(model);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteItem(int id)
        {
            if (Session["UserId"] != null)
            {
                bool result = false;
                result = itemService.Delete(id);
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return RedirectToAction("CLogin", "Account");
            }

        }

    }
}