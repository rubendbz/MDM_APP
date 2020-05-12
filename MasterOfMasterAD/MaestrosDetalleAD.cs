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
    public class MaestrosDetalleAD
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private string obtenerConexion()
        {
            return ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }

        /// <summary>
        /// Autor: RUBEN BALLESTEROS,RB
        /// Fecha: 16/08/2018
        /// Descripcion: Codificacion del Metodo que permite  Insertar Maestros Detalle, el cual se realiza por medio del servicio WCF a la tabla Maestros Detalle de la Base de Datos.
        /// </summary>
        /// <param name="pMD"> pMD son las variables que recibe el metodo como parametros para almacenar los datos que se van a Ingresar</param>
        /// <returns>El Metodo va a retornar la variable de tipo "String">(mensaje) el cual indica si Fue exitoso o no el ingreso de datos, en caso de no serlo mostrara el tipo de error y su codigo.
        public String InsertarMaestroDetalle(cMaestrosDetalle pMaestroDetalle)
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
                    cmd.CommandText = "[Maestro].[SPInsertUpdateMaestrosDetalles]";
                    cmd.Parameters.Add("@pDescripcionAntigua", SqlDbType.VarChar, 50).Value = null;
                    cmd.Parameters.Add("@pIdMaestroCabecera", SqlDbType.BigInt).Value = pMaestroDetalle.idMaestroCabecera;
                    cmd.Parameters.Add("@pDescripcion", SqlDbType.VarChar, 50).Value = pMaestroDetalle.descripcion;
                    cmd.Parameters.Add("@pUsuarioCreador", SqlDbType.BigInt).Value = pMaestroDetalle.idUsuarioCreador;
                    cmd.Parameters.Add("@pUsuarioActualiza", SqlDbType.BigInt).Value = pMaestroDetalle.idUsuarioCreador;
                    cmd.Parameters.Add("@pIndActivo", SqlDbType.Bit).Value = pMaestroDetalle.indActivo;
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
        /// Autor: Simon Vera
        /// Fecha: 16/08/2018
        /// Descripcion: Codificacion del Metodo de Actualizacion Maestros Detalle, el cual realiza la actualizacion
        /// a la tabla Maestros Detalle de la Base de Datos.
        /// </summary>
        /// <param name="pMaestroDetalle"> pMD son las variables que recibe el metodo como parametros para almacenar los datos que se van a actualizar</param>
        /// <returns>El Metodo va a retornar la variable de tipo "String">(mensaje) la cual nos indica si la Modificacion de los campos fue realizada o no.</returns>
        public string ActualizarMaestroDetalle(cMaestrosDetalle pMaestroDetalle, String pDescripcionAntigua)
        {
            string mensaje = string.Empty;
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
                    cmd.CommandText = "[Maestro].[SPInsertUpdateMaestrosDetalles]";
                    cmd.Parameters.Add("@pIdMaestroCabecera", SqlDbType.BigInt).Value = pMaestroDetalle.idMaestroCabecera;
                    cmd.Parameters.Add("@pDescripcionAntigua", SqlDbType.VarChar, 50).Value = pDescripcionAntigua;
                    cmd.Parameters.Add("@pDescripcion", SqlDbType.VarChar, 50).Value = pMaestroDetalle.descripcion;
                    cmd.Parameters.Add("@pUsuarioActualiza", SqlDbType.BigInt).Value = pMaestroDetalle.idUsuarioActualiza;
                    cmd.Parameters.Add("@pIndActivo", SqlDbType.Bit).Value = pMaestroDetalle.indActivo;
                    cmd.Parameters.Add("@pMensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
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

        // @Nombre : Pedro Marval
        // @Versión " 2
        // @Fecha de Creación del Archivo:  10/09/2018
        // @Descripción: Método que permite obtener la información de los MAESTROS DETALLES
        // @Parámetros : 
        //  pIdMDetalle : Se usa para buscar la información de un Maestro Detalle en especifico 
        //  pIdMCabecera: Se usa para buscar la información de todos los Maestros Detalles que pertenecen a un Maestro Cabecera 
        //  pDescripcion: Se Usa para buscar la información de los Maestro Detalle por su Descripcion
        //  pIndActivo  : Se usa para identificar los Maestros Detalles que están habilitados o no. si se pasa null como parámetros retorna sin importar el estatus(Indicador activo)
        //  pFechaInicial , pFechaFinal: rango de fechas para buscar los maestros creados en un tiempo estimado 
        // @Notas: Este método retorna una Lista de Maestros Detalles dependiendo de los parámetros que se le pasen.
        //         Si no se envían parámetros retornara todos los Maestros detalles 

        public List<cMaestrosDetalle> ConsultarMaestroDetalle(Int64 pIdMDetalle , Int64 pIdMCabecera , string pDescripcion , bool? pIndActivo , string pFechaInicial , string pFechaFinal)
        {
            string mensaje = string.Empty;
            List<cMaestrosDetalle> Lista = new List<cMaestrosDetalle>();
            string cs = obtenerConexion();
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = cs;
            try
            {
                using (cn)
                {
                    SqlCommand cmd = new SqlCommand();
                    SqlDataReader dr;
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "[Maestro].[spConsultarMDetalles]";        
                    cmd.Parameters.Add("@pIdMDetalle", SqlDbType.BigInt).Value = pIdMDetalle;                                     
                    cmd.Parameters.Add("@pIdMCabecera", SqlDbType.BigInt).Value = pIdMCabecera;                                        
                    cmd.Parameters.Add("@pDescripcion", SqlDbType.VarChar, 50).Value = pDescripcion;                   
                    cmd.Parameters.Add("@pIndActivo", SqlDbType.SmallInt).Value = pIndActivo;                                     
                    cmd.Parameters.Add("@pfechaInicial", SqlDbType.Date).Value =  Convert.ToDateTime(pFechaInicial);
                    cmd.Parameters.Add("@pfechaFinal", SqlDbType.Date).Value = Convert.ToDateTime(pFechaFinal);       
                    try
                    {
                        cn.Open();
                        dr = cmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                cMaestrosDetalle dato = new cMaestrosDetalle();
                                dato.idMaestroDetalle = dr.GetInt64(0);
                                dato.idMaestroCabecera = dr.GetInt64(1);
                                dato.descripcion = dr.GetString(2);
                                dato.idUsuarioCreador = dr.GetInt64(3);
                                dato.idUsuarioActualiza = dr.GetInt64(4);
                                dato.indActivo = dr.GetBoolean(5);
                                dato.fechaCreacion = dr.GetDateTime(6);
                                dato.fechaOcurrencia = dr.GetDateTime(7);
                                Lista.Add(dato);
                            } //end while

                        }//end if
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
            return Lista;
        }
    }
}
