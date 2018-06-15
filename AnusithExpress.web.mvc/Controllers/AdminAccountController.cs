using AnousithExpress.Data.Implementation;
using AnousithExpress.Data.SingleViewModels;
using System.Web.Mvc;

namespace AnusithExpress.web.mvc.Controllers
{
    public class AdminAccountController : Controller
    {
        // GET: AdminAccount
        private SystemUserService userService;
        public AdminAccountController(SystemUserService systemUserService)
        {
            userService = systemUserService;
        }
        public ActionResult Accounts()
        {
            var model = userService.GetAll();
            return View(model);
        }

        public ActionResult CreateUpdateAccount(int id = 0)
        {
            CreateUpdateDropdownlist();
            var model = userService.GetSingle(id);
            return View(model);
        }

        private void CreateUpdateDropdownlist()
        {
            var role = userService.GetRoles();
            var status = userService.GetStatus();
            ViewBag.Roles = new SelectList(role, "Role", "Role");
            ViewBag.Statuses = new SelectList(status, "Status", "Status");
        }

        [HttpPost]
        public ActionResult CreateUpdateAccount(UserSingleModel model)
        {
            CreateUpdateDropdownlist();
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
                        ViewBag.Message = "ບໍ່ສຳເລັດ ກະລຸນາກວດໃຫມ່";
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
                        ViewBag.Message = "ບໍ່ສຳເລັດ ກະລຸນາກວດໃຫມ່";
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
            return RedirectToAction("Accounts", "AdminAccount");
        }
    }
}