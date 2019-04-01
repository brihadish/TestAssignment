using EventFlow.Aggregates;

namespace DomainModel.Suit
{
    /// <summary>
    /// Represents the selling of a suit to a customer.
    /// </summary>
    public sealed class SuitSoldEvent : AggregateEvent<SuitAggregate, SuitId>
    {
        public SuitSoldEvent(CustomerId customerId)
        {
            CustomerId = customerId;
        }

        /// <summary>
        /// Gets the unique identity of the customer to whom the suit was sold.
        /// </summary>
        public CustomerId CustomerId { get; }
    }
}
