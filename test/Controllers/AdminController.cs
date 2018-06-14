using AnousithExpress.Data.Implementation;
using AnousithExpress.Data.SingleViewModels;
using System.Web.Mvc;

namespace AnusithExpress.web.mvc.Controllers
{
    public class AdminController : Controller
    {
        private SystemUserService userService;
        public AdminController(SystemUserService systemUser)
        {
            userService = systemUser;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Accounts()
        {
            var model = userService.GetAll();
            return View(model);
        }

        public ActionResult CreateUpdateAccount(int id = 0)
        {
            AccountDropdownlist();
            if (id > 0)
            {
                var model = userService.GetSingle(id);
                return View(model);
            }
            else
            {
                return View();
            }

        }

        private void AccountDropdownlist()
        {
            var role = userService.GetRoles();
            var status = userService.GetStatus();
            ViewBag.Roles = new SelectList(role, "Role", "Role");
            ViewBag.Statuses = new SelectList(status, "Status", "Status");
        }

        [HttpPost]
        public ActionResult CreateUpdateAccount(UserSingleModel model)
        {
            AccountDropdownlist();
            if (ModelState.IsValid)
            {
                if (model.Id > 0)
                {
                    bool result = userService.Edit(model);
                    if (result == true)
                    {
                        ViewBag.Message = "ສຳເລັດ";
                        return View(model);
                    }
                    else
                    {
                        ViewBag.Message = "ບໍ່ສຳເລັດ ກະລຸນາລອງໃຫມ່ອີກຄັ້ງ";
                        return View(model);
                    }
                }
                else
                {
                    bool result = userService.Create(model);
                    if (result == true)
                    {
                        ViewBag.Message = "ສຳເລັດ";
                        return View();
                    }
                    else
                    {
                        ViewBag.Message = "ບໍ່ສຳເລັດ ກະລຸນາລອງໃຫມ່ອີກຄັ້ງ";
                        return View(model);
                    }
                }
            }
            else
            {
                return View();
            }

        }

        public ActionResult DeleteAccount(int id)
        {
            bool result = userService.Delete(id);
            if (result == true)
            {
                TempData["Message"] = "ສຳເລັດ";
                return RedirectToAction("Accounts", "Admin");
            }
            else
            {
                TempData["Message"] = "ບໍ່ສຳເລັດ";
                return RedirectToAction("Accounts", "Admin");
            }
        }

        public ActionResult Customer()
        {
            //GridView
            return View();
        }




        public ActionResult CreateUpdateItem()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateUpdateItem(ItemSingleModel model)
        {
            return View();
        }

        public ActionResult AllItem()
        {
            return View();
        }


       

        public ActionResult Customer_Items()
        {
            //ListView
            return View();
        }
        public ActionResult PrintBarCode()
        {
            return View();
        }
        public ActionResult BarCodeRegister()
        {
            return View();
        }
        public ActionResult Consolidate()
        {
            return View();
        }

    }
}