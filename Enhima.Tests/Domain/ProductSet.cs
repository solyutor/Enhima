using System.Collections.Generic;

namespace Enhima.Tests.Domain
{
    //Used to test joined sublass mapping
    public class ProductSet : Product
    {
        public IList<Product> Components { get; set; }
    }
}