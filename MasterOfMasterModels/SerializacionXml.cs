using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace MasterOfMasterModels
{
    public class SerializacionXml
    {
        #region SerializadorXML
        /// <summary>
        /// @Autor: Simon Vera.
        /// @Fecha: 18/09/2018.
        /// @Descripción: Metodo que serializa un objecto de cualquier clase al formato XML.
        /// </summary>
        /// <param name="cMaestroAvanzado">Objeto que sera convertido a XML.</param>
        /// <returns>Cadena XML que representa el objeto serializado.</returns>
        public string serializarXML(Tabla MA)
        {
            XmlWriterSettings xmlSettings = new XmlWriterSettings();
            xmlSettings.OmitXmlDeclaration = true;                             //Omito la declaracion XML al inicio del string.
            xmlSettings.NewLineOnAttributes = true; xmlSettings.Indent = true; //Propiedades para incluir saltos de linea (para mejor legibilidad).

            XmlSerializerNamespaces names = new XmlSerializerNamespaces(); names.Add("", ""); //De esta forma de omite la declaracion del namespace en el root elemente del xml.

            XmlSerializer xmlSerializer = new XmlSerializer(MA.GetType());

            using (StringWriter stringWriter = new StringWriter())                            //stringWritter donde se generara el xml resultado.
            using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter, xmlSettings))         // xmlWritter, necesario para aplicarle al xml la configuracion que se realizo al principio del metodo.
            {
                xmlSerializer.Serialize(xmlWriter, MA, names);               //se serializa el objeto generando el xml resultado en el stringWriter.
                return stringWriter.ToString();
            }
        }
        #endregion
    }
}
