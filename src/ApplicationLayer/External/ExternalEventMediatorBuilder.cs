using EventFlow.Configuration;
using LightInject;
using MediatR;

namespace ApplicationLayer.External
{
    /// <summary>
    /// Builder for creating a mediator so as to process external events.
    /// </summary>
    public sealed class ExternalEventMediatorBuilder
    {
        private readonly ServiceContainer _container = new ServiceContainer();

        public ExternalEventMediatorBuilder(IResolver resolver)
        {
            _container.RegisterInstance(typeof(IResolver), resolver);
        }

        public IMediator Build()
        {
            _container.RegisterAssembly(
                typeof(IMediator).Assembly, () => new PerContainerLifetime(), (serviceType, implementingType) => !serviceType.IsClass);
            _container.Register<IMediator, Mediator>();
            _container.RegisterAssembly(this.GetType().Assembly, (serviceType, implementingType) =>
                typeof(IExternalEventProcessor).IsAssignableFrom(implementingType));
            _container.Register<ServiceFactory>(fac => fac.GetInstance);
            return _container.GetInstance<IMediator>();
        }
    }
}
