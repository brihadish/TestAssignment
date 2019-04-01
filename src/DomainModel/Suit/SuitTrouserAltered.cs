using EventFlow.Aggregates;

namespace DomainModel.Suit
{
    /// <summary>
    /// Represents alteration of trousers in a suit.
    /// </summary>
    public sealed class SuitTrouserAltered : AggregateEvent<SuitAggregate, SuitId>
    {
        public SuitTrouserAltered(SuitAlterationId suitAlterationId, TailorId tailorId, SuitTrouserPair suitTrouserPair)
        {
            SuitAlterationId = suitAlterationId;
            TailorId = tailorId;
            SuitTrouserPair = suitTrouserPair;
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
        /// Gets the modified suit trouser pair data.
        /// </summary>
        public SuitTrouserPair SuitTrouserPair { get; }
    }
}
