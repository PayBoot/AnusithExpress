using AnousithExpress.DataEntry.Implimentation;
using AnousithExpress.DataEntry.ViewModels.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
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
            if (Session["Role"] != null)
            {
                if (Session["Role"].ToString() == "9")
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("CLogin", "Account");
                }
            }
            else
            {
                return RedirectToAction("CLogin", "Account");
            }

        }



        public ActionResult ItemsCreateUpdate(int itemId = 0)
        {
            TempData["Title"] = "ສ້າງລາຍການສີນຄ້າ";
            var model = _product.GetSingle(itemId);
            return View(model);
        }
        [HttpPost]
        public ActionResult ItemsCreateUpdateSave(ItemsModel model)
        {
            model.CustomerId = (int)Session["UserId"];
            if (ModelState.IsValid)
            {
                if (model.Id > 0)
                {
                    int result = _product.Update(model);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    int result = _product.Create(model);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var errorList = (from item in ModelState.Values
                                 from error in item.Errors
                                 select error.ErrorMessage).ToList();
                return Json(errorList, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult ItemDetail(int id)
        {
            ViewData["Title"] = "ລາຍລະອຽດສີນຄ້າ";
            var model = _product.GetSingle(id);
            return View(model);
        }

        public ActionResult ItemsInStore()
        {

            return View();
        }

        public ActionResult ItemsInStoreTable(DateTime? fromDate = null, DateTime? toDate = null)
        {
            string draw, searchValue, sortColumnName, sortDir;
            int start, length;
            int UserId = 0;
            if (Session["UserId"] != null)
            {
                UserId = (int)Session["UserId"];
            }

            DatatableInitiator(out draw, out start, out length, out searchValue, out sortColumnName, out sortDir);
            List<ItemsModel> AllList = new List<ItemsModel>();
            AllList = _product.GetProductInStorePerCustomer(UserId, fromDate, toDate);

            int totalRecord = AllList.Count;
            if (!String.IsNullOrEmpty(searchValue)) // filter
            {
                AllList = AllList.Where(x =>
                        x.TrackingNumber.Contains(searchValue) ||
                        x.ItemName.Contains(searchValue) ||
                        x.ReceiverName.Contains(searchValue) ||
                        x.CreatedDate.Contains(searchValue)).ToList();
            }
            int totalRowAfterFilter = AllList.Count;
            //sorting

            if (!String.IsNullOrEmpty(sortColumnName) && !String.IsNullOrEmpty(sortDir))
            {
                AllList = AllList.OrderBy(sortColumnName + " " + sortDir).ToList();
            }

            //paging

            AllList = AllList.ToList();

            return Json(new
            {
                draw = draw,
                recordsTotal = totalRecord,
                recordsFiltered = totalRowAfterFilter,
                data = AllList
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ItemsInProcess()
        {
            return View();
        }

        public ActionResult ItemsInProcessTable(DateTime? fromDate = null, DateTime? toDate = null)
        {
            string draw, searchValue, sortColumnName, sortDir;
            int start, length;
            int UserId = 0;
            if (Session["UserId"] != null)
            {
                UserId = (int)Session["UserId"];
            }

            DatatableInitiator(out draw, out start, out length, out searchValue, out sortColumnName, out sortDir);
            List<ItemsModel> AllList = new List<ItemsModel>();
            AllList = _product.GetProductInProcessPerCustomer(UserId, fromDate, toDate);

            int totalRecord = AllList.Count;
            if (!String.IsNullOrEmpty(searchValue)) // filter
            {
                AllList = AllList.Where(x =>
                        x.TrackingNumber.Contains(searchValue) ||
                        x.ItemName.Contains(searchValue) ||
                        x.ReceiverName.Contains(searchValue) ||
                        x.CreatedDate.Contains(searchValue)).ToList();
            }
            int totalRowAfterFilter = AllList.Count;
            //sorting

            if (!String.IsNullOrEmpty(sortColumnName) && !String.IsNullOrEmpty(sortDir))
            {
                AllList = AllList.OrderBy(sortColumnName + " " + sortDir).ToList();
            }

            //paging

            AllList = AllList.ToList();

            return Json(new
            {
                draw = draw,
                recordsTotal = totalRecord,
                recordsFiltered = totalRowAfterFilter,
                data = AllList
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ItemDetailPartial(int itemId)
        {
            var model = _product.GetSingle(itemId);
            return PartialView("ItemDetailPartial", model);
        }

        public ActionResult ItemsProcessed()
        {
            return View();
        }

        public ActionResult ItemsProcessedTable(DateTime? fromDate = null, DateTime? toDate = null)
        {
            string draw, searchValue, sortColumnName, sortDir;
            int start, length;
            int UserId = 0;
            if (Session["UserId"] != null)
            {
                UserId = (int)Session["UserId"];
            }

            DatatableInitiator(out draw, out start, out length, out searchValue, out sortColumnName, out sortDir);
            List<ItemsModel> AllList = new List<ItemsModel>();
            AllList = _product.GetProductProcessedPerCustomer(UserId, fromDate, toDate);

            int totalRecord = AllList.Count;
            if (!String.IsNullOrEmpty(searchValue)) // filter
            {
                AllList = AllList.Where(x =>
                        x.TrackingNumber.Contains(searchValue) ||
                        x.ItemName.Contains(searchValue) ||
                        x.ReceiverName.Contains(searchValue) ||
                        x.CreatedDate.Contains(searchValue)).ToList();
            }
            int totalRowAfterFilter = AllList.Count;
            //sorting

            if (!String.IsNullOrEmpty(sortColumnName) && !String.IsNullOrEmpty(sortDir))
            {
                AllList = AllList.OrderBy(sortColumnName + " " + sortDir).ToList();
            }

            //paging

            AllList = AllList.Skip(start).Take(length).ToList();

            return Json(new
            {
                draw = draw,
                recordsTotal = totalRecord,
                recordsFiltered = totalRowAfterFilter,
                data = AllList
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ItemsCount(DateTime? fromDate = null, DateTime? toDate = null)
        {
            int UserId = 0;
            if (Session["UserId"] != null)
            {
                UserId = (int)Session["UserId"];
            }
            var source = _product.GetItemsCount(UserId, fromDate, toDate);
            return Json(new { t = source.totalItem, s = source.totalSuccess, f = source.totalSendBack }, JsonRequestBehavior.AllowGet);
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

        public ActionResult Consolidation()
        {
            return View();
        }
        public ActionResult ConsolidationTable(DateTime? fromDate = null, DateTime? toDate = null)
        {
            string draw, searchValue, sortColumnName, sortDir;
            int start, length;
            int UserId = 0;
            if (Session["UserId"] != null)
            {
                UserId = (int)Session["UserId"];
            }

            DatatableInitiator(out draw, out start, out length, out searchValue, out sortColumnName, out sortDir);
            List<ConsolidationListModel> AllList = new List<ConsolidationListModel>();
            AllList = _consolidation.GetConsolidationListByCustomerId(UserId, fromDate, toDate);

            int totalRecord = AllList.Count;
            if (!String.IsNullOrEmpty(searchValue)) // filter
            {
                AllList = AllList.Where(x =>
                        x.ConsolidateNumber.Contains(searchValue) ||
                        x.ConsolidatedDate.Contains(searchValue)).ToList();
            }
            int totalRowAfterFilter = AllList.Count;
            //sorting

            if (!String.IsNullOrEmpty(sortColumnName) && !String.IsNullOrEmpty(sortDir))
            {
                AllList = AllList.OrderBy(sortColumnName + " " + sortDir).ToList();
            }

            //paging

            AllList = AllList.ToList();

            return Json(new
            {
                draw = draw,
                recordsTotal = totalRecord,
                recordsFiltered = totalRowAfterFilter,
                data = AllList
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConsolidationDetail(int consolidationId)
        {
            var model = _consolidation.GetConsolidationDetailByConsolidationId(consolidationId);
            return PartialView("ConsolidationDetail", model);
        }


        /// <summary>
        /// /////////
        /// </summary>
        /// <param name="status"></param>
        /// <param name="createDate"></param>
        /// <param name="submitDate"></param>
        /// <returns></returns>
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


        public ActionResult ProfileDetail()
        {
            if (Session["UserId"] != null)
            {
                int UserId = (int)Session["UserId"];
                var model = _customer.GetCustomerProfileItems(UserId);
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }

        }
        public ActionResult ProfileUpdate()
        {
            var model = _customer.GetCustomerProfile(1);
            return View(model);
        }

        [HttpPost]
        public ActionResult ProfileUpdateFunction(ProfileModel model)
        {

            StringBuilder errorList = new StringBuilder();
            if (model.Name == null)
            {
                errorList.Append("ກະລຸນາໃສ່ຊື່ <br/>");
            }
            if (model.Phonenumber == null || model.Phonenumber.Length != 8)
            {
                errorList.Append("ກະລຸນາໃສ່ເບີໂທ 8 ໂຕເລກ <br/>");
            }
            if (!string.IsNullOrEmpty(model.Password))
            {
                if (model.Password.Length < 6)
                {
                    errorList.Append("ລະຫັດຕ້ອງຢ່າງຫນ້ອຍ 6 ໂຕເລກ <br/>");
                }
            }
            if (model.Address == null)
            {
                errorList.Append("ກະລຸນາໃສ່ທີ່ຢູ່ຂອງທ່ານ <br/>");
            }
            if (string.IsNullOrEmpty(errorList.ToString()))
            {
                bool result = _customer.Update(model);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(errorList.ToString(), JsonRequestBehavior.AllowGet);
            }
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