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
        static List<CimiTarget> cimiTargets;
        static List<ContractMapping> contractMappings;
        static List<Tenant> tenants;
        static List<User> users;

        public List<Customer> Customers { get { return customers; } }
        public List<CimiTarget> CimiTargets { get { return cimiTargets; } }
        public List<ContractMapping> ContractMappings { get { return contractMappings; } }
        public List<Tenant> Tenants { get { return tenants; } }
        public List<User> Users { get { return users; } }

        static CoreRepositoryMock()
        {
            // 1. tenants
            tenants = new List<Tenant>();
            Tenant tenantParent = new Tenant()
            {
                Id = System.Guid.NewGuid(),
                ExternalId = "ex id parent",
                ExternalType = "ex type"
            };
            tenants.Add(tenantParent);
            Tenant tenant = new Tenant()
            {
                Id = System.Guid.NewGuid(),
                ExternalId = "ex id child",
                ExternalType = "ex type",
                Parent = tenantParent,
                ParentId = tenantParent.Id
            };
            tenants.Add(tenant);
            tenantParent.Children.Add(tenant);


            // 2. Customers
            customers = new List<Customer>();
            Customer customer = new Customer()
            {
                Name = "Mocked customer",
                Description = "Mocked description", 
            };
            customer.Tenants.AddRange(tenants);
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


            // Users
            users = new List<User>();
            User user = new User()
            {
                Name = "Mocked user",
                Description = "Mocked description",
                ExternalId = "ex id", 
                Type="user type"
            };
            users.Add(user);


            // cimi targets
            cimiTargets = new List<CimiTarget>();
            CimiTarget cimitarget = new CimiTarget()
            {
                Name = "Mocked CimiTarget",
                Description = "Mocked description",
                CimiId = "333",
                CimiType = "cimi type",
                CatalogueItemId = -5
            };
            cimiTargets.Add(cimitarget);

        }
        
        


        
        public void AddToCustomers(Customer entity) {
            entity.Id = DateTime.Now.Ticks;
            customers.Add(entity);
        }
        public void AddToCimiTargets(CimiTarget entity)
        {
            entity.Id = DateTime.Now.Ticks;
            cimiTargets.Add(entity);
        }
        public void AddToContractMappings(ContractMapping entity)
        {
            entity.Id = DateTime.Now.Ticks;
            contractMappings.Add(entity);
        }
        public void AddToTenants(Tenant entity)
        {
            if (entity.ParentId != Guid.Empty)
            {
                Tenant parent = tenants.FirstOrDefault(t => t.Id == entity.ParentId);
                parent.Children.Add(entity);
                entity.Parent = parent;
            }
            tenants.Add(entity);
        }
        public void AddToUsers(User entity)
        {
            entity.Id = DateTime.Now.Ticks;
            users.Add(entity);
        }
    }
}