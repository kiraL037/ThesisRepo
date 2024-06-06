using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThesisProjectARM.Core.Models;

namespace ThesisProjectARM.Core.Interfaces
{
    public interface IDataService
    {
        DataTable LoadData(string filePath);
        void SaveData(DataTable data, string filePath);
    }
}