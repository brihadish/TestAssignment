using System;
using EventFlow.Aggregates;

namespace DomainModel.SuitAlteration
{
    /// <summary>
    /// Represents successful payment for a suit alteration.
    /// </summary>
    public sealed class SuitAlterationPaymentReceived : AggregateEvent<SuitAlterationAggregate, SuitAlterationId>
    {
        public SuitAlterationPaymentReceived(DateTime timestampUtc)
        {
            Status = SuitAlterationStatus.Paid;
            TimestampUtc = timestampUtc;
        }

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
