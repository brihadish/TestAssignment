using System.Linq;
using DomainModel;
using DomainModel.Suit;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public sealed class SuitAggregateTests
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
        public void Should_be_able_to_sell_to_customer()
        {
            // Arrange
            var customerId = CustomerId.New;

            // Act
            _suit.Sell(customerId);

            // Assert
            var @event = _suit.UncommittedEvents.Select(t => t.AggregateEvent).OfType<SuitSoldEvent>().FirstOrDefault();
            @event.Should().NotBeNull();
            @event.CustomerId.Should().BeEquivalentTo(customerId);
        }

        [TestMethod]
        public void Should_be_able_to_positively_alter_both_sleeves()
        {
            // Arrange
            var customerId = CustomerId.New;
            var suitAlterationId = SuitAlterationId.New;
            var tailorId = TailorId.New;
            var alteration = new MeasurementAlteration(5, MeasurementUnit.Centimeter);
            _suit.Sell(customerId);

            // Act
            var result = _suit.Alter(suitAlterationId, tailorId, SuitSleeveAlterationChoice.Both, alteration);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var @event = _suit.UncommittedEvents.Select(t => t.AggregateEvent).OfType<SuitSleeveAltered>().FirstOrDefault();
            @event.SuitAlterationId.Should().BeEquivalentTo(suitAlterationId);
            @event.TailorId.Should().BeEquivalentTo(tailorId);
            @event.SuitSleevePair.LeftSleeveLength.Should().BeEquivalentTo(new Measurement(95, MeasurementUnit.Centimeter));
            @event.SuitSleevePair.RightSleeveLength.Should().BeEquivalentTo(new Measurement(95, MeasurementUnit.Centimeter));
        }

        [TestMethod]
        public void Should_be_able_to_positively_alter_left_sleeve()
        {
            // Arrange
            var customerId = CustomerId.New;
            var suitAlterationId = SuitAlterationId.New;
            var tailorId = TailorId.New;
            var alteration = new MeasurementAlteration(5, MeasurementUnit.Centimeter);
            _suit.Sell(customerId);

            // Act
            var result = _suit.Alter(suitAlterationId, tailorId, SuitSleeveAlterationChoice.Left, alteration);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var @event = _suit.UncommittedEvents.Select(t => t.AggregateEvent).OfType<SuitSleeveAltered>().FirstOrDefault();
            @event.SuitAlterationId.Should().BeEquivalentTo(suitAlterationId);
            @event.TailorId.Should().BeEquivalentTo(tailorId);
            @event.SuitSleevePair.LeftSleeveLength.Should().BeEquivalentTo(new Measurement(95, MeasurementUnit.Centimeter));
            @event.SuitSleevePair.RightSleeveLength.Should().BeEquivalentTo(new Measurement(90, MeasurementUnit.Centimeter));
        }

        [TestMethod]
        public void Should_be_able_to_positively_alter_right_sleeve()
        {
            // Arrange
            var customerId = CustomerId.New;
            var suitAlterationId = SuitAlterationId.New;
            var tailorId = TailorId.New;
            var alteration = new MeasurementAlteration(5, MeasurementUnit.Centimeter);
            _suit.Sell(customerId);

            // Act
            var result = _suit.Alter(suitAlterationId, tailorId, SuitSleeveAlterationChoice.Right, alteration);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var @event = _suit.UncommittedEvents.Select(t => t.AggregateEvent).OfType<SuitSleeveAltered>().FirstOrDefault();
            @event.SuitAlterationId.Should().BeEquivalentTo(suitAlterationId);
            @event.TailorId.Should().BeEquivalentTo(tailorId);
            @event.SuitSleevePair.LeftSleeveLength.Should().BeEquivalentTo(new Measurement(90, MeasurementUnit.Centimeter));
            @event.SuitSleevePair.RightSleeveLength.Should().BeEquivalentTo(new Measurement(95, MeasurementUnit.Centimeter));
        }

        [TestMethod]
        public void Should_be_able_to_negatively_alter_both_sleeves()
        {
            // Arrange
            var customerId = CustomerId.New;
            var suitAlterationId = SuitAlterationId.New;
            var tailorId = TailorId.New;
            var alteration = new MeasurementAlteration(-5, MeasurementUnit.Centimeter);
            _suit.Sell(customerId);

            // Act
            var result = _suit.Alter(suitAlterationId, tailorId, SuitSleeveAlterationChoice.Both, alteration);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var @event = _suit.UncommittedEvents.Select(t => t.AggregateEvent).OfType<SuitSleeveAltered>().FirstOrDefault();
            @event.SuitAlterationId.Should().BeEquivalentTo(suitAlterationId);
            @event.TailorId.Should().BeEquivalentTo(tailorId);
            @event.SuitSleevePair.LeftSleeveLength.Should().BeEquivalentTo(new Measurement(85, MeasurementUnit.Centimeter));
            @event.SuitSleevePair.RightSleeveLength.Should().BeEquivalentTo(new Measurement(85, MeasurementUnit.Centimeter));
        }

        [TestMethod]
        public void Should_be_able_to_negatively_alter_left_sleeve()
        {
            // Arrange
            var customerId = CustomerId.New;
            var suitAlterationId = SuitAlterationId.New;
            var tailorId = TailorId.New;
            var alteration = new MeasurementAlteration(-5, MeasurementUnit.Centimeter);
            _suit.Sell(customerId);

            // Act
            var result = _suit.Alter(suitAlterationId, tailorId, SuitSleeveAlterationChoice.Left, alteration);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var @event = _suit.UncommittedEvents.Select(t => t.AggregateEvent).OfType<SuitSleeveAltered>().FirstOrDefault();
            @event.SuitAlterationId.Should().BeEquivalentTo(suitAlterationId);
            @event.TailorId.Should().BeEquivalentTo(tailorId);
            @event.SuitSleevePair.LeftSleeveLength.Should().BeEquivalentTo(new Measurement(85, MeasurementUnit.Centimeter));
            @event.SuitSleevePair.RightSleeveLength.Should().BeEquivalentTo(new Measurement(90, MeasurementUnit.Centimeter));
        }

        [TestMethod]
        public void Should_be_able_to_negatively_alter_right_sleeve()
        {
            // Arrange
            var customerId = CustomerId.New;
            var suitAlterationId = SuitAlterationId.New;
            var tailorId = TailorId.New;
            var alteration = new MeasurementAlteration(-5, MeasurementUnit.Centimeter);
            _suit.Sell(customerId);

            // Act
            var result = _suit.Alter(suitAlterationId, tailorId, SuitSleeveAlterationChoice.Right, alteration);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var @event = _suit.UncommittedEvents.Select(t => t.AggregateEvent).OfType<SuitSleeveAltered>().FirstOrDefault();
            @event.SuitAlterationId.Should().BeEquivalentTo(suitAlterationId);
            @event.TailorId.Should().BeEquivalentTo(tailorId);
            @event.SuitSleevePair.LeftSleeveLength.Should().BeEquivalentTo(new Measurement(90, MeasurementUnit.Centimeter));
            @event.SuitSleevePair.RightSleeveLength.Should().BeEquivalentTo(new Measurement(85, MeasurementUnit.Centimeter));
        }

        [TestMethod]
        public void Should_be_able_to_positively_alter_both_trousers()
        {
            // Arrange
            var customerId = CustomerId.New;
            var suitAlterationId = SuitAlterationId.New;
            var tailorId = TailorId.New;
            var alteration = new MeasurementAlteration(5, MeasurementUnit.Centimeter);
            _suit.Sell(customerId);

            // Act
            var result = _suit.Alter(suitAlterationId, tailorId, SuitTrouserAlterationChoice.Both, alteration);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var @event = _suit.UncommittedEvents.Select(t => t.AggregateEvent).OfType<SuitTrouserAltered>().FirstOrDefault();
            @event.SuitAlterationId.Should().BeEquivalentTo(suitAlterationId);
            @event.TailorId.Should().BeEquivalentTo(tailorId);
            @event.SuitTrouserPair.LeftTrouserLength.Should().BeEquivalentTo(new Measurement(125, MeasurementUnit.Centimeter));
            @event.SuitTrouserPair.RightTrouserLength.Should().BeEquivalentTo(new Measurement(125, MeasurementUnit.Centimeter));
        }

        [TestMethod]
        public void Should_be_able_to_positively_alter_left_trouser()
        {
            // Arrange
            var customerId = CustomerId.New;
            var suitAlterationId = SuitAlterationId.New;
            var tailorId = TailorId.New;
            var alteration = new MeasurementAlteration(5, MeasurementUnit.Centimeter);
            _suit.Sell(customerId);

            // Act
            var result = _suit.Alter(suitAlterationId, tailorId, SuitTrouserAlterationChoice.Left, alteration);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var @event = _suit.UncommittedEvents.Select(t => t.AggregateEvent).OfType<SuitTrouserAltered>().FirstOrDefault();
            @event.SuitAlterationId.Should().BeEquivalentTo(suitAlterationId);
            @event.TailorId.Should().BeEquivalentTo(tailorId);
            @event.SuitTrouserPair.LeftTrouserLength.Should().BeEquivalentTo(new Measurement(125, MeasurementUnit.Centimeter));
            @event.SuitTrouserPair.RightTrouserLength.Should().BeEquivalentTo(new Measurement(120, MeasurementUnit.Centimeter));
        }

        [TestMethod]
        public void Should_be_able_to_positively_alter_right_trouser()
        {
            // Arrange
            var customerId = CustomerId.New;
            var suitAlterationId = SuitAlterationId.New;
            var tailorId = TailorId.New;
            var alteration = new MeasurementAlteration(5, MeasurementUnit.Centimeter);
            _suit.Sell(customerId);

            // Act
            var result = _suit.Alter(suitAlterationId, tailorId, SuitTrouserAlterationChoice.Right, alteration);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var @event = _suit.UncommittedEvents.Select(t => t.AggregateEvent).OfType<SuitTrouserAltered>().FirstOrDefault();
            @event.SuitAlterationId.Should().BeEquivalentTo(suitAlterationId);
            @event.TailorId.Should().BeEquivalentTo(tailorId);
            @event.SuitTrouserPair.LeftTrouserLength.Should().BeEquivalentTo(new Measurement(120, MeasurementUnit.Centimeter));
            @event.SuitTrouserPair.RightTrouserLength.Should().BeEquivalentTo(new Measurement(125, MeasurementUnit.Centimeter));
        }

        [TestMethod]
        public void Should_be_able_to_negatively_alter_both_trousers()
        {
            // Arrange
            var customerId = CustomerId.New;
            var suitAlterationId = SuitAlterationId.New;
            var tailorId = TailorId.New;
            var alteration = new MeasurementAlteration(-5, MeasurementUnit.Centimeter);
            _suit.Sell(customerId);

            // Act
            var result = _suit.Alter(suitAlterationId, tailorId, SuitTrouserAlterationChoice.Both, alteration);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var @event = _suit.UncommittedEvents.Select(t => t.AggregateEvent).OfType<SuitTrouserAltered>().FirstOrDefault();
            @event.SuitAlterationId.Should().BeEquivalentTo(suitAlterationId);
            @event.TailorId.Should().BeEquivalentTo(tailorId);
            @event.SuitTrouserPair.LeftTrouserLength.Should().BeEquivalentTo(new Measurement(115, MeasurementUnit.Centimeter));
            @event.SuitTrouserPair.RightTrouserLength.Should().BeEquivalentTo(new Measurement(115, MeasurementUnit.Centimeter));
        }

        [TestMethod]
        public void Should_be_able_to_negatively_alter_left_trouser()
        {
            // Arrange
            var customerId = CustomerId.New;
            var suitAlterationId = SuitAlterationId.New;
            var tailorId = TailorId.New;
            var alteration = new MeasurementAlteration(-5, MeasurementUnit.Centimeter);
            _suit.Sell(customerId);

            // Act
            var result = _suit.Alter(suitAlterationId, tailorId, SuitTrouserAlterationChoice.Left, alteration);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var @event = _suit.UncommittedEvents.Select(t => t.AggregateEvent).OfType<SuitTrouserAltered>().FirstOrDefault();
            @event.SuitAlterationId.Should().BeEquivalentTo(suitAlterationId);
            @event.TailorId.Should().BeEquivalentTo(tailorId);
            @event.SuitTrouserPair.LeftTrouserLength.Should().BeEquivalentTo(new Measurement(115, MeasurementUnit.Centimeter));
            @event.SuitTrouserPair.RightTrouserLength.Should().BeEquivalentTo(new Measurement(120, MeasurementUnit.Centimeter));
        }

        [TestMethod]
        public void Should_be_able_to_negatively_alter_right_trouser()
        {
            // Arrange
            var customerId = CustomerId.New;
            var suitAlterationId = SuitAlterationId.New;
            var tailorId = TailorId.New;
            var alteration = new MeasurementAlteration(-5, MeasurementUnit.Centimeter);
            _suit.Sell(customerId);

            // Act
            var result = _suit.Alter(suitAlterationId, tailorId, SuitTrouserAlterationChoice.Right, alteration);

            // Assert
            result.IsSuccess.Should().BeTrue();
            var @event = _suit.UncommittedEvents.Select(t => t.AggregateEvent).OfType<SuitTrouserAltered>().FirstOrDefault();
            @event.SuitAlterationId.Should().BeEquivalentTo(suitAlterationId);
            @event.TailorId.Should().BeEquivalentTo(tailorId);
            @event.SuitTrouserPair.LeftTrouserLength.Should().BeEquivalentTo(new Measurement(120, MeasurementUnit.Centimeter));
            @event.SuitTrouserPair.RightTrouserLength.Should().BeEquivalentTo(new Measurement(115, MeasurementUnit.Centimeter));
        }

        [TestMethod]
        public void Should_prevent_alteration_from_removing_both_sleeve()
        {
            // Arrange
            var customerId = CustomerId.New;
            var suitAlterationId = SuitAlterationId.New;
            var tailorId = TailorId.New;
            var alteration = new MeasurementAlteration(-90, MeasurementUnit.Centimeter);
            _suit.Sell(customerId);

            // Act
            var result = _suit.Alter(suitAlterationId, tailorId, SuitSleeveAlterationChoice.Both, alteration);

            // Assert
            result.IsSuccess.Should().BeFalse();
        }

        [TestMethod]
        public void Should_prevent_alteration_from_removing_left_sleeve()
        {
            // Arrange
            var customerId = CustomerId.New;
            var suitAlterationId = SuitAlterationId.New;
            var tailorId = TailorId.New;
            var alteration = new MeasurementAlteration(-90, MeasurementUnit.Centimeter);
            _suit.Sell(customerId);

            // Act
            var result = _suit.Alter(suitAlterationId, tailorId, SuitSleeveAlterationChoice.Left, alteration);

            // Assert
            result.IsSuccess.Should().BeFalse();
        }

        [TestMethod]
        public void Should_prevent_alteration_from_removing_right_sleeve()
        {
            // Arrange
            var customerId = CustomerId.New;
            var suitAlterationId = SuitAlterationId.New;
            var tailorId = TailorId.New;
            var alteration = new MeasurementAlteration(-90, MeasurementUnit.Centimeter);
            _suit.Sell(customerId);

            // Act
            var result = _suit.Alter(suitAlterationId, tailorId, SuitSleeveAlterationChoice.Right, alteration);

            // Assert
            result.IsSuccess.Should().BeFalse();
        }

        [TestMethod]
        public void Should_prevent_alteration_from_removing_both_trouser()
        {
            // Arrange
            var customerId = CustomerId.New;
            var suitAlterationId = SuitAlterationId.New;
            var tailorId = TailorId.New;
            var alteration = new MeasurementAlteration(-120, MeasurementUnit.Centimeter);
            _suit.Sell(customerId);

            // Act
            var result = _suit.Alter(suitAlterationId, tailorId, SuitTrouserAlterationChoice.Both, alteration);

            // Assert
            result.IsSuccess.Should().BeFalse();
        }

        [TestMethod]
        public void Should_prevent_alteration_from_removing_left_trouser()
        {
            // Arrange
            var customerId = CustomerId.New;
            var suitAlterationId = SuitAlterationId.New;
            var tailorId = TailorId.New;
            var alteration = new MeasurementAlteration(-120, MeasurementUnit.Centimeter);
            _suit.Sell(customerId);

            // Act
            var result = _suit.Alter(suitAlterationId, tailorId, SuitTrouserAlterationChoice.Left, alteration);

            // Assert
            result.IsSuccess.Should().BeFalse();
        }

        [TestMethod]
        public void Should_prevent_alteration_from_removing_right_trouser()
        {
            // Arrange
            var customerId = CustomerId.New;
            var suitAlterationId = SuitAlterationId.New;
            var tailorId = TailorId.New;
            var alteration = new MeasurementAlteration(-120, MeasurementUnit.Centimeter);
            _suit.Sell(customerId);

            // Act
            var result = _suit.Alter(suitAlterationId, tailorId, SuitTrouserAlterationChoice.Right, alteration);

            // Assert
            result.IsSuccess.Should().BeFalse();
        }
    }
}
