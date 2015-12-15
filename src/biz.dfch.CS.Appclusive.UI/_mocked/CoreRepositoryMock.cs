using biz.dfch.CS.Appclusive.Api.Core;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Web;

namespace biz.dfch.CS.Appclusive.UI._mocked
{
    public class CoreRepositoryMock : biz.dfch.CS.Appclusive.Api.Core.Core
    {
        public CoreRepositoryMock(Uri serviceRoot) : base(serviceRoot) { }

        static List<Customer> customers;
        static List<ContractMapping> contractMappings;

        public List<Customer> Customers { get { return customers; } }
        public List<ContractMapping> ContractMappings { get { return contractMappings; } }

        static CoreRepositoryMock()
        {
            biz.dfch.CS.Appclusive.Api.Core.Core core = new Api.Core.Core(new Uri(Properties.Settings.Default.AppculsiveApiCoreUrl));


            // 2. Customers
            customers = new List<Customer>();
            Customer customer = new Customer()
            {
                Name = "Mocked customer",
                Description = "Mocked description",
            };
            customer.Tenants.AddRange(core.Tenants);
            customers.Add(customer);

            // 3. contracts
            contractMappings = new List<ContractMapping>();
            ContractMapping contract = new ContractMapping()
            {
                Name = "Mocked contract",
                Description = "Mocked contract",
                ExternalId = "ext id",
                ExternalType = "Ext Type",
                Parameters = "{}"
            };
            contract.Customer = customers.FirstOrDefault();
            contractMappings.Add(contract);
            customer.ContractMappings.AddRange(contractMappings);


        }

        public void AddToCustomers(Customer entity)
        {
            entity.Id = DateTime.Now.Ticks;
            customers.Add(entity);
        }
        public void AddToContractMappings(ContractMapping entity)
        {
            entity.Id = DateTime.Now.Ticks;
            contractMappings.Add(entity);
        }

    }
}