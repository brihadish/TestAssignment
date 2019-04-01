using System.Linq;
using DomainModel;
using DomainModel.Suit;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomainModel.SuitAlteration;

namespace UnitTests
{
    [TestClass]
    public sealed class SuitAlterationAggregateTests
    {
        private SuitAggregate _suit;

        [TestInitialize]
        public void Initialize()
        {
            _suit = new SuitAggregate(SuitId.New);
            _suit.Create(
                    new SuitSleevePair(
                        new Measurement(90, MeasurementUnit.Centimeter),
                        new Measurement(90, MeasurementUnit.Centimeter)),
                    new SuitTrouserPair(
                        new Measurement(120, MeasurementUnit.Centimeter),
                        new Measurement(120, MeasurementUnit.Centimeter)));
        }

        [TestMethod]
        public void Should_allow_sleeve_alteration_if_payment_done()
        {
            // Arrange
            var tailorId = TailorId.New;
            var suitAlteration = new SuitAlterationAggregate(SuitAlterationId.New);
            suitAlteration.CreateForSleeve(
                _suit, SuitSleeveAlterationChoice.Both, new MeasurementAlteration(5, MeasurementUnit.Centimeter));
            suitAlteration.RecordPayment();

            // Act
            var result = suitAlteration.ExecuteAlteration(_suit, tailorId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var @event = _suit.UncommittedEvents.Select(t => t.AggregateEvent).OfType<SuitSleeveAltered>().FirstOrDefault();
            @event.SuitAlterationId.Should().BeEquivalentTo(suitAlteration.Id);
            @event.TailorId.Should().BeEquivalentTo(tailorId);
            @event.SuitSleevePair.LeftSleeveLength.Should().BeEquivalentTo(new Measurement(95, MeasurementUnit.Centimeter));
            @event.SuitSleevePair.RightSleeveLength.Should().BeEquivalentTo(new Measurement(95, MeasurementUnit.Centimeter));
        }

        [TestMethod]
        public void Should_not_allow_sleeve_alteration_if_payment_not_done()
        {
            // Arrange
            var tailorId = TailorId.New;
            var suitAlteration = new SuitAlterationAggregate(SuitAlterationId.New);
            suitAlteration.CreateForSleeve(
                _suit, SuitSleeveAlterationChoice.Both, new MeasurementAlteration(5, MeasurementUnit.Centimeter));

            // Act
            var result = suitAlteration.ExecuteAlteration(_suit, tailorId);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ToString().Should().EndWith(SuitAlterationAggregateFailureReasons.PaymentRequiredBeforeSuitAlteration);            
        }

        [TestMethod]
        public void Should_allow_trouser_alteration_if_payment_done()
        {
            // Arrange
            var tailorId = TailorId.New;
            var suitAlteration = new SuitAlterationAggregate(SuitAlterationId.New);
            suitAlteration.CreateForTrouser(
                _suit, SuitTrouserAlterationChoice.Both, new MeasurementAlteration(5, MeasurementUnit.Centimeter));
            suitAlteration.RecordPayment();

            // Act
            var result = suitAlteration.ExecuteAlteration(_suit, tailorId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var @event = _suit.UncommittedEvents.Select(t => t.AggregateEvent).OfType<SuitTrouserAltered>().FirstOrDefault();
            @event.SuitAlterationId.Should().BeEquivalentTo(suitAlteration.Id);
            @event.TailorId.Should().BeEquivalentTo(tailorId);
            @event.SuitTrouserPair.LeftTrouserLength.Should().BeEquivalentTo(new Measurement(125, MeasurementUnit.Centimeter));
            @event.SuitTrouserPair.RightTrouserLength.Should().BeEquivalentTo(new Measurement(125, MeasurementUnit.Centimeter));
        }

        [TestMethod]
        public void Should_not_allow_trouser_alteration_if_payment_not_done()
        {
            // Arrange
            var tailorId = TailorId.New;
            var suitAlteration = new SuitAlterationAggregate(SuitAlterationId.New);
            suitAlteration.CreateForTrouser(
                _suit, SuitTrouserAlterationChoice.Both, new MeasurementAlteration(5, MeasurementUnit.Centimeter));

            // Act
            var result = suitAlteration.ExecuteAlteration(_suit, tailorId);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ToString().Should().EndWith(SuitAlterationAggregateFailureReasons.PaymentRequiredBeforeSuitAlteration);
        }

        [TestMethod]
        public void Should_handle_payment_in_idempotent_manner()
        {
            // Arrange
            var suitAlteration = new SuitAlterationAggregate(SuitAlterationId.New);
            suitAlteration.CreateForTrouser(
                _suit, SuitTrouserAlterationChoice.Both, new MeasurementAlteration(5, MeasurementUnit.Centimeter));
            suitAlteration.RecordPayment();

            // Act
            var result = suitAlteration.RecordPayment();

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [TestMethod]
        public void Should_not_allow_payment_after_altering_has_succeeded()
        {
            // Arrange
            var tailorId = TailorId.New;
            var suitAlteration = new SuitAlterationAggregate(SuitAlterationId.New);
            suitAlteration.CreateForTrouser(
                _suit, SuitTrouserAlterationChoice.Both, new MeasurementAlteration(5, MeasurementUnit.Centimeter));
            suitAlteration.RecordPayment();
            var alterationResult = suitAlteration.ExecuteAlteration(_suit, tailorId);
            if (alterationResult.IsSuccess)
            {
                suitAlteration.MarkAsSucceeded(tailorId);
            }

            // Act
            var result = suitAlteration.RecordPayment();

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ToString().Should().EndWith(SuitAlterationAggregateFailureReasons.SuitAlterationAlreadyPerformed);
        }

        [TestMethod]
        public void Should_not_allow_payment_after_altering_has_failed()
        {
            // Arrange
            var tailorId = TailorId.New;
            var suitAlteration = new SuitAlterationAggregate(SuitAlterationId.New);
            suitAlteration.CreateForTrouser(
                _suit, SuitTrouserAlterationChoice.Both, new MeasurementAlteration(-120, MeasurementUnit.Centimeter));
            suitAlteration.RecordPayment();
            var alterationResult = suitAlteration.ExecuteAlteration(_suit, tailorId);
            if (!alterationResult.IsSuccess)
            {
                suitAlteration.MarkAsFailed(tailorId);
            }

            // Act
            var result = suitAlteration.RecordPayment();

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ToString().Should().EndWith(SuitAlterationAggregateFailureReasons.SuitAlterationAlreadyPerformed);
        }
    }
}
