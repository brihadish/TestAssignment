using System;
using EventFlow.Aggregates;

namespace DomainModel.SuitAlteration
{
    /// <summary>
    /// Represents the addition of an sleeve alteration request on a suit.
    /// </summary>
    public sealed class SuitSleeveAlterationCreated : AggregateEvent<SuitAlterationAggregate, SuitAlterationId>
    {
        public SuitSleeveAlterationCreated(
                    CustomerId customerId,
                    SuitId suitId,
                    SuitSleeveAlterationChoice suitSleeveAlterationChoice,
                    MeasurementAlteration alteration,
                    DateTime timestampUtc)
        {
            CustomerId = customerId;
            SuitId = suitId;
            SuitSleeveAlterationChoice = suitSleeveAlterationChoice;
            Alteration = alteration;
            Status = SuitAlterationStatus.Created;
            TimestampUtc = timestampUtc;
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
        /// Gets the choice for sleeve alteration.
        /// </summary>
        public SuitSleeveAlterationChoice SuitSleeveAlterationChoice { get; }


        /// <summary>
        /// Gets the alteration value.
        /// </summary>
        public MeasurementAlteration Alteration { get; }

        /// <summary>
        /// Get the status of suit alteration.
        /// </summary>
        public SuitAlterationStatus Status { get; }

        /// <summary>
        /// Gets the timestamp of creation.
        /// </summary>
        public DateTime TimestampUtc { get; }
    }
}
