namespace CommonLibrary.RabbitMQEvents.DoctorEvents;

public class DoctorRequestCheckConsistancyEvent
{
    public Guid UserId { get; set; }
    public DateTime DateTime { get; set; }
}

