using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MasterOfMasterModels
{
    /// <summary>
    /// @Autor: Jessy Aguilera
    /// @Versión: 1.1
    /// @Fecha: 17/08/2018
    /// @Descripción: Es la clase que representa los datos del maestro detalle en la aplicación, mediante ella
    /// se asignan los valores generados en los procedimientos para ejecutar los servicios y operaciones.
    /// </summary>
    public class cMaestrosDetalle
    {
        public Int64 idMaestroDetalle { get; set; }
        [Required(ErrorMessage = "Debe ingresar una descripción")]
        public Int64 idMaestroCabecera { get; set; }
        [Required(ErrorMessage = "Debe seleccionar un maestro cabecera")]
        public String descripcion { get; set; }
        public Int64 idUsuarioCreador { get; set; }
        public Int64 idUsuarioActualiza { get; set; }
        [Required(ErrorMessage = "Debe especificar el estado del maestro detalle")]
        public Boolean indActivo { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fechaOcurrencia { get; set; }
    }
}
