namespace AuthorizationAPI.Shared.DTOs
{
    public class RoleDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
    }
}
