using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IDataEntryRepository
    {
        Task<bool> TestConnectionAsync(string connectionString);
        Task InitializeDatabaseAsync(string connectionString);
        Task<bool> AdminUserExistsAsync();
    }
}
