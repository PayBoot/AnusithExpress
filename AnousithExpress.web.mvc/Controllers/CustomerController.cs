using AnousithExpress.DataEntry.Implimentation;
using AnousithExpress.DataEntry.ViewModels.Customer;
using System;
using System.Web.Mvc;

namespace AnousithExpress.web.mvc.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        private ProductService _product;
        private CustomerService _customer;
        private ConsolidationService _consolidation;
        public CustomerController(ProductService product, CustomerService customer, ConsolidationService consolidation)
        {
            _product = product;
            _customer = customer;
            _consolidation = consolidation;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Items(string status = "", DateTime? createDate = null, DateTime? submitDate = null)
        {
            var itemStatus = _product.GetStatus();
            ViewBag.Status = new SelectList(itemStatus, "Status", "Status");
            var model = _product.GetProductByCustomerId(1, null, null, "ຢູ່ຮ້ານ");
            return View(model);
        }

        public ActionResult ItemsTable(string status, DateTime? createDate, DateTime? submitDate)
        {
            TempData["status"] = status;
            TempData["createDate"] = createDate;
            TempData["submitDate"] = submitDate;
            var model = _product.GetProductByCustomerId(1, createDate, submitDate, status);
            return PartialView("ItemsTable", model);
        }

        public ActionResult ItemsCreateUpdate(int itemId = 0)
        {
            var model = _product.GetSingle(itemId);
            return View(model);
        }
        [HttpPost]
        public ActionResult ItemsCreateUpdate(ItemsModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id > 0)
                {

                    var result = _product.Update(model);
                    if (result == true)
                    {
                        ViewBag.Message = "ສຳເລັດ";
                    }
                    else
                    {
                        ViewBag.Message = "ບໍ່ສຳເລັດ ກະລຸນາລອງໃຫມ່";
                    }
                    return View();
                }
                else
                {

                    var result = _product.Create(model);
                    if (result == true)
                    {
                        ViewBag.Message = "ສຳເລັດ";
                        ModelState.Clear();
                    }
                    else
                    {
                        ViewBag.Message = "ບໍ່ສຳເລັດ ກະລຸນາລອງໃຫມ່";
                    }

                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        public ActionResult ItemDelete(int itemId)
        {
            bool result = _product.Delete(itemId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ItemSubmit(int itemId)
        {
            bool result = _product.Submit(itemId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ItemUndoSubmit(int itemId)
        {
            bool result = _product.UndoSubmit(itemId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ProfileDetail()
        {
            var model = _customer.GetCustomerProfileItems(1);
            return View(model);
        }
        public ActionResult ProfileUpdate()
        {
            var model = _customer.GetCustomerProfile(1);
            return View(model);
        }

        [HttpPost]
        public ActionResult ProfileUpdate(ProfileModel model)
        {

            bool result = _customer.Update(model);
            if (result == true)
            {
                ViewBag.Message = "ບັນທຶກສຳເລັດ";
            }
            else
            {
                ViewBag.Message = "ບັນທຶກບໍ່ສຳເລັດ";
            }
            return View(model);
        }
        public ActionResult Consolidation()
        {
            var model = _consolidation.GetConsolidationListByCustomerId(1, null, null);
            return View(model);
        }

        [HttpGet]
        public ActionResult Consolidation(DateTime? searchDateFrom = null, DateTime? searchDateTo = null)
        {
            TempData["searchDateFrom"] = searchDateFrom;
            TempData["searchDateTo"] = searchDateTo;
            var model = _consolidation.GetConsolidationListByCustomerId(1, searchDateFrom, searchDateTo);
            return View(model);
        }
        public ActionResult ConsolidationItem(int consolidationId)
        {
            var model = _consolidation.GetConsolidationDetailByConsolidationId(consolidationId);
            return View(model);
        }



        private void DatatableInitiator(out string draw, out int start, out int length, out string searchValue, out string sortColumnName, out string sortDir)
        {
            draw = Request["draw"];
            start = Convert.ToInt32(Request["start"]);
            length = Convert.ToInt32(Request["length"]);
            searchValue = Request["search[value]"];
            sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            sortDir = Request["order[0][dir]"];
        }

    }
}