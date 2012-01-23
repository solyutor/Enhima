using Iesi.Collections.Generic;

namespace Enhima.Tests.Domain
{
    public class Order : AggregateRoot
    {
        private Customer _customer;
        private ISet<OrderItem> _items;

        protected Order()
        {
        }

        public Order(Customer customer)
        {
            _customer = customer;
            _items = new HashedSet<OrderItem>();
        }

        public virtual Customer Customer
        {
            get { return _customer; }
        }

        public virtual YesNo? Approved { get; set; }

        public virtual ISet<OrderItem> Items
        {
            get { return _items; }
        }
    }
}