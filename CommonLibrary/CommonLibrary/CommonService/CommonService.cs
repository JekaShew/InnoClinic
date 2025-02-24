using CommonLibrary.DTOs;
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

        public CurrentUserInfoDTO? GetCurrentUserInfo()
        {
            if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return null;
            }

            var claimId = _httpContextAccessor
                        .HttpContext
                        .User
                        .Claims
                        .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            var claimRole = _httpContextAccessor
                        .HttpContext
                        .User
                        .Claims
                        .FirstOrDefault(x => x.Type == ClaimTypes.Role);
            if (claimId is null || claimRole is null)
            {
                return null;
            }
            var currentUserInfoDTO = new CurrentUserInfoDTO
            {
                Id = Guid.Parse(claimId.Value),
                Role = claimRole.Value
            };
            return currentUserInfoDTO;
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
