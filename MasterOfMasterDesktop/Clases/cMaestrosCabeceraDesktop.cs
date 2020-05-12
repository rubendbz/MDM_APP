using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterOfMasterDesktop.Clases
{
    /// <summary>
    /// @Autor: Jessy Aguilera
    /// @Versión: 1
    /// @Fecha: 10/09/2018
    /// @Descripción: clase para usar los datos de los maestros detalles en la aplicación de escritorio
    /// </summary>
    public class cMaestrosCabeceraDesktop
    {
        private static readonly log4net.ILog Log
       = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Int64 idMaestroCabecera { get; set; }
        public String descripcion { get; set; }
        public Int64 idUsuarioCreador { get; set; }
        public Int64 idUsuarioActualiza { get; set; }
        public Boolean indActivo { get; set; }
        public String fechaCreacion { get; set; }
        public String fechaOcurrencia { get; set; }

        public List<cMaestrosCabeceraDesktop> CargarLista()
        {
            List<cMaestrosCabeceraDesktop> result = new List<cMaestrosCabeceraDesktop>();
            try
            {
                svcMOM.MasterOfMasterSvcClient svc = new svcMOM.MasterOfMasterSvcClient();
                var lista = svc.ConsultarMaestroCabecera(-1, "", null, "0001-01-01", "9999-12-31");
                foreach (MasterOfMasterModels.cMaestrosCabecera MC in lista)
                {
                    cMaestrosCabeceraDesktop datos = new cMaestrosCabeceraDesktop();
                    datos.descripcion = MC.descripcion;
                    datos.idMaestroCabecera = MC.idMaestroCabecera;
                    datos.idUsuarioActualiza = MC.idUsuarioActualiza;
                    datos.idUsuarioCreador = MC.idUsuarioCreador;
                    datos.indActivo = MC.indActivo;
                    datos.fechaCreacion = MC.fechaCreacion.ToShortDateString();
                    datos.fechaOcurrencia = MC.fechaOcurrencia.ToShortDateString();
                    result.Add(datos);
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                Log.Error(Ex.Message);
            }
            return result;
        }
    }
}
