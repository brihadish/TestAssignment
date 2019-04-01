using System.Collections.Generic;
using System.Reflection;
using ApplicationLayer.Queries;
using ApplicationLayer.QueryHandlers;
using ApplicationLayer.ReadModels;
using DomainModel;
using EventFlow;
using EventFlow.Extensions;
using ApplicationLayer.External;
using EventFlow.Configuration;
using ApplicationLayer.Jobs;
using ApplicationLayer.Services;

namespace ApplicationLayer
{
    /// <summary>
    /// Configures eventflow for running the application layer.
    /// </summary>
    public static class EventFlowApplicationLayer
    {
        public static Assembly Assembly { get; } = typeof(EventFlowApplicationLayer).Assembly;

        public static IEventFlowOptions Configure(IExternalEventReceiver externalEventReceiver)
        {
            // TODO :- Remove 'InMemoryReadStore' and 'InMemoryEventPersitence'. Take durable implementatiions 
            // as input.
            return EventFlowOptions.New
                .AddDefaults(typeof(EventFlowApplicationLayer).Assembly)
                .AddDefaults(typeof(SuitId).Assembly)
                .UseInMemoryReadStoreFor<SuitAlterationReadModel>()
                .AddQueryHandler<GetSuitAlterationsByStatusQueryHandler, GetSuitAlterationsByStatusQuery, IReadOnlyCollection<SuitAlterationReadModel>>()
                .RegisterServices(register =>
                {
                    register.Register<ExternalEventMediatorBuilder, ExternalEventMediatorBuilder>(Lifetime.Singleton);
                    register.Register<IExternalEventReceiver>(ctx => externalEventReceiver, Lifetime.Singleton);
                })
                .AddJobs(typeof(ExternalEventProcessorJob));
        }
    }
}
