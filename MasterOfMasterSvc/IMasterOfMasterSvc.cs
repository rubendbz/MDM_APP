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
    [ServiceContract]
    public interface IMasterOfMasterSvc
    {
        //Metodos del servicio Maestros Detalles
        [OperationContract]
        String InsertarMaestroDetalle(cMaestrosDetalle pMaestroDetalle);

        [OperationContract]
        string ActualizarMaestroDetalle(cMaestrosDetalle pMaestroDetalle, String pDescripcionAntigua);

        [OperationContract]
        List<cMaestrosDetalle> ConsultarMaestroDetalle(Int64 pIdMDetalle, Int64 pIdMCabecera, string pDescripcion, bool? pIndActivo, string pFechaInicial, string pFechaFinal);



        //metodos del servicio de Maestros Cabecera
        [OperationContract]
        String InsertarMaestroCabecera(cMaestrosCabecera pMaestroCabecera);

        [OperationContract]
        String ActualizarMaestroCabecera(cMaestrosCabecera pMaestroCabecera, String pDescripcionAntigua);

        [OperationContract]
        List<cMaestrosCabecera> ConsultarMaestroCabecera(Int64 pIdMCabecera, string pDescripcion, bool? pIndActivo, string pFechaInicial, string pFechaFinal);


        //metodos del servicio de Maestros Avanzados
        [OperationContract]
        string CrearTabla(Tabla MA);

        //Pedro MArval
        // 24-09-2018
        //Metodo para retornar el nombre de las tablas(Maestros Avanzados)
        [OperationContract]
        List<Tabla> ListarMAestrosAvanzados();

        //Pedro MArval
        // 24-09-2018
        //Metodo para retornar el nombre de las columnas y tipo de datos de una tabla(Maestros Avanzados)
        [OperationContract]
        List<Tabla> buscarMAvanzado(string pNombreTabla);

        [OperationContract]
        string buscarClavePrimaria(string pNombreTabla); 
    }
}
