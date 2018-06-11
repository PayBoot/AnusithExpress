﻿using AnousithExpress.Data.Models;
using AnousithExpress.Data.SingleViewModels;
using System.Collections.Generic;

namespace AnousithExpress.Data.Interfaces
{
    public interface ICustomer
    {
        List<CustomerSingleModel> GetAll();
        bool CheckDuplicateNumber(string number);
        bool Create(CustomerSingleModel model);
        bool Edit(CustomerSingleModel model);
        bool Delete(int id);
        TbCustomer LogIn(string phonenumber, string password);

        List<CustomerItemModel> ShowCustomer();
    }
}
