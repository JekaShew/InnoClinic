namespace CommonLibrary.RabbitMQEvents.SpecializationEvents;

public class SpecializationRequestCheckConsistancyEvent
{
    public Guid UserId { get; set; }
    public DateTime DateTime { get; set; }
}
