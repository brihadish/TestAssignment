using EventFlow.Core;

namespace DomainModel
{
    /// <summary>
    /// Represents unique identity of an alteration pertaining to a <see cref="SuitAggregate"/>.
    /// </summary>
    public sealed class SuitAlterationId : Identity<SuitAlterationId>
    {
        public SuitAlterationId(string value) : base(value)
        {
        }
    }
}
