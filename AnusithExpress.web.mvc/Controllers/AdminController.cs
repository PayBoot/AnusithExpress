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
        public AdminController(CustomerListService _customerListService, ItemService item)
        {
            customerListService = _customerListService;
            itemService = item;
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

    }
}