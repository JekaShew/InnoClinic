namespace CommonLibrary.RabbitMQEvents.DoctorSpecializationEvents;

public class DoctorSpecializationUpdatedEvent
{
    public Guid Id { get; set; }
    public Guid DoctorId { get; set; }
    public Guid SpecializationId { get; set; }
}
