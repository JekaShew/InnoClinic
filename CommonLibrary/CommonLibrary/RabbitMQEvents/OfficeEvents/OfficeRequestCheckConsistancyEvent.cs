namespace CommonLibrary.RabbitMQEvents.OfficeEvents;

public class OfficeRequestCheckConsistancyEvent
{
    public Guid UserId { get; set; }
    public DateTime DateTime { get; set; }
}
