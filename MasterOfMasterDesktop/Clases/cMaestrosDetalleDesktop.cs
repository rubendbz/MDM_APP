using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterOfMasterDesktop.Clases
{
    /// <summary>
    /// @Autor: Pedro Marval
    /// @Versión: 1
    /// @Fecha: 10/09/2018
    /// @Descripción: clase para usar los datos de los maestros detalles en la aplicación de escritorio
    /// </summary>
    public class cMaestrosDetalleDesktop
    {
        private static readonly log4net.ILog Log
       = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Int64 idMaestroDetalle { get; set; }
        public Int64 idMaestroCabecera { get; set; }
        public String descripcion { get; set; }
        public Int64 idUsuarioCreador { get; set; }
        public Int64 idUsuarioActualiza { get; set; }
        public Boolean indActivo { get; set; }
        public String fechaCreacion { get; set; }
        public String fechaOcurrencia { get; set; }

        public List<cMaestrosDetalleDesktop> CargarLista(Int64 pId)
        {
            List<cMaestrosDetalleDesktop> result = new List<cMaestrosDetalleDesktop>();
            try
            {
                svcMOM.MasterOfMasterSvcClient svc = new svcMOM.MasterOfMasterSvcClient();
                var lista = svc.ConsultarMaestroDetalle(-1, pId, "", null, "0001-01-01", "9999-12-31");
                foreach (MasterOfMasterModels.cMaestrosDetalle MD in lista)
                {
                    cMaestrosDetalleDesktop datos = new cMaestrosDetalleDesktop();
                    datos.idMaestroDetalle = MD.idMaestroDetalle;
                    datos.descripcion = MD.descripcion;
                    datos.idMaestroCabecera = MD.idMaestroCabecera;
                    datos.idUsuarioActualiza = MD.idUsuarioActualiza;
                    datos.idUsuarioCreador = MD.idUsuarioCreador;
                    datos.indActivo = MD.indActivo;
                    datos.fechaCreacion = MD.fechaCreacion.ToShortDateString();
                    datos.fechaOcurrencia = MD.fechaOcurrencia.ToShortDateString();
                    result.Add(datos);
                }
            }
            catch(Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                Log.Error(Ex.Message);
            }
            return result;
        }
    }
}
