using System;
using BookStore.Permissions;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.Repositories;

namespace BookStore.Customers
{ 


    [Authorize(BookStorePermissions.Customers.Default)]
    public class CustomerAppService : CrudAppService<
        Customer, 
        CustomerDto, 
        Guid, 
        PagedAndSortedResultRequestDto, 
        CreateUpdateCustomerDto,
        CreateUpdateCustomerDto
    >, ICustomerAppService
    {
        
        public CustomerAppService(IRepository<Customer, Guid> repository)
            : base(repository)
        {
            GetPolicyName = BookStorePermissions.Customers.Default;
            GetListPolicyName = BookStorePermissions.Customers.Default;
            CreatePolicyName = BookStorePermissions.Customers.Create;
            UpdatePolicyName = BookStorePermissions.Customers.Edit;
            DeletePolicyName = BookStorePermissions.Customers.Delete;
        }
        
        // Sobrescreve para usar o construtor com validação (SetNome/SetDataNascimento)
        // ao invés do mapeamento automático do AutoMapper, que só faria set direto.
        protected override Customer MapToEntity(CreateUpdateCustomerDto createInput)
        {
            return new Customer(
                GuidGenerator.Create(),
                createInput.Code,
                createInput.Cpf,
                createInput.Name,
                createInput.DateOfBirth
            );
        }

        protected override void MapToEntity(CreateUpdateCustomerDto updateInput, Customer entity)
        {
            entity.Code = updateInput.Code;
            entity.Cpf = updateInput.Cpf;
            entity.SetName(updateInput.Name);
            entity.SetDateOfBirth(updateInput.DateOfBirth);
        }
    }
}