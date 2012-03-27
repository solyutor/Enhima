using System.Collections.Generic;
using Iesi.Collections.Generic;

namespace Enhima.Tests.Domain
{
    public class Tag : AggregateRoot
    {
        public virtual IList<Product> Products { get; set; }

        public virtual IDictionary<string, string> Reviews { get; set; }

        public virtual ISet<Tag> Children { get; set; }

    }
}