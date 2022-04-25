using System.Windows;
using System.Windows.Controls;

namespace PUE.Views.License
{
    /// <summary>
    /// Interaction logic for ucAviso.xaml
    /// </summary>
    public partial class ucAviso : UserControl
    {
        public static int Sol = 0, foli = 0;
        public ucAviso(int folio, int solicitud)
        {
            InitializeComponent();
            Sol = solicitud;
            foli = folio;
        }

        private void btnSi(object sender, RoutedEventArgs e)
        {
            Controllers.CaptureInforLic.CsCierre(Controllers.CaptureInforLic.idfoli.ToString());
            var vari = Window.GetWindow(this);
            Controllers.CaptureInforLic obj = new Controllers.CaptureInforLic();
            obj.idFolioLaser(Sol, foli);
            vari.Close();
        }

        private void btnNo(object sender, RoutedEventArgs e)
        {
            var vari = Window.GetWindow(this);
            vari.Close();
        }
    }
}
