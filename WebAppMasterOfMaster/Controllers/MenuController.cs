using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MasterOfMasterModels;
using log4net;

namespace WebAppMasterOfMaster.Controllers
{
    public class MenuController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        SvcMom.MasterOfMasterSvcClient svc = new SvcMom.MasterOfMasterSvcClient();
        // GET: Menu

       
        public ActionResult Inicio()
        {
            return View();
        }
       
        public ActionResult Configuracion()
        {
            return PartialView();
        }
        public JsonResult MaestrosCabeceras()
        {
            string mensaje;
            cMaestrosCabecera[] mc = null;
            try
            {
                 mc = svc.ConsultarMaestroCabecera(-1, "", true, "0001-01-01", "9999-12-31");
            }
            catch (Exception Ex)
            {
                mensaje = "Error... " + Ex.Message;
                Log.Error(mensaje);
            }
            return Json(mc, JsonRequestBehavior.AllowGet);
        }
      
    }

}