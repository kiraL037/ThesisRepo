using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThesisProjectARM.Core.Models
{
    public class InfoMetaData
    {
        public string DataNameInfo { get; set; }
        public List<ColumnMetadata> Columns { get; set; }
    }
}

