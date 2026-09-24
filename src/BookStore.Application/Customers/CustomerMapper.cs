using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace BookStore.Customers
{
    [Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
    public partial class CustomerMapper : MapperBase<Customer, CustomerDto>
    {
        public override partial CustomerDto Map(Customer source);

        public override partial void Map(Customer source, CustomerDto destination);
    }
}