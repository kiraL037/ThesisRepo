using System.Data;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IClassificationClustering
    {
        Task<int[]> KMeansClusteringAsync(DataTable data, string[] columnNames, int clusterCount);
        Task<string> ClassifyAsync(DataTable data, string[] columnNames, string targetColumnName, double[] newObject);
    }
}
