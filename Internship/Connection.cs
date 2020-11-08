using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internship
{
    class Connection
    {
        public SqlConnection GetConnection() {

            return new SqlConnection() { ConnectionString = "server=LAPTOP-RD66DFN1;database=dropdown;Integrated Security =true" };
        }
    }
}
