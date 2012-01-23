using System.Collections.Generic;

namespace Enhima.Tests.Domain
{
    public class Tag : AggregateRoot
    {
        public IList<Product> Products { get; set; }
    }
}