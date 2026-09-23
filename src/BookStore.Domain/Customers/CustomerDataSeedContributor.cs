using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace BookStore.Customers
{
    public class CustomerDataSeedContributor
        : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Customer, Guid> _customerRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentTenant _currentTenant;

        public CustomerDataSeedContributor(
            IRepository<Customer, Guid> customerRepository,
            IGuidGenerator guidGenerator,
            ICurrentTenant currentTenant)
        {
            _customerRepository = customerRepository;
            _guidGenerator = guidGenerator;
            _currentTenant = currentTenant;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            using (_currentTenant.Change(context?.TenantId))
            {
                if (await _customerRepository.GetCountAsync() > 0)
                {
                    return;
                }

                await _customerRepository.InsertAsync(
                    new Customer(
                        _guidGenerator.Create(),
                        "CUST-001", 
                        "00100200300", 
                        "João da Silva",
                        new DateTime(1990, 5, 14)
                    ),
                    autoSave: true
                );

                await _customerRepository.InsertAsync(
                    new Customer(
                        _guidGenerator.Create(),
                        "CUST-002",
                        "00100200301", 
                        "Maria Oliveira",
                        new DateTime(1985, 11, 2)
                    ),
                    autoSave: true
                );

                await _customerRepository.InsertAsync(
                    new Customer(
                        _guidGenerator.Create(),
                        "CUST-003",
                        "00100200303", 
                        "Carlos Pereira",
                        new DateTime(2000, 3, 30)
                    ),
                    autoSave: true
                );
            }
        }
    }
}