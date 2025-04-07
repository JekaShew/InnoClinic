namespace CommonLibrary.RabbitMQEvents.SpecializationEvents;

public class SpecializationUpdatedEvent
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
}
