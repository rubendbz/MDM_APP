using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MasterOfMasterModels;
using System.Text.RegularExpressions;
using log4net;

/// <summary>
/// Autor: Ruben Ballesteros
/// Fecha: 20/08/2018
/// Version: 1.0
/// Descripcion: Clase del Controlador para la Configuracion de Maestros Cabeceras, en esta clase se encuentran todos los metodos utiliazdos para el registro y modificacion de los datos de Maestros Cabeceras


namespace WebAppMasterOfMaster.Controllers
{
   
    public class ConfiguracionMController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Regex reglas = new Regex(@"^[a-zA-ZáéíóúàèìòùÀÈÌÒÙÁÉÍÓÚñÑüÜ][0-9a-zA-ZáéíóúàèìòùÀÈÌÒÙÁÉÍÓÚñÑüÜ_\s]+$");
        SvcMom.MasterOfMasterSvcClient svc = new SvcMom.MasterOfMasterSvcClient();
        // GET: ConfiguracionM
        public ActionResult Config()
        {
            return View();
        }
        [HttpGet]
        public JsonResult MaestrosCabeceras()
        {
            var mc = svc.ConsultarMaestroCabecera(-1,"",null, "0001-01-01", "9999-12-31");
            
            //returning json result  
            return Json(mc, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult MaestrosCabecerasBusqueda(string pDescripcion, bool? pIndActivo, string pFechaInicial, string pFechaFinal)
        {
            var mc = svc.ConsultarMaestroCabecera(-1,pDescripcion,pIndActivo,pFechaInicial,pFechaFinal);

            //returning json result  
            return Json(mc, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult IngresarMaestrosCabeceras(string Descripcion, Boolean Estado)
        {
            string message;
            string titulo2=" ";
            try
            {
                if (!(reglas.IsMatch(Descripcion) && Descripcion.Length>3 && Descripcion.Length <= 50))
                {
                    message = "*Solo se admiten caracteres de [A-Z] o [0-9]. <br> *La cadena debe tener una longitud mínima de cuatro(4) caracteres. <br> *La cadena debe tener una longitud máxima de cincuenta(50) caracteres.<br> La cadena no puede empezar con un carácter numérico(0-9).<br>*La cadena debe contener al menos un (1) carácter alfabético.";
                    titulo2 = "Por favor tome en cuenta las siguientes reglas para ingresar los datos. ";
                    return Json(new { titulo = titulo2, message = message }, JsonRequestBehavior.AllowGet);
                }
                cMaestrosCabecera mc = new cMaestrosCabecera();
                mc.descripcion = Descripcion;
                mc.idUsuarioCreador = 1;
                mc.idUsuarioActualiza = 1;
                mc.indActivo = Estado;
                message = svc.InsertarMaestroCabecera(mc);
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
            return Json(new {titulo = titulo2, message= message}, JsonRequestBehavior.AllowGet);//Redirect(Url.Action("Config", "ConfiguracionM"));
        }
        [HttpPost]
        public ActionResult ModificarMaestrosCabeceras(string DescripcionAntigua, string Descripcion, Boolean Estado)
        {
            string message;
            string titulo2=" ";
            try
            {
                if (!(reglas.IsMatch(Descripcion) && Descripcion.Length > 3 && Descripcion.Length <= 50))
                {
                    message = "*Solo se admiten caracteres de [A-Z] o [0-9]. <br> *La cadena debe tener una longitud mínima de cuatro(4) caracteres. <br> *La cadena debe tener una longitud máxima de cincuenta (50) caracteres.<br> La cadena no puede empezar con un carácter numérico(0-9).<br>*La cadena debe contener al menos un (1) carácter alfabético.";
                    titulo2 = "Por favor tome en cuenta las siguientes reglas para ingresar los datos. ";
                    return Json(new { titulo = titulo2, message = message }, JsonRequestBehavior.AllowGet);
                }
                cMaestrosCabecera mc = new cMaestrosCabecera();
                mc.descripcion = Descripcion;
                mc.idUsuarioCreador = 1;
                mc.idUsuarioActualiza = 1;
                mc.indActivo = Estado;
                message = svc.ActualizarMaestroCabecera(mc,DescripcionAntigua);
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


   
