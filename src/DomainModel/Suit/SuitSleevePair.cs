using System.Collections.Generic;
using EventFlow.ValueObjects;

namespace DomainModel.Suit
{
    /// <summary>
    /// Represents a pair of sleeves in a suit.
    /// </summary>
    public sealed class SuitSleevePair : ValueObject
    {
        public SuitSleevePair(Measurement leftSleeveLength, Measurement rightSleeveLength)
        {
            LeftSleeveLength = leftSleeveLength;
            RightSleeveLength = rightSleeveLength;
        }

        /// <summary>
        /// Gets the length of left trouser.
        /// </summary>
        public Measurement LeftSleeveLength { get; }

        /// <summary>
        /// Gets the length of right trouser.
        /// </summary>
        public Measurement RightSleeveLength { get; }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return LeftSleeveLength;
            yield return RightSleeveLength;
        }
    }
}
