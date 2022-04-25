using log4net;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PUE_UI.Views.Login
{


    /// <summary>
    /// Lógica de interacción para ucLogin.xaml
    /// </summary>
    public partial class ucLogin : UserControl
    {
        public delegate void EventHandlerLoginClic(object sender, EventArgs e, PUE.Controllers.Login.ButtonType buttonType, String UserName, String Password);


        private static readonly ILog loggerLog = LogManager.GetLogger(typeof(ucLogin));


        public event EventHandlerLoginClic onLoginClick;

        private String _UserName = "",
                        _Password = "";

        public String DetError
        {
            set { lblLoginError.Content = value; }
        }

        public ucLogin()
        {
            InitializeComponent();
            MakeDraggable(this, this);

            string LogFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "log", "Login");
            if (!Directory.Exists(LogFile))
            {
                Directory.CreateDirectory(LogFile);
            }
            string LogFileName = Path.Combine(LogFile, "LogFile_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".log");
            PUE.Controllers.PUELogger.Setup(LogFileName);

            loggerLog.Info(DateTime.Now.ToString("yyyyMMddmmss") + " ucLogin " + "---Ejecución de la aplicación");

        }


        private void Access(object sender, RoutedEventArgs e)
        {

            loggerLog.Info(DateTime.Now.ToString("yyyyMMddmmss") + " Access " + "---Log in con botón ");

            oLabelName.Text = "";
            try
            {
                HttpClient client = new HttpClient();

                string url = ConfigurationManager.AppSettings["LinkPublish"].ToString() + "?pg=AppLogin&usname=" + txtUser.Text + "&uspwd=" + psbPasswordUser.Password;
                var response = new WebClient().DownloadString(url);

                dynamic data = JObject.Parse(response);

                string msg = data.msg;
                string v = data.v;

                if (v != "no")
                {
                    loggerLog.Info(DateTime.Now.ToString("yyyyMMddmmss") + " psbPasswordUser_KeyDown " + "---Se logueó bien");

                    Application.Current.Properties["usuarioId"] = data.UsuarioId;
                    Application.Current.Properties["ModuloId"] = data.Modulo;
                    getDataUserLogin();
                    txtUser.Focus();
                    this.onLoginClick(this, new EventArgs(), PUE.Controllers.Login.ButtonType.ACCEPT, this._UserName, this._Password);
                }
                else
                {
                    loggerLog.Info(DateTime.Now.ToString("yyyyMMddmmss") + " psbPasswordUser_KeyDown " + "---error de logueo--- error: " + msg);
                    oLabelName.Text = msg;
                }
            }
            catch (Exception l)
            {
                loggerLog.Error(DateTime.Now.ToString("yyyyMMddmmss") + " ERROR!!!" + "---No se pudo validar el usuario ni la contraseña--- Descripción--> " + l.Message);
                MessageBoxResult result = MessageBox.Show("No hay conexión");
                return;
            }




        }

        private void loginEvent_Click(object sender, RoutedEventArgs e)
        {
            loggerLog.Info(DateTime.Now.ToString("yyyyMMddmmss") + " loginEvent_Click " + "---Botón para cerrar la aplicación");
            PUE.Controllers.Login.ButtonType typeClicked = PUE.Controllers.Login.ButtonType.CLOSE;
            getDataUserLogin();
            this.onLoginClick(this, new EventArgs(), typeClicked, this._UserName, this._Password);
        }

        private void psbPasswordUser_KeyDown(object sender, KeyEventArgs e)
        {
            loggerLog.Info(DateTime.Now.ToString("yyyyMMddmmss") + " psbPasswordUser_KeyDown " + "---Función KeyDown en contrasenia");
            if (e.Key == Key.Enter)
            {
                loggerLog.Info(DateTime.Now.ToString("yyyyMMddmmss") + " psbPasswordUser_KeyDown " + "---Inicio de sesión con Enter");
                try
                {
                    oLabelName.Text = "";
                    HttpClient client = new HttpClient();

                    string url = ConfigurationManager.AppSettings["LinkPublish"].ToString() + "?pg=AppLogin&usname=" + txtUser.Text + "&uspwd=" + psbPasswordUser.Password;
                    var response = new WebClient().DownloadString(url);

                    dynamic data = JObject.Parse(response);

                    string msg = data.msg;
                    string v = data.v;

                    if (v != "no")
                    {
                        loggerLog.Info(DateTime.Now.ToString("yyyyMMddmmss") + " psbPasswordUser_KeyDown " + "---Se logueó bien");
                        Application.Current.Properties["usuarioId"] = data.UsuarioId;
                        Application.Current.Properties["ModuloId"] = data.Modulo;
                        getDataUserLogin();
                        txtUser.Focus();
                        this.onLoginClick(this, new EventArgs(), PUE.Controllers.Login.ButtonType.ACCEPT, this._UserName, this._Password);
                    }
                    else
                    {
                        loggerLog.Info(DateTime.Now.ToString("yyyyMMddmmss") + " psbPasswordUser_KeyDown " + "--- error de logueo--- Descripción: " + msg);
                        oLabelName.Text = msg;
                    }
                }
                catch (Exception err)
                {
                    loggerLog.Error(DateTime.Now.ToString("yyyyMMddmmss") + " ERROR!!!" + "---No se pudo validar el usuario ni la contraseña--- Descripción--> " + err.Message);
                    string d = err.ToString();
                    MessageBoxResult result = MessageBox.Show("No hay conexión");
                    return;
                }
            }
        }

        private void getDataUserLogin()
        {
            loggerLog.Info(DateTime.Now.ToString("yyyyMMddmmss") + " getDataUserLogin " + "---Se logueó el usuario");
            this._UserName = txtUser.Text;
            this._Password = psbPasswordUser.Password;
            txtUser.Text = String.Empty;
            psbPasswordUser.Password = String.Empty;
        }

        private void txtUser_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        #region MAKE DG CONTROL
        //---------------------------------------/////////////////////////////////////////////
        public void MakeDraggable(System.Windows.UIElement moveThisElement, System.Windows.UIElement movedByElement)
        {

            loggerLog.Info(DateTime.Now.ToString("yyyyMMddmmss") + " MakeDraggable " + "---Función de arrastre de la app");
            Point? OriginalWindowPoint = new Point?();

            bool isMousePressed = false;
            System.Windows.Media.TranslateTransform transform = new System.Windows.Media.TranslateTransform(0, 0);

            moveThisElement.RenderTransform = transform;

            System.Windows.Point originalPoint = new System.Windows.Point(0, 0), currentPoint;

            movedByElement.MouseLeftButtonDown += (a, b) =>
            {
                isMousePressed = true;
                originalPoint = ((System.Windows.Input.MouseEventArgs)b).GetPosition(moveThisElement);
            };

            movedByElement.MouseLeftButtonUp += (a, b) => isMousePressed = false;

            movedByElement.MouseLeave += (a, b) => isMousePressed = false;

            movedByElement.MouseMove += (a, b) =>
            {
                if (!OriginalWindowPoint.HasValue)
                    OriginalWindowPoint = this.TransformToAncestor(Window.GetWindow(this)).Transform(new Point(0, 0));

                if (!isMousePressed) return;

                currentPoint = ((System.Windows.Input.MouseEventArgs)b).GetPosition(moveThisElement);

                Size windowSize = new Size(((FrameworkElement)(Window.GetWindow(this).Content)).ActualWidth, ((FrameworkElement)(Window.GetWindow(this).Content)).ActualHeight);

                if (OriginalWindowPoint.Value.X + +transform.X + currentPoint.X - originalPoint.X > windowSize.Width - this.ActualWidth)
                    transform.X = 0 - OriginalWindowPoint.Value.X + windowSize.Width - this.ActualWidth;
                else
                    if (OriginalWindowPoint.Value.X + transform.X + currentPoint.X - originalPoint.X < 0)
                    transform.X = 0 - OriginalWindowPoint.Value.X;
                else
                    transform.X += currentPoint.X - originalPoint.X;

                if (OriginalWindowPoint.Value.Y + transform.Y + currentPoint.Y - originalPoint.Y > windowSize.Height - this.ActualHeight)
                    transform.Y = 0 - OriginalWindowPoint.Value.Y + windowSize.Height - this.ActualHeight;
                else
                    if (OriginalWindowPoint.Value.Y + transform.Y + currentPoint.Y - originalPoint.Y < 0)
                    transform.Y = 0 - OriginalWindowPoint.Value.Y;
                else
                    transform.Y += currentPoint.Y - originalPoint.Y;
            };
        }
        #endregion
    }
}
