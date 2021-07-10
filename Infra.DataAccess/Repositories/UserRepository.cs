using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infra.DataAccess.Contracts;
using Infra.DataAccess.Entities;
using Infra.Common;
using System.Data.SqlClient;
using System.Data;

namespace Infra.DataAccess.Repositories
{
    public class UserRepository : MasterRepository, IUserRepository
    {
        /// <summary>
        /// Esta clase hereda de clase MasterRepository e implementa la interfaz IUserRepository.
        /// Aquí se realiza las diferentes transacciones y consultas a la base de datos de la entidad
        /// usuario.
        /// </summary>
        ///       
        public User Login(string username, string password)
        {//Ejemplo de una consulta con varios parámetros mediante un procedimiento almacenado:
         //Validar el usuario y contraseña del usuario para el inicio de sesion.
            string commandText = "loginUser";//Establecer el comando de texto (Transact-SQL o procedimiento almacenado).
            CommandType commandType = CommandType.StoredProcedure;//Establecer el tipo de comando.
            var parameters = new List<SqlParameter>();//Crear una lista genérica de para los parámetros de la consulta.
            parameters.Add(new SqlParameter("@Users_UserName", username));//Crear y agregar el parámetro usuario (Nombre de parámetro y valor).
            parameters.Add(new SqlParameter("@Users_Password", password));//Crear y agregar el parámetro contraseña (Nombre de parámetro y valor).
            var table = ExecuteReader(commandText, parameters, commandType);//Ejecutar el método lector de la clase base MasterRepository y enviar los parámetros necesarios.
            if (table.Rows.Count > 0)//Si la consulta fue exitosa (usuario y contraseña válidos).
            {
                var user = new User();//Crear objeto-entidad usuario.
                foreach (DataRow row in table.Rows)//Recorrer las filas de la tabla y asignar los valores respectivos del objeto usuario.
                {
                    user.Users_Id = (int)(row[0]);//Celda posición [0].
                    user.Users_UserName = row[1].ToString();
                    user.Users_Password = row[2].ToString();
                    user.Users_FirsName = row[3].ToString();
                    user.Users_LastName = row[4].ToString();
                    user.Users_Position = row[5].ToString();
                    user.Users_Email = row[6].ToString();
                    if (row[7]!= DBNull.Value) user.Users_Photo = (byte[])row[7];//Establecer el valor si el valor de celda es diferente a nulo.
                }
                return user;//Retornar objeto usuario.
            }
            else //Si la consulta no fue exitosa - retornar un objeto nulo.
                return null;
        }
        public int Add(User entity)
        {//Ejemplo de una transacción con varios parámetros usando un procedimiento almacenado:
         //Agregar un nuevo usuario.
            var parameters = new List<SqlParameter>();//Crear una lista para los parámetros de la transacción.
            parameters.Add(new SqlParameter("@Users_UserName", entity.Users_UserName));
            parameters.Add(new SqlParameter("@Users_Password", entity.Users_Password));
            parameters.Add(new SqlParameter("@Users_FirsName", entity.Users_FirsName));
            parameters.Add(new SqlParameter("@Users_LastName", entity.Users_LastName));
            parameters.Add(new SqlParameter("@Users_Position", entity.Users_Position));
            parameters.Add(new SqlParameter("@Users_Email", entity.Users_Email));
            if (entity.Users_Photo != null)//Si la propiedad Foto es diferente a nulo, asignar el valor de la propiedad.
                parameters.Add(new SqlParameter("@Users_Photo", entity.Users_Photo) { SqlDbType = SqlDbType.VarBinary });//En este caso del campo Foto, es importante especificar explícitamente el tipo de dato de SQL.
            else //Caso contrario asignar un valor nulo de SQL.                                              //Puedes hacer lo mismo con los otros parámetros, sin embargo es opcional,  
                parameters.Add(new SqlParameter("@Users_Photo", DBNull.Value) { SqlDbType = SqlDbType.VarBinary });//El tipo de datos será derivará del tipo de dato de su valor.

            //Ejecutar el método ExecuteNonQuery de la clase MasterRepository para realizar una transacción de insertar,
            //y enviar los parámetros necesarios (Comando de texto, parámetros y tipo de comando).
            return ExecuteNonQuery("AddUser", parameters, CommandType.StoredProcedure);
        }
        public int Edit(User entity)
        {//Ejemplo de una transacción con varios parámetros usando un procedimiento almacenado:
         //Editar usuario.

            var parameters = new List<SqlParameter>();//Crear una lista para los parámetros de la transacción.
            parameters.Add(new SqlParameter("@Users_Id", entity.Users_Id));
            parameters.Add(new SqlParameter("@Users_UserName", entity.Users_UserName));
            parameters.Add(new SqlParameter("@Users_Password", entity.Users_Password));
            parameters.Add(new SqlParameter("@Users_FirsName", entity.Users_FirsName));
            parameters.Add(new SqlParameter("@Users_LastName", entity.Users_LastName));
            parameters.Add(new SqlParameter("@Users_Position", entity.Users_Position));
            parameters.Add(new SqlParameter("@Users_Email", entity.Users_Email));
            if (entity.Users_Photo != null)
                parameters.Add(new SqlParameter("@Users_Photo", entity.Users_Photo) { SqlDbType = SqlDbType.VarBinary });
            else parameters.Add(new SqlParameter("@Users_Photo", DBNull.Value) { SqlDbType = SqlDbType.VarBinary });

            //Ejecutar el método ExecuteNonQuery de la clase MasterRepository para realizar una transacción de actualizar,
            //y enviar los parámetros necesarios (Comando de texto, parámetros y tipo de comando).
            return ExecuteNonQuery("EditUser", parameters, CommandType.StoredProcedure);
        }
        public int Remove(User entity)
        {//Ejemplo de una transacción con un solo parámetro usando un comando Transact-SQL:
         //Eliminar usuario.
            string sqlCommand = "delete from DataMg_Users where Users_Id=@Users_Id";//Comando de tipo texto (Transact-SQL)
            return ExecuteNonQuery(sqlCommand, new SqlParameter("@Users_Id", entity.Users_Id), CommandType.Text);
        }
        public int AddRange(List<User> users)
        {//Ejemplo de una transacción masiva usando un procedimiento almacenado:
         //Agregar varios usuarios.
            var transactions = new List<BulkTransaction>();//Crear una lista genérica para las transacciones.
            foreach (var user in users)//Recorrer la lista de usuarios y agregar la instrucciones a la lista de transacciones.
            {
                var trans = new BulkTransaction();//Crear un objeto de transacción.
                var parameters = new List<SqlParameter>();//Crear una lista para los parámetros de la transacción.
                //En este caso de una transacción masiva, es conveniente especificar el tipo de dato del parámetro.
                parameters.Add(new SqlParameter("@Users_UserName", user.Users_UserName) { SqlDbType = SqlDbType.NVarChar });
                parameters.Add(new SqlParameter("@Users_Password", user.Users_Password) { SqlDbType = SqlDbType.NVarChar });
                parameters.Add(new SqlParameter("@Users_FirsName", user.Users_FirsName) { SqlDbType = SqlDbType.NVarChar });
                parameters.Add(new SqlParameter("@Users_LastName", user.Users_LastName) { SqlDbType = SqlDbType.NVarChar });
                parameters.Add(new SqlParameter("@Users_Position", user.Users_Position) { SqlDbType = SqlDbType.NVarChar });
                parameters.Add(new SqlParameter("@Users_Email", user.Users_Email) { SqlDbType = SqlDbType.NVarChar });
                if (user.Users_Photo != null)
                    parameters.Add(new SqlParameter("@Users_Photo", user.Users_Photo) { SqlDbType = SqlDbType.VarBinary });
                else parameters.Add(new SqlParameter("@Users_Photo", DBNull.Value) { SqlDbType = SqlDbType.VarBinary });
                trans.CommandText = "AddUser";//Establecer el comando de texto(En este caso un procedimiento almacenado).
                trans.Parameters = parameters;//Establecer los parametros de la instrucción (Comando de texto).
                transactions.Add(trans);//Agregar la transacción a la lista de transacciones.
            }
            //Puede continuar agregando más transacciones a otras tablas a la lista genérica de transacciones.
            //Finalmente ejecutar todas las instrucciones de la lista de transacciones usando el método
            //BulkExecuteNonQuery de la clase base MasterRepository, enviar los parámetros necesarios
            //(Lista de transacciones y el tipo de comando.)
            return BulkExecuteNonQuery(transactions, CommandType.StoredProcedure);
        }
        public int RemoveRange(List<User> users)
        {//Ejemplo de una transacción masiva usando un comando Transact-SQL:
         //Eliminar varios usuarios.
            var transactions = new List<BulkTransaction>();
            foreach (var user in users)
            {
                var trans = new BulkTransaction();
                trans.CommandText = "delete from DataMg_Users where Users_Id=@Users_Id";
                trans.Parameters = new List<SqlParameter>{new SqlParameter("@Users_Id", user.Users_Id){SqlDbType=SqlDbType.Int}};
                transactions.Add(trans);
            }
            return BulkExecuteNonQuery(transactions, CommandType.Text);
        }
        public User GetSingle(string value)
        {//Ejemplo de una consulta usando un comando Transact-SQL con un parámetro:
         //Obtener un usuario según el valor espeficicado (Buscar).
            string sqlCommand;
            DataTable table;
            int idUser;
            bool isNumeric = int.TryParse(value, out idUser);//Determinar si el parámetro valor es un numero entero.
            if (isNumeric)//Si el valor es un número, realizar la consulta usando el id del usuario.
            {
                sqlCommand = "select *from DataMg_Users where Users_Id= @Users_Id";
                table = ExecuteReader(sqlCommand, new SqlParameter("@Users_Id", idUser), CommandType.Text);               
            }
            else //Caso contrario, realizar la consulta usando el nombre de usuario o correo electrónico.
            {
                sqlCommand = "select *from DataMg_Users where Users_UserName= @findValue or Users_Email=@findValue";
                table = ExecuteReader(sqlCommand, new SqlParameter("@findValue", value), CommandType.Text);
            }            
            if (table.Rows.Count > 0)//Si la consulta tiene resultado
            {
                var user = new User();//Crear un objeto usuario y asignar los valores.
                foreach (DataRow row in table.Rows)
                {
                    user.Users_Id = Convert.ToInt32(row[0]);
                    user.Users_UserName = row[1].ToString();
                    user.Users_Password = row[2].ToString();
                    user.Users_FirsName = row[3].ToString();
                    user.Users_LastName = row[4].ToString();
                    user.Users_Position = row[5].ToString();
                    user.Users_Email = row[6].ToString();
                    if (row[7] != DBNull.Value) user.Users_Photo = (byte[])row[7];                   
                }
                //Opcionalmente desechar la tabla para liberar memoria.
                table.Clear();
                table = null;  
                return user;//Retornar usuario encontrado.
            }
            else//Si la consulta no fué exitosa, retornar nulo.
                return null;
        }
        public IEnumerable<User> GetAll()
        {
            var userList = new List<User>();
            var table = ExecuteReader("SelectAllUsers", CommandType.StoredProcedure);
            foreach (DataRow row in table.Rows)
            {
                var user = new User();
                user.Users_Id = Convert.ToInt32(row[0]);
                user.Users_UserName = row[1].ToString();
                user.Users_Password = row[2].ToString();
                user.Users_FirsName = row[3].ToString();
                user.Users_LastName = row[4].ToString();
                user.Users_Position = row[5].ToString();
                user.Users_Email = row[6].ToString();
                if (row[7] != DBNull.Value) user.Users_Photo = (byte[])row[7];
                userList.Add(user);
            }
            table.Clear();
            table = null;                
            return userList;
        }
        public IEnumerable<User> GetByValue(string value)
        {
            var userList = new List<User>();
            var table = ExecuteReader("SelectUser", new SqlParameter("@findValue", value), CommandType.StoredProcedure);

            foreach (DataRow row in table.Rows)
            {
                var user = new User();
                user.Users_Id = Convert.ToInt32(row[0]);
                user.Users_UserName = row[1].ToString();
                user.Users_Password = row[2].ToString();
                user.Users_FirsName = row[3].ToString();
                user.Users_LastName = row[4].ToString();
                user.Users_Position = row[5].ToString();
                user.Users_Email = row[6].ToString();
                if (row[7] != DBNull.Value) user.Users_Photo = (byte[])row[7];
                userList.Add(user);
            }
            table.Clear();
            table = null;  
            return userList;
        }
    }
}
