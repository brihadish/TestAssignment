using EventFlow.Aggregates;

namespace DomainModel.SuitAlteration
{
    /// <summary>
    /// Represents successful payment for a suit alteration.
    /// </summary>
    public sealed class SuitAlterationPaymentReceived : AggregateEvent<SuitAlterationAggregate, SuitAlterationId>
    {
        public SuitAlterationPaymentReceived()
        {
            Status = SuitAlterationStatus.Paid;
        }

        /// <summary>
        /// Get the status of suit alteration.
        /// </summary>
        public SuitAlterationStatus Status { get; }
    }
}
