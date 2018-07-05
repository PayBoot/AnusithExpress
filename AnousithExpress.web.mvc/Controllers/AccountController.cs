using AnousithExpress.DataEntry.Implimentation;
using AnousithExpress.DataEntry.ViewModels.Customer;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnousithExpress.web.mvc.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        private CustomerService _customer;
        private ProductService _product;
        private AccountService _account;
        public AccountController(CustomerService customer, ProductService product, AccountService account)
        {
            _customer = customer;
            _product = product;
            _account = account;

        }
        public ActionResult Index()
        {

            return View();
        }
        public ActionResult CheckTrackingNumber(string trackingnumber)
        {
            bool result = _product.CheckTrackingNumber(trackingnumber);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult TrackItems(string trackingNumber)
        {
            ItemsModel model = new ItemsModel();
            model = _product.GetSingle(trackingNumber);
            return View(model);
        }
        public ActionResult CLogin()
        {

            return View();
        }

        [HttpPost]
        public ActionResult LoginForCustomer(string phonenumber, string password)
        {
            var model = _customer.Login(phonenumber, password);
            if (model != null)
            {

                Session["UserId"] = model.Id;
                Session["Username"] = model.Name;
                Session["Role"] = 9;
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult ULogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginForUser(string username, string password)
        {
            var user = _account.UserLogin(username, password);
            if (user != null)
            {

                Session["UserId"] = user.Id;
                Session["Username"] = user.Username;
                Session["Role"] = user.Role.Id;
                if (user.Role.Id == 1)
                {
                    return Json("Admin", JsonRequestBehavior.AllowGet);
                }
                else if (user.Role.Id == 2)
                {
                    return Json("Officer", JsonRequestBehavior.AllowGet);
                }
                else if (user.Role.Id == 3)
                {
                    return Json("Delivery", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Officer", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterCustomer(ProfileModel model)
        {
            if (ModelState.IsValid)
            {
                if (!_customer.CheckExistingPhonenumber(model.Phonenumber, null))
                {
                    bool result = _customer.Register(model);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("DuplicatePhonenumber", JsonRequestBehavior.AllowGet);
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
        public ActionResult SuccessRegistration()
        {
            return View();
        }
        public ActionResult CLogout()
        {
            Session.Clear();
            Session.Abandon();
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
            return RedirectToAction("Index");
        }
        public ActionResult ULogout()
        {
            Session.Clear();
            Session.Abandon();
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
            return RedirectToAction("ULogin");
        }


    }
}