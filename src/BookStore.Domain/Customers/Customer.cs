using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace BookStore.Customers;

public class Customer : FullAuditedAggregateRoot<Guid>
{
    public string Code { get; set; }
    
    public string Cpf { get; set; }
    
    public string Name { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
    
    protected Customer()
    {
        // Necessário para o EF Core
    }

    public Customer(
        Guid id,
        string code,
        string cpf,
        string name,
        DateTime dateOfBirth) : base(id)
    {
        Code = code;
        Cpf = cpf;
        SetName(name);
        SetDateOfBirth(dateOfBirth);
    }

    public void SetName(string name)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));

        if (name.Trim().Length < 3)
        {
            throw new BusinessException(CustomerErrorCodes.NomeMuitoCurto)
                .WithData("MinLength", 3);
        }

        Name = name;
    }

    public void SetDateOfBirth(DateTime dateOfBirth)
    {
        var age = CalculateAge(dateOfBirth);

        if (age < 18)
        {
            throw new BusinessException(CustomerErrorCodes.MenorDeIdade)
                .WithData("MinIdade", 18);
        }

        DateOfBirth = dateOfBirth;
    }

    private static int CalculateAge(DateTime dateOfBirth)
    {
        var hoje = DateTime.Today;
        var age  = hoje.Year - dateOfBirth.Year;

        if (dateOfBirth.Date > hoje.AddYears(-age))
        {
            age--;
        }

        return age;
    }
    
}
