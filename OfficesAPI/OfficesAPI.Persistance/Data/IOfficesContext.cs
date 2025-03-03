using MongoDB.Driver;

namespace OfficesAPI.Persistance.Data;

public interface IOfficesContext : IDisposable
{
    public void AddCommand(Func<Task> func);
    public Task SingleExecution();
    public Task TransactionExecution();
    IMongoCollection<T> GetMongoCollection<T>(string name);
}
