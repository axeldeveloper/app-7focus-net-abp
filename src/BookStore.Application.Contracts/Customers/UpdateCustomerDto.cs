using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStore.Customers;

public class UpdateCustomerDto : IValidatableObject
{
    [Required]
    [StringLength(14)]
    public string Cpf { get; set; }

    [Required]
    [MinLength(3)]
    [StringLength(128)]
    public string Name { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var idade = DateTime.Today.Year - DateOfBirth.Year;
        if (DateOfBirth.Date > DateTime.Today.AddYears(-idade)) idade--;

        if (idade < 18)
        {
            yield return new ValidationResult(
                "Cliente precisa ter no mínimo 18 anos.",
                new[] { nameof(DateOfBirth) }
            );
        }
    }
}