using EventFlow.Aggregates.ExecutionResults;
using MediatR;
using Newtonsoft.Json;

namespace ApplicationLayer.External
{
    /// <summary>
    /// Represents an event that happened outside the scope of the application.
    /// </summary>
    public abstract class ExternalEvent : IRequest<IExecutionResult>
    {
        public string Type { get; set; }
    }
}
