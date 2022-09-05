using Dapr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Subscriber.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Subscriber.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class SubscriberController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private const string PubSubName = "pubsub";

        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger<SubscriberController> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public SubscriberController(ILogger<SubscriberController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        [Topic(PubSubName, nameof(RoutingIntegrationEvent), "event.type ==\"routing.v1\"", 1)]
        [HttpPost("routingv1")]
        public IActionResult HandleRoutingV1(RoutingIntegrationEvent @event)
        {
            // success move to the next message
            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        [Topic(PubSubName, nameof(RoutingIntegrationEvent), "event.type ==\"routing.v2\"", 2)]
        [HttpPost("routingv2")]
        public IActionResult HandleRoutingV2(RoutingIntegrationEvent @event)
        {
            // success move to the next message
            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        [Topic(PubSubName, nameof(RoutingIntegrationEvent))]
        [HttpPost("routingdefault")]
        public IActionResult HandleRoutingDefault(RoutingIntegrationEvent @event)
        {
            // success move to the next message
            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        [Topic(PubSubName, nameof(PureIntegrationEvent))]
        [HttpPost("pure")]
        public IActionResult HandlePure(PureIntegrationEvent @event)
        {
            // success move to the next message
            return Ok();
        }
    }
}
