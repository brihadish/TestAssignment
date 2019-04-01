using System.Threading;
using System.Threading.Tasks;
using ApplicationLayer.Commands;
using DomainModel;
using DomainModel.SuitAlteration;
using EventFlow.Commands;
using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;
using DomainModel.Suit;

namespace ApplicationLayer.CommandHandlers
{
    /// <summary>
    /// Handles <see cref="CreateSuitTrouserAlterationCommand"/>.
    /// </summary>
    public sealed class CreateSuitTrouserAlterationCommandHandler : ICommandHandler<SuitAlterationAggregate, SuitAlterationId, IExecutionResult, CreateSuitTrouserAlterationCommand>
    {
        private readonly IAggregateStore _aggregateStore;

        public CreateSuitTrouserAlterationCommandHandler(IAggregateStore aggregateStore)
        {
            _aggregateStore = aggregateStore;
        }

        public async Task<IExecutionResult> ExecuteCommandAsync(
            SuitAlterationAggregate aggregate, CreateSuitTrouserAlterationCommand command, CancellationToken cancellationToken)
        {
            var suit = await _aggregateStore.LoadAsync<SuitAggregate, SuitId>(command.SuitId, cancellationToken);
            if (suit.IsNew)
            {
                return ExecutionResult.Failed($"Suit [Id :: {command.SuitId}] not found.");
            }

            aggregate.CreateForTrouser(suit, command.SuitTrouserAlterationChoice, command.SleeveAlteration);
            return ExecutionResult.Success();
        }
    }
}
