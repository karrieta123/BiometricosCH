using Entidades;
using PUE.Views.Shared;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using CntLogin = PUE.Controllers.Login;
using CntMenu = PUE.Controllers.Menu;

namespace PUE_UI.Views.Home
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region USER CONTROLS
        Login.ucLogin _ucLogin;
        Shared.ucMenu _ucMenu;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.WorkArea.Height;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //checa si hay otra version

            var version = ConfigurationManager.AppSettings["version"];
            EntiVersion ResVersion = Format.CheckVersion(version);
            if (ResVersion.HayNuevaVersion)
            {
                MessageBox.Show("Existe una nueva versión del sistema, está se descargará automaticamente, espere.");
                WriteConfig(ResVersion.version);
                arrancaProceso();
                var vari = Window.GetWindow(this);
                vari.Close();
            }
            else
                loadLogin();
        }

        #region Escribimos en el config la nueva version
        private void WriteConfig(string version)
        {
            string PathConfig = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string ConfigFile = System.IO.Path.Combine(PathConfig, "PUE.exe.config");
            ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
            configMap.ExeConfigFilename = ConfigFile;
            System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
            config.AppSettings.Settings["version"].Value = version;
            config.Save();
        }
        #endregion

        #region UC - LOGIN
        /// <summary>
		/// Ini Login UC
		/// </summary>
		void loadLogin()
        {
            _ucLogin = new Login.ucLogin();
            _ucLogin.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            _ucLogin.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            _ucLogin.onLoginClick += new Login.ucLogin.EventHandlerLoginClic(clicLoginEvent);
            this.grdContent.Children.Add(_ucLogin);
        }


        private void arrancaProceso()
        {
            Process[] namePHuella = Process.GetProcessesByName("Finger");
            if (namePHuella.Length == 0)
            {
                ProcessStartInfo pro = new ProcessStartInfo();
                string path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
                pro.FileName = System.IO.Path.Combine(path, "VersionExec.exe");
                Process process = Process.Start(pro);
            }
        }
        /// <summary>
        /// Event Clic - Login
        /// </summary>
        /// <param name="sender">uc</param>
        /// <param name="e">arguments</param>
        /// <param name="buttonClic">button clicked</param>
        void clicLoginEvent(object sender, EventArgs e, CntLogin.ButtonType buttonClick, String UserName, String Password)
        {
            switch (buttonClick)
            {
                case CntLogin.ButtonType.ACCEPT:

                    //ESTE ES EL CÓDIGO CORRECTO
                    //if (!CntLogin.ValidateUser(UserName, Password))
                    //{
                    //    if (DataPUE.CATALOGOS.cv_mpio == "*")
                    //        _ucLogin.DetError = "El sistema no puede \n conectarse a la BD";
                    //        else
                    //    _ucLogin.DetError = "*Usuario no válido";
                    //}
                    //else
                    //{
                    //    _ucLogin.DetError = String.Empty;
                    //    grdContent.Children.Remove(_ucLogin);
                    //    loadMenu(UserName);
                    //}

                    if (CntLogin.ValidateUser(UserName, Password))
                    {
                        if (DataPUE.CATALOGOS.cv_mpio == "*")
                            _ucLogin.DetError = "El sistema no puede \n conectarse a la BD";
                        else
                            _ucLogin.DetError = "*Usuario no válido";
                    }
                    else
                    {
                        _ucLogin.DetError = String.Empty;
                        grdContent.Children.Remove(_ucLogin);
                        loadMenu(UserName);
                    }

                    break;
                default:
                    Application.Current.Shutdown();
                    break;
            }
        }
        #endregion

        #region UC - MENU
        /// <summary>
        /// Ini UC - MENU
        /// </summary>
        void loadMenu(String UserNameAuth)
        {
            //_ucMenu = new Shared.ucMenu();
            //_ucMenu.Width = this.Width;
            //_ucMenu.Height = this.MaxHeight;

            //_ucMenu.UserNameAuth = UserNameAuth;
            //_ucMenu.onMenuClick += new Shared.ucMenu.EventHandlerMenuClic(clicMenuEvent);
            //grdContent.Children.Add(_ucMenu);
            Busqueda bsq = new Busqueda();
            bsq.Show();
        }

        /// <summary>
        /// Event Clic - Menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="ButtonType"></param>
        void clicMenuEvent(object sender, EventArgs e, CntMenu.ButtonType ButtonType)
        {
            switch (ButtonType)
            {
                case CntMenu.ButtonType.Minimize:
                    this.WindowState = System.Windows.WindowState.Minimized;
                    break;
                case CntMenu.ButtonType.Close:
                    Application.Current.Shutdown();
                    break;
            }
        }
        #endregion
    }
}
