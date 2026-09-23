using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace BookStore.Customers;

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