namespace CommonLibrary.RabbitMQEvents.SpecializationEvents;

public class SpecializationCreatedEvent
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
}
