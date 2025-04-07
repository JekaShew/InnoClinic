namespace CommonLibrary.RabbitMQEvents.SpecializationEvents;

public class SpecializationCheckConsistancyEvent
{
    public Guid UserId { get; set; }
    public DateTime DateTime { get; set; }
}
