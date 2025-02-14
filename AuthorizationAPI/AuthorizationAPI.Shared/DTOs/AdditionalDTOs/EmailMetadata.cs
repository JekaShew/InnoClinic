namespace AuthorizationAPI.Shared.DTOs.AdditionalDTOs
{
    public class EmailMetadata
    {
        public string ToAddress { get; set; }
        public string Subject { get; set; }
        public string? Body { get; set; }
        public Guid FromAdress { get; set; }

        public EmailMetadata(
                string toAddress,
                string subject,
                string? body = "",
                Guid? fromAdress = null)
        {
            ToAddress = toAddress;
            Subject = subject;
            Body = body;
            FromAdress = fromAdress.Value;
        }
    }
}
