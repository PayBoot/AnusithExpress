using AnousithExpress.DataEntry.Interface;
using AnousithExpress.DataEntry.Models;
using System.Collections.Generic;
using System.Linq;

namespace AnousithExpress.DataEntry.Implimentation
{
    public class DatabaseInit : IDatabaseInit
    {
        public bool InitiateData()
        {
            using (var db = new EntityContext())
            {
                using (var dbtransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        db.Database.ExecuteSqlCommand("DBCC CHECKIDENT('TbRole', RESEED)");
                        db.Database.ExecuteSqlCommand("DBCC CHECKIDENT('TbItemStatus', RESEED)");
                        db.Database.ExecuteSqlCommand("DBCC CHECKIDENT('TbStatus', RESEED)");
                        db.Database.ExecuteSqlCommand("DBCC CHECKIDENT('TbUser', RESEED)");
                        db.Database.ExecuteSqlCommand("DBCC CHECKIDENT('TbRoute', RESEED)");
                        db.Database.ExecuteSqlCommand("DBCC CHECKIDENT('TbTime', RESEED)");
                        List<TbStatus> status = new List<TbStatus>
                        {
                        new TbStatus { Id = 1, Status = "ພ້ອມນຳໃຊ້" },
                        new TbStatus{ Id= 2, Status = "ບລ໋ອກ"}
                        };
                        db.tbStatuses.AddRange(status);
                        db.SaveChanges();
                        List<TbRole> role = new List<TbRole>
                        {
                        new TbRole { Id= 1, Role= "ແອດມີນ"},
                        new TbRole { Id= 2, Role= "Call Center"},
                        new TbRole { Id= 3, Role= "ພະນັກງານຮັບສົ່ງ"},
                        new TbRole { Id= 4, Role= "ຄົນຈັດຄົນ"},
                        new TbRole { Id= 5, Role= "ນາຍສາງ"},
                        new TbRole { Id= 6, Role= "ພະນັກງານບັນຊີ"}

                        };
                        db.tbRoles.AddRange(role);
                        db.SaveChanges();
                        List<TbItemStatus> itemstatus = new List<TbItemStatus>
                        {
                        new TbItemStatus{Id = 1, Status = "ຢູ່ຮ້ານ"},
                        new TbItemStatus{Id = 2, Status = "ຢືນຢັນ" },
                        new TbItemStatus{Id = 3, Status = "ພະນັກງານຮັບເຄື່ອງແລ້ວ"},
                        new TbItemStatus{Id = 4, Status = "ກຳລັງດຳເນີນການ"},
                        new TbItemStatus{Id = 5, Status = "ເຄື່ອງກຳລັງສົ່ງ"},
                        new TbItemStatus{Id = 6, Status = "ສີນຄ້າຖືກສົ່ງໃຫ້ປາຍທາງແລ້ວ"},
                        new TbItemStatus{Id = 7, Status = "ບໍ່ສາມາດຕິຕໍ່ກັບປາຍທາງໄດ້"},
                        new TbItemStatus{Id = 8, Status = "ສົ່ງກັບຄືນແລ້ວ" },
                        new TbItemStatus{Id = 9, Status= "ເຄື່ອງກຳລັງສົ່ງກັບຄືນ" },
                        new TbItemStatus{Id = 10, Status= "ລຸກຄ້າບໍ່ຮັບເຄື່ອງ" }
                        };
                        db.tbItemStatuses.AddRange(itemstatus);
                        db.SaveChanges();
                        List<TbUser> user = new List<TbUser>
                        {
                        new TbUser{Id=1
                            ,Username ="Admin"
                            ,Password="Admin"
                            ,Phonenumber=""
                            ,Role=db.tbRoles.FirstOrDefault(r=>r.Id==1)
                            ,Status=db.tbStatuses.FirstOrDefault(s=>s.Id==1)}
                        };

                        db.tbUsers.AddRange(user);
                        db.SaveChanges();
                        TbRoute route = new TbRoute
                        {
                            Id = 1,
                            Route = "A1"
                        };
                        db.tbRoutes.Add(route);
                        db.SaveChanges();
                        TbTime time = new TbTime
                        {
                            Id = 1,
                            Time = "8-10 ໂມງ"
                        };
                        db.tbTimes.Add(time);
                        db.SaveChanges();
                        dbtransaction.Commit();
                        return true;
                    }
                    catch
                    {
                        dbtransaction.Rollback();
                        return false;
                    }

                }

            }
        }
    }
}
