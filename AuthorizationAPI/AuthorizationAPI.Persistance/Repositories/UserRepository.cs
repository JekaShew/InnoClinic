using AuthorizationAPI.Domain.Data.Models;
using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Persistance.Data;
using InnoClinic.CommonLibrary.Response;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace AuthorizationAPI.Persistance.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDBContext _authDBContext;

        public UserRepository(AuthDBContext authDBContext)
        {
            _authDBContext = authDBContext;
        }

        public async Task<CustomResponse> AddUser(User updatedUser)
        {

            await _authDBContext.Users.AddAsync(updatedUser);
            await _authDBContext.SaveChangesAsync();

            return new CustomResponse(true, "Successfully Added!");
        }

        public async Task<CustomResponse> DeleteUserById(Guid userId)
        {
            var user = await _authDBContext.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == userId);

            _authDBContext.Users.Remove(user);
            await _authDBContext.SaveChangesAsync();

            return new CustomResponse(true, "Successfully Deleted!");
        }

        public async Task<CustomResponse<List<User>>> TakeAllUsers()
        {
            var users = await _authDBContext.Users
                    .AsNoTracking()
                    .ToListAsync();

            return new CustomResponse<List<User>>(true, "Success!", users);
        }

        public async Task<CustomResponse<User>> TakeUserById(Guid userId)
        {
            var user = await _authDBContext.Users
                    .Include(r => r.Role)
                    .Include(us => us.UserStatus)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id.Equals(userId));

            return new CustomResponse<User>(true, "Success!", user);
        }

        public async Task<CustomResponse> UpdateUser(User updatedUser)
        {
            var user = await _authDBContext.Users.FindAsync(updatedUser.Id);

            _authDBContext.Users.Entry(user).State = EntityState.Detached;
            _authDBContext.Users.Update(updatedUser);
            await _authDBContext.SaveChangesAsync();

            return new CustomResponse(true, "Successfully Updated!");
        }

        public async Task<CustomResponse<User>> TakeUserWithPredicate(Expression<Func<User, bool>> predicate)
        {
            var user = await _authDBContext.Users
                    .Where(predicate)
                    .FirstOrDefaultAsync();

            return new CustomResponse<User>(true, "Success!", user);
        }
    }
}
