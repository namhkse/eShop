using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Text.Json;
using EventBus;

namespace IntegrationEventLogEF;

public class IntegrationEventLogEntry
{
    private IntegrationEventLogEntry()
    {
    }

    public IntegrationEventLogEntry(IntegrationEvent evt, Guid transactionId)
    {
        EventId = evt.Id;
        CreationTime = evt.CreationDate;
        EventTypeName = evt.GetType().FullName;
        Content = JsonSerializer.Serialize(evt,
            evt.GetType());
        State = EventStateEnum.NotPublished;
        TransactionId = transactionId;
    }

    public Guid EventId { get; private set; }

    public string EventTypeName { get; private set; }

    [NotMapped] public string EventTypeShortName => EventTypeName.Split('.').Last();

    [NotMapped] public IntegrationEvent IntegrationEvent { get; private set; }

    public int TimeSent { get; set; }

    public DateTime CreationTime { get; private set; }

    [Required] public string Content { get; private set; }

    public EventStateEnum State { get; set; }

    public Guid TransactionId { get; private set; }

    public IntegrationEventLogEntry DeserializedJsonContent(Type type)
    {
        IntegrationEvent = JsonSerializer.Deserialize(Content, type) as IntegrationEvent;
        return this;
    }
}