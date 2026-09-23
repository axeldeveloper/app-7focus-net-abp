using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace BookStore.Customers;

public class Customer : AuditedAggregateRoot<Guid>
{
    public string Code { get; set; }
    
    public string Name { get; set; }
    
    public DateTime DateOfBirth { get; set; }

    public float Price { get; set; }
}
