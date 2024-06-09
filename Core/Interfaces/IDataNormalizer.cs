using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace Core.Interfaces
{
    public interface IDataNormalizer
    {
        DataTable Normalize(DataTable data);
    }
}

