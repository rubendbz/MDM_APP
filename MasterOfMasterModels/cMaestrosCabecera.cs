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
    /// @Autor: Jessy Aguilera
    /// @Versión: 1.1
    /// @Fecha: 17/08/2018
    /// @Descripción: Es la clase que representa los datos del maestro cabecera en la aplicación, mediante ella
    /// se asignan los valores generados en los procedimientos para ejecutar los servicios y operaciones.
    /// </summary>
    public class cMaestrosCabecera
    {
        public Int64 idMaestroCabecera { get; set; }
        [Required(ErrorMessage = "Debe ingresar una descripción")]
        public String descripcion { get; set; }
        public Int64 idUsuarioCreador { get; set; }
        public Int64 idUsuarioActualiza { get; set; }
        [Required(ErrorMessage = "Debe especificar el estado del maestro cabecera")]
        public Boolean indActivo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaOcurrencia { get; set; }

    }
}
