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
    /// Handles <see cref="CreateSuitSleeveAlterationCommand"/>.
    /// </summary>
    public sealed class CreateSuitSleeveAlterationCommandHandler : ICommandHandler<SuitAlterationAggregate, SuitAlterationId, IExecutionResult, CreateSuitSleeveAlterationCommand>
    {
        private readonly IAggregateStore _aggregateStore;

        public CreateSuitSleeveAlterationCommandHandler(IAggregateStore aggregateStore)
        {
            _aggregateStore = aggregateStore;
        }

        public async Task<IExecutionResult> ExecuteCommandAsync(
            SuitAlterationAggregate aggregate, CreateSuitSleeveAlterationCommand command, CancellationToken cancellationToken)
        {
            var suit = await _aggregateStore.LoadAsync<SuitAggregate, SuitId>(command.SuitId, cancellationToken);
            if (suit.IsNew)
            {
                return ExecutionResult.Failed($"Suit [Id :: {command.SuitId}] not found.");
            }

            aggregate.CreateForSleeve(suit, command.SuitSleeveAlterationChoice, command.SleeveAlteration);
            return ExecutionResult.Success();
        }
    }
}
