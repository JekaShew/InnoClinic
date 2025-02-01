using OfficesAPI.Domain.Data.Models;
using System.Linq.Expressions;

namespace OfficesAPI.Domain.IRepositories
{
    public interface IOffice
    {
        public Task<bool> AddOffice(Office office);
        public Task<List<Office>> TakeAllOffices();
        public Task<Office> TakeOfficeById(string officeId);
        public Task<bool> UpdateOffice(Office office);
        public Task<bool> DeleteOfficeById(string officeId);
        //public Task<Office> TakeOfficeWithPredicate(Expression<Func<Office, bool>> predicate);
    }
}
