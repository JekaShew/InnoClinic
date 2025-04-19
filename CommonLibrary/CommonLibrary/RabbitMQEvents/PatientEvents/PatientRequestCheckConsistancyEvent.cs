namespace CommonLibrary.RabbitMQEvents.PatientEvents;

public class PatientRequestCheckConsistancyEvent
{
    public Guid UserId { get; set; }
    public DateTime DateTime { get; set; }
}

