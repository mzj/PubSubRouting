using System;

namespace Publisher.Events
{
    /// <summary>
    /// 
    /// </summary>
    public class IntegrationEvent
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid EventId { get; init; } = Guid.NewGuid();

        /// <summary>
        /// 
        /// </summary>
        public DateTime EventCreationDate { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public IntegrationEvent()
        {
            EventCreationDate = DateTime.UtcNow;
        }
    }
}
