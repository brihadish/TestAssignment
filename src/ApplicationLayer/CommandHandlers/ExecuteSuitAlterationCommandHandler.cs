using System.Threading;
using System.Threading.Tasks;
using ApplicationLayer.Commands;
using DomainModel;
using DomainModel.Suit;
using DomainModel.SuitAlteration;
using EventFlow;
using EventFlow.Commands;
using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Logs;

namespace ApplicationLayer.CommandHandlers
{
    /// <summary>
    /// Handles <see cref="ExecuteSuitAlterationCommand"/>.
    /// </summary>
    public sealed class ExecuteSuitAlterationCommandHandler : ICommandHandler<SuitAggregate, SuitId, IExecutionResult, ExecuteSuitAlterationCommand>
    {
        private readonly IAggregateStore _aggregateStore;
        private readonly ICommandBus _commandBus;
        private readonly ILog _log;

        public ExecuteSuitAlterationCommandHandler(IAggregateStore aggregateStore, ICommandBus commandBus, ILog log)
        {
            _aggregateStore = aggregateStore;
            _commandBus = commandBus;
            _log = log;
        }

        public  async Task<IExecutionResult> ExecuteCommandAsync(
            SuitAggregate aggregate, ExecuteSuitAlterationCommand command, CancellationToken cancellationToken)
        {
            if (aggregate.IsNew)
            {
                var failedResult = ExecutionResult.Failed($"Suit[Id :: {command.AggregateId}] not found.");
                _log.Error(failedResult.ToString());
                return failedResult;
            }

            var suitAlteration = await _aggregateStore.LoadAsync<SuitAlterationAggregate, SuitAlterationId>(command.SuitAlterationId, cancellationToken);
            if (suitAlteration.IsNew)
            {
                var failedResult = ExecutionResult.Failed($"SuitAlteration[Id :: {command.SuitAlterationId}] not found.");
                _log.Error(failedResult.ToString());
                return failedResult;
            }

            var result = suitAlteration.ExecuteAlteration(aggregate, command.TailorId);
            if (!result.IsSuccess)
            {
                if (result.ToString() == SuitAggregateFailureReasons.SpecifiedAlterationAlreadyPerformed)
                {
                    _log.Warning("Executing command to mark suitalteration [{0}] as success since previous operation seems to have crashed.", command.SuitAlterationId);
                    // This means that there was crash during the previous command execution which lead to suit alteration
                    // not getting updated. Explicitly execute the command to mark the alteration as success.
                    return await _commandBus.PublishAsync(new MarkSuitAlterationAsSuccessCommand(command.SuitAlterationId, command.TailorId), cancellationToken);
                }
                else
                {
                    _log.Error(result.ToString());
                    return await _commandBus.PublishAsync(new MarkSuitAlterationAsFailureCommand(command.SuitAlterationId, command.TailorId), cancellationToken);
                }
            }
            else
            {
                _log.Information("Successfully altered suit [{0}] via suitalteration [{1}]", command.AggregateId, command.SuitAlterationId);
            }

            return result;
        }
    }
}
