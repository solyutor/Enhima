using System.Collections.Generic;

namespace Enhima.Tests.Domain
{
    public class Product : AggregateRoot
    {
        private string _name;

        public virtual string Name
        {
            get { return _name ?? string.Empty; }
            set { _name = value; }
        }

        public virtual YesNo Active { get; set; }

        public IList<Tag> Tags { get; set; }

        public IList<string> Opinions { get; set; }
    }
}