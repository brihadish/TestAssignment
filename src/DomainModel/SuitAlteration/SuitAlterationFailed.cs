using System;
using EventFlow.Aggregates;

namespace DomainModel.SuitAlteration
{
    /// <summary>
    /// Represents failure in performing an alteration on a suit's sleeve(s).
    /// </summary>
    public sealed class SuitAlterationFailed : AggregateEvent<SuitAlterationAggregate, SuitAlterationId>
    {
        public SuitAlterationFailed(CustomerId customerId, SuitId suitId, string failureReason, TailorId tailorId, DateTime timestampUtc)
        {
            CustomerId = customerId;
            SuitId = suitId;
            FailureReason = failureReason;
            TailorId = tailorId;
            Status = SuitAlterationStatus.Failed;
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
        /// Gets the reason for failure of alteration.
        /// </summary>
        public string FailureReason { get; }

        /// <summary>
        /// Gets the unique identity of the tailor who performed the alteration.
        /// </summary>
        public TailorId TailorId { get; }

        /// <summary>
        /// Get the status of suit alteration.
        /// </summary>
        public SuitAlterationStatus Status { get; }

        /// <summary>
        /// Gets the timestamp of payment.
        /// </summary>
        public DateTime TimestampUtc { get; }
    }
}
