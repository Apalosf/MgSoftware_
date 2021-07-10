using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.DataAccess
{
    public abstract class Repository
    {
        /// <summary>
        /// Esta clase abstracta es responsable de establecer la cadena de conexion
        /// y obtener la conexion a SQL.
        /// </summary>
        private readonly string connectionString;//Obtiene o establece la cadena de conexión.

        public Repository()
        {
            connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MgData;Integrated Security=True";
        }
        protected SqlConnection GetConnection()
        {//Este métedo se encarga de establecer y devolver el objeto de conexión a SQL Server.
            return new SqlConnection(connectionString);
        }
    }
}
