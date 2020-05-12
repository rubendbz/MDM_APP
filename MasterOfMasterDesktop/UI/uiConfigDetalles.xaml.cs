using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MasterOfMasterModels;
using MasterOfMasterDesktop.Clases;
using System.Text.RegularExpressions;
using log4net;

namespace MasterOfMasterDesktop.UI
{
    /// <summary>
    /// Lógica de interacción para uiConfigDetalles.xaml
    /// </summary>
    public partial class uiConfigDetalles : Page
    {
        MainWindow pw;
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Int64 id;
        Regex reglas = new Regex(@"^[a-zA-ZáéíóúàèìòùÀÈÌÒÙÁÉÍÓÚñÑüÜ][0-9a-zA-ZáéíóúàèìòùÀÈÌÒÙÁÉÍÓÚñÑüÜ_\s]+$");
        svcMOM.MasterOfMasterSvcClient svc = new svcMOM.MasterOfMasterSvcClient();
        public uiConfigDetalles(Int64 idMC, string descripcion,MainWindow pMW)
        {
            InitializeComponent();
            cargarMDetalles(idMC);
            id = idMC;
            txtbMaestro.Text += descripcion;
            dpFecha.DisplayDateEnd = DateTime.Now;
            dpFechaFinal.DisplayDateEnd = DateTime.Now;
            dpFechaInicial.DisplayDateEnd = DateTime.Now;
            pw = pMW;
            txtDescripcion.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, pw.Foo));
        }
    
        //metodo que carga la data de los maestros detalles en el listview
        public void cargarMDetalles(Int64 id)
        {
            cMaestrosDetalleDesktop MDD = new cMaestrosDetalleDesktop();
            listaMaestrosDetalle.ItemsSource = MDD.CargarLista(id);
        }
        
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Content = new uiIndex();
        }
        //metodo para guardar un nuevo maestro detalle 
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (reglas.IsMatch(txtDescripcion.Text) && txtDescripcion.Text.Length > 3 && txtDescripcion.Text.Length < 51)
                {
                    Random rnd = new Random();
                    cMaestrosDetalle MD = new cMaestrosDetalle();
                    MD.descripcion = txtDescripcion.Text;
                    MD.idMaestroCabecera = id;
                    MD.idUsuarioCreador = rnd.Next(1, 3);
                    MD.idUsuarioActualiza = MD.idUsuarioCreador;

                    if (chkActivo.IsChecked == true)
                    {
                        MD.indActivo = true;
                    }
                    else
                    {
                        MD.indActivo = false;
                    }
                    txtDescripcion.Text = "";
                    MessageBox.Show(svc.InsertarMaestroDetalle(MD));
                    cargarMDetalles(id);
                    pw.CargarMenus();
                }
                else
                {
                    MessageBox.Show("Ingrese una descripción valida", "Alerta");
                }
            }
            catch(Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                Log.Error(Ex.Message);
            }
        }
        //método para limpiar la caja de texto de descripción y quitar el marcado al checkbox de indicador de activo
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            txtDescripcion.Text = "";
            chkActivo.IsChecked = false;
        }

        //método que valida que no se inserten caracteres no permitidos en las cajas de texto
        private void txt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox txt = sender as TextBox;
            if ((txt.Text.Length) < 1)
            {
                int ascci = Convert.ToInt32(Convert.ToChar(e.Text));
                if (ascci >= 65 && ascci <= 90 || ascci >= 97 && ascci <= 122 || ascci == 193 || ascci == 201 || ascci == 205 || ascci == 209 || ascci == 211 || ascci == 218 || ascci == 225 || ascci == 233 || ascci == 237 || ascci == 241 || ascci == 243 || ascci == 250)
                    e.Handled = false;
                else
                    e.Handled = true;
            }
            else
            {
                if (txt.Text.Length <= 50)
                {
                    int ascci = Convert.ToInt32(Convert.ToChar(e.Text));
                    if (ascci >= 48 && ascci <= 57 || ascci >= 65 && ascci <= 90 || ascci >= 97 && ascci <= 122 || ascci == 193 || ascci == 201 || ascci == 205 || ascci == 209 || ascci == 211 || ascci == 218 || ascci == 225 || ascci == 233 || ascci == 237 || ascci == 241 || ascci == 243 || ascci == 250)
                        e.Handled = false;
                    else
                        e.Handled = true;
                }
                else
                {
                    e.Handled = true;
                }
            }
        }

        //carga el modal con la data del maestro detalle seleccionado para ser modificado
        private void imgModificar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image img = sender as Image;
            cMaestrosDetalleDesktop row = img.DataContext as cMaestrosDetalleDesktop;
            cMaestrosDetalleDesktop MD = new cMaestrosDetalleDesktop();
            MD.idMaestroCabecera = row.idMaestroCabecera;
            MD.descripcion = row.descripcion;
            MD.indActivo = row.indActivo;
            uiModificarMaestros modificar = new uiModificarMaestros(MD.idMaestroCabecera, MD.descripcion, MD.indActivo,this,pw);
            AplicarEfecto();
            modificar.ShowDialog();
        }
        /// <summary>
        /// Pedro Marval
        /// v1
        /// Efecto de desenfoque al abrir el modal
        /// </summary>
        private void AplicarEfecto()
        {
            var objBlur = new System.Windows.Media.Effects.BlurEffect();
            objBlur.Radius = 10;
            App.Current.MainWindow.Effect = objBlur;
        }

        private void btnPanelAgregar_Click(object sender, RoutedEventArgs e)
        {
            btnPanelAgregar.Visibility = Visibility.Collapsed;
            btnPanelBuscar.Visibility = Visibility.Visible;
            lvCambiarPanel.Content = "Buscar detalle";
        }

        private void btnPanelBuscar_Click(object sender, RoutedEventArgs e)
        {
            btnPanelAgregar.Visibility = Visibility.Visible;
            btnPanelBuscar.Visibility = Visibility.Collapsed;
            lvCambiarPanel.Content = "Agregar detalle";
        }
        /// <summary>
        /// @Autor:Simon Vera, Jose Perez , Pedro Marval
        /// @Fecha:12/09/2018
        /// @Descripcion: método para retornar los maestros detalles que coincidan con los parámetros de búsquedas que se le pasen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            bool? bandera = null;
            string desde;
            string hasta;

            try
            {
                if (!(BtnBDia.IsVisible))
                {

                    desde = hasta = dpFecha.Text != "" ? dpFecha.Text : "0001-01-01";

                    if (cmbIndActivo.Text == "Activo")
                    {
                        bandera = true;
                    }
                    if (cmbIndActivo.Text == "Inactivo")
                    {
                        bandera = false;
                    }
                    List<cMaestrosDetalleDesktop> result = new List<cMaestrosDetalleDesktop>();
                    var lista = svc.ConsultarMaestroDetalle(-1, id, txtBuscarDescripcion.Text, bandera, desde, hasta);
                    foreach (MasterOfMasterModels.cMaestrosDetalle MD in lista)
                    {
                        cMaestrosDetalleDesktop datos = new cMaestrosDetalleDesktop();
                        datos.idMaestroDetalle = MD.idMaestroDetalle;
                        datos.descripcion = MD.descripcion;
                        datos.idMaestroCabecera = MD.idMaestroCabecera;
                        datos.idUsuarioActualiza = MD.idUsuarioActualiza;
                        datos.idUsuarioCreador = MD.idUsuarioCreador;
                        datos.indActivo = MD.indActivo;
                        datos.fechaCreacion = MD.fechaCreacion.ToShortDateString();
                        datos.fechaOcurrencia = MD.fechaOcurrencia.ToShortDateString();
                        result.Add(datos);
                    
                    }

                    if (!(result.Count > 0))
                    {
                        MessageBox.Show("No se encontraron resultados", "Información ");
                    }

                    listaMaestrosDetalle.ItemsSource = result;

                }

              

                else
                {
                    desde = dpFechaInicial.Text;
                    hasta = dpFechaFinal.Text;
                    bool flag = true;
                  
                    if (desde == "" && hasta != "")
                    {
                        flag = false;
                        MessageBox.Show("Debe ingresar la fecha de inicio", "Alerta");
                    }

                    if (desde != "" && hasta == "")
                    {
                        flag = false;
                        MessageBox.Show("Debe ingresar la fecha fin", "Alerta");
                    }

                    if (Convert.ToDateTime(desde) > Convert.ToDateTime(hasta))
                    {
                        flag = false;
                        MessageBox.Show("La fecha de inicio (Desde) debe ser menor a la fecha fin (Hasta)", "Alerta");
                    }

                    if (flag)
                    {
                        if (cmbIndActivo.Text == "Activo")
                        {
                            bandera = true;
                        }
                        if (cmbIndActivo.Text == "Inactivo")
                        {
                            bandera = false;
                        }
                        List<cMaestrosDetalleDesktop> result = new List<cMaestrosDetalleDesktop>();
                        var lista = svc.ConsultarMaestroDetalle(-1, id, txtBuscarDescripcion.Text, bandera, desde, hasta);
                        foreach (MasterOfMasterModels.cMaestrosDetalle MD in lista)
                        {
                            cMaestrosDetalleDesktop datos = new cMaestrosDetalleDesktop();
                            datos.idMaestroDetalle = MD.idMaestroDetalle;
                            datos.descripcion = MD.descripcion;
                            datos.idMaestroCabecera = MD.idMaestroCabecera;
                            datos.idUsuarioActualiza = MD.idUsuarioActualiza;
                            datos.idUsuarioCreador = MD.idUsuarioCreador;
                            datos.indActivo = MD.indActivo;
                            datos.fechaCreacion = MD.fechaCreacion.ToShortDateString();
                            datos.fechaOcurrencia = MD.fechaOcurrencia.ToShortDateString();
                            result.Add(datos);
                        }


                        if (!(result.Count > 0))
                        {
                            MessageBox.Show("No se encontraron resultados", "Información ");
                        }

                        listaMaestrosDetalle.ItemsSource = result;
                    }
                }
            }
            catch(Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                Log.Error(Ex.Message);
            }
        }
        /// <summary>
        /// @Autor:Jose Perez
        /// @Fecha:12/09/2018
        /// @Descripcion: Método para limpiar los campos de búsqueda y recargar a tabla de maestros detalles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBuscarLimpiar_Click(object sender, RoutedEventArgs e)
        {
            dpFecha.Text = "";
            dpFechaInicial.Text = "";
            dpFechaFinal.Text = "";
            cmbIndActivo.Text = "";
            txtBuscarDescripcion.Clear();
            txtDescripcion.Clear();
            this.cargarMDetalles(id);
        }

        private void btnBFechas_Clik(object sender, RoutedEventArgs e)
        {

            BtnBDia.Visibility = Visibility.Visible;
            btnBFechas.Visibility = Visibility.Collapsed;

            dpFecha.Visibility = Visibility.Collapsed;
            dpFechaFinal.Visibility = Visibility.Visible;
            dpFechaInicial.Visibility = Visibility.Visible;
            dpFecha.Text = dpFechaFinal.Text = dpFechaInicial.Text = "";

        }
        private void btnBDia_Clik(object sender, RoutedEventArgs e)
        {

            btnBFechas.Visibility = Visibility.Visible;
            BtnBDia.Visibility = Visibility.Collapsed;
            

            dpFecha.Visibility = Visibility.Visible;
            dpFechaFinal.Visibility = Visibility.Collapsed;
            dpFechaInicial.Visibility = Visibility.Collapsed;

            dpFecha.Text = dpFechaFinal.Text = dpFechaInicial.Text = "";

        }

        /// <summary>
        /// @Fecha:1/10/2018
        /// @Descripcion: Elimina los espacios en blanco al comienzo y al final cuando está fuera de foco 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
   
        private void txtCNombre_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(@"Solo se admiten caracteres de [A - Z] o [0 - 9]." + Environment.NewLine + "La cadena debe tener una longitud mínima de cuatro (4) caracteres." + Environment.NewLine + "La cadena debe tener una longitud máxima de cincuenta (50) caracteres." + Environment.NewLine + "La cadena no puede empezar con un carácter numérico (0 - 9)." + Environment.NewLine + "La cadena debe contener al menos un (1) carácter alfabético." + Environment.NewLine + "La cadena no puede empezar con un espacio en blanco.", "Por favor tome en cuenta las siguientes reglas para ingresar los datos.");
        }

        private void btnVolver_Click(object sender, RoutedEventArgs e)
        {
            
            NavigationService.GoBack();
        }

    }
}
