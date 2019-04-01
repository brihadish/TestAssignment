using EventFlow.ValueObjects;

namespace DomainModel
{
    /// <summary>
    /// Represents unit of measurement.
    /// </summary>
    public sealed class MeasurementUnit : SingleValueObject<string>
    {
        public static readonly MeasurementUnit Centimeter = new MeasurementUnit("cm");

        public MeasurementUnit(string value)
            : base(value?.ToLowerInvariant())
        {
        }
    }
}
