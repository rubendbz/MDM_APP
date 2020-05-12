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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MasterOfMasterDesktop.UI;
using MasterOfMasterDesktop.Clases;
using MasterOfMasterModels;


namespace MasterOfMasterDesktop
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        svcMOM.MasterOfMasterSvcClient svc = new svcMOM.MasterOfMasterSvcClient();
        private static readonly log4net.ILog Log
       = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public MainWindow()
        {
            InitializeComponent();
            CargarMenus();
        }

        /// <summary>
        /// @Autor: Jessy Aguilera
        /// @Fecha: 08/2018
        /// @Descripción: Al darle click carga en el frame la página de uiConfigMaestros. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfiguracion_Click(object sender, RoutedEventArgs e)
        {
            FrameBody.NavigationService.Content = new uiConfigMaestros(this);
        }
        public void Foo(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
        }
        /// <summary>
        /// @Autor: Pedro Marval
        /// @Fecha: 08/2018
        /// @Descripción: Al darle click carga en el frame la página de uiConfigDetalles. 
        /// </summary>
        private void btnDetalles_Click(object sender, RoutedEventArgs e)
        {
            Button menuMaestros = sender as Button;
            cMaestrosCabecera maestro = menuMaestros.DataContext as cMaestrosCabecera;
            FrameBody.Content = new uiConfigDetalles(maestro.idMaestroCabecera, maestro.descripcion, this);
            tbMnBuscar.Text = "";
        }

        /// <summary>
        /// @Autor: Jessy Aguilera
        /// @Fecha: 08/2018
        /// @Descripción: Permite arrastrar la ventana presionando el boton izquierdo del mouse
        /// </summary>
        private void BarraSuperior_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            string messageBoxText = "¿Está seguro que desea cerrar la aplicación?";
            string caption = "Cerrar aplicación";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Information;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    Application.Current.Shutdown();
                    break;
                case MessageBoxResult.No:
                    break;
            }
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            FrameBody.NavigationService.Content = new uiCrearMaestroAvanzado(this);
        }

        private void btnMAMod_Click(object sender, RoutedEventArgs e)
        {
            Button menuMA = sender as Button;
            Tabla maestro = menuMA.DataContext as Tabla;
            FrameBody.Content = new uiModMAvanzado(maestro.NombreTabla, this);
            tbMnBuscarMA.Text = "";
        }

        private void exMaestros_Expanded(object sender, RoutedEventArgs e)
        {
            exMA.IsExpanded = false;
            tbMnBuscarMA.Text = "";
            tbMnBuscar.Text = "";
        }

        private void exMA_Expanded(object sender, RoutedEventArgs e)
        {
            exMaestros.IsExpanded = false;
            tbMnBuscarMA.Text = "";
            tbMnBuscar.Text = "";
            
        }

        public void CargarMenus()
        {
            var listaMC = svc.ConsultarMaestroCabecera(-1, "", true, "0001-01-01", "9999-12-31");
            var listaMA = svc.ListarMAestrosAvanzados();
            lvMnMCabecera.ItemsSource = listaMC;
            lvMnMAvanzado.ItemsSource = listaMA;
        }

        private void lvMnMAvanzado_StylusButtonDown(object sender, StylusButtonEventArgs e)
        {
            lvMnMAvanzado.SelectedItems.Clear();
        }

      

        private void tbMnBuscar_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var s = tbMnBuscar.Text;
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
           
        }

        private void tbMnBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
          
            var listaMC = svc.ConsultarMaestroCabecera(-1, tbMnBuscar.Text, true, "0001-01-01", "9999-12-31");
            lvMnMCabecera.ItemsSource = listaMC;
        }

        private void tbMnBuscarMA_TextChanged(object sender, TextChangedEventArgs e)
        {
            var listaMA = svc.ListarMAestrosAvanzados();
            List<Tabla> lista = new List<Tabla>();
            foreach (var item in listaMA )
            {
                if (item.NombreTabla.ToUpper().Contains(tbMnBuscarMA.Text.ToUpper()))
                {
                    lista.Add(item);
                }
            }
           
         
            lvMnMAvanzado.ItemsSource = lista;
        }
    }
}
