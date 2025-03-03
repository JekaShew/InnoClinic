using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OfficesAPI.Persistance.Extensions;
using System.Data;

namespace OfficesAPI.Persistance.Data;

public class OfficesContext : IOfficesContext
{
    public IClientSessionHandle session { get; set; }
    public MongoClient mongoClient { get; set; }
    private IMongoDatabase _officesDB { get; set; }
    private readonly List<Func<Task>> _commandTasks;

    public OfficesContext(IOptions<ConnectionStringsSettings> options)
    {
        var mongoUrl = MongoUrl.Create(options.Value.OfficesDB);
        mongoClient = new MongoClient(mongoUrl);
        _officesDB = mongoClient.GetDatabase("OfficesDB");
        _commandTasks = new List<Func<Task>>();
    }
    public void AddCommand(Func<Task> func)
    {
        _commandTasks.Add(func);
    }

    public IMongoCollection<T> GetMongoCollection<T>(string name)
    {
        return _officesDB.GetCollection<T>(name);
    }

    public async Task SingleExecution()
    {
        var commandTask = _commandTasks.FirstOrDefault();
        await commandTask.Invoke();
    }

    public async Task TransactionExecution()
    {
        try
        {
            using (session = await mongoClient.StartSessionAsync())
            {
                session.StartTransaction();

                var commandTasks = _commandTasks.Select(async c => await c.Invoke());

                await Task.WhenAll(commandTasks);

                await session.CommitTransactionAsync();
            }
        }
        catch (Exception ex)
        {
            await session.AbortTransactionAsync();
            session?.Dispose();
            throw new Exception(ex.Message, ex.InnerException);
        }
        finally
        {
            session?.Dispose();
        }
    }
    public void Dispose()
    {
        session?.Dispose();
        GC.SuppressFinalize(this);
    }
}
