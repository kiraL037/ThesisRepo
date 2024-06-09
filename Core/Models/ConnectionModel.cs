using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThesisProjectARM.Core.Models
{
    public class ConnectionModel
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConnectionString { get; set; }
        public bool UseWindowsAuthentication { get; set; }
    }
}
