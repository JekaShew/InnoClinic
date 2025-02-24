using CommonLibrary.DTOs;

namespace CommonLibrary.CommonService
{
    public interface ICommonService
    {
        public CurrentUserInfoDTO? GetCurrentUserInfo();
        public Task<string> GetHashString(string stringToHash);
    }
}
