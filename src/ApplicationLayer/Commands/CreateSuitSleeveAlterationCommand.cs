using System;
using DomainModel;
using DomainModel.SuitAlteration;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using Newtonsoft.Json;

namespace ApplicationLayer.Commands
{
    /// <summary>
    /// Represents a command for creating a sleeve alteration on a suit.
    /// </summary>
    public sealed class CreateSuitSleeveAlterationCommand : Command<SuitAlterationAggregate, SuitAlterationId, IExecutionResult>
    {
        private const int MinAllowedSleeveAlteration = -5;
        private const int MaxAllowedSleeveAlteration = 5;

        [JsonConstructor]
        public CreateSuitSleeveAlterationCommand(
            SuitAlterationId aggregateId, SuitId suitId, SuitSleeveAlterationChoice suitSleeveAlterationChoice, MeasurementAlteration sleeveAlteration)
            : base(aggregateId)
        {
            SuitId = suitId ?? throw new ArgumentNullException(nameof(suitId));
            SuitSleeveAlterationChoice = suitSleeveAlterationChoice;
            SleeveAlteration = sleeveAlteration ?? throw new ArgumentNullException(nameof(sleeveAlteration));

            if (SleeveAlteration.Value > MaxAllowedSleeveAlteration || SleeveAlteration.Value < MinAllowedSleeveAlteration)
            {
                throw new ArgumentOutOfRangeException(nameof(sleeveAlteration));
            }
        }

        public SuitId SuitId { get; set; }

        public SuitSleeveAlterationChoice SuitSleeveAlterationChoice { get; set; }

        public MeasurementAlteration SleeveAlteration { get; set; }
    }
}
