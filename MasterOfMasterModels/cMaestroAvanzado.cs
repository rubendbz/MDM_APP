using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace MasterOfMasterModels
{
    /// <summary>
    /// @Autor:Simon Vera
    /// @Fecha:19/09/2018
    /// @Decripcion: Creacion del Modelo de Datos para la creacion de Tablas de Maestros Avanzados,
    ///  se le llamo "Tabla" para que pueda ser leido por la Base de datos al generar el xml.
    /// </summary>
    public class Tabla
    {
        public string NombreTabla { get; set; }
        public string Atributo { get; set; }
        public string Longitud { get; set; }
        public string TipoDeDato { get; set; }
        public string pNulo { get; set; }
        //public string mensaje { get; set; }

    }
}
