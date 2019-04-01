using System.Threading;
using System.Threading.Tasks;
using EventFlow.Configuration;
using EventFlow.Jobs;
using ApplicationLayer.Services;
using ApplicationLayer.External;
using System;
using MediatR;

namespace ApplicationLayer.Jobs
{
    /// <summary>
    /// Job that processes events received from external applications.
    /// </summary>
    public sealed class ExternalEventProcessorJob : IJob
    {
        public async Task ExecuteAsync(IResolver resolver, CancellationToken cancellationToken)
        {
            IExternalEventReceiver receiver = null;
            IMediator mediator = null;
            try
            {
                receiver = resolver.Resolve<IExternalEventReceiver>();
                var mediatorBuilder = resolver.Resolve<ExternalEventMediatorBuilder>();
                mediator = mediatorBuilder.Build();
            }
            catch(Exception e)
            {
                // TODO :: Log here.
                return;
            }

            while (true)
            {
                // This is a waiting call and will return when an event is received.
                var @event = await receiver.ReceiveEventAsync();
                try
                {
                    var result = await mediator.Send(@event);
                    // TODO :: Log here.
                }
                catch(Exception e)
                {
                    // TODO :: Log here.
                }
            }
        }
    }
}
