using System;

namespace SampleStore.Infrastructure.Events.Outbox
{
    public class OutboxMessage
    {
        public Guid Id { get; set; }
        public DateTime OccurredOn { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }
        public DateTime? ProcessedDate { get; set; }

        private OutboxMessage()
        {

        }

        public OutboxMessage(DateTime occuredOn, string type, string data)
        {
            Id = Guid.NewGuid();
            OccurredOn = occuredOn;
            Type = type;
            Data = data;
        }
    }
}
