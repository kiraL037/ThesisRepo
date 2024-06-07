using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThesisProjectARM.Core.Interfaces
{
    public interface IClassificationClustering
    {
        Task<int[]> KMeansClusteringAsync(DataTable data, string[] columnNames, int clusterCount);
        Task<string> ClassifyAsync(DataTable data, string[] columnNames, string targetColumnName, double[] newObject);
    }
}
