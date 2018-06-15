using AnousithExpress.Data.Implementation;
using AnousithExpress.Data.SingleViewModels;
using System.Web;
using System.Web.Mvc;

namespace AnusithExpress.web.mvc.Controllers
{
    public class AccountController : Controller
    {
        private CustService _custService;
        private SystemUserService _userService;
        public AccountController(CustService cust, SystemUserService user)
        {
            _custService = cust;
            _userService = user;
        }

        public ActionResult CLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CLogin(string phonenumber, string password)
        {
            var Account = _custService.LogIn(phonenumber, password);
            if (Account == null)
            {
                return RedirectToAction("CLogin");
            }
            else
            {
                Session["UserId"] = Account.Id;
                Session["Username"] = Account.Name;
 
                return RedirectToAction("Index", "Customer");
            }

        }
        public ActionResult ULogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ULogin(string username, string password)
        {
            var Account = _userService.Login(username, password);
            if (Account == null)
            {
                return RedirectToAction("ULogin");
            }
            else
            {
                Session["UserId"] = Account.Id;
                Session["Role"] = Account.Role.Id;
                Session["Username"] = Account.Username;
                if (Account.Role.Id == 1)
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (Account.Role.Id == 2)
                {
                    return RedirectToAction("Index", "Delivery");
                }
                else
                {
                    return RedirectToAction("ULogin");
                }
            }
        }
        public ActionResult CRegister()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CRegister(CustomerSingleModel model)
        {
            if (ModelState.IsValid)
            {
                bool duplicateCheck = _custService.CheckDuplicateNumber(model.Phonenumber);
                if (duplicateCheck == true)
                {
                    ViewBag.DuplicateMessage = "ເບີດັ່ງກ່າວໄດ້ຖືກໄຊ້ແລ້ວ";
                    return View("CRegister", model);
                }
                else
                {
                    bool result = _custService.Create(model);
                    if (result == true)
                    {
                        ViewBag.Message = "ລົງທະບຽນສຳເລັດ";
                        return View("CRegister");
                    }
                    else
                    {
                        ViewBag.Message = "ລົງທະບຽນບໍ່ສຳເລັດ ກະລຸນາກວດເບີ່ງຂໍ້ມູນຂອງທ່ານ";
                        return View("CRegister", model);
                    }

                }

            }
            else
            {
                return View();
            }

        }

        public ActionResult CLogout()
        {
            Session.Clear();
            Session.Abandon();
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
            return RedirectToAction("CLogin");
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