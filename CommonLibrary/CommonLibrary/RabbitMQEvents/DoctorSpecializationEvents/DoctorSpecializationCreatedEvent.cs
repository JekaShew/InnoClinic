namespace CommonLibrary.RabbitMQEvents.DoctorSpecializationEvents;

public class DoctorSpecializationCreatedEvent
{
    public Guid Id { get; set; }
    public Guid DoctorId { get; set; }
    public Guid SpecializationId { get; set; }
}
