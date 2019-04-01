using System;
using System.Collections.Generic;
using EventFlow.ValueObjects;

namespace DomainModel
{
    /// <summary>
    /// Represents an alteration that is applied to a measurement.
    /// </summary>
    public sealed class MeasurementAlteration : ValueObject
    {
        /// <summary>
        /// Gets the measurement value.
        /// </summary>
        public int Value { get; }

        /// <summary>
        /// Gets the measurement unit.
        /// </summary>
        public MeasurementUnit Unit { get; }

        public MeasurementAlteration(int value, MeasurementUnit unit)
        {
            Value = value;
            Unit = unit;
        }

        /// <summary>
        /// Determines whether the alteration can be applied to the specified measurement.
        /// </summary>
        /// <param name="measurement"><see cref="Measurement"/> for which the check is to be done.</param>
        /// <returns>True if alteration can be applied;Otherwise false.</returns>
        public bool CanApply(Measurement measurement)
        {
            return Unit == measurement.Unit;
        }

        /// <summary>
        /// Applies the alteration on the specified measurement.
        /// </summary>
        /// <param name="measurement"><see cref="Measurement"/> on which alteration is to be applied.</param>
        /// <returns>New measurement after applying the alteration.</returns>
        public Measurement Apply(Measurement measurement)
        {
            if (Unit != measurement.Unit)
            {
                throw new InvalidOperationException();
            }

            if (Value < 0)
            {
                var reduce = new Measurement(Math.Abs(Value), Unit);
                return measurement - reduce;
            }
            else
            {
                var add = new Measurement(Value, Unit);
                return measurement + add;
            }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
            yield return Unit;
        }
    }
}
