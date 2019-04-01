using System.Threading;
using System.Threading.Tasks;
using ApplicationLayer.Commands;
using DomainModel;
using DomainModel.Suit;
using DomainModel.SuitAlteration;
using EventFlow.Commands;
using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Logs;

namespace ApplicationLayer.CommandHandlers
{
    /// <summary>
    /// Handles <see cref="CreateSuitSleeveAlterationCommand"/>.
    /// </summary>
    public sealed class CreateSuitSleeveAlterationCommandHandler : ICommandHandler<SuitAlterationAggregate, SuitAlterationId, IExecutionResult, CreateSuitSleeveAlterationCommand>
    {
        private readonly IAggregateStore _aggregateStore;
        private readonly ILog _log;

        public CreateSuitSleeveAlterationCommandHandler(IAggregateStore aggregateStore, ILog log)
        {
            _aggregateStore = aggregateStore;
            _log = log;
        }

        public async Task<IExecutionResult> ExecuteCommandAsync(
            SuitAlterationAggregate aggregate, CreateSuitSleeveAlterationCommand command, CancellationToken cancellationToken)
        {
            var suit = await _aggregateStore.LoadAsync<SuitAggregate, SuitId>(command.SuitId, cancellationToken);
            if (suit.IsNew)
            {
                var result = ExecutionResult.Failed($"Suit [Id :: {command.SuitId}] not found.");
                _log.Error(result.ToString());
                return result;
            }

            aggregate.CreateForSleeve(suit, command.SuitSleeveAlterationChoice, command.SleeveAlteration);
            _log.Information("Created sleeve alteration with id [{0}] for suit [{1}]", aggregate.Id, suit.Id);
            return ExecutionResult.Success();
        }
    }
}
