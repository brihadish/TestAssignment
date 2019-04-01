using EventFlow.Core;
using EventFlow.ValueObjects;
using Newtonsoft.Json;

namespace DomainModel
{
    /// <summary>
    /// Represents unique identity of <see cref="SuitAggregate"/>.
    /// </summary>
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public sealed class SuitId : Identity<SuitId>
    {
        public SuitId(string value) : base(value)
        {
        }
    }
}
