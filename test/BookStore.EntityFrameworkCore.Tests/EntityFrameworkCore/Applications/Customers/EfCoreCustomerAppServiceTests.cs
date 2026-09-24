using BookStore.Customers;
using Xunit;

namespace BookStore.EntityFrameworkCore.Applications.Customers;



[Collection(BookStoreTestConsts.CollectionDefinitionName)]
public class EfCoreCustomerAppServiceTests : CustomerServiceTests<BookStoreEntityFrameworkCoreTestModule>
{

}
