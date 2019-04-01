using System.Collections.Generic;
using EventFlow.ValueObjects;

namespace DomainModel.Suit
{
    /// <summary>
    /// Represents a pair of trousers in a suit.
    /// </summary>
    public sealed class SuitTrouserPair : ValueObject
    {
        public SuitTrouserPair(Measurement leftTrouserLength, Measurement rightTrouserLength)
        {
            LeftTrouserLength = leftTrouserLength;
            RightTrouserLength = rightTrouserLength;
        }

        /// <summary>
        /// Gets the length of left trouser.
        /// </summary>
        public Measurement LeftTrouserLength { get; }

        /// <summary>
        /// Gets the length of right trouser.
        /// </summary>
        public Measurement RightTrouserLength { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return LeftTrouserLength;
            yield return RightTrouserLength;
        }
    }
}
