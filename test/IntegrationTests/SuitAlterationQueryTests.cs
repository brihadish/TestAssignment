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
using ApplicationLayer.Queries;
using EventFlow.Queries;
using System.Linq;

namespace IntegrationTests
{
    [TestClass]
    public sealed class SuitAlterationQueryTests
    {
        private SuitAggregate _suit;
        private IAggregateStore _aggregateStore;
        private ICommandBus _commandBus;
        private FakeExternalEventReceiver _fakeExternalEventReceiver;
        private NotificationServiceSpy _notificationServiceSpy;
        private IQueryProcessor _queryProcessor;

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
            _queryProcessor = resolver.Resolve<IQueryProcessor>();

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
        public async Task Should_return_created_alterations()
        {
            // Arrange
            var suitAlterationId = SuitAlterationId.New;
            var createSleeveAlterationCommand =
                new CreateSuitSleeveAlterationCommand(
                                suitAlterationId,
                                _suit.Id,
                                SuitSleeveAlterationChoice.Both,
                                new MeasurementAlteration(5, MeasurementUnit.Centimeter));
            await _commandBus.PublishAsync(createSleeveAlterationCommand, CancellationToken.None);

            // Act and Assert
            var query = new GetSuitAlterationsByStatusQuery("created");
            var result = await _queryProcessor.ProcessAsync(query, CancellationToken.None);
            result.Count.Should().Be(1);
            var model = result.First();
            model.SuitAlterationId.Should().Be(suitAlterationId.Value);
            model.CustomerId.Should().Be(_suit.CustomerId.Value);
            model.SuitId.Should().Be(_suit.Id.Value);
            model.Status.Should().Be("created");            
        }

        [TestMethod]
        public async Task Should_return_paid_alterations()
        {
            // Arrange
            var suitAlterationId = SuitAlterationId.New;
            var createSleeveAlterationCommand =
                new CreateSuitSleeveAlterationCommand(
                                suitAlterationId,
                                _suit.Id,
                                SuitSleeveAlterationChoice.Both,
                                new MeasurementAlteration(5, MeasurementUnit.Centimeter));
            var reportAlterationPaymentCommand = new RecordSuitAlterationPaymentCommand(suitAlterationId);
            await _commandBus.PublishAsync(createSleeveAlterationCommand, CancellationToken.None);
            await _commandBus.PublishAsync(reportAlterationPaymentCommand, CancellationToken.None);

            // Act and Assert
            var query = new GetSuitAlterationsByStatusQuery("paid");
            var result = await _queryProcessor.ProcessAsync(query, CancellationToken.None);
            result.Count.Should().Be(1);
            var model = result.First();
            model.SuitAlterationId.Should().Be(suitAlterationId.Value);
            model.CustomerId.Should().Be(_suit.CustomerId.Value);
            model.SuitId.Should().Be(_suit.Id.Value);
            model.Status.Should().Be("paid");
        }

        [TestMethod]
        public async Task Should_return_succeeded_alterations()
        {
            // Arrange
            var suitAlterationId = SuitAlterationId.New;
            var createSleeveAlterationCommand =
                new CreateSuitSleeveAlterationCommand(
                                suitAlterationId,
                                _suit.Id,
                                SuitSleeveAlterationChoice.Both,
                                new MeasurementAlteration(5, MeasurementUnit.Centimeter));
            var reportAlterationPaymentCommand = new RecordSuitAlterationPaymentCommand(suitAlterationId);
            var executeAlterationCommand = new ExecuteSuitAlterationCommand(_suit.Id, suitAlterationId, TailorId.New);
            await _commandBus.PublishAsync(createSleeveAlterationCommand, CancellationToken.None);
            await _commandBus.PublishAsync(reportAlterationPaymentCommand, CancellationToken.None);
            await _commandBus.PublishAsync(executeAlterationCommand, CancellationToken.None);

            // Act and Assert
            var query = new GetSuitAlterationsByStatusQuery("succeeded");
            var result = await _queryProcessor.ProcessAsync(query, CancellationToken.None);
            result.Count.Should().Be(1);
            var model = result.First();
            model.SuitAlterationId.Should().Be(suitAlterationId.Value);
            model.CustomerId.Should().Be(_suit.CustomerId.Value);
            model.SuitId.Should().Be(_suit.Id.Value);
            model.Status.Should().Be("succeeded");
        }
    }
}
