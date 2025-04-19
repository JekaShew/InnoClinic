namespace CommonLibrary.RabbitMQEvents.ServiceEvents;

public class ServiceRequestCheckConsistancyEvent
{
    public Guid UserId { get; set; }
    public DateTime DateTime { get; set; }
}

