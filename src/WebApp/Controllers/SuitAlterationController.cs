using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventFlow.Configuration;
using ApplicationLayer.Queries;
using EventFlow.Queries;
using System.Threading;
using ApplicationLayer.ReadModels;
using EventFlow;
using ApplicationLayer.Commands;
using DomainModel;
using ApplicationLayer.External;
using WebApp.Models;
using EventFlow.Aggregates;
using DomainModel.Suit;
using EventFlow.Core;
using EventFlow.Aggregates.ExecutionResults;
using System;
using System.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApp.Controllers
{
    public class SuitAlterationController : Controller
    {
        private readonly IAggregateStore _aggregateStore;
        private readonly IQueryProcessor _queryProcessor;
        private readonly ICommandBus _commandBus;
        private readonly FakeExternalEventReceiver _fakeExternalEventReceiver;

        public SuitAlterationController(FakeExternalEventReceiver fakeExternalEventReceiver, IResolver resolver)
        {
            _aggregateStore = resolver.Resolve<IAggregateStore>();
            _queryProcessor = resolver.Resolve<IQueryProcessor>();
            _commandBus = resolver.Resolve<ICommandBus>();
            _fakeExternalEventReceiver = fakeExternalEventReceiver;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var query = new GetAllSuitAlterationsQuery();
            var result = await _queryProcessor.ProcessAsync(query, CancellationToken.None);
            return View(result.Select(item => new SuitAlterationViewModel
            {
                SuitAlterationId = item.SuitAlterationId,
                CustomerId = item.CustomerId,
                SuitId = item.SuitId,
                Status = item.Status,
                LastModified = item.LastModifiedUtc.ToLocalTime()
            }));
        }

        [HttpGet]
        public IActionResult NewSuitAlteration()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> SuitAlterationDetails(string suitalterationid)
        {
            var query = new GetSuitAlterationByIdQuery(suitalterationid);
            var result = await _queryProcessor.ProcessAsync(query, CancellationToken.None);
            return View(new SuitAlterationViewModel
            {
                SuitAlterationId = result.SuitAlterationId,
                CustomerId = result.CustomerId,
                SuitId = result.SuitId,
                Status = result.Status
            });
        }

        [HttpPost]
        public async Task<IActionResult> PayForAlteration(SuitAlterationViewModel model)
        {
            if (model.Status == "created")
            {
                var reportAlterationPaymentCommand = new RecordSuitAlterationPaymentCommand(SuitAlterationId.With(model.SuitAlterationId));
                var result = await _commandBus.PublishAsync(reportAlterationPaymentCommand, CancellationToken.None);
                if (!result.IsSuccess)
                {
                    return RedirectToAction("Home", "Error", result.ToString());
                }

                await _fakeExternalEventReceiver.AddEventAsync(new OrderPaidEvent
                {
                    SuitAlterationId = model.SuitAlterationId,
                    Type = "orderpaid"
                });
            }
            
            if (model.Status == "paid")
            {
                var executeAlterationCommand = new ExecuteSuitAlterationCommand(SuitId.With(model.SuitId), SuitAlterationId.With(model.SuitAlterationId) , TailorId.New);
                var result = await _commandBus.PublishAsync(executeAlterationCommand, CancellationToken.None);
                if (!result.IsSuccess)
                {
                    return RedirectToAction("Home", "Error", result.ToString());
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> NewSuitAlteration(NewSuitAlterationViewModel model)
        {
            // If the suit does not exist then create it.
            var suitId = SuitId.NewDeterministic(Guid.NewGuid(), model.SuitName);
            var customerId = CustomerId.NewDeterministic(Guid.NewGuid(), model.CustomerName);
            var suit = await _aggregateStore.LoadAsync<SuitAggregate, SuitId>(suitId, CancellationToken.None);
            if (suit.IsNew)
            {
                suit.Create(
                    new SuitSleevePair(
                        new Measurement(90, MeasurementUnit.Centimeter),
                        new Measurement(90, MeasurementUnit.Centimeter)),
                    new SuitTrouserPair(
                        new Measurement(120, MeasurementUnit.Centimeter),
                        new Measurement(120, MeasurementUnit.Centimeter)));
                suit.Sell(customerId);
                await _aggregateStore.StoreAsync<SuitAggregate, SuitId>(suit, SourceId.New, CancellationToken.None);
            }

            IExecutionResult result = null;
            switch (model.AlterationType)
            {
                case NewSuitAlterationType.PlusFiveBothSleeve:
                    var plusBothSleeveCommand = new CreateSuitSleeveAlterationCommand(SuitAlterationId.New, suitId, SuitSleeveAlterationChoice.Both, new MeasurementAlteration(5, MeasurementUnit.Centimeter));
                    result = await _commandBus.PublishAsync(plusBothSleeveCommand, CancellationToken.None);
                    break;

                case NewSuitAlterationType.PlusFiveLeftSleeve:
                    var plusLeftSleeveCommand = new CreateSuitSleeveAlterationCommand(SuitAlterationId.New, suitId, SuitSleeveAlterationChoice.Left, new MeasurementAlteration(5, MeasurementUnit.Centimeter));
                    result = await _commandBus.PublishAsync(plusLeftSleeveCommand, CancellationToken.None);
                    break;

                case NewSuitAlterationType.PlusFiveRightSleeve:
                    var plusRightSleeveCommand = new CreateSuitSleeveAlterationCommand(SuitAlterationId.New, suitId, SuitSleeveAlterationChoice.Right, new MeasurementAlteration(5, MeasurementUnit.Centimeter));
                    result = await _commandBus.PublishAsync(plusRightSleeveCommand, CancellationToken.None);
                    break;

                case NewSuitAlterationType.MinusFiveBothSleeve:
                    var minusBothSleeveCommand = new CreateSuitSleeveAlterationCommand(SuitAlterationId.New, suitId, SuitSleeveAlterationChoice.Both, new MeasurementAlteration(-5, MeasurementUnit.Centimeter));
                    result = await _commandBus.PublishAsync(minusBothSleeveCommand, CancellationToken.None);
                    break;

                case NewSuitAlterationType.MinusFiveLeftSleeve:
                    var minusLeftSleeveCommand = new CreateSuitSleeveAlterationCommand(SuitAlterationId.New, suitId, SuitSleeveAlterationChoice.Left, new MeasurementAlteration(-5, MeasurementUnit.Centimeter));
                    result = await _commandBus.PublishAsync(minusLeftSleeveCommand, CancellationToken.None);
                    break;

                case NewSuitAlterationType.MinusFiveRightSleeve:
                    var minusRightSleeveCommand = new CreateSuitSleeveAlterationCommand(SuitAlterationId.New, suitId, SuitSleeveAlterationChoice.Right, new MeasurementAlteration(-5, MeasurementUnit.Centimeter));
                    result = await _commandBus.PublishAsync(minusRightSleeveCommand, CancellationToken.None);
                    break;

                case NewSuitAlterationType.PlusFiveBothTrouser:
                    var plusBothTrouserCommand = new CreateSuitTrouserAlterationCommand(SuitAlterationId.New, suitId, SuitTrouserAlterationChoice.Both, new MeasurementAlteration(5, MeasurementUnit.Centimeter));
                    result = await _commandBus.PublishAsync(plusBothTrouserCommand, CancellationToken.None);
                    break;

                case NewSuitAlterationType.PlusFiveLeftTrouser:
                    var plusLeftTrouserCommand = new CreateSuitTrouserAlterationCommand(SuitAlterationId.New, suitId, SuitTrouserAlterationChoice.Left, new MeasurementAlteration(5, MeasurementUnit.Centimeter));
                    result = await _commandBus.PublishAsync(plusLeftTrouserCommand, CancellationToken.None);
                    break;

                case NewSuitAlterationType.PlusFiveRightTrouser:
                    var plusRightTrouserCommand = new CreateSuitTrouserAlterationCommand(SuitAlterationId.New, suitId, SuitTrouserAlterationChoice.Right, new MeasurementAlteration(5, MeasurementUnit.Centimeter));
                    result = await _commandBus.PublishAsync(plusRightTrouserCommand, CancellationToken.None);
                    break;

                case NewSuitAlterationType.MinusFiveBothTrouser:
                    var minusBothTrouserCommand = new CreateSuitTrouserAlterationCommand(SuitAlterationId.New, suitId, SuitTrouserAlterationChoice.Both, new MeasurementAlteration(-5, MeasurementUnit.Centimeter));
                    result = await _commandBus.PublishAsync(minusBothTrouserCommand, CancellationToken.None);
                    break;

                case NewSuitAlterationType.MinusFiveLeftTrouser:
                    var minusLeftTrouserCommand = new CreateSuitTrouserAlterationCommand(SuitAlterationId.New, suitId, SuitTrouserAlterationChoice.Left, new MeasurementAlteration(-5, MeasurementUnit.Centimeter));
                    result = await _commandBus.PublishAsync(minusLeftTrouserCommand, CancellationToken.None);
                    break;

                case NewSuitAlterationType.MinusFiveRightTrouser:
                    var minusRightTrouserCommand = new CreateSuitTrouserAlterationCommand(SuitAlterationId.New, suitId, SuitTrouserAlterationChoice.Right, new MeasurementAlteration(-5, MeasurementUnit.Centimeter));
                    result = await _commandBus.PublishAsync(minusRightTrouserCommand, CancellationToken.None);
                    break;
            }

            if (result != null && !result.IsSuccess)
            {
                return RedirectToAction("Home", "Error", result.ToString());
            }

            return RedirectToAction("Index");
        }
    }
}
