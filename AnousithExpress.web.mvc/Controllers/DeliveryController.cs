using AnousithExpress.DataEntry.Implimentation;
using AnousithExpress.DataEntry.ViewModels.Customer;
using AnousithExpress.DataEntry.ViewModels.Delivery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
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

        public ActionResult ItemsToPickUp()
        {
            if (Session["Role"] != null)
            {
                if (Session["Role"].ToString() == "3")
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
                return RedirectToAction("Index", "Account");
            }

        }

        public ActionResult ItemsToPickUpTable(int customerId = 0)
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
            AllList = _product.GetProductToPickUpByCustomerId(customerId, UserId);

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

        public ActionResult PickUp(int[] itemsId)
        {
            if (Session["UserId"] != null)
            {
                int userId = (int)Session["UserID"];
                if (itemsId == null || itemsId.Length == 0)
                {
                    return Json("inValid", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    bool result = _product.PickUp(itemsId, userId);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("inValid", JsonRequestBehavior.AllowGet);
            }




        }

        public ActionResult ItemsToSent()
        {
            if (Session["Role"] != null)
            {
                if (Session["Role"].ToString() == "3")
                {
                    var route = _product.GetRoute();
                    var time = _product.GetTime();
                    ViewBag.Routes = new SelectList(route, "Id", "Route");
                    ViewBag.Times = new SelectList(time, "Id", "Time");
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

        public ActionResult ItemsToSentTable(int routeId = 0, int timeId = 0, DateTime? dateToDeliver = null)
        {

            string draw, searchValue, sortColumnName, sortDir;
            int start, length;
            int UserId = 0;
            if (Session["UserId"] != null)
            {
                UserId = (int)Session["UserId"];
            }
            DatatableInitiator(out draw, out start, out length, out searchValue, out sortColumnName, out sortDir);
            List<ItemsSentAllocationModel> AllList = new List<ItemsSentAllocationModel>();
            AllList = _product.GetProductToSend(routeId, timeId, dateToDeliver, UserId);
            int totalRecord = AllList.Count;
            if (!String.IsNullOrEmpty(searchValue)) // filter
            {
                AllList = AllList.Where(x =>
                        x.TrackingNumber.Contains(searchValue) ||
                        x.ItemName.Contains(searchValue) ||
                        x.ReceiverName.Contains(searchValue)).ToList();
            }
            int totalRowAfterFilter = AllList.Count;
            AllList = AllList.Skip(start).Take(length).OrderBy(x => x.Status).ToList();

            return Json(new
            {
                draw = draw,
                recordsTotal = totalRecord,
                recordsFiltered = totalRowAfterFilter,
                data = AllList
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ItemDetail(int itemId)
        {
            var model = new ItemsModel();
            if (itemId > 0)
            {
                model = _product.GetSingle(itemId);

            }
            return PartialView("ItemDetail", model);
        }

        public ActionResult SentItem(int itemId, int statusId, double? kip, double? baht, double? dollar)
        {
            if (Session["UserId"] != null)
            {
                int userId = (int)Session["UserId"];
                if (itemId > 0 && statusId > 0)
                {
                    bool result = _product.Send(itemId, statusId, userId, kip, baht, dollar);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Invalid", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("Invalid", JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult UnWantedItem(int itemId, double? kip, double? baht, double? dollar)
        {
            if (Session["UserId"] != null)
            {
                int userId = (int)Session["UserId"];
                if (itemId > 0)
                {
                    bool result = _product.SentButUnwantedItem(itemId, userId, kip, baht, dollar);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Invalid", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json("Invalid", JsonRequestBehavior.AllowGet);
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