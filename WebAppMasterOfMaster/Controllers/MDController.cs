using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using MasterOfMasterModels;
using log4net;

namespace WebAppMasterOfMaster.Controllers
{
    public class MDController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Regex reglas = new Regex(@"^[a-zA-ZáéíóúàèìòùÀÈÌÒÙÁÉÍÓÚñÑüÜ][0-9a-zA-ZáéíóúàèìòùÀÈÌÒÙÁÉÍÓÚñÑüÜ_\s]+$");
        SvcMom.MasterOfMasterSvcClient svc = new SvcMom.MasterOfMasterSvcClient();
        // GET: MD
        public ActionResult Index()
        {
            return View();
        }
        [Route("MaestrosDetalles/{pIdMaestroCabecera}")]
        public JsonResult MaestrosDetalles(Int64 pIdMaestroCabecera )
        {
            string mensaje;
            List<cMaestrosDetalle> MaestrosStatusInfo = null;
            try
            {
                //calling and storing web service output into the variable  
                 MaestrosStatusInfo = svc.ConsultarMaestroDetalle(-1,pIdMaestroCabecera,"",null, "0001-01-01", "9999-12-31").ToList();
            }
            catch (Exception Ex)
            {
                mensaje = "Error... " + Ex.Message;
                Log.Error(mensaje);
            }
          
            return Json(MaestrosStatusInfo, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult MaestrosDetallesBusqueda(Int64 pIdMCabecera,string pDescripcion, bool? pIndActivo, string pFechaInicial, string pFechaFinal)
        {

            string mensaje;
            cMaestrosDetalle[] mc = null;
            try
            {
                mc = svc.ConsultarMaestroDetalle(-1, pIdMCabecera, pDescripcion, pIndActivo, pFechaInicial, pFechaFinal);

                mensaje = "Lista de Maestros Detalles retornada exitosamente";
                Log.Info(mensaje);
            }
            catch (Exception Ex)
            {
                mensaje = "Error... " + Ex.Message;
                Log.Error(mensaje);
            }
            //returning json result  
            return Json(mc, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult IngresarMaestrosDetalles(Int64 pIdMaestroCabecera, string Descripcion,Boolean Estado)
        {
            string message;
            string titulo2 = "";
            try
            {
                if (!(reglas.IsMatch(Descripcion) && Descripcion.Length > 3 && Descripcion.Length <= 50))
                {
                    message = "*Solo se admiten caracteres de [A-Z] o [0-9]. <br> *La cadena debe tener una longitud mínima de cuatro(4) caracteres. <br> *La cadena debe tener una longitud máxima de cincuenta(50) caracteres.<br> La cadena no puede empezar con un carácter numérico(0-9).<br>*La cadena debe contener al menos un (1) carácter alfabético.";
                    titulo2 = "Por favor tome en cuenta las siguientes reglas para ingresar los datos. ";
                    return Json(new { titulo = titulo2, message = message }, JsonRequestBehavior.AllowGet);
                }
                cMaestrosDetalle md = new cMaestrosDetalle();
                md.idMaestroCabecera = pIdMaestroCabecera;
                md.descripcion = Descripcion;
                md.idUsuarioCreador = 1;
                md.idUsuarioActualiza = 1;
                md.indActivo = Estado;
                message = svc.InsertarMaestroDetalle(md);
                if (message == "Modificación completada" || message == "Registro exitoso")
                {
                    titulo2 = "Excelente";
                }
                else
                {
                    titulo2 = "Advertencia";
                }
                Log.Info(message);
            }
            catch (Exception e)
            {
                message = e.ToString();
                titulo2 = "Ha Ocurrido un Error!!!";
                Log.Error(message);
            }
            return Json(new { titulo = titulo2, message = message }, JsonRequestBehavior.AllowGet);//Redirect(Url.Action("Config", "ConfiguracionM"));
        }   
        public ActionResult ModificarMaestrosDetalles(Int64 pIdMaestroCabecera, string DescripcionAntigua, string Descripcion, Boolean Estado)
        {
            string message;
            string titulo2 = ""; 
            try
            {
                if (!(reglas.IsMatch(Descripcion) && Descripcion.Length > 3 && Descripcion.Length <= 50))
                {
                    message = "*Solo se admiten caracteres de [A-Z] o [0-9]. <br> *La cadena debe tener una longitud mínima de cuatro(4) caracteres. <br> *La cadena debe tener una longitud máxima de cincuenta (50) caracteres.<br> La cadena no puede empezar con un carácter numérico(0-9).<br>*La cadena debe contener al menos un (1) carácter alfabético.";
                    titulo2 = "Por favor tome en cuenta las siguientes reglas para ingresar los datos. ";
                    return Json(new { titulo = titulo2, message = message }, JsonRequestBehavior.AllowGet);
                }
                cMaestrosDetalle md = new cMaestrosDetalle();
                md.idMaestroCabecera = pIdMaestroCabecera;
                md.descripcion = Descripcion;
                md.idUsuarioCreador = 1;
                md.idUsuarioActualiza = 1;
                md.indActivo = Estado;
                message = svc.ActualizarMaestroDetalle(md, DescripcionAntigua);
                if (message == "Modificación completada" || message == "Registro exitoso")
                {
                    titulo2 = "Excelente";
                }
                else
                {
                    titulo2 = "Advertencia";
                }
                Log.Info(message);
            }
            catch (Exception e)
            {
                message = e.ToString();
                titulo2 = "Ha Ocurrido un Error!!!";
                Log.Error(message);
            }
            return Json(new { titulo = titulo2, message = message }, JsonRequestBehavior.AllowGet);//Redirect(Url.Action("Config", "ConfiguracionM"));
        }
    }
}
