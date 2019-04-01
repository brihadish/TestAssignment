using EventFlow.Core;
using EventFlow.ValueObjects;
using Newtonsoft.Json;

namespace DomainModel
{
    /// <summary>
    /// Represents unique identity of a tailor.
    /// </summary>
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public sealed class TailorId : Identity<TailorId>
    {
        public TailorId(string value) : base(value)
        {
        }
    }
}