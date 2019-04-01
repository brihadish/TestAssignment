using System;
using DomainModel;
using DomainModel.SuitAlteration;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using Newtonsoft.Json;

namespace ApplicationLayer.Commands
{
    /// <summary>
    /// Represents a command for creating a trouser alteration on a suit.
    /// </summary>
    public sealed class CreateSuitTrouserAlterationCommand : Command<SuitAlterationAggregate, SuitAlterationId, IExecutionResult>
    {
        private const int MinAllowedTrouserAlteration = -5;
        private const int MaxAllowedTrouserAlteration = 5;

        [JsonConstructor]
        public CreateSuitTrouserAlterationCommand(
            SuitAlterationId aggregateId, SuitId suitId, SuitTrouserAlterationChoice suitTrouserAlterationChoice, MeasurementAlteration sleeveAlteration)
            : base(aggregateId)
        {
            SuitId = suitId ?? throw new ArgumentNullException(nameof(suitId));
            SuitTrouserAlterationChoice = suitTrouserAlterationChoice;
            SleeveAlteration = sleeveAlteration ?? throw new ArgumentNullException(nameof(sleeveAlteration));

            if (SleeveAlteration.Value > MaxAllowedTrouserAlteration || SleeveAlteration.Value < MinAllowedTrouserAlteration)
            {
                throw new ArgumentOutOfRangeException(nameof(sleeveAlteration));
            }
        }

        public SuitId SuitId { get; set; }

        public SuitTrouserAlterationChoice SuitTrouserAlterationChoice { get; set; }

        public MeasurementAlteration SleeveAlteration { get; set; }
    }
}
