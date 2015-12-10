using biz.dfch.CS.Appclusive.Api.Core;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI._fake
{
    public class CoreRepositoryMock : biz.dfch.CS.Appclusive.Api.Core.Core
    {
        static CoreRepositoryMock()
        {
            customers = new List<Customer>();
            Customer entity = new Customer()
            {
                Name = "Mocked customer",
                Description = "Mocked description"
            };
            customers.Add(entity);
        }
        static List<Customer> customers;

        public CoreRepositoryMock(Uri serviceRoot) : base(serviceRoot) { }
        
        public List<Customer> Customers {
            get
            {
                return customers;
            }
        }
        public void AddToCustomers(Customer entity) {
            entity.Id = DateTime.Now.Ticks;
            customers.Add(entity);
        }
    }
}