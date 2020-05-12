using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using MasterOfMasterModels;
using log4net;
using MasterOfMasterDesktop.UI;

namespace WebAppMasterOfMaster.Controllers
{
    public class MAController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        SvcMom.MasterOfMasterSvcClient svc = new SvcMom.MasterOfMasterSvcClient();
        // GET: MA
        public ActionResult CreateMaster()
        {
            return View();
        }

   
        [HttpPost]
        public String NuevoMaestroAvanzado(string pAccion, string pNombreTabla, string pAtributo)
        {
            string mensaje;
            try
            {
                if (pAccion == "insertar")
                {
                    var listaMA = svc.ListarMAestrosAvanzados();
                    foreach (var item in listaMA)
                    {
                        if (item.NombreTabla == pNombreTabla)
                        {
                            return "Ya existe un maestro avanzado con este nombre";
                        }
                    }
                }
                
                Tabla maestro = new Tabla();
                maestro.NombreTabla = pNombreTabla;
                maestro.Atributo = pAtributo;
                mensaje = svc.CrearTabla(maestro);
                //return (svc.CrearTabla(maestro));
                Log.Info(mensaje);
            }
            catch (Exception Ex)
            {
             mensaje = "Error... " + Ex.Message;
                Log.Error(mensaje);
            }
            if (pAccion == "modificar" && mensaje == "Operación exitosa") {
                return "Modificación exitosa";
            }
            return mensaje;
        }
        public JsonResult MaestrosAvanzados()
        {
            string mensaje;

            Tabla[] mc = null;

            try
            {
                mc = svc.ListarMAestrosAvanzados();
            }
            catch (Exception Ex)
            {
                mensaje = "Error... " + Ex.Message;
                Log.Error(mensaje);
            }
            return Json(mc, JsonRequestBehavior.AllowGet);
        }


        [Route("BuscarMaestroAvanzado/{NombreTabla}")]
        public JsonResult BuscarMaestroAvanzado(string NombreTabla)
        {
            string mensaje;
            var nomPk= "";

            var lista = new List<data>();
            nomPk = svc.buscarClavePrimaria(NombreTabla);
        
            try
            {

                foreach (var item in svc.buscarMAvanzado(NombreTabla))
                {
                    data objeto = new data();
                    objeto.NomColumn = item.Atributo;
                    objeto.TypeData = /*item.TipoDeDato == "varchar" ? item.TipoDeDato + "(" + item.Longitud + ")" :*/ item.TipoDeDato;
                    objeto.Longitud = item.Longitud;


                    if (item.TipoDeDato.Contains("varchar") && objeto.Longitud == "-1")
                    {
                        objeto.TypeData = "varchar(MAX)";
                    }
                    if (item.TipoDeDato == "binary" && objeto.Longitud == "50")
                    {
                        objeto.TypeData = "binary(50)";
                    }
                    if (item.TipoDeDato == "char" && objeto.Longitud == "10")
                    {
                        objeto.TypeData = "char(10)";
                    }
                    if (item.TipoDeDato == "datetime2" && objeto.Longitud == "null")
                    {
                        objeto.TypeData = "datetime2(7)";
                    }
                    if (item.TipoDeDato == "datetimeoffset" && objeto.Longitud == "null")
                    {
                        objeto.TypeData = "datetimeoffset(7)";
                    }
                    if (item.TipoDeDato == "decimal" && objeto.Longitud == "null")
                    {
                        objeto.TypeData = "decimal(18, 0)";
                    }
                    if (item.TipoDeDato == "nchar" && objeto.Longitud == "20")
                    {
                        objeto.TypeData = "nchar(10)";
                    }
                    if (item.TipoDeDato == "numeric" && objeto.Longitud == "null")
                    {
                        objeto.TypeData = "numeric(18,0)";
                    }
                    if (item.TipoDeDato == "nvarchar" && objeto.Longitud == "100")
                    {
                        objeto.TypeData = "nvarchar(50)";
                    }
                    if (item.TipoDeDato == "nvarchar" && objeto.Longitud == "-1")
                    {
                        objeto.TypeData = "nvarchar(MAX)";
                    }
                    if (item.TipoDeDato == "time" && objeto.Longitud == "null")
                    {
                        objeto.TypeData = "time(7)";
                    }
                    if (item.TipoDeDato == "varbinary" && objeto.Longitud == "50")
                    {
                        objeto.TypeData = "varbinary(50)";
                    }
                    if (item.TipoDeDato == "varbinary" && objeto.Longitud == "-1")
                    {
                        objeto.TypeData = "varbinary(MAX)";
                    }
                    if (nomPk == item.Atributo)
                    {
                        objeto.PK = true;
                    }

                    if (item.pNulo == "YES")
                    {
                        objeto.PN = true;
                    }
                    lista.Add(objeto);
                }


            }
            catch (Exception Ex)
            {
                mensaje = "Error... " + Ex.Message;
                Log.Error(mensaje);
            }
            return Json(new { nomPk = nomPk, datos = lista },JsonRequestBehavior.AllowGet);
            
        }
    }
}
