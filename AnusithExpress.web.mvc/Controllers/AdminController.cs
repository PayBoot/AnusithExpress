using AnousithExpress.Data.Implementation;
using AnousithExpress.Data.SingleViewModels;
using AnusithExpress.web.mvc.Reports;
using CrystalDecisions.CrystalReports.Engine;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
            var status = itemService.GetItemStatus();
            ViewBag.Statuses = new SelectList(status, "Status", "Status");
            var itemModel = itemService.GetSingle(itemId);
            return View(itemModel);
        }

        [HttpPost]
        public ActionResult CreateUpdateItem(ItemSingleModel model)
        {
            var status = itemService.GetItemStatus();
            ViewBag.Statuses = new SelectList(status, "Status", "Status");
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

        public ActionResult Reportexport(int id)
        {

            var model = itemService.GetSingle(id);
            BillingDataSet dataset = new BillingDataSet();
            DataTable table = new DataTable();
            table = dataset.Barcode;
            ReportDocument report = new ReportDocument();
            report.Load(Path.Combine(Server.MapPath("~/Reports/billdelivery.rpt")));
            report.SetDataSource(table);
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = report.ExportToStream
                (CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "Barcode.pdf");
        }

        public ActionResult ItemList()
        {
            var model = itemService.GetAll();
            return View(model);
        }

        public ActionResult Allocation(int itemId)
        {
            var route = itemService.GetRoute();
            var time = itemService.GetTime();
            ViewData["itemId"] = itemId;
            ViewBag.Routes = new SelectList(route, "Id", "Route");
            ViewBag.Times = new SelectList(time, "Id", "Time");
            return View();
        }

        [HttpPost]
        public ActionResult Allocation(int itemId, int Routes, int Times)
        {
            var route = itemService.GetRoute();
            var time = itemService.GetTime();
            ViewData["itemId"] = itemId;
            ViewBag.Routes = new SelectList(route, "Id", "Route");
            ViewBag.Times = new SelectList(time, "Id", "Time");
            bool result = itemService.AllocateItem(itemId, Routes, Times);
            return View(result);
        }
    }
}