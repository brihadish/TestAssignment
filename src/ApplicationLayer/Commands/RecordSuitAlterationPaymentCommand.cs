using DomainModel;
using DomainModel.SuitAlteration;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using Newtonsoft.Json;

namespace ApplicationLayer.Commands
{
    /// <summary>
    /// Represents a command for recording payment for a suit's alteration.
    /// </summary>
    public sealed class RecordSuitAlterationPaymentCommand : Command<SuitAlterationAggregate, SuitAlterationId, IExecutionResult>
    {
        [JsonConstructor]
        public RecordSuitAlterationPaymentCommand(SuitAlterationId aggregateId)
            : base(aggregateId)
        {
        }
    }
}
