namespace AuthorizationAPI.Shared.Constants
{
    public static class DBConstants
    {
        public static Guid PatientRoleId { get; } = new Guid("78B25FDF-7199-4066-B677-5BC465BC3D1A");
        public static Guid DoctorRoleId { get; } = new Guid("0EEC148A-43D6-4B32-AFB6-1ECF3341BE6D");
        public static Guid AdministratorRoleId { get; } = new Guid("73B795D3-4917-4219-A1A0-044FCC6606EA");
        public static Guid ActivatedUserStatusId { get; } = new Guid("B9F67CF2-60DE-48EB-82D0-8A5D6CDE1B0F");
        public static Guid NonActivatedUserStatusId { get; } = new Guid("A780B7F4-3C8B-4452-A426-E7ABC1A46949");
        public static Guid DeletedUserStatusId { get; } = new Guid("6C6FEEBA-0919-4266-B2D1-9F5B724DB31A");
        public static Guid BannedUserStatusId { get; } = new Guid("7B31946C-6D14-44DC-9F93-3A4C06DB902E");
    }
}
