using AnousithExpress.Data.Implementation;
using AnousithExpress.Data.SingleViewModels;
using System;
using System.Web.Mvc;

namespace AnusithExpress.web.mvc.Controllers
{
    public class CustomerController : Controller
    {
        private ItemService _itemServ;
        private CustService _custService;
        private ConsolidatedService _consolidate;
        public CustomerController(ItemService itemService, CustService custService, ConsolidatedService consolidatedService)
        {
            _itemServ = itemService;
            _custService = custService;
            _consolidate = consolidatedService;
        }
        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("CLogin", "Account");
            }


        }

        public ActionResult CreateUpdateItem(int id = 0)
        {
            if (Session["UserId"] != null)
            {
                var model = _itemServ.GetSingle(id);
                return View(model);
            }
            else
            {
                return RedirectToAction("CLogin", "Account");
            }

        }

        [HttpPost]
        public ActionResult CreateUpdateItem(ItemSingleModel model)
        {
            if (Session["UserId"] != null)
            {
                if (ModelState.IsValid)
                {
                    model.Status = "ຢູ່ຮ້ານ";
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
                            ModelState.Clear();
                            return View(new ItemSingleModel());
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
            else
            {
                return RedirectToAction("CLogin", "Account");
            }


        }

        public ActionResult ViewItems()
        {
            if (Session["UserId"] != null)
            {
                int CustId = (int)Session["UserId"];
                var model = _itemServ.GetForCustomer(CustId);
                return View(model);
            }
            else
            {
                return RedirectToAction("CLogin", "Account");
            }

        }

        public ActionResult ConfirmItem(int id)
        {
            if (Session["UserId"] != null)
            {
                bool result = false;
                result = _itemServ.ConfirmItem(id);
                if (result == true)
                {
                    return RedirectToAction("ViewItems");
                }
                else
                {

                    return RedirectToAction("ViewItems");
                }
            }
            else
            {
                return RedirectToAction("CLogin", "Account");
            }
        }

        public ActionResult UnConfirmItem(int id)
        {
            if (Session["UserId"] != null)
            {
                bool result = false;
                result = _itemServ.UnConfirmItem(id);
                if (result == true)
                {
                    return RedirectToAction("ViewItems");
                }
                else
                {

                    return RedirectToAction("ViewItems");
                }
            }
            else
            {
                return RedirectToAction("CLogin", "Account");
            }
        }

        public ActionResult DeleteItem(int id)
        {
            if (Session["UserId"] != null)
            {
                bool result = false;
                result = _itemServ.Delete(id);
                if (result == true)
                {

                    return RedirectToAction("ViewItems");
                }
                else
                {
                    ViewBag.Message = "ລືບລາຍການບໍ່ສຳເລັດ ກະລຸນາກວດໃຫມ່ອີກຄັ້ງ";
                    return RedirectToAction("ViewItems");
                }
            }
            else
            {
                return RedirectToAction("CLogin", "Account");
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
            if (Session["UserId"] == null)
            {
                return RedirectToAction("CLogin", "Account");
            }
            else
            {
                var model = _custService.GetSingle(id);
                return View(model);
            }

        }

        [HttpPost]
        public ActionResult CUpdateProfile(CustomerSingleModel model)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("CLogin", "Account");
            }
            else
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

        public ActionResult ConsolidatedList(DateTime? searchDateFrom = null, DateTime? searchDateTo = null)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("CLogin", "Account");
            }
            else
            {
                int userId = (int)Session["UserId"];
                var model = _consolidate.GetSingle(userId, searchDateFrom, searchDateTo);
                return View(model);
            }
        }

        public ActionResult ConsolidatedItems(int consolidatedId)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction("CLogin", "Account");
            }
            else
            {
                int userId = (int)Session["UserId"];
                var model = _consolidate.GetItems(consolidatedId);
                return View(model);
            }
        }
    }
}