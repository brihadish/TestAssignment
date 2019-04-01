using EventFlow.Aggregates;

namespace DomainModel.Suit
{
    /// <summary>
    /// Represents alteration of sleeves in a suit.
    /// </summary>
    public sealed class SuitSleeveAltered : AggregateEvent<SuitAggregate, SuitId>
    {
        public SuitSleeveAltered(SuitAlterationId suitAlterationId, TailorId tailorId, SuitSleevePair suitSleevePair)
        {
            SuitAlterationId = suitAlterationId;
            TailorId = tailorId;
            SuitSleevePair = suitSleevePair;
        }

        /// <summary>
        /// Gets the unique identity of the alteration which caused the suit to get altered.
        /// </summary>
        public SuitAlterationId SuitAlterationId { get; }

        /// <summary>
        /// Gets the unique identity of the tailor who performed the alteration.
        /// </summary>
        public TailorId TailorId { get; }

        /// <summary>
        /// Gets the modified suit sleeve pair data.
        /// </summary>
        public SuitSleevePair SuitSleevePair { get; }
    }
}
