using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisProjectARM.Core.Models;

namespace ThesisProjectARM.Core.Interfaces
{
    public interface IDatabaseService
    {
        Task<bool> SetupDatabaseAsync(ConnectionModel connection);
        string BuildConnectionString(ConnectionModel connection);
    }
}