using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CommonLibrary.CommonService
{
    public class CommonService :ICommonService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CommonService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? GetCurrentUserId()
        {
            if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return Guid.Empty;
            }

            var claim = _httpContextAccessor
                        .HttpContext
                        .User
                        .Claims
                        .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            if (claim is null)
            {
                return Guid.Empty;
            }

            return Guid.Parse(claim.Value);
        }

        public async Task<string> GetHashString(string stringToHash)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes($"{stringToHash}");
                var ms = new MemoryStream(inputBytes);
                var hashBytes = await md5.ComputeHashAsync(ms);
                var stringHash = Encoding.UTF8.GetString(hashBytes);

                return stringHash;
            }
        }
    }
}
