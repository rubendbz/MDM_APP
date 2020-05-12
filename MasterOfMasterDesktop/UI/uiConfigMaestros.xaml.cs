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
using MasterOfMasterDesktop;
using MasterOfMasterDesktop.Clases;
using System.Data;
using MasterOfMasterModels;
using System.Text.RegularExpressions;
using log4net;

namespace MasterOfMasterDesktop.UI
{
    /// <summary>
    /// Lógica de interacción para uiConfigMaestros.xaml
    /// </summary>
    public partial class uiConfigMaestros : Page
    {
        MainWindow pw;
        private static readonly log4net.ILog Log
       = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Regex reglas = new Regex(@"^[a-zA-ZáéíóúàèìòùÀÈÌÒÙÁÉÍÓÚñÑüÜ][0-9a-zA-ZáéíóúàèìòùÀÈÌÒÙÁÉÍÓÚñÑüÜ_]+$");
        svcMOM.MasterOfMasterSvcClient svc = new svcMOM.MasterOfMasterSvcClient();
        /// <summary>
        /// @Autor: Jessy Aguilera
        /// @Fecha: 08/2018
        /// @Descripción: Carga la lista con los datos de la base de datos y los ingresa en un collectionView para 
        /// poder filtrar directamente en el listview 
        /// </summary>
        public uiConfigMaestros(MainWindow pMW)
        {
            InitializeComponent();
            
            dpFecha.DisplayDateEnd = DateTime.Now;
            pw = pMW;

            txtDescripcion.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, pw.Foo ));
        }
      

        private void btnPanelAgregar_Click(object sender, RoutedEventArgs e)
        {
            btnPanelAgregar.Visibility = Visibility.Collapsed;
            btnPanelBuscar.Visibility = Visibility.Visible;
            lvCambiarPanel.Content = "Buscar maestro";
        }

        private void btnPanelBuscar_Click(object sender, RoutedEventArgs e)
        {
            btnPanelAgregar.Visibility = Visibility.Visible;
            btnPanelBuscar.Visibility = Visibility.Collapsed;
            lvCambiarPanel.Content = "Agregar maestro";
        }
        /// <summary>
        /// @Autor: Jessy Aguilera
        /// @Fecha: 08/2018
        /// @Descripción: Al darle click carga en el frame la página de uiIndex. 
        /// </summary>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Content = new uiIndex();
        }
        /// <summary>
        /// @Autor: Jessy Aguilera
        /// @Fecha: 08/2018
        /// @Descripción: Objeto de la clase interna de la aplicación desktop que convierte los datos 
        /// obtenidos del servicio para cargarlos en la lista 
        /// </summary>
        public void cargarListaMaestros()
        {
            cMaestrosCabeceraDesktop MCD = new cMaestrosCabeceraDesktop();
            listaMaestros.ItemsSource = MCD.CargarLista();
        }
        /// <summary>
        /// @Autor: Jessy Aguilera
        /// @Fecha: 08/2018
        /// @Descripción: Verifica las validaciones para poder ingresar un nuevo maestro cabecera usando el servicio.
        /// Al ingresarlo pone los campos predeterminados y carga nuevamente tanto el menu, como la lista.
        /// </summary>
        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (reglas.IsMatch(txtDescripcion.Text) && txtDescripcion.Text.Length > 3 && txtDescripcion.Text.Length < 51 )
                {
                    cMaestrosCabecera MC = new cMaestrosCabecera();
                    MC.descripcion = txtDescripcion.Text;
                    MC.idUsuarioCreador = 1;
                    MC.indActivo = chkActivo.IsChecked.Value;
                    MessageBox.Show(svc.InsertarMaestroCabecera(MC),"Información");
                    cargarListaMaestros();
                    pw.CargarMenus();
                    txtDescripcion.Text = "";
                    chkActivo.IsChecked = false;
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

        /// <summary>
        /// @Autor: Jessy Aguilera
        /// @Fecha: 08/2018
        /// @Descripción: Al darle doble-click a un maestro cabecera en la listview
        /// Se carga la página para poder ingresar, modificar detalles.
        /// </summary>
        private void listaMaestros_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBlock item = sender as TextBlock;
            cMaestrosCabeceraDesktop maestro = item.DataContext as cMaestrosCabeceraDesktop;
            uiConfigDetalles uiCD = new uiConfigDetalles(maestro.idMaestroCabecera, maestro.descripcion, pw);
            uiCD.btnVolver.Visibility = Visibility.Visible;
            NavigationService.Content = uiCD;
        }

        /// <summary>
        /// @Autor: Jessy Aguilera
        /// @Fecha: 08/2018
        /// @Descripción: Al darle click carga un dialog de uiModificarMaestros para poder modificar la cabecera.
        /// </summary>
        private void imgModificar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image img = sender as Image;
            cMaestrosCabeceraDesktop row = img.DataContext as cMaestrosCabeceraDesktop;
            cMaestrosCabeceraDesktop MC = new cMaestrosCabeceraDesktop();
            MC.descripcion = row.descripcion;
            MC.indActivo = row.indActivo;
            uiModificarMaestros modificar = new uiModificarMaestros(MC.idMaestroCabecera,MC.descripcion,MC.indActivo,this,pw);
            AplicarEfecto();
            modificar.ShowDialog();
        }

        /// <summary>
        /// @Autor: Jessy Aguilera
        /// @Fecha: 08/2018
        /// @Descripción: Validaciones en los textblock que permite solo letras y números.
        /// </summary>
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
       
       
        /// <summary>
        /// @Autor: Jessy Aguilera
        /// @Fecha: 08/2018
        /// @Descripción: Aplica efecto a la ventana principal al mostrar el uiModificarMaestros.
        /// </summary>
        private void AplicarEfecto()

        {

            var objBlur = new System.Windows.Media.Effects.BlurEffect();
            objBlur.Radius = 10;
            App.Current.MainWindow.Effect = objBlur;

        }

        /// <summary>
        /// @Autor:Simon Vera, Jose Perez , Pedro Marval
        /// @Fecha:12/09/2018
        /// @Descripcion: método para retornar los maestros cabecera que coincidan con los parámetros de búsquedas que se le pasen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            bool? bandera = null;
            string desde = dpFechaInicial.Text != "" ? dpFechaInicial.Text : "0001-01-01";
            string hasta = dpFechaFinal.Text != "" ? dpFechaFinal.Text : "9999-12-31";

            try {
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
                    List<cMaestrosCabeceraDesktop> result = new List<cMaestrosCabeceraDesktop>();
                    var lista = svc.ConsultarMaestroCabecera(-1, txtBuscarDescripcion.Text, bandera, desde, hasta);
                    foreach (MasterOfMasterModels.cMaestrosCabecera MC in lista)
                    {
                        cMaestrosCabeceraDesktop datos = new cMaestrosCabeceraDesktop();
                        datos.descripcion = MC.descripcion;
                        datos.idMaestroCabecera = MC.idMaestroCabecera;
                        datos.idUsuarioActualiza = MC.idUsuarioActualiza;
                        datos.idUsuarioCreador = MC.idUsuarioCreador;
                        datos.indActivo = MC.indActivo;
                        datos.fechaCreacion = MC.fechaCreacion.ToShortDateString();
                        datos.fechaOcurrencia = MC.fechaOcurrencia.ToShortDateString();
                        result.Add(datos);
                    }

                    if (!(result.Count > 0))
                    {
                        MessageBox.Show("No se encontraron resultados","Información");
                    }

                    listaMaestros.ItemsSource = result;
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

                        List<cMaestrosCabeceraDesktop> result = new List<cMaestrosCabeceraDesktop>();
                        var lista = svc.ConsultarMaestroCabecera(-1, txtBuscarDescripcion.Text, bandera, desde, hasta);
                        foreach (MasterOfMasterModels.cMaestrosCabecera MC in lista)
                        {
                            cMaestrosCabeceraDesktop datos = new cMaestrosCabeceraDesktop();
                            datos.descripcion = MC.descripcion;
                            datos.idMaestroCabecera = MC.idMaestroCabecera;
                            datos.idUsuarioActualiza = MC.idUsuarioActualiza;
                            datos.idUsuarioCreador = MC.idUsuarioCreador;
                            datos.indActivo = MC.indActivo;
                            datos.fechaCreacion = MC.fechaCreacion.ToShortDateString();
                            datos.fechaOcurrencia = MC.fechaOcurrencia.ToShortDateString();
                            result.Add(datos);
                        }

                        if (!(result.Count > 0))
                        {
                            MessageBox.Show("No se encontraron resultados","Información");
                        }

                       listaMaestros.ItemsSource = result;
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
        /// @Descripcion: Método para limpiar los campos de búsqueda y recargar a tabla de maestros cabecera
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
            this.cargarListaMaestros();
        }

        private void btnBFechas_Clik(object sender, RoutedEventArgs e)
        {
         

            BtnBDia.Visibility = Visibility.Visible;
            btnBFechas.Visibility = Visibility.Collapsed;

            dpFecha.Visibility = Visibility.Collapsed;
            dpFechaFinal.DisplayDateEnd = DateTime.Today;
            dpFechaInicial.DisplayDateEnd = DateTime.Now;
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
            dpFecha.DisplayDateEnd = DateTime.Today;
            dpFecha.DisplayDateEnd = DateTime.MaxValue;

            dpFecha.Text = dpFechaFinal.Text = dpFechaInicial.Text = "";

        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            txtDescripcion.Text = "";
            chkActivo.IsChecked = false;
        }

        private void txtDescripcion_LostFocus(object sender, RoutedEventArgs e)
        {
            txtDescripcion.Text = txtDescripcion.Text.Trim();
        }

        private void txbDescripcion_Click(object sender, RoutedEventArgs e)
        {
            Button item = sender as Button;
            cMaestrosCabeceraDesktop maestro = item.DataContext as cMaestrosCabeceraDesktop;
            uiConfigDetalles uiCD = new uiConfigDetalles(maestro.idMaestroCabecera, maestro.descripcion, pw);
            uiCD.btnVolver.Visibility = Visibility.Visible;
            NavigationService.Content = uiCD;
        }

        private void btnInfo_Click(object sender, RoutedEventArgs e)
        { 
            MessageBox.Show(@"Solo se admiten caracteres de [A - Z] o [0 - 9]." + Environment.NewLine + "La cadena debe tener una longitud mínima de cuatro (4) caracteres." + Environment.NewLine + "La cadena debe tener una longitud máxima de cincuenta (50) caracteres." + Environment.NewLine + "La cadena no puede empezar con un carácter numérico (0 - 9)." + Environment.NewLine + "La cadena debe contener al menos un (1) carácter alfabético." + Environment.NewLine + "La cadena no puede empezar con un espacio en blanco.", "Por favor tome en cuenta las siguientes reglas para ingresar los datos.");
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            cargarListaMaestros();
        }
    }
}
