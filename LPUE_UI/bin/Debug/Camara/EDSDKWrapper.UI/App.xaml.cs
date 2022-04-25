using System.Windows;

namespace EDSDKWrapper.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public bool on_off { get; set; }
        public App()
        {
            this.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(
                (s, e) =>
                {
                    string message = e.Exception.ToString();

                    MessageBox.Show(message);

                    e.Handled = true;
                });
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var view = new MainView();
            // var forma = new Form1();
            var viewModel = new MainViewModel(e.ToString());
            view.DataContext = viewModel;
            view.Show();
        }
    }
}
