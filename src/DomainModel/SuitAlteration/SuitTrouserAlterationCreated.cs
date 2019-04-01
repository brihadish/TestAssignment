using DomainModel.Suit;
using EventFlow.Aggregates;

namespace DomainModel.SuitAlteration
{
    /// <summary>
    /// Represents the addition of an trouser alteration request on a suit.
    /// </summary>
    public sealed class SuitTrouserAlterationCreated : AggregateEvent<SuitAlterationAggregate, SuitAlterationId>
    {
        public SuitTrouserAlterationCreated(
            CustomerId customerId, SuitId suitId, SuitTrouserAlterationChoice suitTrouserAlterationChoice, MeasurementAlteration alteration)
        {
            CustomerId = customerId;
            SuitId = suitId;
            SuitTrouserAlterationChoice = suitTrouserAlterationChoice;
            Alteration = alteration;
            Status = SuitAlterationStatus.Created;
        }

        /// <summary>
        /// Gets the unique identity of the customer to whom the suit belongs to.
        /// </summary>
        public CustomerId CustomerId { get; }

        /// <summary>
        /// Gets the unique identity of the suit on which sleeve alteration is to be performed.
        /// </summary>
        public SuitId SuitId { get; }

        /// <summary>
        /// Gets the choice for trouser alteration.
        /// </summary>
        public SuitTrouserAlterationChoice SuitTrouserAlterationChoice { get; }

        /// <summary>
        /// Gets the alteration value.
        /// </summary>
        public MeasurementAlteration Alteration { get; }

        /// <summary>
        /// Get the status of suit alteration.
        /// </summary>
        public SuitAlterationStatus Status { get; }
    }
}
