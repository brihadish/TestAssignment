using EventFlow.Core;
using EventFlow.ValueObjects;
using Newtonsoft.Json;

namespace DomainModel
{
    /// <summary>
    /// Represents unique identity of a customer.
    /// </summary>
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public sealed class CustomerId : Identity<CustomerId>
    {
        public CustomerId(string value) : base(value)
        {
        }
    }
}
