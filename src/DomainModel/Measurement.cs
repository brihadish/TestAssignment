using System;
using System.Collections.Generic;
using EventFlow.ValueObjects;

namespace DomainModel
{
    /// <summary>
    /// Represents a measurement and its unit.
    /// </summary>
    public sealed class Measurement : ValueObject
    {
        /// <summary>
        /// Gets the measurement value.
        /// </summary>
        public int Value { get; }

        /// <summary>
        /// Gets the measurement unit.
        /// </summary>
        public MeasurementUnit Unit { get; }

        public Measurement(int value, MeasurementUnit unit)
        {
            Value = value;
            Unit = unit;
        }

        public static Measurement operator + (Measurement a, Measurement b)
        {
            if (a.Unit != b.Unit)
            {
                throw new InvalidOperationException();
            }

            return new Measurement(a.Value + b.Value, a.Unit);
        }

        public static Measurement operator -(Measurement a, Measurement b)
        {
            if (a.Unit != b.Unit)
            {
                throw new InvalidOperationException();
            }

            return new Measurement(a.Value - b.Value, a.Unit);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
            yield return Unit;
        }
    }
}