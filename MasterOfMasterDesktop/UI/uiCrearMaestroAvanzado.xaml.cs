using MasterOfMasterDesktop.Clases;
using MasterOfMasterModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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

namespace MasterOfMasterDesktop.UI
{/// <summary>
 /// //Autores: Pedro Marval, Ruben Ballesteros
 /// //Version: 1.0
 /// //Fecha: 26/09/2018
 /// //Descripcion: Clase donde se almacenan los metodos que permiten el correcto registros de datos a la tabla para la creacion de Maestros Avanzados.
 /// Interaction logic for uiCrearMaestroAvanzado.xaml
 /// </summary>

    public partial class uiCrearMaestroAvanzado : Page
    {
        MainWindow pw;
        private static readonly log4net.ILog Log
       = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        List<data> lista;
        int idCol = -1;

        public uiCrearMaestroAvanzado(MainWindow pMW)
        {
            InitializeComponent();
            txtbNombreTabla.Focus();
            lista = new List<data>();

            pw = pMW;
            txtbNombreTabla.Focus();

            DataObject.AddCopyingHandler(txtbNombreTabla, NoDragCopy);
            DataObject.AddCopyingHandler(txtCNombre, NoDragCopy);
            DataObject.AddCopyingHandler(txtTamanioR1, NoDragCopy);

        }

        private void NoDragCopy(object sender, DataObjectCopyingEventArgs e)
        {
            if (e.IsDragDrop)
            { e.CancelCommand(); }
        }

        private void aggcol_Click(object sender, RoutedEventArgs e)
        {
            string mensaje = "";
            string NombreTabla = txtbNombreTabla.Text;
            Regex _regex = new Regex("^([1-9]|[1-9][0-9]|[1-9][0-9][0-9]|[1-7][0-9][0-9][0-9]|8000)$");
            // Comprueba que el nomnbre de la columna no sea una palabra reservada de SQL
            bool flag = ComprobarNombreColumna(txtCNombre.Text);
            int pk = 0;

            // Validando nombre del maestro avanzado
            if (!(Regex.IsMatch(NombreTabla, @"^[A-Z][0-9a-zA-Z_]+$")))
            {
                mensaje = "El nombre de la tabla:" + " " + txtbNombreTabla.Text + " " + "no es válido \n";
            }
            else
            {
                if (txtbNombreTabla.Text.Length < 2 || txtbNombreTabla.Text.Length > 25)
                {
                    mensaje = "El nombre de la tabla debe contener dos caracteres como mínimo y venticinco como máximo\n";
                }
                else
                {
                    if (flag == false)
                    {
                        mensaje = "Nombre de columna no válido  \n";
                    }
                    else
                    {
                        if (txtCNombre.Text.Length < 2 || txtCNombre.Text.Length > 25)
                        {
                            mensaje = "El nombre de la columna debe contener dos caracteres como mínimo y venticinco como máximo\n";
                        }
                        else
                        {
                            if (!(Regex.IsMatch(txtCNombre.Text, @"^[a-zA-Z][0-9a-zA-Z_]+$")))
                            {
                                mensaje = "El nombre de la columna:" + " " + txtCNombre.Text + " " + "no es válido \n";
                            }
                            else
                            {
                                if (cbTD.Text == "")
                                {
                                    mensaje = " Debe seleccionar un tipo de dato \n";
                                }
                                else
                                {
                                    if (cbTD.Text == "varchar" && txtTamanioR1.Text == "")
                                    {
                                        mensaje = "Debe especificar el tamaño del tipo de dato Varchar \n";
                                    }
                                    else
                                    {
                                        if (!_regex.IsMatch(txtTamanioR1.Text) && cbTD.Text == "varchar")
                                        {
                                            mensaje = "El tamaño del tipo de dato Varchar debe ser mayor a 0 y menor a 8000 \n";
                                        }

                                    }
                                }
                            }
                        }

                    }
                }
            }

            // La clave primaria no permite valores nulos 
            if (chkPN.IsChecked.Value == true && chkPK.IsChecked.Value == true)
            {
                mensaje = "La clave primaria no puede permitir valores nulos \n";
            }
            // Las claves primaria solo deben ser BigInt o Int
            if ((chkPK.IsChecked.Value == true) && !(cbTD.Text == "bigint" || cbTD.Text == "int"))
            {
                mensaje = "La clave primaria solo puede ser Int o BigInt \n";
            }

            try
            {

                if (mensaje == "")
                {

                    data obj = new data();

                    obj.NomColumn = txtCNombre.Text;
                    obj.TypeData = cbTD.Text == "varchar" ? cbTD.Text + "(" + txtTamanioR1.Text + ")" : cbTD.Text;
                    obj.PN = chkPN.IsChecked.Value;
                    obj.PK = chkPK.IsChecked.Value;
                    obj.Longitud = txtTamanioR1.Text;

                    if (idCol > -1)
                    {

                        if(lista[idCol].PK)
                        {
                            lista[idCol].NomColumn = obj.NomColumn;
                            lista[idCol].TypeData = obj.TypeData;
                            lista[idCol].PN = obj.PN;
                            lista[idCol].PK = obj.PK;
                            lista[idCol].Longitud = obj.Longitud;
                            idCol = -1;
                            if (obj.TypeData == "varchar")
                            {
                                lbTamanioR1.Visibility = Visibility.Collapsed;
                                lbTa.Visibility = Visibility.Collapsed;
                                txtTamanioR1.Visibility = Visibility.Collapsed;
                            }
                            btnCancelar.IsEnabled = true;
                            btnGuardar.IsEnabled = true;
                            limpiarColAgg();
                        }
                        else
                        {

                            foreach (var item in lista)
                            {
                                if (item.PK)
                                {
                                    pk++;
                                }
                            }
                            if(pk <= 1  )
                            {
                                if (pk ==1 && obj.PK ==true)
                                {
                                    mensaje = "Solo se permite una clave primaria";
                                }
                                else
                                {
                                    lista[idCol].NomColumn = obj.NomColumn;
                                    lista[idCol].TypeData = obj.TypeData;
                                    lista[idCol].PN = obj.PN;
                                    lista[idCol].PK = obj.PK;
                                    lista[idCol].Longitud = obj.Longitud;
                                    idCol = -1;
                                    if (obj.TypeData == "varchar")
                                    {
                                        lbTamanioR1.Visibility = Visibility.Collapsed;
                                        lbTa.Visibility = Visibility.Collapsed;
                                        txtTamanioR1.Visibility = Visibility.Collapsed;
                                    }
                                    btnCancelar.IsEnabled = true;
                                    btnGuardar.IsEnabled = true;
                                    limpiarColAgg();
                                }
                            }
                            else
                            {
                                mensaje = "Solo se permite una clave primaria";
                            }                         
                        }                                       
                    }
                    else
                    {
                        foreach (var item in lista)
                        {
                            mensaje += txtCNombre.Text.ToLower() == item.NomColumn.ToLower() ? "El nombre de la columna ya existe \n" : "";
                            if (chkPK.IsChecked.Value && item.PK)
                            {
                                mensaje = "solo puede asignar una clave primaria \n";
                                break;
                            }
                        }

                        if (mensaje == "")
                        {
                            lista.Add(obj);
                            limpiarColAgg();
                        }
                    }
                }

                if (mensaje != "")
                {
                    MessageBox.Show(mensaje, "Alerta");
                }

            }
            catch (Exception Ex)
            {
                Log.Error(Ex.Message);
                Console.WriteLine(Ex.Message);
            }
        }
        private void limpiarCampos()
        {
            lista.Clear();
            ICollectionView view = CollectionViewSource.GetDefaultView(lista);
            view.Refresh();
            lvMAvanzado.ItemsSource = lista;
            txtbNombreTabla.Clear();
            txtCNombre.Clear();
            cbTD.Text = "";
            cbTD.SelectedIndex = -1;
            chkPK.IsChecked = false;
            chkPN.IsChecked = false;
            txtTamanioR1.Text = "";

        }
        private void limpiarColAgg()
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(lista);
            view.Refresh();
            lvMAvanzado.ItemsSource = lista;
            txtCNombre.Clear();
            cbTD.Text = "";
            cbTD.SelectedIndex = -1;
            chkPK.IsChecked = false;
            chkPN.IsChecked = false;
            txtTamanioR1.Text = "";
        }

        private void quitarColumna_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var x = btn.DataContext as data;
            lista.Remove(x);
            btnCancelar.IsEnabled = true;
            btnGuardar.IsEnabled = true;
            ICollectionView view = CollectionViewSource.GetDefaultView(lista);
            view.Refresh();
            lvMAvanzado.ItemsSource = lista;
        }

        private void cmbMostrarOcultarTamanio_Selected(object sender, SelectionChangedEventArgs e)
        {
            var tipoDato = sender as ComboBox;

            if (tipoDato.SelectedIndex == 0)
            {
                lbTamanioR1.Visibility = Visibility.Visible;
                lbTa.Visibility = Visibility.Visible;
                txtTamanioR1.Visibility = Visibility.Visible;
            }
            else
            {
                lbTamanioR1.Visibility = Visibility.Collapsed;
                lbTa.Visibility = Visibility.Collapsed;
                txtTamanioR1.Visibility = Visibility.Collapsed;
            }

        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            string messageBoxText = "¿Está seguro de crear éste maestro avanzado?";
            string caption = "Crear maestro avanzado";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Information;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);
            svcMOM.MasterOfMasterSvcClient svc = new svcMOM.MasterOfMasterSvcClient();

            bool flag = ComprobarNombreColumna(txtbNombreTabla.Text);

            string mensaje = "";
            var listaMA = svc.ListarMAestrosAvanzados();
            int countMA = 0;

            if (flag)
            {
                switch (result)
                {
                    case MessageBoxResult.Yes:

                        int ClavesPrimarias = 0;
                        try
                        {
                            if ((Regex.IsMatch(txtCNombre.Text, @"^[A-Z][0-9a-zA-Z_]+$")))
                            {
                                mensaje = "El nombre del maestro avanzado no es válido ";
                            }
                            else
                            {
                                if (txtbNombreTabla.Text == "")
                                {
                                    MessageBox.Show("Ingrese nombre del maestro avanzado", "Alerta");
                                }
                                else
                                {
                                    if (lista.Count < 2)
                                    {
                                        mensaje = "Los maestros avanzados deben tener como mínimo dos columnas";
                                    }
                                    else
                                    {
                                        if (chkPN.IsChecked.Value == true && chkPK.IsChecked.Value == true)
                                        {
                                            mensaje += "La clave primaria no puede permitir valores nulos \n";
                                        }
                                        else
                                        {
                                            if ((chkPK.IsChecked.Value == true) && !(cbTD.Text == "bigint" || cbTD.Text == "int"))
                                            {
                                                mensaje += "La clave primaria solo puede ser Int o BigInt \n";
                                            }
                                            else
                                            {
                                                // validaciones PK
                                                foreach (var item in lista)
                                                {
                                                    if (item.PK)
                                                    {
                                                        ClavesPrimarias++;
                                                        if (!(item.TypeData == "bigint" || item.TypeData == "int"))
                                                        {
                                                            mensaje = "La clave primaria solo puede ser de tipo BigInt o Int";
                                                            break;
                                                        }
                                                        if (item.PN)
                                                        {
                                                            mensaje = "La clave primaria no puede permitir valores nulos";
                                                        }                                                       
                                                    }
                                                }
                                                if (ClavesPrimarias == 0)
                                                {
                                                    mensaje = "Debe asignar una clave primaria \n";
                                                }
                                                else
                                                {
                                                    if (ClavesPrimarias > 1)
                                                    {
                                                        mensaje += "Solo se puede tener una clave primaria, tiene: " + ClavesPrimarias + "\n";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                                               
                            // fin validaciones pk

                            if (mensaje == "" && ClavesPrimarias == 1 && txtbNombreTabla.Text != "")
                            {

                                Tabla datos = new Tabla();
                                datos.NombreTabla = txtbNombreTabla.Text;
                                datos.Atributo = "";
                                foreach (var item in listaMA)
                                {
                                    if (item.NombreTabla == datos.NombreTabla)
                                    {
                                        countMA++;
                                    }
                                }
                                if (countMA > 0)
                                {
                                    MessageBox.Show("Ya existe un maestro avanzado con este nombre", "Alerta");
                                }
                                else
                                {
                                    foreach (var item in lista)
                                    {
                                        datos.Atributo += item.NomColumn +
                                                          " " +
                                                          item.TypeData;
                                        if (!item.PN)
                                        {
                                            datos.Atributo += " Not Null ";
                                        }
                                        if (item.PK)
                                        {
                                            datos.Atributo += " PRIMARY KEY IDENTITY(1,1) ";
                                        }

                                        if (item != lista[lista.Count - 1])
                                        {
                                            datos.Atributo += " , ";
                                        }

                                    }

                                    this.Cursor = Cursors.Wait;
                                    do
                                    {
                                        
                                        e.Handled = true;
                                        mensaje = svc.CrearTabla(datos);
                                        this.Cursor = Cursors.Arrow;
                                    } while (this.Cursor.ToString() == "Wait");

                            
                                    pw.CargarMenus();
                                    if (!mensaje.Contains("Error"))
                                    {
                                        limpiarCampos();
                                    }
                                    MessageBox.Show(mensaje, "Información");
                                }
                            }
                            else
                            {
                                MessageBox.Show(mensaje, "Alerta");
                            }


                        }
                        catch (Exception Ex)
                        {
                            Log.Error(Ex.Message);
                            Console.WriteLine(Ex.Message);
                        }

                        break;

                    case MessageBoxResult.No:
                        break;

                    default:
                        break;
                }
            }
            else
            {
                MessageBox.Show("El nombre del maestro avanzado no es válido", "Alerta");
            }

        }

        private void txtbNombreTabla_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox txt = sender as TextBox;
            int ascci = Convert.ToInt32(Convert.ToChar(e.Text));


            if (txt.Text == "")
            {

                if (ascci >= 65 && ascci <= 90 || ascci >= 97 && ascci <= 122 || ascci == 64 || ascci == 95 || ascci == 35)
                    e.Handled = false;
                else
                    e.Handled = true;
            }
            else
            {

                if ((txtbNombreTabla.Text.Length <= 24) && (ascci >= 48 && ascci <= 57 || ascci >= 65 && ascci <= 90 || ascci >= 97 && ascci <= 122))
                    e.Handled = false;
                else
                    e.Handled = true;

            }


        }

        private bool ComprobarNombreColumna(string nombreCol)
        {
            bool flag = true;

            nombreCol = nombreCol.ToUpper();
            List<string> nombresSql = new List<string>();
            nombresSql.Add("ID");
            nombresSql.Add("NEW");
            nombresSql.Add("IDENTITY");
            nombresSql.Add("@@IDENTITY");
            nombresSql.Add("ENCRYPTION");
            nombresSql.Add("ORDER");
            nombresSql.Add("ADD");
            nombresSql.Add("END");
            nombresSql.Add("OUTER");
            nombresSql.Add("ALL");
            nombresSql.Add("ERRLVL");
            nombresSql.Add("OVER");
            nombresSql.Add("ALTER");

            nombresSql.Add("ESCAPE");

            nombresSql.Add("PERCENT");

            nombresSql.Add("AND");

            nombresSql.Add("EXCEPT");

            nombresSql.Add("PLAN");

            nombresSql.Add("ANY");

            nombresSql.Add("EXEC");

            nombresSql.Add("PRECISION");

            nombresSql.Add("AS");

            nombresSql.Add("EXECUTE");

            nombresSql.Add("PRIMARY");

            nombresSql.Add("ASC");

            nombresSql.Add("EXISTS");

            nombresSql.Add("PRINT");

            nombresSql.Add("AUTHORIZATION");

            nombresSql.Add("EXIT");

            nombresSql.Add("PROC");

            nombresSql.Add("AVG");

            nombresSql.Add("EXPRESSION");

            nombresSql.Add("PROCEDURE");

            nombresSql.Add("BACKUP");

            nombresSql.Add("FETCH");

            nombresSql.Add("PUBLIC");

            nombresSql.Add("BEGIN");

            nombresSql.Add("FILE");

            nombresSql.Add("RAISERROR");

            nombresSql.Add("BETWEEN");

            nombresSql.Add("FILLFACTOR");

            nombresSql.Add("READ");

            nombresSql.Add("BREAK");

            nombresSql.Add("FOR");

            nombresSql.Add("READTEXT");

            nombresSql.Add("BROWSE");

            nombresSql.Add("FOREIGN");

            nombresSql.Add("RECONFIGURE");

            nombresSql.Add("BULK");

            nombresSql.Add("FREETEXT");

            nombresSql.Add("REFERENCES");

            nombresSql.Add("BY");

            nombresSql.Add("FREETEXTTABLE");

            nombresSql.Add("REPLICATION");

            nombresSql.Add("CASCADE");

            nombresSql.Add("FROM");

            nombresSql.Add("RESTORE");

            nombresSql.Add("CASE");

            nombresSql.Add("FULL");

            nombresSql.Add("RESTRICT");

            nombresSql.Add("CHECK");

            nombresSql.Add("FUNCTION");

            nombresSql.Add("RETURN");

            nombresSql.Add("CHECKPOINT");

            nombresSql.Add("GOTO");

            nombresSql.Add("REVOKE");

            nombresSql.Add("CLOSE");

            nombresSql.Add("GRANT");

            nombresSql.Add("RIGHT");

            nombresSql.Add("CLUSTERED");

            nombresSql.Add("GROUP");

            nombresSql.Add("ROLLBACK");

            nombresSql.Add("COALESCE");

            nombresSql.Add("HAVING");

            nombresSql.Add("ROWCOUNT");

            nombresSql.Add("COLLATE");

            nombresSql.Add("HOLDLOCK");

            nombresSql.Add("ROWGUIDCOL");

            nombresSql.Add("COLUMN");

            nombresSql.Add("IDENTITY");

            nombresSql.Add("RULE");

            nombresSql.Add("COMMIT");

            nombresSql.Add("IDENTITY_INSERT");

            nombresSql.Add("SAVE");

            nombresSql.Add("COMPUTE");

            nombresSql.Add("IDENTITYCOL");

            nombresSql.Add("SCHEMA");

            nombresSql.Add("CONSTRAINT");

            nombresSql.Add("IF");

            nombresSql.Add("SELECT");

            nombresSql.Add("CONTAINS");

            nombresSql.Add("IN");

            nombresSql.Add("SESSION_USER");

            nombresSql.Add("CONTAINSTABLE");

            nombresSql.Add("INDEX");

            nombresSql.Add("SET");

            nombresSql.Add("CONTINUE");

            nombresSql.Add("INNER");

            nombresSql.Add("SETUSER");

            nombresSql.Add("CONVERT");

            nombresSql.Add("INSERT");

            nombresSql.Add("SHUTDOWN");

            nombresSql.Add("COUNT");

            nombresSql.Add("INTERSECT");

            nombresSql.Add("SOME");

            nombresSql.Add("CREATE");

            nombresSql.Add("INTO");

            nombresSql.Add("STATISTICS");

            nombresSql.Add("CROSS");

            nombresSql.Add("IS");

            nombresSql.Add("SUM");

            nombresSql.Add("CURRENT");

            nombresSql.Add("JOIN");

            nombresSql.Add("SYSTEM_USER");

            nombresSql.Add("CURRENT_DATE");

            nombresSql.Add("KEY");

            nombresSql.Add("TABLE");

            nombresSql.Add("CURRENT_TIME");

            nombresSql.Add("KILL");

            nombresSql.Add("TEXTSIZE");

            nombresSql.Add("CURRENT_TIMESTAMP");

            nombresSql.Add("LEFT");

            nombresSql.Add("THEN");

            nombresSql.Add("CURRENT_USER");

            nombresSql.Add("LIKE");

            nombresSql.Add("TO");

            nombresSql.Add("CURSOR");

            nombresSql.Add("LINENO");

            nombresSql.Add("TOP");

            nombresSql.Add("DATABASE");

            nombresSql.Add("LOAD");

            nombresSql.Add("TRAN");

            nombresSql.Add("DATABASEPASSWORD");

            nombresSql.Add("MAX");

            nombresSql.Add("TRANSACTION");

            nombresSql.Add("DATEADD");

            nombresSql.Add("MIN");

            nombresSql.Add("TRIGGER");

            nombresSql.Add("DATEDIFF");

            nombresSql.Add("NATIONAL");

            nombresSql.Add("TRUNCATE");

            nombresSql.Add("DATENAME");

            nombresSql.Add("NOCHECK");

            nombresSql.Add("TSEQUAL");

            nombresSql.Add("DATEPART");

            nombresSql.Add("NONCLUSTERED");

            nombresSql.Add("UNION");

            nombresSql.Add("DBCC");

            nombresSql.Add("NOT");

            nombresSql.Add("UNIQUE");

            nombresSql.Add("DEALLOCATE");

            nombresSql.Add("NULL");

            nombresSql.Add("UPDATE");

            nombresSql.Add("DECLARE");

            nombresSql.Add("NULLIF");

            nombresSql.Add("UPDATETEXT");

            nombresSql.Add("DEFAULT");

            nombresSql.Add("OF");

            nombresSql.Add("USE");

            nombresSql.Add("DELETE");

            nombresSql.Add("OFF");

            nombresSql.Add("USER");

            nombresSql.Add("DENY");

            nombresSql.Add("OFFSETS");

            nombresSql.Add("VALUES");

            nombresSql.Add("DESC");

            nombresSql.Add("ON");

            nombresSql.Add("VARYING");

            nombresSql.Add("DISK");

            nombresSql.Add("OPEN");

            nombresSql.Add("VIEW");

            nombresSql.Add("DISTINCT");

            nombresSql.Add("OPENDATASOURCE");

            nombresSql.Add("WAITFOR");

            nombresSql.Add("DISTRIBUTED");

            nombresSql.Add("OPENQUERY");

            nombresSql.Add("WHEN");

            nombresSql.Add("DOUBLE");

            nombresSql.Add("OPENROWSET");

            nombresSql.Add("WHERE");

            nombresSql.Add("DROP");

            nombresSql.Add("OPENXML");

            nombresSql.Add("WHILE");

            nombresSql.Add("DUMP");

            nombresSql.Add("OPTION");

            nombresSql.Add("WITH");

            nombresSql.Add("ELSE");

            nombresSql.Add("OR");

            nombresSql.Add("WRITETEXT");
            foreach (var item in nombresSql)
            {
                if (item == nombreCol)
                {
                    flag = false;
                }
            }
            return flag;
        }


        private void txtCNombre_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            TextBox txt = sender as TextBox;
            int ascci = Convert.ToInt32(Convert.ToChar(e.Text));

            if (txt.Text == "")
            {

                if (ascci >= 65 && ascci <= 90 || ascci >= 97 && ascci <= 122 || ascci == 95)
                    e.Handled = false;
                else
                    e.Handled = true;
            }


            if ((txtCNombre.Text.Length <= 24) && ((ascci >= 48 && ascci <= 57 || ascci >= 65 && ascci <= 90 || ascci >= 97 && ascci <= 122 || ascci == 95)))
                e.Handled = false;
            else
                e.Handled = true;


        }

        // @Autor: Ruben Ballesteros  
        // @fecha:28/09/2018
        // @Descripcion: Metodo que Elimina el registro de todos los campos en la creacion de Maestros Avanzados.
        //  @Notas: Le da funcionalidad al boton "Cancelar".
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            string messageBoxText = "¿Está seguro qué desea limpiar los campos?";
            string caption = "Crear maestro avanzado";
            MessageBoxButton button = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Information;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    lista.Clear();
                    ICollectionView view = CollectionViewSource.GetDefaultView(lista);
                    view.Refresh();
                    lvMAvanzado.ItemsSource = lista;
                    txtbNombreTabla.Clear();
                    txtCNombre.Clear();
                    cbTD.Text = "";
                    cbTD.SelectedIndex = -1;
                    chkPK.IsChecked = false;
                    chkPN.IsChecked = false;
                    break;
                case MessageBoxResult.No:
                    break;
            }

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Content = new uiIndex();
        }

        /// <summary>
        /// @Fecha:1/10/2018
        /// @Descripcion: Elimina los espacios en blanco al comienzo y al final cuando está fuera de foco 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnModCol_Click(object sender, RoutedEventArgs e)
        {
            idCol = -1;
            var btn = sender as Button;
            var x = btn.DataContext as data;
            idCol = lista.IndexOf(x);
            if (x.TypeData.Contains("varchar"))
            {
                lbTamanioR1.Visibility = Visibility.Visible;
                lbTa.Visibility = Visibility.Visible;
                txtTamanioR1.Visibility = Visibility.Visible;
            }
            txtCNombre.Text = x.NomColumn;

            cbTD.Text = x.TypeData.Contains("varchar") ? "varchar" : x.TypeData;


            if (x.TypeData.Contains("varchar(MAX)") )
            {
                cbTD.Text = "varchar(MAX)";
            }
            if (x.TypeData.Contains("binary(50)"))
            {
                cbTD.Text = "binary(50)";
            }
            if (x.TypeData.Contains("binary(50)"))
            {
                cbTD.Text = "binary(50)";
            }
            if (x.TypeData.Contains("char(10)"))
            {
                cbTD.Text = "char(10)";
            }
            if (x.TypeData.Contains("datetime2(7)"))
            {
                cbTD.Text = "datetime2(7)";
            }
            if (x.TypeData.Contains("datetimeoffset(7)"))
            {
                cbTD.Text = "datetimeoffset(7)";
            }
            if (x.TypeData.Contains("decimal(18, 0)"))
            {
                cbTD.Text = "decimal(18, 0)";
            }
            if (x.TypeData.Contains("nchar(10)"))
            {
                cbTD.Text = "nchar(10)";
            }
            if (x.TypeData.Contains("numeric(18, 0)"))
            {
                cbTD.Text = "numeric(18, 0)";
            }
            if (x.TypeData.Contains("nvarchar(50)"))
            {
                cbTD.Text = "nvarchar(50)";
            }
            if (x.TypeData.Contains("nvarchar(MAX)"))
            {
                cbTD.Text = "nvarchar(MAX)";
            }
            if (x.TypeData.Contains("time(7)"))
            {
                cbTD.Text = "time(7)";
            }
            if (x.TypeData.Contains("varbinary(50)"))
            {
                cbTD.Text = "varbinary(50)";
            }
            if (x.TypeData.Contains("varbinary(MAX)"))
            {
                cbTD.Text = "varbinary(MAX)";
            }

            chkPK.IsChecked = x.PK;
            chkPN.IsChecked = x.PN;
            txtTamanioR1.Text = x.Longitud;
        }


        private void txtTamanioR1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            var s = txtTamanioR1.Text.Length;
            Regex _regex = new Regex("[^0-9]+");


            e.Handled = _regex.IsMatch(e.Text);


        }

        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy ||
                e.Command == ApplicationCommands.Cut ||
                e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }

        private void txtCNombre_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Solo se admiten caracteres de [A - Z] o [0 - 9]." + Environment.NewLine + "El nombre de la Tabla debe tener una longitud mínima de tres (3) caracteres." + Environment.NewLine + "El nombre de la tabla debe comenzar con mayuscula." + Environment.NewLine + "El nombre de la columna debe tener una longitud mínima de dos (2) caracteres." + Environment.NewLine + "Los nombres deben tener una longitud máxima de veinticinco (25) caracteres." + Environment.NewLine + "Los nombres no pueden empezar con un carácter numérico (0 - 9)." + Environment.NewLine + "Los nombres deben contener al menos un (1) carácter alfabético." + Environment.NewLine + "Los nombres no pueden iniciar con un espacio en blanco.", "Por favor tome en cuenta las siguientes reglas para ingresar los datos. ");
        }


    }
    public class data
    {
        public string NomColumn { get; set; }
        public string TypeData { get; set; }
        public string Longitud { get; set; }
        public bool PN { get; set; }
        public bool PK { get; set; }
    }


}
