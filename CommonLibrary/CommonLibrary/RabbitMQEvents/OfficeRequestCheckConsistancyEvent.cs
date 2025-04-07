namespace CommonLibrary.RabbitMQEvents;

public class OfficeRequestCheckConsistancyEvent
{
    public Guid UserId { get; set; }
    public DateTime DateTime { get; set; }
}
