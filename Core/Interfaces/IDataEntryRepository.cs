using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThesisProjectARM.Core.Interfaces
{
    public interface IDataEntryRepository
    {
        Task<bool> TestConnectionAsync(string connectionString);
        Task InitializeDatabaseAsync(string connectionString);
        Task<bool> AdminUserExistsAsync();
    }
}
