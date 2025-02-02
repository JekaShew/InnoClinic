using AuthorizationAPI.Domain.Data.Models;
using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Persistance.Data;
using InnoClinic.CommonLibrary.Response;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace AuthorizationAPI.Persistance.Repositories
{
    public class UserStatusRepository : IUserStatusRepository
    {
        private readonly AuthDBContext _authDBContext;

        public UserStatusRepository(AuthDBContext authDBContext)
        {
            _authDBContext = authDBContext;
        }

        public async Task<CustomResponse> AddUserStatus(UserStatus userStatus)
        {
            await _authDBContext.UserStatuses.AddAsync(userStatus);
            await _authDBContext.SaveChangesAsync();

            return new CustomResponse(true, "Successfully Added!");
        }

        public async Task<CustomResponse> DeleteUserStatusById(Guid userStatusId)
        {
            var userStatus = await _authDBContext.UserStatuses
                    .AsNoTracking()
                    .FirstOrDefaultAsync(us => us.Id.Equals(userStatusId)); ;

            _authDBContext.UserStatuses.Remove(userStatus);
            await _authDBContext.SaveChangesAsync();

            return new CustomResponse(true, "Successfully Deleted!");
        }

        public async Task<CustomResponse<List<UserStatus>>> TakeAllUserStatuses()
        {
            var userStatuses = await _authDBContext.UserStatuses
                    .AsNoTracking()
                    .ToListAsync();

            return new CustomResponse<List<UserStatus>>(true, "Success!", userStatuses);
        }

        public async Task<CustomResponse<UserStatus>> TakeUserStatusById(Guid userStatusId)
        {
            var userStatus = await _authDBContext.UserStatuses
                .AsNoTracking()
                .FirstOrDefaultAsync(us => us.Id == userStatusId);

            return new CustomResponse<UserStatus>(true, "Success!", userStatus);
        }

        public async Task<CustomResponse> UpdateUserStatus(UserStatus updatedUserStatus)
        {
            var userStatus = await _authDBContext.UserStatuses.FindAsync(updatedUserStatus.Id);

            _authDBContext.UserStatuses.Entry(userStatus).State = EntityState.Detached;
            _authDBContext.UserStatuses.Update(updatedUserStatus);
            await _authDBContext.SaveChangesAsync();

            return new CustomResponse(true, "Successfully Updated!");
        }

        public async Task<CustomResponse<UserStatus>> TakeUserStatusWithPredicate(Expression<Func<UserStatus, bool>> predicate)
        {
            var userStatus = await _authDBContext.UserStatuses
                    .Where(predicate)
                    .FirstOrDefaultAsync();

            return new CustomResponse<UserStatus>(true,"Success!", userStatus);
        }
    }
}
