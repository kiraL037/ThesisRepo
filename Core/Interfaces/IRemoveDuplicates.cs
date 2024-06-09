using System.Data;

namespace Core.Interfaces
{
    public interface IRemoveDuplicates
    {
        DataTable Remove(DataTable data);
    }
}
