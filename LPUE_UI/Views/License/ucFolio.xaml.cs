using System.Windows;
using System.Windows.Controls;

namespace PUE.Views.License
{
    /// <summary>
    /// Lógica de interacción para ucFolio.xaml
    /// </summary>
    public partial class ucFolio : UserControl
    {
        public ucFolio()
        {
            InitializeComponent();
        }

        private void btnNo(object sender, RoutedEventArgs e)
        {
            string NumOficio = txtOficio.Text;
            Controllers.CaptureInforLic.FolioExce = NumOficio;

            var vari = Window.GetWindow(this);
            vari.Close();
        }
    }
}
