using System.Threading.Tasks;

namespace ИнCore.Interfaces
{
    public interface IDataEntryRepository
    {
        Task<bool> TestConnectionAsync(string connectionString);
        Task InitializeDatabaseAsync(string connectionString);
        Task<bool> AdminUserExistsAsync();
    }
}
