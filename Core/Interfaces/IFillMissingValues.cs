using System.Data;

namespace Core.Interfaces
{
    public interface IFillMissingValues
    {
        DataTable Fill(DataTable data);
    }
}
