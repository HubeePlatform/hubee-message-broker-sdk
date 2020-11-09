using Events.Order;
using Hubee.MessageBroker.Sdk.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PublisherApi.Model;
using System;

namespace PublisherApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IEventBusService _eventBusService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IEventBusService eventBusService, ILogger<OrderController> logger)
        {
            _eventBusService = eventBusService;
            _logger = logger;
        }

        [HttpPost]
        public ActionResult Create(Order order)
        {
            // Create order
            // Create order success
            var eventId = Guid.NewGuid();

            _eventBusService.Publish<OrderCreatedEvent>(new
            {
                EventId = eventId,
                OrderId = order.Id,
                order.Description,
                Status = "PENDING_PAYMENT"
            });

            _logger.LogInformation($"OrderController: Publish (OrderCreatedEvent)");

            return Ok($"EventId: {eventId}");
        }
    }
}