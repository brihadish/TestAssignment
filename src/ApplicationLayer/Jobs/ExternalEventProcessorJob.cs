using System.Threading;
using System.Threading.Tasks;
using EventFlow.Configuration;
using EventFlow.Jobs;
using ApplicationLayer.Services;
using ApplicationLayer.External;
using System;
using MediatR;
using EventFlow.Logs;
using Newtonsoft.Json;

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
            ILog log = null;
            try
            {
                log = resolver.Resolve<ILog>();
                receiver = resolver.Resolve<IExternalEventReceiver>();
                var mediatorBuilder = resolver.Resolve<ExternalEventMediatorBuilder>();
                mediator = mediatorBuilder.Build();
            }
            catch(Exception e)
            {
                if (log != null)
                {
                    log.Fatal(e, "Error happened during resolving dependencies in 'ExternalEventProcessorJob'");
                }

                return;
            }

            while (true)
            {
                // This is a waiting call and will return when an event is received.
                var @event = await receiver.ReceiveEventAsync();
                try
                {
                    var result = await mediator.Send(@event);
                    if (result.IsSuccess)
                    {
                        log.Information(
                            "Successfully processed event with payload {0}",
                            JsonConvert.SerializeObject(@event));
                    }
                    else
                    {
                        log.Error(
                            "Failure during processing of event with payload {0}. Error [{1}]",
                            JsonConvert.SerializeObject(@event),
                            result.ToString());
                    }
                }
                catch(Exception e)
                {
                    log.Fatal(e, "Unexpected error occurred when receiving or processing events.");
                }
            }
        }
    }
}
