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
using MasterOfMasterDesktop.UI;
using System.Text.RegularExpressions;
using log4net;

namespace MasterOfMasterDesktop.UI
{
    /// <summary>
    /// Interaction logic for uiModificarMaestros.xaml
    /// </summary>
    public partial class uiModificarMaestros : Window
    {
        MainWindow pw;
        private static readonly log4net.ILog Log
       = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        Int64 idCabecera;
        String descripcionAntigua;
        Boolean indActivo;
        Page UI;
        Regex reglas = new Regex(@"^[a-zA-ZáéíóúàèìòùÀÈÌÒÙÁÉÍÓÚñÑüÜ][0-9a-zA-ZáéíóúàèìòùÀÈÌÒÙÁÉÍÓÚñÑüÜ_\s]+$");
        svcMOM.MasterOfMasterSvcClient svc = new svcMOM.MasterOfMasterSvcClient();
        public uiModificarMaestros(Int64 pIdCabecera, String pDescripcion, Boolean pIndActivo, Page pUI, MainWindow pMW)
        {
            UI = pUI;
            InitializeComponent();
            idCabecera = pIdCabecera;
            descripcionAntigua = pDescripcion;
            indActivo = pIndActivo;
            CargarModificar(pIdCabecera, pDescripcion, pIndActivo);
            pw = pMW;
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            quitarEfecto();
            this.Close();
        }

        private void CargarModificar(Int64 pIdCabecera, String pDescripcion, Boolean pIndActivo)
        {
            txtDescripcion.Text = pDescripcion;
            if (pIndActivo == true)
            {
                chkActivo.IsChecked = true;
            }
            else
            {
                chkActivo.IsChecked = false;
            }
        }

        private void quitarEfecto()
        {
            var objBlur = new System.Windows.Media.Effects.BlurEffect();
            objBlur.Radius = 0;
            App.Current.MainWindow.Effect = objBlur;

        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            string mensaje = "";
            try
            {
                if (reglas.IsMatch(txtDescripcion.Text) && txtDescripcion.Text.Length > 3 && txtDescripcion.Text.Length < 51)
                {
                    if (txtDescripcion.Text == descripcionAntigua && chkActivo.IsChecked.Value == indActivo)
                    {
                        MessageBox.Show("Realice por lo menos un cambio", "Error");
                    }
                    else
                    {
                        if (UI.ToString() == "MasterOfMasterDesktop.UI.uiConfigMaestros")
                        {
                            cMaestrosCabecera MC = new cMaestrosCabecera();
                            MC.descripcion = txtDescripcion.Text;
                            MC.indActivo = chkActivo.IsChecked.Value;
                            MC.idUsuarioActualiza = 2;
                            mensaje = svc.ActualizarMaestroCabecera(MC, descripcionAntigua);                       
                            uiConfigMaestros CM = UI as uiConfigMaestros;
                            CM.cargarListaMaestros();

                        }
                        else
                        {
                            cMaestrosDetalle MD = new cMaestrosDetalle();
                            MD.idMaestroCabecera = idCabecera;
                            MD.descripcion = txtDescripcion.Text;
                            MD.indActivo = chkActivo.IsChecked.Value;
                            MD.idUsuarioActualiza = 2;
                            mensaje = svc.ActualizarMaestroDetalle(MD, descripcionAntigua);
                            uiConfigDetalles CD = UI as uiConfigDetalles;                                                     
                            CD.cargarMDetalles(CD.id);
                            
                        }
                        MessageBox.Show(mensaje);
                        pw.CargarMenus();
                      
                        if (mensaje == "Modificación completada")
                        {

                            this.Close();
                            quitarEfecto();
                        }
                      
                    }
                }
                else
                {
                    MessageBox.Show("Ingrese una descripción valida");
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
                Log.Error(Ex.Message);
            }
        }

        private void txtDescripcion_PreviewTextInput(object sender, TextCompositionEventArgs e)
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
        /// @Fecha:1/10/2018
        /// @Descripcion: Elimina los espacios en blanco al comienzo y al final cuando está fuera de foco 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDescripcion_LostFocus(object sender, RoutedEventArgs e)
        {
            txtDescripcion.Text = txtDescripcion.Text.Trim();
        }
    }
}
