using System;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Customers;
using Shouldly;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Xunit;

namespace BookStore.Customers;

public abstract class CustomerServiceTests<TStartupModule> : BookStoreApplicationTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly ICustomerAppService _customerAppService;

    protected CustomerServiceTests()
    {
        _customerAppService = GetRequiredService<ICustomerAppService>();
    }
    
    
    [Fact]
    public async Task Should_Get_List_Of_Customers()
    {
        //Act
        var result = await _customerAppService.GetListAsync(
            new PagedAndSortedResultRequestDto()
        );

        //Assert
        result.TotalCount.ShouldBeGreaterThan(0);
        result.Items.ShouldContain(b => b.Code == "CUST-001");
    }
    
    [Fact]
    public async Task Should_Create_A_Valid_Customer()
    {
        //Act
        var result = await _customerAppService.CreateAsync(
            new CreateUpdateCustomerDto
            {
                Name = "Zenitsu",
                Code = "CUST-006",
                DateOfBirth = DateTime.Today.AddYears(-20),
                Cpf = "00100200307",
            }
        );

        //Assert
        result.Id.ShouldNotBe(Guid.Empty);
        result.Name.ShouldBe("Zenitsu");
    }
    
    [Fact]
    public async Task Should_Not_Create_A_Customer_Without_Name()
    {
        var exception = await Assert.ThrowsAsync<AbpValidationException>(async () =>
        {
            await _customerAppService.CreateAsync(
                new CreateUpdateCustomerDto
                {
                    Name = "",
                    Code = "CUST-007",
                    DateOfBirth = DateTime.Today.AddYears(-20),
                    Cpf = "00100200308",
                }
            );
        });

        exception.ValidationErrors
            .ShouldContain(err => err.MemberNames.Any(mem => mem == "Name"));
    }


}
