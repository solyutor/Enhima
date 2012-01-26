namespace Enhima.Tests.Domain
{
    public abstract class Customer
    {
        public virtual long Id { get; set; }

        public virtual Address DeliveryAddress { get; set; }
    }

    public class JuristicPerson : Customer
    {
        
    }

    public class NaturalPerson : Customer
    {
        
    }
}