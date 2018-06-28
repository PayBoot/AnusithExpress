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
            return View();
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
            AllList = _product.GetProductToPickUpByCustomerId(customerId);

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
            if (itemsId == null || itemsId.Length == 0)
            {
                return Json("inValid", JsonRequestBehavior.AllowGet);
            }
            else
            {
                bool result = _product.PickUp(itemsId);
                return Json(result, JsonRequestBehavior.AllowGet);
            }



        }

        public ActionResult ItemsToSent()
        {
            var route = _product.GetRoute();
            var time = _product.GetTime();
            ViewBag.Routes = new SelectList(route, "Id", "Route");
            ViewBag.Times = new SelectList(time, "Id", "Time");
            return View();
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
            List<ItemsAllocationModel> AllList = new List<ItemsAllocationModel>();
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
            //sorting



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

        public ActionResult ItemDetail(int itemId)
        {
            var model = new ItemsModel();
            if (itemId > 0)
            {
                model = _product.GetSingle(itemId);

            }
            return PartialView("ItemDetail", model);

        }

        public ActionResult SentItem(int itemId, int statusId)
        {
            if (itemId > 0 && statusId > 0)
            {
                bool result = _product.Send(itemId, statusId);
                return Json(result, JsonRequestBehavior.AllowGet);
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