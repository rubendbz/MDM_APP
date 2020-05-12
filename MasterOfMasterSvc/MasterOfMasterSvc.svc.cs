using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using MasterOfMasterModels;
using MasterOfMasterAD;

namespace MasterOfMasterSvc
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "MasterOfMasterSvc" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione MasterOfMasterSvc.svc o MasterOfMasterSvc.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class MasterOfMasterSvc : IMasterOfMasterSvc
    {
        MaestrosAvanzadosAD MAA = new MaestrosAvanzadosAD();
        MaestrosCabeceraAD MAC = new MaestrosCabeceraAD();
        MaestrosDetalleAD MAD = new MaestrosDetalleAD();


        public String InsertarMaestroDetalle(cMaestrosDetalle pMaestroDetalle)
        {
            return MAD.InsertarMaestroDetalle(pMaestroDetalle);
        }

        public string ActualizarMaestroDetalle(cMaestrosDetalle pMaestroDetalle, String pDescripcionAntigua)
        {
            return MAD.ActualizarMaestroDetalle(pMaestroDetalle, pDescripcionAntigua);
        }
        /// <summary>
        /// @Autor: Simon Vera
        /// @Fecha: 07/09/2018
        /// @Descripcion:Modificacion
        /// Se declararon los parametros que van a servir para filtrar la busqueda de Maestros por (Descripcion, Estatus, Fecha)
        /// </summary>
        /// <param name="pIdMDetalle"></param>
        /// <param name="pIdMCabecera"></param>
        /// <param name="pDescripcion"></param>
        /// <param name="pIndActivo"></param>
        /// <param name="pFechaInicial"></param>
        /// <param name="pFechaFinal"></param>
        /// <returns></returns>
        public List<cMaestrosDetalle> ConsultarMaestroDetalle(Int64 pIdMDetalle, Int64 pIdMCabecera, string pDescripcion, bool? pIndActivo, string pFechaInicial, string pFechaFinal)
        {
            return MAD.ConsultarMaestroDetalle(pIdMDetalle, pIdMCabecera, pDescripcion, pIndActivo, pFechaInicial, pFechaFinal);
        }

        public String InsertarMaestroCabecera(cMaestrosCabecera pMaestroCabecera)
        {
            return MAC.InsertarMaestroCabecera(pMaestroCabecera);
        }

        public String ActualizarMaestroCabecera(cMaestrosCabecera pMaestroCabecera, String pDescripcionAntigua)
        {
            return MAC.ActualizarMaestroCabecera(pMaestroCabecera, pDescripcionAntigua);
        }

        /// <summary>
        /// @Autor: Simon Vera
        /// @Fecha: 07/09/2018
        /// @Descripcion:Consultar
        /// Se declararon los parametros que van a servir para filtrar la busqueda de Maestros por (Descripcion, Estatus, Fecha)
        /// </summary>
        /// <param name="pIdMCabecera"></param>
        /// <param name="pDescripcion"></param>
        /// <param name="pIndActivo"></param>
        /// <param name="pFechaInicial"></param>
        /// <param name="pFechaFinal"></param>
        /// <returns></returns>
        public List<cMaestrosCabecera> ConsultarMaestroCabecera(Int64 pIdMCabecera, string pDescripcion, bool? pIndActivo, string pFechaInicial, string pFechaFinal)
        {
            return MAC.ConsultarMaestroCabecera(pIdMCabecera,pDescripcion,pIndActivo,pFechaInicial,pFechaFinal);
        }

        public String CrearTabla(Tabla MA)
        {
            return MAA.CrearTabla(MA);
        }

        public List<Tabla> ListarMAestrosAvanzados()
        {

            return MAA.ListarMAestrosAvanzados();

        }

        public List<Tabla> buscarMAvanzado(string pNombreTabla)
        {
            return MAA.buscarMAvanzado(pNombreTabla);
        }

        public string buscarClavePrimaria(string pNombreTabla)
        {
            return MAA.buscarClavePrimaria(pNombreTabla);
        }

    }
}
