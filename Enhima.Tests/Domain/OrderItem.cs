namespace Enhima.Tests.Domain
{
    public class OrderItem : Entity
    {
        private readonly Product _product;
        private Order _ownedOrder;

        protected OrderItem()
        {
        }

        public OrderItem(Product product, Order order)
        {
            _product = product;
            _ownedOrder = order;
        }

        public virtual Order OwnedOrder
        {
            get { return _ownedOrder; }
        }

        public virtual Product Product
        {
            get { return _product; }
        }
    }
}