namespace CommonLibrary.RabbitMQEvents.DoctorSpecializationEvents;

public class DoctorSpecializationRequestCheckConsistancyEvent
{
    public Guid UserId { get; set; }
    public DateTime DateTime { get; set; }
}
