using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Publisher.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Publisher.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class PublisherController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private const string PubSubName = "pubsub";

        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger<PublisherController> _logger;

        /// <summary>
        /// 
        /// </summary>
        private readonly DaprClient _dapr;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="dapr"></param>
        public PublisherController(ILogger<PublisherController> logger, DaprClient dapr)
        {
            _logger = logger;
            _dapr = dapr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PublishEvents()
        {
            #region v1
            var cloudEventWrapV1 = new CloudEvent<RoutingIntegrationEvent>(new RoutingIntegrationEvent { Name = "Routing test" }) 
            { 
                Type = "routing.v1" 
            };
            // publish cloud event with custom type
            await _dapr.PublishEventAsync(PubSubName, nameof(RoutingIntegrationEvent), (dynamic)cloudEventWrapV1);
            #endregion

            #region v2
            var cloudEventWrapV2 = new CloudEvent<RoutingIntegrationEvent>(new RoutingIntegrationEvent { Name = "Routing test" })
            {
                Type = "routing.v2"
            };
            // publish cloud event with custom type
            await _dapr.PublishEventAsync(PubSubName, nameof(RoutingIntegrationEvent), (dynamic)cloudEventWrapV2);
            #endregion

            #region default
            var cloudEventWrapDefault = new CloudEvent<RoutingIntegrationEvent>(new RoutingIntegrationEvent { Name = "Routing test" });
            // publish cloud event with custom type
            await _dapr.PublishEventAsync(PubSubName, nameof(RoutingIntegrationEvent), (dynamic)cloudEventWrapDefault);
            #endregion

            #region pure
            var pureEvent = new PureIntegrationEvent 
            { 
                Name = "Pure event test" 
            };
            // publish pure event without custom cloud envelope
            await _dapr.PublishEventAsync(PubSubName, nameof(PureIntegrationEvent), (dynamic)pureEvent);
            #endregion

            return Ok();
        }
    }
}
