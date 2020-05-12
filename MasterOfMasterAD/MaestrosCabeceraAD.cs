using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using MasterOfMasterModels;
using log4net;

namespace MasterOfMasterAD
{
    public class MaestrosCabeceraAD
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string obtenerConexion()
        {
            return ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        /// <summary>
        /// Autor: JEINE TALAVERA,JT
        /// Fecha: 20/08/2018
        /// Descripcion: Codificacion del Metodo que permite  Insertar Maestros Cabecera, el cual se realiza por medio del servicio WCF a la tabla MaestrosCabecera de la Base de Datos.
        /// </summary>
        /// <param name="pMaestroCabecera"> pMCI son las variables que recibe el metodo como parametros para almacenar los datos que se van a Ingresar</param>
        /// <returns>El Metodo va a retornar la variable de tipo "String">(mensaje) el cual indica si Fue exitoso o no el ingreso de datos, en caso de no serlo mostrara el tipo de error y su codigo.
        public String InsertarMaestroCabecera(cMaestrosCabecera pMaestroCabecera)
        {
            string mensaje = String.Empty;
            string cs = obtenerConexion();
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = cs;
            try
            {
                using (cn)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[Maestro].[spUpdateMaestrosCabecera]";
                    cmd.Parameters.Add("@pDescripcion", SqlDbType.VarChar, 50).Value = pMaestroCabecera.descripcion;
                    cmd.Parameters.Add("@pUsuarioCreador", SqlDbType.BigInt).Value = pMaestroCabecera.idUsuarioCreador;
                    cmd.Parameters.Add("@pUsuarioActualiza", SqlDbType.BigInt).Value = pMaestroCabecera.idUsuarioCreador;
                    cmd.Parameters.Add("@pIndActivo", SqlDbType.Bit).Value = pMaestroCabecera.indActivo;
                    cmd.Parameters.Add("@pMensaje", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    try
                    {
                        cn.Open();
                        cmd.ExecuteNonQuery();
                        mensaje = Convert.ToString(cmd.Parameters["@pMensaje"].Value);
                        cn.Close();
                        Log.Info(mensaje);
                    }
                    catch (Exception Ex)
                    {
                        mensaje = "Error... " + Ex.Message;
                        Log.Error(mensaje);
                    }
                }
            }
            catch (SqlException Ex)
            {
                mensaje = "Error... " + Ex.Message;
                Log.Error(mensaje);
            }
            return mensaje;
        }

        /// <summary>
        /// Autor: Jose Cabrera
        /// Fecha: 16/08/2018
        /// Descripcion: Codificacion del Metodo de Actualizacion Maestros Cabecera, el cual realiza la actualizacion
        /// a la tabla Maestros Cabecera de la Base de Datos.
        /// </summary>
        /// <param name="pMaestroCabecera"> master son las variables que recibe el metodo como parametros para almacenar los datos que se van a actualizar
        /// </param>
        ///   /// <param name="pDescripcionAntigua"> pDescripcionAntigua variable que recibe el metodo como parametros para buscar una Descripcion a Actualizar, que ya Exista en la BD
        /// </param>
        /// <returns>El Metodo va a retornar la variable de tipo "String">(mensaje) la cual nos indica si la Modificacion de los campos fue realizada o no.</returns>
        public String ActualizarMaestroCabecera(cMaestrosCabecera pMaestroCabecera, String pDescripcionAntigua)
        {
            string mensaje = String.Empty;
            string cs = obtenerConexion();
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = cs;
            try
            {
                using (cn)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[Maestro].[spUpdateMaestrosCabecera]";
                    cmd.Parameters.Add("@pDescripcionAntigua", SqlDbType.VarChar, 50).Value = pDescripcionAntigua;
                    cmd.Parameters.Add("@pDescripcion", SqlDbType.VarChar, 50).Value = pMaestroCabecera.descripcion;
                    cmd.Parameters.Add("@pUsuarioActualiza", SqlDbType.BigInt).Value = pMaestroCabecera.idUsuarioActualiza;
                    cmd.Parameters.Add("@pIndActivo", SqlDbType.Bit).Value = pMaestroCabecera.indActivo;
                    cmd.Parameters.Add("@pMensaje", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    try
                    {
                        cn.Open();
                        cmd.ExecuteNonQuery();
                        mensaje = Convert.ToString(cmd.Parameters["@pMensaje"].Value);
                        cn.Close();
                        Log.Info(mensaje);
                    }
                    catch (Exception Ex)
                    {
                        mensaje = "Error... " + Ex.Message;
                        Log.Error(mensaje);
                    }
                }
            }
            catch (SqlException Ex)
            {
                mensaje = "Error... " + Ex.Message;
                Log.Error(mensaje);
            }
            return mensaje;
        }

        //@Autor Enderson Brito
        //@Fecha  20/08/2018
        //@Descripcion El siguiente metodo se encarga de realizar una consulta  a la tabla MaestrosCabeceras de la BD mediante el procedimiento de almacenado spConsultarMaestrosCabecera y me guarda los datos leidos en una lista 
        //@Parámetros  no recibe ningun parametro
        //@ValorRetornado Nos  devuelve una Lista con los datos de la tabla MaestrosCabeceras

        /// <summary>
        /// @Autor: Simon Vera
        /// @Fecha: 07/09/2018
        /// @Descripcion:Consultar
        /// Se le agregaron los parametros que se van a utilizar para realizar la busqueda 
        /// de los maestros a traves de los filtros (Descripcion, Estatus, Fecha).
        /// </summary>
        /// <param name="pIdMCabecera"></param>
        /// <param name="pDescripcion"></param>
        /// <param name="pIndActivo"></param>
        /// <param name="pFechaInicial"></param>
        /// <param name="pFechaFinal"></param>
        /// <returns>Busqueda de Maestros a traves de un filtro de busqueda </returns>
        public List<cMaestrosCabecera> ConsultarMaestroCabecera(Int64 pIdMCabecera, string pDescripcion, bool? pIndActivo, string pFechaInicial, string pFechaFinal)
        {

            List<cMaestrosCabecera> lista = new List<cMaestrosCabecera>();
            string cs = obtenerConexion();
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = cs;
            string mensaje = String.Empty;
            try
            {
                using (cn)
                {
                    SqlCommand cmd = new SqlCommand();
                    SqlDataReader dr;
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[Maestro].[spConsultarMCabeceras]";
                    cmd.Parameters.Add("@pIdMCabecera", SqlDbType.BigInt).Value = pIdMCabecera;
                    cmd.Parameters.Add("@pDescripcion", SqlDbType.VarChar, 50).Value = pDescripcion;
                    cmd.Parameters.Add("@pIndActivo", SqlDbType.SmallInt).Value = pIndActivo;
                    cmd.Parameters.Add("@pfechaInicial", SqlDbType.Date).Value = Convert.ToDateTime(pFechaInicial);
                    cmd.Parameters.Add("@pfechaFinal", SqlDbType.Date).Value = Convert.ToDateTime(pFechaFinal);
                    try
                    {
                        cn.Open();
                        dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            cMaestrosCabecera dato = new cMaestrosCabecera();
                            dato.idMaestroCabecera = dr.GetInt64(0);
                            dato.descripcion = dr.GetString(1);
                            dato.idUsuarioCreador = dr.GetInt64(2);
                            dato.idUsuarioActualiza = dr.GetInt64(3);
                            dato.indActivo = dr.GetBoolean(4);
                            dato.fechaCreacion = dr.GetDateTime(5);
                            dato.fechaOcurrencia = dr.GetDateTime(6);
                            lista.Add(dato);
                        }
                        dr.Close();
                        cn.Close();
                    }
                    catch (Exception Ex)
                    {
                        mensaje = "Error... " + Ex.Message;
                        Log.Error(mensaje);
                    }
                }
            }
            catch (SqlException Ex)
            {
                mensaje = "Error... " + Ex.Message;
                Log.Error(mensaje);
            }
            return lista;
        }
    }
}
