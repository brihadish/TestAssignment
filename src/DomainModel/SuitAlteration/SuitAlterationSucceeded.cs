using System;
using EventFlow.Aggregates;

namespace DomainModel.SuitAlteration
{
    /// <summary>
    /// Represents the success of alteration performed on a suit's sleeve(s).
    /// </summary>
    public sealed class SuitAlterationSucceeded : AggregateEvent<SuitAlterationAggregate, SuitAlterationId>
    {
        public SuitAlterationSucceeded(CustomerId customerId, SuitId suitId, TailorId tailorId, DateTime timestampUtc)
        {
            CustomerId = customerId;
            SuitId = suitId;
            TailorId = tailorId;
            Status = SuitAlterationStatus.Succeeded;
            TimestampUtc = timestampUtc;
        }

        /// <summary>
        /// Gets the unique identity of the customer to whom the suit belongs to.
        /// </summary>
        public CustomerId CustomerId { get; }

        /// <summary>
        /// Gets the unique identity of the suit on which sleeve alteration was performed.
        /// </summary>
        public SuitId SuitId { get; }

        /// <summary>
        /// Gets the unique identity of the tailor who performed the alteration.
        /// </summary>
        public TailorId TailorId { get; }

        /// <summary>
        /// Get the status of suit alteration.
        /// </summary>
        public SuitAlterationStatus Status { get; }

        /// <summary>
        /// Gets the timestamp of success.
        /// </summary>
        public DateTime TimestampUtc { get; }
    }
}
