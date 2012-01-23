namespace Enhima.Tests.Domain
{
    public abstract class Customer
    {
        public long Id { get; set; }

        public Address DeliveryAddress { get; set; }
    }

    public class JuristicPerson : Customer
    {
        
    }

    public class NaturalPerson : Customer
    {
        
    }
}