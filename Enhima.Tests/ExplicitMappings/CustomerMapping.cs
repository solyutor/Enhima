using Enhima.Tests.Domain;
using NHibernate.Mapping.ByCode.Conformist;

namespace Enhima.Tests.ExplicitMappings
{
    public class CustomerMapping : ClassMapping<Customer>
    {
        public CustomerMapping()
        {
            Id(x => x.Id);
        }
    }
}