using DataPUE;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PUE.Views.License
{
    /// <summary>
    /// Lógica de interacción para Search.xaml
    /// </summary>
    public partial class Search : Window
    {
        public List<Colonia> DataColonias = null;
        public static List<Colonia> Colonias = null;
        public static string CodigoPostal { get; set; }
        private I_SEARCH se = new I_SEARCH();
        public string Municipio { get; set; }
        public string Localidad { get; set; }
        public Search()
        {
            InitializeComponent();
            //Loaded += Search_Loaded;
        }

        private void Search_Loaded(object sender, RoutedEventArgs e)
        {
            List<Colonia> res = se.Colonias(Municipio, Localidad);
            dataGrid.ItemsSource = res;
        }

        internal void Loader(string muni, string loca)
        {
            Municipio = muni;
            Localidad = loca;

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var grid = sender as DataGrid;
            var colaa = grid.SelectedItems;
            if (grid.SelectedIndex == DataColonias.Count)
            {
                return;
            }
            Colonias = null;
            Colonias = new List<Colonia>();
            foreach (Colonia items in colaa)
            {
                Colonias.Add(new Colonia
                {
                    CVE_COL = items.CVE_COL.ToString(),
                    CVE_MPIO = null,
                    Descripcion = items.Descripcion.ToString(),
                    CODIGO_POSTAL = items.CODIGO_POSTAL.ToString()
                });
                //CodigoPostal = items.CODIGO_POSTAL.ToString();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }

        private void txtcolonia_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                dataGrid.ItemsSource = null;
                DataColonias = se.SearchForColonia(Municipio, Localidad, txtcolonia.Text);
                dataGrid.ItemsSource = DataColonias;
            }
        }

        private void txtcolonia_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //this.Close();
        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}