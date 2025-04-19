namespace CommonLibrary.RabbitMQEvents.ServiceEvents;

public class ServiceCreatedEvent
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
}
