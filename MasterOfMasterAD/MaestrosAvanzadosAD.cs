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
using System.Windows;
using log4net;
using log4net.Config;

namespace MasterOfMasterAD
{
    /// <summary>
    /// @Autor: Simon Vera
    /// @Fecha Creacion: 10/09/2018 
    /// @Fecha Actualizacion: 19/09/2018
    /// @Descripcion:Acceso a Datos de Maestros Avanzados, el cual recibe un XML de la clase "SerializacionXml"
    ///   para enviarsela al Sp que crea la tabla.
    /// </summary>
    public class MaestrosAvanzadosAD
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string NombreTabla;
        private string obtenerConexion()
        {
            return ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
        }


        public String CrearTabla(Tabla MA)
        {
            SerializacionXml Xml = new SerializacionXml();
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
                    cmd.CommandText = "[Maestro].[spMaestrosAvanzados]";
                    cmd.Parameters.Add("@XmlDocument", SqlDbType.Xml).Value = Xml.serializarXML(MA);
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
            catch (Exception Ex)
            {
                mensaje = "Error... " + Ex.Message;
                Log.Error(mensaje);
            }
            return mensaje;

        }


        public List<Tabla> ListarMAestrosAvanzados()
        {
            string cs = obtenerConexion();
            SqlConnection cn = new SqlConnection();
            List<Tabla> Lista = new List<Tabla>();
            cn.ConnectionString = cs;

            try
            {
                using (cn)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    SqlDataReader dr;
                    cmd.CommandText = "SELECT TABLE_NAME FROM Information_Schema.TABLES WHERE TABLE_SCHEMA = 'MAvanzado' ORDER BY TABLE_NAME ASC";
                    cn.Open();
                    dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            Tabla dato = new Tabla();
                            dato.NombreTabla = dr.GetString(0);

                            Lista.Add(dato);
                        } //end while

                    }//end if
                    dr.Close();
                    cn.Close();
                }

            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }

            return Lista;

        }


        public List<Tabla> buscarMAvanzado(string pNombre)
        {
            string cs = obtenerConexion();
            SqlConnection cn = new SqlConnection();
            List<Tabla> Lista = new List<Tabla>();
            cn.ConnectionString = cs;

            try
            {
                using (cn)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    SqlDataReader dr;
                    cmd.CommandText = "select COLUMN_NAME , DATA_TYPE , IS_NULLABLE , CHARACTER_OCTET_LENGTH from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = '" + pNombre + "'";
                    cn.Open();
                    dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            Tabla dato = new Tabla();
                            dato.Atributo = dr.GetString(0);
                            dato.TipoDeDato = dr.GetString(1);
                            dato.pNulo = dr.GetString(2);
                            if (!dr.IsDBNull(3) )
                            {
                                dato.Longitud = dr.GetInt32(3).ToString();
                            }
                            else
                            {
                                dato.Longitud = "null";
                            }
                         
                            
                            Lista.Add(dato);
                        } //end while

                    }//end if
                    dr.Close();
                    cn.Close();
                }

            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }

            return Lista;

        }

        public string buscarClavePrimaria(string pNombreTabla)
        {
            string cs = obtenerConexion();
            SqlConnection cn = new SqlConnection();
            string clave = null;
            cn.ConnectionString = cs;

            try
            {
                using (cn)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    SqlDataReader dr;
                    cmd.CommandText = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + QUOTENAME(CONSTRAINT_NAME)), 'IsPrimaryKey') = 1 AND TABLE_NAME = '" + pNombreTabla + "' ";
                    cn.Open();
                    dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            clave = dr.GetString(0);
                        } //end while

                    }//end if
                    dr.Close();
                    cn.Close();
                }

            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }

            return clave;
        }

    }

}


