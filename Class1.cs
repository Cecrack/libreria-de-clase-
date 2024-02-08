using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace libreriaSqL_
{

    public class Libros
    {
        string cadenaConexion = "Data Source=LAPTOP-M0P6SDBJ\\SQLEXPRESS;Initial Catalog=carros;Integrated Security=True";

        public string Error;


        public SqlConnection AbrirConexion()
        {
            SqlConnection conexion = new SqlConnection();
            try
            {

                //variable para crear conexion 
                conexion = new SqlConnection(cadenaConexion);
                conexion.Open();
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
            return conexion;
        }

        public bool InsertDatos(int folio, string Cliente, string telefono, string domicilio, DateTime fecha_de_salida, DateTime fecha_de_entrega, string carro, string monto)
        {
            bool Estado = false;
            try
            {
                SqlConnection conn = AbrirConexion();
                string Query = "";
                SqlCommand command;
                if (folio > 0)
                {
                    Query = "UPDATE rentaDCarros SET Cliente = @Cliente, telefono = @telefono, domicilio = @domicilio, fecha_de_salida = @fecha_de_salida, fecha_de_entrega = @fecha_de_entrega, carro = @carro, monto = @monto WHERE folio = @folio";
                    command = new SqlCommand(Query, conn);


                    command.Parameters.AddWithValue("@folio", folio);
                }


                else
                {
                    Query = "INSERT INTO rentaDCarros (Cliente, telefono, domicilio, fecha_de_salida, fecha_de_entrega, carro, monto) VALUES (@Cliente, @telefono, @domicilio, @fecha_de_salida, @fecha_de_entrega, @carro, @monto)";
                    command = new SqlCommand(Query, conn);

                }

                command.Parameters.AddWithValue("@Cliente", Cliente);
                command.Parameters.AddWithValue("@telefono", telefono);
                command.Parameters.AddWithValue("@domicilio", domicilio);
                command.Parameters.AddWithValue("@fecha_de_salida", fecha_de_salida);
                command.Parameters.AddWithValue("@fecha_de_entrega", fecha_de_entrega);
                command.Parameters.AddWithValue("@carro", carro);
                command.Parameters.AddWithValue("@monto", monto);

                command.ExecuteNonQuery();
                conn.Close();
                Estado = true;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
            return Estado;


        }


        public DataTable MostrarDatos()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlConnection conn = AbrirConexion();
                string Query = "SELECT * FROM rentaDCarros";
                SqlCommand command = new SqlCommand(Query, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(dt);
                conn.Close();
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
            return dt;
        }

        public bool EliminarDatos(int folio)
        {
            bool Estado = false;
            try
            {
                SqlConnection conn = AbrirConexion();
                string Query = "DELETE FROM rentaDCarros WHERE folio = @folio";
                SqlCommand command = new SqlCommand(Query, conn);
                command.Parameters.AddWithValue("@folio", folio);
                command.ExecuteNonQuery();
                conn.Close();
                Estado = true;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
            return Estado;
        }

        public bool ConsultarRenta(int folio, out string Cliente, out string telefono, out string domicilio, out DateTime fecha_de_salida, out DateTime fecha_de_entrega, out string carro, out string monto)
        {
            bool Estado = false;
            Cliente = "";
            telefono = "";
            domicilio = "";
            fecha_de_salida = DateTime.MinValue;
            fecha_de_entrega = DateTime.MinValue;
            carro = "";
            monto = "";
            try
            {
                SqlConnection conn = AbrirConexion();
                string Query = "SELECT * FROM rentaDCarros WHERE folio = @folio";
                SqlCommand command = new SqlCommand(Query, conn);
                command.Parameters.AddWithValue("@folio", folio);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Cliente = reader["Cliente"].ToString();
                    telefono = reader["telefono"].ToString();
                    domicilio = reader["domicilio"].ToString();
                    fecha_de_salida = Convert.ToDateTime(reader["fecha_de_salida"]);
                    fecha_de_entrega = Convert.ToDateTime(reader["fecha_de_entrega"]);
                    carro = reader["carro"].ToString();
                    monto = reader["monto"].ToString();
                }
                reader.Close();
                conn.Close();
                Estado = true;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
            return Estado;
        }

        public DataTable FiltrarPorFechas(DateTime fecha_de_salida, DateTime fecha_de_entrega)
        {
            
            SqlConnection conn = AbrirConexion();

            
            string consulta = "SELECT * FROM rentaDcarros WHERE fecha_de_salida BETWEEN @fecha_de_salida AND @fecha_de_entrega";
            SqlCommand comando = new SqlCommand(consulta, conn);

            // Pasar las fechas límites como parámetros a la consulta
            comando.Parameters.AddWithValue("@fecha_de_salida", fecha_de_salida);
            comando.Parameters.AddWithValue("@fecha_de_entrega", fecha_de_entrega);

            // Crear un objeto SqlDataAdapter y asignar el comando
            SqlDataAdapter adaptador = new SqlDataAdapter(comando);
            DataTable tabla = new DataTable();
            adaptador.Fill(tabla);
            conn.Close();
            return tabla;
        }

        public DataTable ObtenerDatos()
        {
           
            SqlConnection conn = AbrirConexion();
            {
                
                SqlDataAdapter adaptador = new SqlDataAdapter("SELECT * FROM rentaDcarros", conn);
                DataTable tablaDatos = new DataTable();

                // Llenar la tabla con los datos del adaptador
                adaptador.Fill(tablaDatos);

            
                return tablaDatos;
            }
        }


        public DataTable FiltrarPorCarro(string carroSeleccionado)
        {
            SqlConnection conn = AbrirConexion();

            // Construir la consulta SQL con la cláusula WHERE para filtrar por carro
            string consulta = $"select *from rentaDCarros "+$"where carro='{carroSeleccionado}'";

            // Crear un objeto SqlCommand y asignar la consulta y la conexión
            SqlCommand comando = new SqlCommand(consulta, conn);

            DataTable tabla = new DataTable();
            tabla.Load(comando.ExecuteReader());
            // Crear un objeto SqlDataAdapter y asignar el comando
            ////SqlDataAdapter adaptador = new SqlDataAdapter(comando);

            ////// Crear un objeto DataTable y llenarlo con los datos del adaptador
            ////
            ////adaptador.Fill(tabla);


            conn.Close();

            
            return tabla;
        }


    }
}
