namespace CommonLibrary.CommonService
{
    public interface ICommonService
    {
        public Guid? GetCurrentUserId();
        public Task<string> GetHashString(string stringToHash);
    }
}
