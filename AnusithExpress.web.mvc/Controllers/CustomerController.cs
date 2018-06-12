using AnousithExpress.Data.Implementation;
using AnousithExpress.Data.SingleViewModels;
using System.Web.Mvc;

namespace AnusithExpress.web.mvc.Controllers
{
    public class CustomerController : Controller
    {
        private ItemService _itemServ;
        private CustService _custService;
        public CustomerController(ItemService itemService, CustService custService)
        {
            _itemServ = itemService;
            _custService = custService;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateUpdateItem(int id = 0)
        {
            var model = _itemServ.GetSingle(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateUpdateItem(ItemSingleModel model)
        {
            if (ModelState.IsValid)
            {
                int userId = (int)Session["UserId"];
                model.CustomerId = userId;
                if (model.Id > 0)
                {
                    bool result = _itemServ.Update(model);
                    if (result == true)
                    {
                        ViewBag.Message = "ປ່ຽນແປງຂໍ້ມູນສີນຄ້າສຳເລັດ";
                        int id = model.Id ?? default(int);
                        var postModel = _itemServ.GetSingle(id);
                        return View(postModel);
                    }
                    else
                    {
                        ViewBag.Message = "ປ່ຽນແປງຂໍ້ມູນສີນຄ້າບໍ່ສຳເລັດ";
                        return View(model);
                    }
                }
                else
                {

                    bool result = _itemServ.Create(model);
                    if (result == true)
                    {
                        ViewBag.Message = "ເພີ່ມລາຍການສຳເລັດ";
                        return View();
                    }
                    else
                    {
                        ViewBag.Message = "ເພີ່ມລາຍການບໍ່ສຳເລັດ";
                        return View(model);
                    }

                }

            }
            else
            {
                return View();
            }

        }
        public ActionResult ViewItems()
        {
            int CustId = (int)Session["UserId"];
            var model = _itemServ.GetForCustomer(CustId);
            return View(model);
        }
        public ActionResult DeleteItem(int id)
        {
            bool result = false;
            result = _itemServ.Delete(id);
            if (result == true)
            {
                ViewBag.Message = "ລືບລາຍການສຳເລັດ";
                return RedirectToAction("ViewItems");
            }
            else
            {
                ViewBag.Message = "ລືບລາຍການບໍ່ສຳເລັດ ກະລຸນາກວດໃຫມ່ອີກຄັ້ງ";
                return RedirectToAction("ViewItems");
            }
        }

        public ActionResult CProfile()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("CLogin", "Account");
            }
            else
            {
                int Id = (int)Session["UserId"];
                var model = _custService.CustomerProfile(Id);
                return View(model);
            }

        }

        public ActionResult CUpdateProfile(int id)
        {
            var model = _custService.GetSingle(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult CUpdateProfile(CustomerSingleModel model)
        {
            if (ModelState.IsValid)
            {
                int id = model.Id ?? default(int);
                bool checkDuplicate = _custService.CheckDuplicateNumber(id, model.Phonenumber);
                if (checkDuplicate == true)
                {
                    ViewBag.Duplicate = "ເບີດັ່ງກ່າວໄດ້ຖືກນຳໃຊ້ແລ້ວ ກະລຸນາເລືອກເບີໃຫມ່";
                    return View();
                }
                else
                {
                    bool result = _custService.Edit(model);
                    if (result == true)
                    {
                        var postModel = _custService.GetSingle(id);
                        ViewBag.Message = "ປ່ຽນແປງສຳເລັດ";
                        return View(postModel);
                    }
                    else
                    {
                        ViewBag.Message = "ປ່ຽນແປງບໍ່ສຳເລັດ";
                        return View(model);
                    }
                }

            }
            else
            {
                return View();
            }
        }
    }
}