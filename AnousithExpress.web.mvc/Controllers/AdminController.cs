﻿using AnousithExpress.DataEntry.Implimentation;
using AnousithExpress.DataEntry.ViewModels.Admin;
using AnousithExpress.DataEntry.ViewModels.Customer;
using AnousithExpress.DataEntry.ViewModels.Delivery;
using AnousithExpress.web.mvc.Reports;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web.Mvc;

namespace AnousithExpress.web.mvc.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        private ProductService _product;
        private CustomerService _customer;
        private AllocationService _allocation;
        private AccountService _account;
        private ConsolidationService _consolidation;
        public AdminController(ConsolidationService consolidationService, ProductService product, CustomerService customer, AllocationService allocation, AccountService account)
        {
            _product = product;
            _customer = customer;
            _allocation = allocation;
            _account = account;
            _consolidation = consolidationService;
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
        public ActionResult Index()
        {
            if (Session["Role"] != null)
            {
                if (Session["Role"].ToString() == "1" || Session["Role"].ToString() == "2"
                    || Session["Role"].ToString() == "4" || Session["Role"].ToString() == "5"
                    || Session["Role"].ToString() == "6")
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

        public ActionResult Account()
        {
            if (Session["Role"] != null)
            {
                if (Session["Role"].ToString() == "1")
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Admin");
                }
            }
            else
            {
                return RedirectToAction("ULogin", "Account");
            }


        }

        public ActionResult AccountTable()
        {
            string draw, searchValue, sortColumnName, sortDir;
            int start, length;
            int UserId = 0;
            if (Session["UserId"] != null)
            {
                UserId = (int)Session["UserId"];
            }

            DatatableInitiator(out draw, out start, out length, out searchValue, out sortColumnName, out sortDir);
            List<AccountModel> AllList = new List<AccountModel>();
            AllList = _account.GetAll();
            int totalRecord = AllList.Count;
            if (!String.IsNullOrEmpty(searchValue)) // filter
            {
                AllList = AllList.Where(x =>
                        x.Username.Contains(searchValue) ||
                        x.Status.Contains(searchValue) ||
                        x.Role.Contains(searchValue) ||
                        x.Phonenumber.Contains(searchValue)).ToList();
            }
            int totalRowAfterFilter = AllList.Count;
            //sorting

            if (!String.IsNullOrEmpty(sortColumnName) && !String.IsNullOrEmpty(sortDir))
            {
                AllList = AllList.OrderBy(sortColumnName + " " + sortDir).ToList();
            }

            //paging

            AllList = AllList.Skip(start).Take(length).OrderBy(x => x.Status).ToList();

            return Json(new
            {
                draw = draw,
                recordsTotal = totalRecord,
                recordsFiltered = totalRowAfterFilter,
                data = AllList
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AccountDelete(int userId)
        {
            bool result = _account.DeleteAccount(userId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AccountCreateUpdate(int userId = 0)
        {
            var roles = _account.GetRoles();
            var status = _account.GetStatus();
            ViewBag.Roles = new SelectList(roles, "Role", "Role");
            ViewBag.Statuses = new SelectList(status, "Status", "Status");
            var model = _account.GetSingle(userId);
            return PartialView("AccountCreateUpdate", model);
        }

        public ActionResult AccountCreateUpdateAction(AccountModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id > 0)
                {
                    if (!_account.CheckExistingAccount(model.Username, (int)model.Id))
                    {
                        int result = _account.UpdateAccount(model);
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("Existed", JsonRequestBehavior.AllowGet);
                    }


                }
                else
                {
                    if (!_account.CheckExistingAccount(model.Username))
                    {
                        int result = _account.CreateAccount(model);
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json("Existed", JsonRequestBehavior.AllowGet);
                    }

                }
            }
            else
            {
                var errorList = (from user in ModelState.Values
                                 from error in user.Errors
                                 select error.ErrorMessage).ToList();
                return Json(errorList, JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Account
        /// </summary>
        /// <returns></returns>

        public ActionResult ScanBarCode()
        {
            if (Session["Role"] != null)
            {
                if (Session["Role"].ToString() == "5" || Session["Role"].ToString() == "1")
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Admin");
                }
            }
            else
            {
                return RedirectToAction("ULogin", "Account");
            }

        }

        public ActionResult ScanBarCodeReceiveItem()
        {
            if (Session["Role"] != null)
            {
                if (Session["Role"].ToString() == "5" || Session["Role"].ToString() == "1")
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Admin");
                }
            }
            else
            {
                return RedirectToAction("ULogin", "Account");
            }
        }
        public ActionResult ScanBarCodeSendingItem()
        {
            if (Session["Role"] != null)
            {
                if (Session["Role"].ToString() == "5" || Session["Role"].ToString() == "1")
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Admin");
                }
            }
            else
            {
                return RedirectToAction("ULogin", "Account");
            }
        }
        public ActionResult ScanBarCodeItemUnableToSend()
        {
            if (Session["Role"] != null)
            {
                if (Session["Role"].ToString() == "5" || Session["Role"].ToString() == "1")
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Admin");
                }
            }
            else
            {
                return RedirectToAction("ULogin", "Account");
            }
        }
        public ActionResult ScanBarCodeItemReturn()
        {
            if (Session["Role"] != null)
            {
                if (Session["Role"].ToString() == "5" || Session["Role"].ToString() == "1")
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Admin");
                }
            }
            else
            {
                return RedirectToAction("ULogin", "Account");
            }
        }
        public ActionResult ScanBarCodeItemIn(string itemTrackingNumber)
        {
            ItemsModel model = new ItemsModel();
            model = _product.ScanBarCodeItemReceive(itemTrackingNumber);
            if (model != null)
            {

                return Json(model, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Invalid", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ScanBarCodeItemOut(string itemTrackingNumber)
        {
            ItemsModel model = new ItemsModel();
            model = _product.ScanBarCodeItemToSend(itemTrackingNumber);
            if (model != null)
            {

                return Json(model, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Invalid", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ScanBarCodeItemUnableToContact(string itemTrackingNumber)
        {
            ItemsModel model = new ItemsModel();
            model = _product.ScanBarCodeItemUnableToContact(itemTrackingNumber);
            if (model != null)
            {

                return Json(model, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Invalid", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ScanBarCodeItemToReturnBack(string itemTrackingNumber)
        {
            ItemsModel model = new ItemsModel();
            model = _product.ScanBarCodeItemToReturn(itemTrackingNumber);
            if (model != null)
            {

                return Json(model, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Invalid", JsonRequestBehavior.AllowGet);
            }
        }
        ////////////////////////////////////////////////////
        //////
        /////////////// Customer

        public ActionResult Customer()
        {
            if (Session["Role"] != null)
            {
                if (Session["Role"].ToString() == "2" || Session["Role"].ToString() == "1" || Session["Role"].ToString() == "6")
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Admin");
                }
            }
            else
            {
                return RedirectToAction("ULogin", "Account");
            }
        }

        public ActionResult CustomerList(string sortBy)
        {
            var model = _customer.CustomerList();
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy == "1")
                {
                    model = model.OrderBy("NumberOfConfirmItem DESC").ToList();
                }
                else if (sortBy == "2")
                {
                    model = model.OrderBy("NumberOfInProcessItem DESC").ToList();
                }

            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CustomerItemList(int customerId, int tabIndex)
        {

            ViewBag.CustomerId = customerId.ToString().PadLeft(4, '0');
            ViewBag.TabIndex = tabIndex.ToString();
            return View();
        }

        public ActionResult CustomerItemConfirm(int customerId)
        {
            var model = _product.GetProductConfirmByCustomerId(customerId);
            return PartialView("CustomerItemConfirm", model);
        }

        public ActionResult CustomerItemInProcess(int customerId)
        {
            ViewBag.CustomerId = customerId;
            return PartialView("CustomerItemInProcess");
        }

        public ActionResult CustomerItemInProcessTable(int customerId)
        {
            string draw, searchValue, sortColumnName, sortDir;
            int start, length;


            DatatableInitiator(out draw, out start, out length, out searchValue, out sortColumnName, out sortDir);
            List<ItemsModel> AllList = new List<ItemsModel>();
            AllList = _product.GetProductInProcessPerCustomerAdminUser(customerId);

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

        public ActionResult AllocateItemFirstStage(int itemId)
        {
            var route = _product.GetRoute();
            var time = _product.GetTime();
            ViewBag.Routes = new SelectList(route, "Id", "Route");
            ViewBag.Times = new SelectList(time, "Id", "Time");
            var model = _product.GetSingle(itemId);
            return PartialView("AllocateItemFirstStage", model);
        }

        public ActionResult AllocateItemFirstSageFunction(int itemId, int routeId, int timeId, DateTime? dateToDeliver, string description)
        {
            if (routeId > 0 && timeId > 0 && dateToDeliver != null)
            {
                bool result1 = _product.AllocateRouteAndTime(itemId, routeId, timeId, (DateTime)dateToDeliver);
                bool result2 = _product.UpdateItemDescription(itemId, description);
                return Json(new { first = result1, second = result2 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string errorList = "";
                if (dateToDeliver == null)
                {
                    errorList += "ກະລຸນາເລືອກວັນທີສົ່ງ";
                }
                return Json(errorList, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult CannotContactCustomer(int itemId, string description)
        {
            bool result1 = _product.CannotContactCustomer(itemId);
            bool result2 = _product.UpdateItemDescription(itemId, description);
            return Json(new { first = result1, second = result2 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateUpdateUpdateItemDetailPage(int itemId, int customerId)
        {
            ViewBag.CustomerId = customerId;
            var status = _product.GetStatus();
            ViewBag.Statuses = new SelectList(status, "Status", "Status");
            var item = _product.GetSingle(itemId);
            return PartialView("CreateUpdateUpdateItemDetailPage", item);
        }

        [HttpPost]
        public ActionResult CreateUpdateItemFunction(ItemsModel model, int customerId)
        {
            model.CustomerId = customerId;
            if (ModelState.IsValid)
            {
                if (model.Id > 0)
                {
                    int result = _product.Update(model);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    int result = _product.CreateByAdmin(model);
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

        public ActionResult DeleteItem(int itemId)
        {
            bool result = _product.Delete(itemId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ItemsTransporting(int customerId)
        {
            ViewBag.CustomerId = customerId;
            return PartialView("ItemsTransporting");
        }

        public ActionResult ItemTransportingTable(int customerId)
        {
            string draw, searchValue, sortColumnName, sortDir;
            int start, length;

            DatatableInitiator(out draw, out start, out length, out searchValue, out sortColumnName, out sortDir);
            List<ItemsModel> AllList = new List<ItemsModel>();
            AllList = _product.GetProductTransportingPerCustomerAdminUser(customerId);

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

        public ActionResult BarCodeReport(int itemId)
        {
            ItemBarCodeModel model = _product.GetBarCodeModel(itemId);

            if (model != null)
            {
                Zen.Barcode.BarcodeDraw barcodeObject = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
                System.Drawing.Image barcodeImage = barcodeObject.Draw(model.Trackingnumber, 60);
                ViewBag.Trackingnumber = model.Trackingnumber;
                ItemBarcodeDataSet barcodeDS = new ItemBarcodeDataSet();
                if (model != null)
                {
                    barcodeDS.ItemTable.AddItemTableRow(
                    model.Itemname,
                    model.Trackingnumber,
                    model.SenderName,
                    model.SenderPhonenumber,
                    model.ReceiverName,
                    model.ReceiverPhoenumber,
                    model.ReceiverAddress,
                    model.Barcode);
                }
                HttpContext.Session["barcodeModel"] = barcodeDS;

            }
            else
            {
                return Content("No item identity is found, please access this page in a proper manner");
            }
            return PartialView("BarCodeReport", model);
        }
        public ActionResult GetBarCode(string trackingnumber)
        {
            FileContentResult result;

            Zen.Barcode.Code128BarcodeDraw barcodeObject = Zen.Barcode.BarcodeDrawFactory.Code128WithChecksum;
            //Image myimg = Code128Rendering.MakeBarcodeImage(trackingnumber, 2, true);
            System.Drawing.Image barcodeImage = barcodeObject.Draw(trackingnumber, 60);
            MemoryStream ms = new MemoryStream();
            barcodeImage.Save(ms, ImageFormat.Png);
            result = this.File(ms.GetBuffer(), "image/jpeg");
            return result;
        }

        public ActionResult CustomerSentItems(int customerId)
        {
            ViewBag.CustomerId = customerId;
            return PartialView("CustomerSentItems");
        }

        public ActionResult ItemsProcessedTable(int UserId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            string draw, searchValue, sortColumnName, sortDir;
            int start, length;



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

        public ActionResult ItemsCount(int customerId, DateTime? fromDate = null, DateTime? toDate = null)
        {

            var source = _product.GetItemsCount(customerId, fromDate, toDate);
            return Json(new { l = source.totalItemReceive, t = source.totalItem, s = source.totalSuccess, f = source.totalSendBack }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ItemsDetail(int itemId)
        {

            var model = _product.GetSingle(itemId);
            ViewBag.CustomerId = model.CustomerId.ToString().PadLeft(4, '0');
            ViewBag.Trackingnumber = model.TrackingNumber;
            return View(model);
        }

        public ActionResult CustomerProfile(int customerId)
        {

            var model = _customer.GetCustomerProfileItems(customerId);

            return PartialView("CustomerProfile", model);
        }

        public ActionResult CustomerProfileDetail(int customerId)
        {

            var model = _customer.GetCustomerProfile(customerId);
            var status = _account.GetStatus();
            ViewBag.Statuses = new SelectList(status, "Status", "Status");
            ViewBag.CustomerId = model.Id.ToString().PadLeft(4, '0');
            return View(model);
        }

        public ActionResult ProfileUpdateFunction(ProfileModel model)
        {

            StringBuilder errorList = new StringBuilder();
            if (_customer.CheckExistingPhonenumber(model.Phonenumber, model.Id))
            {
                errorList.Append("ເບີໂທດັ່ງກ່າວຖືກນຳໃຊ້ແລ້ວ <br/>");
            }
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

        public ActionResult ItemHistoryListOfCustomer()
        {
            return View();
        }

        public ActionResult ItemHistoryTable(DateTime? fromDate, DateTime? toDate)
        {
            string draw, searchValue, sortColumnName, sortDir;
            int start, length;


            DatatableInitiator(out draw, out start, out length, out searchValue, out sortColumnName, out sortDir);
            List<ItemHistoryModel> AllList = new List<ItemHistoryModel>();
            AllList = _product.GetItemHistory(fromDate, toDate);

            int totalRecord = AllList.Count;
            if (!String.IsNullOrEmpty(searchValue)) // filter
            {
                AllList = AllList.Where(x =>
                        x.CustomerIdForshow.Contains(searchValue) ||
                        x.TotalItemReturnForShow == searchValue ||
                        x.TotalItemReceiveForShow == searchValue ||
                        x.TotalItemSentForShow == searchValue ||
                        x.TotalItemInProcessForShow == searchValue).ToList();
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

        public ActionResult AllocateDeliveryMan()
        {
            var delivery = _account.GetDeliveryMan();
            ViewBag.delivery = new SelectList(delivery, "Id", "Username");
            var route = _product.GetRoute();
            var time = _product.GetTime();
            ViewBag.Routes = new SelectList(route, "Id", "Route");
            ViewBag.Times = new SelectList(time, "Id", "Time");
            return View();
        }

        public ActionResult AllocationTable(int routeId, int timeId, DateTime? dateToDeliver)
        {
            string draw, searchValue, sortColumnName, sortDir;
            int start, length;


            DatatableInitiator(out draw, out start, out length, out searchValue, out sortColumnName, out sortDir);
            List<ItemsAllocationModelWithDelivery> AllList = new List<ItemsAllocationModelWithDelivery>();
            AllList = _allocation.GetAllocationForAdmin(routeId, timeId, dateToDeliver);

            int totalRecord = AllList.Count;
            if (!String.IsNullOrEmpty(searchValue)) // filter
            {
                AllList = AllList.Where(x =>
                        x.Trackingnumber.Contains(searchValue) ||
                        x.DeliveryMan.Contains(searchValue)).ToList();
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

        public ActionResult AllocateFunction(int itemId, int deliveryManId)
        {
            bool result = _allocation.updateDeliveryman(itemId, deliveryManId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AllocatePickUpMan()
        {
            var delivery = _account.GetDeliveryMan();
            ViewBag.delivery = new SelectList(delivery, "Id", "Username");
            return View();
        }
        public ActionResult AllocationPickUpTable(int? customerId)
        {
            string draw, searchValue, sortColumnName, sortDir;
            int start, length;


            DatatableInitiator(out draw, out start, out length, out searchValue, out sortColumnName, out sortDir);
            List<ItemsPickUpAllocationModel> AllList = new List<ItemsPickUpAllocationModel>();
            AllList = _allocation.GetItemToPickUp(customerId == null ? 0 : (int)customerId);

            int totalRecord = AllList.Count;
            if (!String.IsNullOrEmpty(searchValue)) // filter
            {
                AllList = AllList.Where(x =>
                        x.TrackingNumber.Contains(searchValue) ||
                        x.UserName.Contains(searchValue)).ToList();
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

        public ActionResult AllocationPickUpFunction(int itemId, int deliveryManId)
        {
            bool result = _allocation.AllocatePersonToPickUp(itemId, deliveryManId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConsolidationListPerCustomer(int customerId)
        {
            ViewBag.CustomerId = customerId;
            return PartialView("ConsolidationListPerCustomer");
        }
        public ActionResult ConsolidateionListPerCustomerTable(int customerId, DateTime? fromDate, DateTime? toDate)
        {
            string draw, searchValue, sortColumnName, sortDir;
            int start, length;


            DatatableInitiator(out draw, out start, out length, out searchValue, out sortColumnName, out sortDir);
            List<ConsolidationListModel> AllList = new List<ConsolidationListModel>();
            AllList = _consolidation.GetConsolidationListByCustomerId(customerId, fromDate, toDate);

            int totalRecord = AllList.Count;
            if (!String.IsNullOrEmpty(searchValue)) // filter
            {
                AllList = AllList.Where(x =>
                        x.CustomerName.Contains(searchValue)).ToList();
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

        public ActionResult ConsolidationCreate(int customerId)
        {
            ViewBag.CustomerId = customerId.ToString().PadLeft(4, '0');


            return View();
        }
        public ActionResult ConsolidationItemTable(int customerId)
        {
            string draw, searchValue, sortColumnName, sortDir;
            int start, length;


            DatatableInitiator(out draw, out start, out length, out searchValue, out sortColumnName, out sortDir);
            List<ItemsModel> AllList = new List<ItemsModel>();
            AllList = _consolidation.GetUnConlidateItems(customerId);

            int totalRecord = AllList.Count;
            if (!String.IsNullOrEmpty(searchValue)) // filter
            {
                AllList = AllList.Where(x =>
                        x.CustomerName.Contains(searchValue) ||
                        x.CustomerPhonenumber.Contains(searchValue)).ToList();
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

        public ActionResult GetPrice(int[] itemId, int customerId)
        {
            ConsolidateFactorModel Factor = new ConsolidateFactorModel();
            if (itemId != null)
            {
                Factor = _consolidation.GetConsolidationFactor(itemId, customerId);
            }
            if (Factor == null)
            {
                return Json(new { price = "0", date = "ວັນທີບໍ່ຕົງກັນ", condition = "ວັນທີບໍ່ຕົງກັນ" }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new { price = Factor.PricePerUnit, date = Factor.DateOfReceive, condition = Factor.ConditionUsed }, JsonRequestBehavior.AllowGet);

            }

        }

        public ActionResult ConsolidationCreateFunction(int cusomterId, double amount, double fee, int[] itemId)
        {
            bool result = _consolidation.CreateConsolidation(cusomterId, amount, fee, itemId);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ConsolidationDelete(int consolidationId)
        {
            bool result = _consolidation.deleteConsolidation(consolidationId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateCustomer()
        {
            return View();
        }


        public ActionResult GetNotification()
        {
            double result = _product.GetNotification();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ItemHistory()
        {
            List<SelectListItem> condition = new List<SelectListItem>();
            condition.Add(new SelectListItem() { Text = "ສີນຄ້າຮັບມາ", Value = "1" });
            condition.Add(new SelectListItem() { Value = "2", Text = "ສີນຄ້າສົ່ງຮອດປາຍທາງ" });
            condition.Add(new SelectListItem() { Value = "3", Text = "ສີນຄ້າກັບຄືນ" });
            ViewBag.Condition = condition;
            return View();
        }
        public ActionResult ItemHistoryDataTable(DateTime? fromDate, DateTime? toDate, int condition)
        {

            string draw, searchValue, sortColumnName, sortDir;
            int start, length;


            DatatableInitiator(out draw, out start, out length, out searchValue, out sortColumnName, out sortDir);
            List<ItemsModel> AllList = new List<ItemsModel>();
            AllList = _product.GetItemsHistoryList(fromDate, toDate, condition);

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




        public ActionResult NeedCustomerConfirm(int consolidationId)
        {
            bool result = _consolidation.AddNeedConfirm(consolidationId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UndoCustomerConfirm(int consolidationId)
        {
            bool result = _consolidation.UndoNeedConfirm(consolidationId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConfirmTransferBalance(int consolidationId)
        {
            bool result = _consolidation.ConfirmTransfer(consolidationId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UndoConfirmTransferBalance(int consolidationId)
        {
            bool result = _consolidation.UndoConfirmTransfer(consolidationId);
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult searchBox(string input)
        {
            if (input.Length == 8)
            {
                int customerId = _customer.GetCustomerId(input);
                return Json(new { id = customerId, t = 1 }, JsonRequestBehavior.AllowGet);
            }
            else if (input.Length < 8)
            {
                int customerId = _customer.CheckCustomerId(input);
                return Json(new { id = customerId, t = 1 }, JsonRequestBehavior.AllowGet);
            }
            else if (input.Length > 8)
            {
                int itemId = _product.GetProductId(input);
                return Json(new { id = itemId, t = 2 }, JsonRequestBehavior.AllowGet);

            }

            else
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult ConsolidateAllConsolidate()
        {
            return View();
        }
        public ActionResult ConsolidateAllConsolidateTable(DateTime? fromDate, DateTime? toDate)
        {
            string draw, searchValue, sortColumnName, sortDir;
            int start, length;


            DatatableInitiator(out draw, out start, out length, out searchValue, out sortColumnName, out sortDir);
            List<ConsolidationListModel> AllList = new List<ConsolidationListModel>();
            AllList = _consolidation.GetAllConsolidationByDate(fromDate, toDate);

            int totalRecord = AllList.Count;
            if (!String.IsNullOrEmpty(searchValue)) // filter
            {
                AllList = AllList.Where(x =>
                        x.CustomerName.Contains(searchValue)
                       ).ToList();
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

        public ActionResult ConsolidationAllCount(DateTime? fromDate, DateTime? toDate)
        {
            List<ConsolidationListModel> AllList = new List<ConsolidationListModel>();
            AllList = _consolidation.GetAllConsolidationByDate(fromDate, toDate);
            double fee = AllList.Sum(x => x.Fee);
            double kip = AllList.Sum(x => x.IncomingBalanceInKip);
            double bath = AllList.Sum(x => x.IncomingBalanceInBaht);
            double dollar = AllList.Sum(x => x.IncomingBalanceInDollar);
            return Json(new { fee = fee, kip = kip, baht = bath, dollar = dollar }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ConsolidationAllReport(DateTime? fromDate, DateTime? toDate)
        {
            List<ConsolidationListModel> AllList = new List<ConsolidationListModel>();
            AllList = _consolidation.GetAllConsolidationByDate(fromDate, toDate);
            ConsolidationDataSet dataSet = new ConsolidationDataSet();

            if (AllList != null)
            {
                foreach (var con in AllList)
                {
                    dataSet.ConsolidationList.AddConsolidationListRow(
                        con.ConsolidateNumber,
                        con.CustomerId.ToString().PadLeft(4, '0'),
                        con.CustomerName,
                        con.CustomerPhoneNumber,
                        con.ConsolidatedDate,
                        con.AmountOfItem.ToString(),
                        con.Fee);
                }
                Session["ConsolidateRerpotDataSet"] = dataSet;
                if (fromDate == null)
                {
                    Session["dt1"] = "ບໍ່ກໍານົດ";

                }

                else
                {
                    Session["dt1"] = fromDate?.ToString("dd/MM/yyyy");

                }
                if (toDate == null)
                {

                    Session["dt2"] = "ບໍ່ກໍານົດ";

                }

                else
                {

                    Session["dt2"] = toDate?.ToString("dd/MM/yyyy");

                }

            }

            return View();
        }



        public ActionResult StatisticScanReceive()
        {
            return View();
        }
        public ActionResult StatisticScanReceiveTable(DateTime? fromDate, DateTime? toDate)
        {
            string draw, searchValue, sortColumnName, sortDir;
            int start, length;


            DatatableInitiator(out draw, out start, out length, out searchValue, out sortColumnName, out sortDir);
            List<ItemsModel> AllList = new List<ItemsModel>();
            AllList = _product.GetStatisticItemScanIn(fromDate, toDate);

            int totalRecord = AllList.Count;
            if (!String.IsNullOrEmpty(searchValue)) // filter
            {
                AllList = AllList.Where(x =>
                        x.CustomerName.Contains(searchValue) ||
                        x.CustomerPhonenumber.Contains(searchValue)).ToList();
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

        public ActionResult StatisticScanReceiveCount(DateTime? fromDate, DateTime? toDate)
        {
            double num = _product.GetStatisticItemScanIn(fromDate, toDate).Count();
            return Json(num, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StatisticScanSent()
        {
            return View();
        }
        public ActionResult StatisticScanSentTable(DateTime? fromDate, DateTime? toDate)
        {
            string draw, searchValue, sortColumnName, sortDir;
            int start, length;


            DatatableInitiator(out draw, out start, out length, out searchValue, out sortColumnName, out sortDir);
            List<ItemsModel> AllList = new List<ItemsModel>();
            AllList = _product.GetStatisticItemScanSent(fromDate, toDate);

            int totalRecord = AllList.Count;
            if (!String.IsNullOrEmpty(searchValue)) // filter
            {
                AllList = AllList.Where(x =>
                        x.CustomerName.Contains(searchValue) ||
                        x.CustomerPhonenumber.Contains(searchValue)).ToList();
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

        public ActionResult StatisticScanSentCount(DateTime? fromDate, DateTime? toDate)
        {
            var source = _product.GetStatisticItemScanSentCount(fromDate, toDate);
            double num = source.totalItem;
            double suc = source.totalSuccess;

            return Json(new { i = num, s = suc }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StatisticScanReturn()
        {
            return View();
        }
        public ActionResult StatisticScanReturnTable(DateTime? fromDate, DateTime? toDate)
        {
            string draw, searchValue, sortColumnName, sortDir;
            int start, length;


            DatatableInitiator(out draw, out start, out length, out searchValue, out sortColumnName, out sortDir);
            List<ItemsModel> AllList = new List<ItemsModel>();
            AllList = _product.GetStatisticItemSentBack(fromDate, toDate);

            int totalRecord = AllList.Count;
            if (!String.IsNullOrEmpty(searchValue)) // filter
            {
                AllList = AllList.Where(x =>
                        x.CustomerName.Contains(searchValue) ||
                        x.CustomerPhonenumber.Contains(searchValue)).ToList();
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

        public ActionResult StatisticScanReturnCount(DateTime? fromDate, DateTime? toDate)
        {
            var source = _product.GetStatisticItemScanReturnCount(fromDate, toDate);
            double num = source.totalItem;
            double suc = source.totalSuccess;
            return Json(new { i = num, s = suc }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeliveryHistory()
        {
            var deliveryMan = _account.GetDeliveryMan();
            ViewBag.DeliveryMan = new SelectList(deliveryMan, "Id", "Username");
            return View();
        }
        public ActionResult DeliveryHistoryTable(DateTime? fromDate, DateTime? toDate, int? deliveryId)
        {
            string draw, searchValue, sortColumnName, sortDir;
            int start, length;


            DatatableInitiator(out draw, out start, out length, out searchValue, out sortColumnName, out sortDir);
            List<DeliveryHistoryModel> AllList = new List<DeliveryHistoryModel>();
            AllList = _product.GetDeliveryHistory(fromDate, toDate, deliveryId);

            int totalRecord = AllList.Count;
            if (!String.IsNullOrEmpty(searchValue)) // filter
            {
                AllList = AllList.Where(x =>
                        x.DeliveryManName.Contains(searchValue) ||
                        x.TrackingNumber.Contains(searchValue)).ToList();
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

        public ActionResult DeliveryHistoryCount(DateTime? fromDate, DateTime? toDate, int? deliveryId)
        {
            var source = _product.GetDeliveryHistoryCount(fromDate, toDate, deliveryId);
            return Json(new
            {
                count = source.CountItem,
                kipNYR = source.KipNotYetReceive,
                bahtNYR = source.BahtNoYetRecieve,
                dollarNYR = source.DollarNotYetReceive,
                kipR = source.KipReceived,
                bahtR = source.BahtReceived,
                dollarR = source.DollarReceived,
                kipT = source.KipTotal,
                bahtT = source.BahtTotal,
                dollarT = source.DollarTotal
            },
                JsonRequestBehavior.AllowGet);
        }


        public ActionResult PickUpHistory()
        {
            var deliveryMan = _account.GetDeliveryMan();
            ViewBag.DeliveryMan = new SelectList(deliveryMan, "Id", "Username");
            return View();
        }


        public ActionResult PickUpHistoryTable(DateTime? fromDate, DateTime? toDate, int? deliveryId)
        {
            string draw, searchValue, sortColumnName, sortDir;
            int start, length;


            DatatableInitiator(out draw, out start, out length, out searchValue, out sortColumnName, out sortDir);
            List<PickUpHistoryModel> AllList = new List<PickUpHistoryModel>();
            AllList = _product.GetPickUpHistory(fromDate, toDate, deliveryId);

            int totalRecord = AllList.Count;
            if (!String.IsNullOrEmpty(searchValue)) // filter
            {
                AllList = AllList.Where(x =>
                        x.DeliveryManName.Contains(searchValue) ||
                        x.TrackingNumber.Contains(searchValue)).ToList();
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

        public ActionResult PickUpHistoryCount(DateTime? fromDate, DateTime? toDate, int? deliveryId)
        {
            double count = _product.GetPickUpHistory(fromDate, toDate, deliveryId).Count();
            return Json(count, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ReceiveMoneyFromDeliveryMan(int historyId)
        {
            bool result = _product.ReceiveMoneyFromDeliveryMan(historyId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UndoReceiveMoneyFromDeliveryMan(int historyId)
        {
            bool result = _product.UndoReceiveMoneyFromDeliveryMan(historyId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }






        public ActionResult NewConfirmItemsList()
        {
            List<NewConfirmItemModel> model = _product.GetNewConfirmItem();
            return Json(model, JsonRequestBehavior.AllowGet);
        }




        public ActionResult NewConsolidateConfirm()
        {
            List<ConsolidationListModel> model = _consolidation.GetConsolidationNotification();
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CountConfirmConsolidation()
        {
            double result = _consolidation.CountConfirmConsolidation();
            return Json(result, JsonRequestBehavior.AllowGet);
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
                    int result = _product.CreateByAdmin(model);
                    if (result == 1)
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