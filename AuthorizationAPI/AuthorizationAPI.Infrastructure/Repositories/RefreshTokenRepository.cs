using AuthorizationAPI.Application.DTOs;
using AuthorizationAPI.Application.Interfaces;
using AuthorizationAPI.Application.Mappers;
using AuthorizationAPI.Domain.Data;
using AuthorizationAPI.Domain.Data.Models;
using InnoShop.CommonLibrary.Response;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AuthorizationAPI.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshToken
    {
        private readonly AuthDBContext _authDBContext;
        public RefreshTokenRepository(AuthDBContext authDBContext )
        {
            _authDBContext = authDBContext;
        }
        public async Task<CustomResponse> AddRefreshToken(RefreshTokenDTO refreshTokenDTO)
        {
           var refreshToken = RefreshTokenMapper.RefreshTokenDTOToRefreshToken(refreshTokenDTO);

            await _authDBContext.RefreshTokens.AddAsync(refreshToken);
            await _authDBContext.SaveChangesAsync();

            return new CustomResponse(true, "Successfully Added!");
        }

        public async Task<CustomResponse> DeleteRefreshTokenByRTokenId(Guid rTokenId)
        {     
            var refreshToken = await _authDBContext.RefreshTokens
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.Id.Equals(rTokenId));

            _authDBContext.RefreshTokens.Remove(refreshToken);
            await _authDBContext.SaveChangesAsync();

            return new CustomResponse(true, "Successfully Deleted!");
        }

        public async Task<List<RefreshTokenDTO>> TakeAllRefreshTokens()
        {
            var refreshTokenDTOs = await _authDBContext.RefreshTokens
                    .Include(u => u.User)
                    .AsNoTracking()
                    .Select(rt => RefreshTokenMapper.RefreshTokenToRefreshTokenDTO(rt))
                    .ToListAsync();

            return refreshTokenDTOs;     
        }

        public async Task<RefreshTokenDTO> TakeRefreshTokenByRTokenId(Guid rTokenId)
        {
         
            var refreshTokenDTO = RefreshTokenMapper.RefreshTokenToRefreshTokenDTO(
            await _authDBContext.RefreshTokens
                    .Include(u => u.User)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.Id == rTokenId));

            return refreshTokenDTO;
            
        }

        public async Task<CustomResponse> UpdateRefreshToken(RefreshTokenDTO refreshTokenDTO)
        {
            var refreshToken = await _authDBContext.RefreshTokens.FindAsync(refreshTokenDTO.Id);

            ApplyPropertiesFromDTOToModel(refreshTokenDTO, refreshToken);

            _authDBContext.RefreshTokens.Update(refreshToken);
            await _authDBContext.SaveChangesAsync();

            return new CustomResponse(true, "Successfully Updated!");
        }

        private void ApplyPropertiesFromDTOToModel(RefreshTokenDTO refreshTokenDTO, RefreshToken refreshToken)
        {
            var dtoProperties = refreshTokenDTO.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var modelProperties = refreshToken.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var dtoProperty in dtoProperties)
            {
                var modelProperty = modelProperties.FirstOrDefault(p => p.Name == dtoProperty.Name && p.PropertyType == dtoProperty.PropertyType);
                if (modelProperty != null)
                {
                    modelProperty.SetValue(refreshToken, dtoProperty.GetValue(refreshTokenDTO));
                }
            }
        }
    }
}
