using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates.ExecutionResults;
using MediatR;
using EventFlow;
using EventFlow.Configuration;
using ApplicationLayer.Commands;
using DomainModel;

namespace ApplicationLayer.External
{
    public sealed class OrderPaidEventProcessor : IExternalEventProcessor, IRequestHandler<OrderPaidEvent, IExecutionResult>
    {
        private readonly ICommandBus _commandBus;

        public OrderPaidEventProcessor(IResolver resolver)
        {
            _commandBus = resolver.Resolve<ICommandBus>();
        }

        /// <summary>
        /// Handles <see cref="OrderPaidEvent"/>.
        /// </summary>
        /// <param name="request"><see cref="OrderPaidEvent"/> which is to be handled.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns></returns>
        public async Task<IExecutionResult> Handle(OrderPaidEvent request, CancellationToken cancellationToken)
        {
            var command = new RecordSuitAlterationPaymentCommand(SuitAlterationId.With(request.SuitAlterationId));
            return await _commandBus.PublishAsync(command, cancellationToken);
        }
    }
}
