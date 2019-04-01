using System.Threading;
using System.Threading.Tasks;
using ApplicationLayer;
using ApplicationLayer.Commands;
using DomainModel;
using DomainModel.Suit;
using EventFlow;
using EventFlow.Aggregates;
using EventFlow.Core;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IntegrationTests
{
    [TestClass]
    public class SuitTrouserAlterationTests
    {
        private SuitAggregate _suit;
        private IAggregateStore _aggregateStore;
        private ICommandBus _commandBus;
        private FakeExternalEventReceiver _fakeExternalEventReceiver;
        private NotificationServiceSpy _notificationServiceSpy;

        [TestInitialize]
        public async Task Initialize()
        {
            _fakeExternalEventReceiver = new FakeExternalEventReceiver();
            _notificationServiceSpy = new NotificationServiceSpy();
            var resolver =
                EventFlowApplicationLayer
                                .Configure(
                                    _fakeExternalEventReceiver,
                                    _notificationServiceSpy,
                                    new SerilogLogger(EventFlow.Logs.LogLevel.Debug, string.Empty))
                                .CreateResolver();
            _aggregateStore = resolver.Resolve<IAggregateStore>();
            _commandBus = resolver.Resolve<ICommandBus>();

            _suit = new SuitAggregate(SuitId.New);
            _suit.Create(
                    new SuitSleevePair(
                        new Measurement(90, MeasurementUnit.Centimeter),
                        new Measurement(90, MeasurementUnit.Centimeter)),
                    new SuitTrouserPair(
                        new Measurement(120, MeasurementUnit.Centimeter),
                        new Measurement(120, MeasurementUnit.Centimeter)));
            _suit.Sell(CustomerId.New);
            await _aggregateStore.StoreAsync<SuitAggregate, SuitId>(_suit, SourceId.New, CancellationToken.None);
        }

        [TestMethod]
        public async Task Should_positively_alter_both_suit_trousers()
        {
            // Arrange
            var suitAlterationId = SuitAlterationId.New;
            var createTrouserAlterationCommand =
                new CreateSuitTrouserAlterationCommand(
                                suitAlterationId,
                                _suit.Id,
                                SuitTrouserAlterationChoice.Both,
                                new MeasurementAlteration(5, MeasurementUnit.Centimeter));
            var reportAlterationPaymentCommand = new RecordSuitAlterationPaymentCommand(suitAlterationId);
            var executeAlterationCommand = new ExecuteSuitAlterationCommand(_suit.Id, suitAlterationId, TailorId.New);

            // Act and Assert
            var result = await _commandBus.PublishAsync(createTrouserAlterationCommand, CancellationToken.None);
            result.IsSuccess.Should().BeTrue();

            result = await _commandBus.PublishAsync(reportAlterationPaymentCommand, CancellationToken.None);
            result.IsSuccess.Should().BeTrue();

            result = await _commandBus.PublishAsync(executeAlterationCommand, CancellationToken.None);
            result.IsSuccess.Should().BeTrue();

            var updatedSuit = await _aggregateStore.LoadAsync<SuitAggregate, SuitId>(_suit.Id, CancellationToken.None);
            updatedSuit.LeftTrouserLength.Equals(new Measurement(125, MeasurementUnit.Centimeter)).Should().BeTrue();
            updatedSuit.RightTrouserLength.Equals(new Measurement(125, MeasurementUnit.Centimeter)).Should().BeTrue();
        }

        [TestMethod]
        public async Task Should_positively_alter_left_suit_trouser()
        {
            // Arrange
            var suitAlterationId = SuitAlterationId.New;
            var createTrouserAlterationCommand =
                new CreateSuitTrouserAlterationCommand(
                                suitAlterationId,
                                _suit.Id,
                                SuitTrouserAlterationChoice.Left,
                                new MeasurementAlteration(5, MeasurementUnit.Centimeter));
            var reportAlterationPaymentCommand = new RecordSuitAlterationPaymentCommand(suitAlterationId);
            var executeAlterationCommand = new ExecuteSuitAlterationCommand(_suit.Id, suitAlterationId, TailorId.New);

            // Act and Assert
            var result = await _commandBus.PublishAsync(createTrouserAlterationCommand, CancellationToken.None);
            result.IsSuccess.Should().BeTrue();

            result = await _commandBus.PublishAsync(reportAlterationPaymentCommand, CancellationToken.None);
            result.IsSuccess.Should().BeTrue();

            result = await _commandBus.PublishAsync(executeAlterationCommand, CancellationToken.None);
            result.IsSuccess.Should().BeTrue();

            var updatedSuit = await _aggregateStore.LoadAsync<SuitAggregate, SuitId>(_suit.Id, CancellationToken.None);
            updatedSuit.LeftTrouserLength.Equals(new Measurement(125, MeasurementUnit.Centimeter)).Should().BeTrue();
        }

        [TestMethod]
        public async Task Should_positively_alter_right_suit_trouser()
        {
            // Arrange
            var suitAlterationId = SuitAlterationId.New;
            var createTrouserAlterationCommand =
                new CreateSuitTrouserAlterationCommand(
                                suitAlterationId,
                                _suit.Id,
                                SuitTrouserAlterationChoice.Right,
                                new MeasurementAlteration(5, MeasurementUnit.Centimeter));
            var reportAlterationPaymentCommand = new RecordSuitAlterationPaymentCommand(suitAlterationId);
            var executeAlterationCommand = new ExecuteSuitAlterationCommand(_suit.Id, suitAlterationId, TailorId.New);

            // Act and Assert

            var result = await _commandBus.PublishAsync(createTrouserAlterationCommand, CancellationToken.None);
            result.IsSuccess.Should().BeTrue();

            result = await _commandBus.PublishAsync(reportAlterationPaymentCommand, CancellationToken.None);
            result.IsSuccess.Should().BeTrue();

            result = await _commandBus.PublishAsync(executeAlterationCommand, CancellationToken.None);
            result.IsSuccess.Should().BeTrue();

            var updatedSuit = await _aggregateStore.LoadAsync<SuitAggregate, SuitId>(_suit.Id, CancellationToken.None);
            updatedSuit.RightTrouserLength.Equals(new Measurement(125, MeasurementUnit.Centimeter)).Should().BeTrue();
        }

        [TestMethod]
        public async Task Should_negatively_alter_both_suit_trousers()
        {
            // Arrange
            var suitAlterationId = SuitAlterationId.New;
            var createTrouserAlterationCommand =
                new CreateSuitTrouserAlterationCommand(
                                suitAlterationId,
                                _suit.Id,
                                SuitTrouserAlterationChoice.Both,
                                new MeasurementAlteration(-5, MeasurementUnit.Centimeter));
            var reportAlterationPaymentCommand = new RecordSuitAlterationPaymentCommand(suitAlterationId);
            var executeAlterationCommand = new ExecuteSuitAlterationCommand(_suit.Id, suitAlterationId, TailorId.New);

            // Act and Assert
            var result = await _commandBus.PublishAsync(createTrouserAlterationCommand, CancellationToken.None);
            result.IsSuccess.Should().BeTrue();

            result = await _commandBus.PublishAsync(reportAlterationPaymentCommand, CancellationToken.None);
            result.IsSuccess.Should().BeTrue();

            result = await _commandBus.PublishAsync(executeAlterationCommand, CancellationToken.None);
            result.IsSuccess.Should().BeTrue();

            var updatedSuit = await _aggregateStore.LoadAsync<SuitAggregate, SuitId>(_suit.Id, CancellationToken.None);
            updatedSuit.LeftTrouserLength.Equals(new Measurement(115, MeasurementUnit.Centimeter)).Should().BeTrue();
            updatedSuit.RightTrouserLength.Equals(new Measurement(115, MeasurementUnit.Centimeter)).Should().BeTrue();
        }

        [TestMethod]
        public async Task Should_negatively_alter_left_suit_trouser()
        {
            // Arrange
            var suitAlterationId = SuitAlterationId.New;
            var createTrouserAlterationCommand =
                new CreateSuitTrouserAlterationCommand(
                                suitAlterationId,
                                _suit.Id,
                                SuitTrouserAlterationChoice.Left,
                                new MeasurementAlteration(-5, MeasurementUnit.Centimeter));
            var reportAlterationPaymentCommand = new RecordSuitAlterationPaymentCommand(suitAlterationId);
            var executeAlterationCommand = new ExecuteSuitAlterationCommand(_suit.Id, suitAlterationId, TailorId.New);

            // Act and Assert
            var result = await _commandBus.PublishAsync(createTrouserAlterationCommand, CancellationToken.None);
            result.IsSuccess.Should().BeTrue();

            result = await _commandBus.PublishAsync(reportAlterationPaymentCommand, CancellationToken.None);
            result.IsSuccess.Should().BeTrue();

            result = await _commandBus.PublishAsync(executeAlterationCommand, CancellationToken.None);
            result.IsSuccess.Should().BeTrue();

            var updatedSuit = await _aggregateStore.LoadAsync<SuitAggregate, SuitId>(_suit.Id, CancellationToken.None);
            updatedSuit.LeftTrouserLength.Equals(new Measurement(115, MeasurementUnit.Centimeter)).Should().BeTrue();
        }

        [TestMethod]
        public async Task Should_negatively_alter_right_suit_trouser()
        {
            // Arrange
            var suitAlterationId = SuitAlterationId.New;
            var createTrouserAlterationCommand =
                new CreateSuitTrouserAlterationCommand(
                                suitAlterationId,
                                _suit.Id,
                                SuitTrouserAlterationChoice.Right,
                                new MeasurementAlteration(-5, MeasurementUnit.Centimeter));
            var reportAlterationPaymentCommand = new RecordSuitAlterationPaymentCommand(suitAlterationId);
            var executeAlterationCommand = new ExecuteSuitAlterationCommand(_suit.Id, suitAlterationId, TailorId.New);

            // Act and Assert

            var result = await _commandBus.PublishAsync(createTrouserAlterationCommand, CancellationToken.None);
            result.IsSuccess.Should().BeTrue();

            result = await _commandBus.PublishAsync(reportAlterationPaymentCommand, CancellationToken.None);
            result.IsSuccess.Should().BeTrue();

            result = await _commandBus.PublishAsync(executeAlterationCommand, CancellationToken.None);
            result.IsSuccess.Should().BeTrue();

            var updatedSuit = await _aggregateStore.LoadAsync<SuitAggregate, SuitId>(_suit.Id, CancellationToken.None);
            updatedSuit.RightTrouserLength.Equals(new Measurement(115, MeasurementUnit.Centimeter)).Should().BeTrue();
        }
    }
}
