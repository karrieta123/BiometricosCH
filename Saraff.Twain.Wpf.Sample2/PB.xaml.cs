using System.Windows;

namespace Saraff.Twain.Wpf.Sample2
{
    /// <summary>
    /// Interaction logic for PB.xaml
    /// </summary>
    public partial class PB : Window
    {
        public PB()
        {
            Loaded += PB_Loaded;
            InitializeComponent();
        }

        private void PB_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
