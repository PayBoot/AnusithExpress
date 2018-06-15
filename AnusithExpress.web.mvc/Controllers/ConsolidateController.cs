using AnousithExpress.Data.Implementation;
using System.Web.Mvc;

namespace AnusithExpress.web.mvc.Controllers
{
    public class ConsolidateController : Controller
    {
        // GET: Consolidate
        private ItemService itemService;
        private CustomerListService customerListService;
        public ConsolidateController(ItemService _item, CustomerListService _customerList)
        {
            itemService = _item;
            customerListService = _customerList;
        }
        public ActionResult Index()
        {
            var model = customerListService.GetSentItemsNotConsolidateNotification();
            return View(model);
        }
        public ActionResult ItemAlreadySent(int custId)
        {
            var model = itemService.GetSentItemsToConsolidate(custId);
            return View(model);
        }
    }
}