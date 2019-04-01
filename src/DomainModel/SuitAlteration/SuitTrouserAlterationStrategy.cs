using DomainModel.Suit;
using EventFlow.Aggregates.ExecutionResults;

namespace DomainModel.SuitAlteration
{
    /// <summary>
    /// Provides implementation for altering a suit's trouser(s).
    /// </summary>
    public sealed class SuitTrouserAlterationStrategy : ISuitAlterationStrategy
    {
        private readonly SuitTrouserAlterationChoice _suitTrouserAlterationChoice;

        public SuitTrouserAlterationStrategy(SuitTrouserAlterationChoice suitTrouserAlterationChoice)
        {
            _suitTrouserAlterationChoice = suitTrouserAlterationChoice;
        }

        /// <summary>
        /// Alters the specified suit.
        /// </summary>
        /// <param name="suitAlteration"><see cref="SuitAlterationAggregate"/> which is to be performed.</param>
        /// <param name="suit"><see cref="SuitAggregate"/> which is to be altered.</param>
        /// <param name="tailorId">Unique identity of the tailor who is performing the alteration.</param>
        /// <returns>Result of alteration.</returns>
        public IExecutionResult Alter(SuitAlterationAggregate suitAlteration, SuitAggregate suit, TailorId tailorId)
        {
            return suit.Alter(suitAlteration.Id, tailorId, _suitTrouserAlterationChoice, suitAlteration.AlterationMeasurement);
        }
    }
}
