namespace CommonLibrary.RabbitMQEvents.DoctorEvents;

public class DoctorCreatedEvent
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? SecondName { get; set; }
}
