using Microsoft.IdentityModel.Tokens;

namespace CommonLibrary.CommonService
{
    public interface ICacheService
    {
        public T? GetData<T>(string key);
        public void SetData<T>(string key, T value, TimeSpan expirationTime);
        public void RemoveData(string key);
    }
}
