using System;
using Volo.Abp.Application.Dtos;

namespace BookStore.Customers;

public class CustomerDto : FullAuditedEntityDto<Guid>
{
    public string Code { get; set; }
    
    public string Cpf { get; set; }
    
    public string Name { get; set; }
    
    public DateTime DateOfBirth { get; set; }
}