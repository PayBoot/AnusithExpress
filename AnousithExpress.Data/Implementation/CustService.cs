using AnousithExpress.Data.Interfaces;
using AnousithExpress.Data.Models;
using AnousithExpress.Data.SingleViewModels;
using System;
using System.Collections.Generic;

namespace AnousithExpress.Data.Implementation
{
    public class CustService : ICustomer
    {
        public bool CheckDuplicateNumber(string number)
        {
            throw new NotImplementedException();
        }

        public bool Create(CustomerSingleModel model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public bool Edit(CustomerSingleModel model)
        {
            throw new NotImplementedException();
        }

        public List<CustomerSingleModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public TbCustomer LogIn(string phonenumber, string password)
        {
            throw new NotImplementedException();
        }

        public List<CustomerItemModel> ShowCustomer()
        {
            throw new NotImplementedException();
        }
    }
}
