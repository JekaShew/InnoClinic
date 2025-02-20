using MongoDB.Driver;
using OfficesAPI.Domain.Data.Models;
using OfficesAPI.Domain.IRepositories;
using OfficesAPI.Persistance.Data;

namespace OfficesAPI.Persistance.Repositories
{
    public class OfficeRepository : IOfficeRepository
    {
        private readonly IMongoCollection<Office> _officeCollection;
        public OfficeRepository(MongoDBService mongoDBService)
        {
            _officeCollection = mongoDBService.Database?.GetCollection<Office>("office");
        }
        public async Task<bool> AddOffice(Office office)
        {
            await _officeCollection.InsertOneAsync(office); 
            
            return true;
        }

        public async Task<bool> DeleteOfficeById(string officeId)
        {
            var filter = Builders<Office>.Filter.Eq(o => o.Id, officeId);
            await _officeCollection.DeleteOneAsync(filter);

            return true;
        }

        public async Task<List<Office>> TakeAllOffices()
        {
            return await _officeCollection.Find(FilterDefinition<Office>.Empty).ToListAsync();
        }

        public async Task<Office> TakeOfficeById(string officeId)
        {
            var filter = Builders<Office>.Filter.Eq(o => o.Id, officeId);
            return await _officeCollection.Find(filter).FirstOrDefaultAsync();
        }

        //public async Task<Office> TakeOfficeWithPredicate(Expression<Func<Office, bool>> predicate)
        //{
        //    var filter = Builders<Office>.Filter.Eq(predicate, predicate.Parameters.FirstOrDefault());

        //    return await _officeCollection.Find(filter).FirstOrDefaultAsync();
        //}

        public async Task<bool> UpdateOffice(Office updatedoffice)
        {
            var filter = Builders<Office>.Filter.Eq(o => o.Id, updatedoffice.Id);
           
            await _officeCollection.ReplaceOneAsync(filter, updatedoffice);

            return true;
        }
    }
}
