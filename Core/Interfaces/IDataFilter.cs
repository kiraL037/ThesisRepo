using System.Data;

namespace Core.Interfaces
{
    public interface IDataFilter
    {
        DataTable Filter(DataTable data);
    }
}
