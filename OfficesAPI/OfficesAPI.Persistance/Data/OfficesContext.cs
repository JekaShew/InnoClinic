using Microsoft.Extensions.Options;
using MongoDB.Driver;
using OfficesAPI.Persistance.Extensions;
using System.Collections.Concurrent;
using System.Data;

namespace OfficesAPI.Persistance.Data;

public class OfficesContext : IOfficesContext
{
    public IClientSessionHandle session { get; set; }
    public MongoClient mongoClient { get; set; }
    private readonly IMongoDatabase _officesDB;
    private readonly  ConcurrentQueue<Func<Task>> _commandTasks;

    public OfficesContext(IOptions<ConnectionStringsSettings> options)
    {
        var mongoUrl = MongoUrl.Create(options.Value.OfficesDB);
        mongoClient = new MongoClient(mongoUrl);
        _officesDB = mongoClient.GetDatabase("OfficesDB");
        _commandTasks = new ConcurrentQueue<Func<Task>>();
    }
    public void AddCommand(Func<Task> func)
    {
        _commandTasks.Enqueue(func);
    }

    public IMongoCollection<T> GetMongoCollection<T>(string name)
    {
        return _officesDB.GetCollection<T>(name);
    }

    public async Task SingleExecution()
    {
        if(!_commandTasks.TryDequeue(out Func<Task>? commandTask))
        {
            throw new Exception("No commands to execute");
        }

        await commandTask.Invoke();
    }

    public async Task TransactionExecution()
    {
        try
        {
            using (session = await mongoClient.StartSessionAsync())
            {
                session.StartTransaction();
                foreach (var command in _commandTasks)
                {
                    _commandTasks.TryDequeue(out Func<Task>? commandTask);
                    await command.Invoke();
                }

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
    }
}
