using System.Threading;
using System.Threading.Tasks;
using ApplicationLayer.Commands;
using DomainModel;
using DomainModel.Suit;
using DomainModel.SuitAlteration;
using EventFlow.Commands;
using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow;

namespace ApplicationLayer.CommandHandlers
{
    /// <summary>
    /// Handles <see cref="ExecuteSuitAlterationCommand"/>.
    /// </summary>
    public sealed class ExecuteSuitAlterationCommandHandler : ICommandHandler<SuitAggregate, SuitId, IExecutionResult, ExecuteSuitAlterationCommand>
    {
        private readonly IAggregateStore _aggregateStore;
        private readonly ICommandBus _commandBus;

        public ExecuteSuitAlterationCommandHandler(IAggregateStore aggregateStore, ICommandBus commandBus)
        {
            _aggregateStore = aggregateStore;
            _commandBus = commandBus;
        }

        public  async Task<IExecutionResult> ExecuteCommandAsync(
            SuitAggregate aggregate, ExecuteSuitAlterationCommand command, CancellationToken cancellationToken)
        {
            if (aggregate.IsNew)
            {
                return ExecutionResult.Failed($"Suit[Id :: {command.AggregateId}] not found.");
            }

            var suitAlteration = await _aggregateStore.LoadAsync<SuitAlterationAggregate, SuitAlterationId>(command.SuitAlterationId, cancellationToken);
            if (suitAlteration.IsNew)
            {
                return ExecutionResult.Failed($"SuitAlteration[Id :: {command.SuitAlterationId}] not found.");
            }

            var result = suitAlteration.ExecuteAlteration(aggregate, command.TailorId);
            if (!result.IsSuccess)
            {
                if (result.ToString() == SuitAggregateFailureReasons.SpecifiedAlterationAlreadyPerformed)
                {
                    // This means that there was crash during the previous command execution which lead to suit alteration
                    // not getting updated. Explicitly execute the command to mark the alteration as success.
                    return await _commandBus.PublishAsync(new MarkSuitAlterationAsSuccessCommand(command.SuitAlterationId, command.TailorId), cancellationToken);
                }
                else
                {
                    return await _commandBus.PublishAsync(new MarkSuitAlterationAsFailureCommand(command.SuitAlterationId, command.TailorId), cancellationToken);
                }
            }

            return result;
        }
    }
}
