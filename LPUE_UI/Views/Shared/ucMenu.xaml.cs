namespace PUE_UI.Views.Shared
{
    using System;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media.Animation;
    using System.Diagnostics;
    using System.Windows.Threading;
    using System.Reflection;
    using TgsCard.LicTlaxcala;
    using System.Net.Http;
    using System.Net;
    using log4net;
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Web;
    using System.Configuration;
    using System.Threading;
    using CnPrevisualizacion = PUE.Views.License.ucPrevisualizacion;
    using PUE.Logger;
    using System.Text;
    using PueblaQR;
    using System.Windows.Data;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Data.Sql;
    using System.Data.SqlClient;




    using System.Drawing;
    using System.Drawing.Imaging;
    using ZXing;
    using Newtonsoft.Json;


    /// <summary>
    /// Lógica de interacción para ucMenu.xaml
    /// </summary>
    public partial class ucMenu : UserControl
    {
        public delegate void EventHandlerMenuClic(object sender, EventArgs e, PUE.Controllers.Menu.ButtonType buttonType);
        public event EventHandlerMenuClic onMenuClick;
        PUE.Views.License.ucBiometrics ucBiometricos;
        sigPlusNET_wpfDemo.Window1 ucFirma;
        PUE.Controllers.CaptureInforLic oDataCaptureInfo = new PUE.Controllers.CaptureInforLic();
        CnPrevisualizacion ucPrevisualizacion;
        const String btnHuellas = "btnHuella";
        const String Firmaelectronica = "btnFirma";
        const String ImprimirLicencia = "btnImprimir";
        const String btnFotografias = "btnFotos";
        const String GrabarChip = "btngrabar";
        const String PrevisualizarLicencia = "btnPrevisualizar";
        const string PrevisualizarLicencia1 = "btnPrevisualizar1";
        const String guardarFoto = "btnGuardar";
        const String MostrarFoto = "btnMostrarImg";
        private string ruta = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        bool tramitevalido = false;
        bool imprimir = false;
        static string licencia = "";
        private const int SW_SHOWMAXIMIZED = 3;

        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        private IntPtr handle;
        private Process processABIS;
        static string folioSegui = "";
        string ChipInfo;
        string Nombre;
        string Curp;
        string Nacionalidad;
        string FechaExpedicion;
        string FechaVencimiento;
        string CodigoPostal;
        string Modalidad;
        string Antiguo;
        string dona;
        string tipolic;
        static string conect;
        bool DonacionOrganos;
        static int UsuarioId;
        int Modulo;

        JObject jObject;
        private static readonly ILog loggerPrincipal = LogManager.GetLogger(typeof(ucMenu));
        String usrName = String.Empty,
            fullPathAviso = String.Empty,
            _NumeroDeAviso = String.Empty;

        public String UserNameAuth { get { return this.lblUserNameAuth.Content.ToString(); } set { this.lblUserNameAuth.Content = value; } }

        #region CONSTRUCTOR
        /// <summary>
        /// Constructor ucMenu
        /// </summary>
        public ucMenu()
        {
            string LogFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "log", "Principal");
            if (!Directory.Exists(LogFile))
            {
                Directory.CreateDirectory(LogFile);
            }
            string LogFileName = Path.Combine(LogFile, "LogFile_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".log");
            PUELogger.Setup(LogFileName);

            loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " ucMenu " + "---Constructor página principal");
            InitializeComponent();

            UsuarioId = Convert.ToInt32(Application.Current.Properties["usuarioId"]);
            Modulo = Convert.ToInt32(Application.Current.Properties["ModuloId"]);
            //FechaHoy.Content = "Fecha " + DateTime.Now.ToString("dd/MM/yyyy");
            conect = ConfigurationManager.AppSettings["LinkPublish"].ToString();

        }
        #endregion


        //DATOS DEL QR
        public static string GenerateCode(string numeroLicemcia, string Nombre, string FechaExpedicion, string FechaVencimiento, string Path)
        {
            bool bandera = true;
            if (bandera==true)
            {
                var writer = new BarcodeWriter();
                writer.Format = BarcodeFormat.QR_CODE;

                var result = writer.Write(" El numero de licencia es:" + numeroLicemcia + "\n" + "El nombre es :" + Nombre + "\n" + "Fecha de expedicion:" + FechaExpedicion + "\n" + "Fecha de vencimiento:" + FechaVencimiento);


                var barcodeBitmap = new Bitmap(result);


                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(Path, FileMode.Create, FileAccess.ReadWrite))
                    {
                        barcodeBitmap.Save(memory, ImageFormat.Jpeg);

                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                    }
                }
                return "OK";
            }
            else
            {

                return "NO";
            }
          

        }
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        #region EVENTOS

        /// <summary>
        /// Click en header [Cerrar - Minimizar]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void headerOptions_MouseDown(object sender, MouseButtonEventArgs e)
        {
            loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " headerOptions_MouseDown " + "---minimizar/cerrar aplicación");

            PUE.Controllers.Menu.ButtonType ButtonTypeClcd = new PUE.Controllers.Menu.ButtonType();


            switch (((FrameworkElement)sender).Name)
            {
                case "imgMinimizeWindow":
                    loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " headerOptions_MouseDown " + "---minimizar aplicación");
                    ButtonTypeClcd = PUE.Controllers.Menu.ButtonType.Minimize;
                    break;
                case "imgCloseWindow":
                    loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " headerOptions_MouseDown " + "---cerrar aplicación");
                    ButtonTypeClcd = PUE.Controllers.Menu.ButtonType.Close;
                    break;
            }
            this.onMenuClick(this, new EventArgs(), ButtonTypeClcd);
        }


        #endregion

        public string Imagen_A_Bytes(String ruta1)
        {
        
            System.Drawing.Image image = System.Drawing.Image.FromFile(ruta1);
            MemoryStream m = new MemoryStream();
            image.Save(m, image.RawFormat);
            byte[] imageBytes = m.ToArray();
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;
           
        }

        /*public static  string Obtenercookie() {


            string url2 = "https://qirsolutions.com.mx/api/v1/login";

            var client = new HttpClient();

            Post post = new Post()
            {
                username = "admin",
                password = "12345678"
            };
            string result = "";
            WebRequest orequest = WebRequest.Create(url2);
            orequest.Method = "post";
            orequest.ContentType = "application/json;charset=UTF-8";
            using (var OSW = new StreamWriter(orequest.GetRequestStream()))
            {
                string json2 = JsonConvert.SerializeObject(post);

                OSW.Write(json2);
                OSW.Flush();
                OSW.Close();


            }


            WebResponse oResponse = orequest.GetResponse();

            string[] cookie = oResponse.Headers.GetValues("Set-Cookie");
            using (var oSR = new StreamReader(oResponse.GetResponseStream()))
            {
                result = oSR.ReadToEnd().Trim();


            }
            string cadena = cookie[0];

            return cadena;



        }*/

        public class Post
        {
            public string username { get; set; }
            public string password { get; set; }
        }
        public static string peticionCodeImg(string url ,string imgqr,string nombre, string tipolic,string licencia,string curp ,string FechaExpedicion,string FechaVencimiento,string galleta)
        {

   
            datos data = new datos()
            {
                data = imgqr,
                name = nombre,
                ap_paterno = "",
                ap_materno = "",
                no_licencia = licencia,
                curp = curp,
                tipo = tipolic,
                expedicion = FechaExpedicion,
                vencimiento = FechaVencimiento
            };


            string result = "";
            WebRequest orequest = WebRequest.Create(url);
            orequest.Method = "post";

            orequest.ContentType = "application/json;charset=UTF-8";
            orequest.Headers.Add("Cookie", galleta);


            using (var OSW = new StreamWriter(orequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(data);

                OSW.Write(json);
                OSW.Flush();
                OSW.Close();
            }

            WebResponse oResponse = orequest.GetResponse();
            using (var oSR = new StreamReader(oResponse.GetResponseStream()))
            {
                result = oSR.ReadToEnd().Trim();
            }

            return result;
        }
        public class datos
        {
            public string data { get; set; }
            public string name { get; set; }
            public string ap_paterno { get; set; }
            public string ap_materno { get; set; }

            public string no_licencia { get; set; }
            public string curp { get; set; }

            public string tipo { get; set; }

            public string expedicion { get; set; }
            public string vencimiento { get; set; }
        }

        public class RootJson
        {
            public string img { get; set; }
            public string qr { get; set; }
            public int id { get; set; }
        }

        public static string respuestaqr(string result,string rutas)
        {

            RootJson oObject = JsonConvert.DeserializeObject<RootJson>(result);

            string imag = oObject.img;
            string base64String = oObject.qr;
            int id = oObject.id;
           // var fullOutputPath = @"C:\Users\DC SOLUCIONES\Documents\CODIGOS\testservice.jpg";


            string imagenqr = GetImageqr(rutas, base64String);


            return "ok";

        }

        public  static string GetImageqr(string fullOutputPath, string base64String)
        {

            // Convert byte[] to Image
            try
            {
                byte[] imgByteArray = Convert.FromBase64String(base64String);
                MemoryStream ms = new MemoryStream(imgByteArray);
                Bitmap bmp = new Bitmap(ms);

                bmp.Save(fullOutputPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                //bmp.Save(@"d:\"test.bmp", ImageFormat.Bmp);
                //bmp.Save(@"d:\"test.gif", ImageFormat.Gif);
                //bmp.Save(@"d:\"test.png", ImageFormat.Png);
                ms.Close();
                return "ok";
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        private void MuestraCapturaBiometricos()
        {

            loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " MuestraCapturaBiometricos " + "---ventana capturar biometricos");
            try
            {


                ucBiometricos = new PUE.Views.License.ucBiometrics(licencia, conect, UsuarioId);
                ucBiometricos.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                ucBiometricos.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;

                ucFirma = new sigPlusNET_wpfDemo.Window1(licencia, conect, UsuarioId);

                new Window
                {
                    Title = "Captura de biometricos",
                    WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
                    WindowStyle = System.Windows.WindowStyle.SingleBorderWindow,
                    Height = SystemParameters.WorkArea.Height,
                    Width = SystemParameters.WorkArea.Width,
                    Content = ucBiometricos,
                }.ShowDialog();

            }
            catch (Exception ex)
            {
                loggerPrincipal.Error(DateTime.Now.ToString("yyyyMMddmmss") + " MuestraCapturaBiometricos " + "---ERROR!!--- Descripción ----> " + ex.Message);
                MessageBoxResult result = MessageBox.Show("Hubo un problema en el proceso", "Aviso Importante");
                return;
            }
        }
        async private void GuardarTramite(object sender, RoutedEventArgs e)
        {
            loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " GuardarTramite " + "---Botón para guardar trámite");
            imprimir = false;

            Button btn = (Button)sender;

            if (btn.Name == "btnNo")
            {
                loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " GuardarTramite " + "---Cerró ventana de guardar trámite");
                popup.IsOpen = false;
                return;
            }

            if (foliolasertext.Text != "")
            {
                var clienttr = new HttpClient();

                try
                {
                    Dictionary<string, string> postParameters = new Dictionary<string, string>();
                    postParameters.Add("pg", "LicImpresa");
                    postParameters.Add("Modulo", Modulo.ToString());
                    //postParameters.Add("NumeroLicencia", licencia);
                    postParameters.Add("Folio", foliolasertext.Text);
                    postParameters.Add("UsuarioId", UsuarioId.ToString());
                    postParameters.Add("FolioSeguimiento",folioSegui);

                    var content = new FormUrlEncodedContent(postParameters);

                    var response = await clienttr.PostAsync(conect, content);

                    var responseString = await response.Content.ReadAsStringAsync();

                    var obj = JObject.Parse(responseString);

                    var success = Convert.ToBoolean((string)obj["success"].ToString());
                    var error = (string)obj["errores"].ToString();

                    if (!success)
                    {
                        loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " GuardarTramite " + "---Error!!!! descripcion: " + error);
                        Error.Content = error;
                        return;
                    }
                    loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " GuardarTramite " + "--- Se guardó folio laser a un trámite");
                    popup.IsOpen = false;
                    imprimeCard();
                    imprimir = true;
                }
                catch (Exception ex)
                {
                    loggerPrincipal.Error(DateTime.Now.ToString("yyyyMMddmmss") + " GuardarTramite " + "---ERROR!!! Descripción " + ex.Message);
                }

            }
            else
            {
                loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " GuardarTramite " + "---Error!!! Descripción: Favor de escribir un Folio");
                Error.Content = "Favor de escribir un Folio";
            }

        }
        private void imprimeCard()
        {
            loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " imprimeCard " + "---Función para mandar a imprimir la tarjeta");
            try
            {
                string fiftelemet = PUE.Controllers.PrintLic.jsonImprime(jObject, conect);
                if (fiftelemet != null)
                {
                    if (fiftelemet != "0")
                    {
                        string[] args = new string[5];
                        args[0] = "-n_" + ConfigurationManager.AppSettings["NameDataCard"].ToString();
                        args[1] = "-r_both";
                        args[2] = "-2";
                        args[3] = "-o_frontport";
                        args[4] = fiftelemet;
                        PUELicencia_ImprimeCard.Form1 frm = new PUELicencia_ImprimeCard.Form1(args);
                        frm.ShowDialog();

                        System.Uri resourceLocater = new System.Uri("/PUE;component/views/login/uclogin.xaml", System.UriKind.Relative);

                    }
                    else
                    {
                        loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " imprimeCard " + "---Hay un error con los datos de impresión");
                    }
                }
                else
                {
                    loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " imprimeCard " + "---No se obtuvieron los datos de impresión");
                }
            }
            catch (Exception ex)
            {
                loggerPrincipal.Error(DateTime.Now.ToString("yyyyMMddmmss") + " imprimeCard " + "---ERROR!!! Descripción: " + ex.Message);
            }
        }
        private void MuestraPrevisualizacion(string id)
        {
            loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " MuestraPrevisualizacion " + "---Botón de previsualización");
            try
            {
                ucPrevisualizacion = new PUE.Views.License.ucPrevisualizacion(id, jObject, conect, UsuarioId);
                ucPrevisualizacion.HorizontalAlignment = HorizontalAlignment.Stretch;
                ucPrevisualizacion.VerticalAlignment = VerticalAlignment.Stretch;

                new Window
                {
                    Title = "Previsualización de licencia",
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    WindowStyle = WindowStyle.SingleBorderWindow,
                    Content = ucPrevisualizacion
                }.ShowDialog();
            }
            catch (Exception ex)
            {
                loggerPrincipal.Error(DateTime.Now.ToString("yyyyMMddmmss") + " MuestraPrevisualizacion " + "Error!!! Descrpción: " + ex.Message);
            }
        }
        private bool Limpiar()
        {
            loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " Limpiar " + "---Función de Limpiar");

            tramitevalido = false;
            imprimir = false;

            try
            {

                if (File.Exists(System.IO.Path.Combine(ruta, "FingerLeft_10.jpg")))
                {
                    File.Delete(System.IO.Path.Combine(ruta, "FingerLeft_10.jpg"));
                }

                if (File.Exists(System.IO.Path.Combine(ruta, "FingerLeft_9.jpg")))
                {
                    File.Delete(System.IO.Path.Combine(ruta, "FingerLeft_9.jpg"));
                }

                if (File.Exists(System.IO.Path.Combine(ruta, "FingerLeft_8.jpg")))
                {
                    File.Delete(System.IO.Path.Combine(ruta, "FingerLeft_8.jpg"));
                }

                if (File.Exists(System.IO.Path.Combine(ruta, "FingerLeft_7.jpg")))
                {
                    File.Delete(System.IO.Path.Combine(ruta, "FingerLeft_7.jpg"));
                }

                if (File.Exists(System.IO.Path.Combine(ruta, "thumbs_6.jpg")))
                {
                    File.Delete(System.IO.Path.Combine(ruta, "thumbs_6.jpg"));
                }

                if (File.Exists(System.IO.Path.Combine(ruta, "FingerRight_5.jpg")))
                {
                    File.Delete(System.IO.Path.Combine(ruta, "FingerRight_5.jpg"));
                }

                if (File.Exists(System.IO.Path.Combine(ruta, "FingerRight_4.jpg")))
                {
                    File.Delete(System.IO.Path.Combine(ruta, "FingerRight_4.jpg"));
                }

                if (File.Exists(System.IO.Path.Combine(ruta, "FingerRight_3.jpg")))
                {
                    File.Delete(System.IO.Path.Combine(ruta, "FingerRight_3.jpg"));
                }

                if (File.Exists(System.IO.Path.Combine(ruta, "FingerRight_2.jpg")))
                {
                    File.Delete(System.IO.Path.Combine(ruta, "FingerRight_2.jpg"));
                }

                if (File.Exists(System.IO.Path.Combine(ruta, "thumbs_1.jpg")))
                {
                    File.Delete(System.IO.Path.Combine(ruta, "thumbs_1.jpg"));
                }

                if (File.Exists(System.IO.Path.Combine(ruta, "fotoRecort.jpeg")))
                {
                    File.Delete(System.IO.Path.Combine(ruta, "fotoRecort.jpeg"));
                }
                if (File.Exists(System.IO.Path.Combine(ruta + "\\QR\\", "fotoR.jpeg")))
                {
                    File.Delete(System.IO.Path.Combine(ruta + "\\QR\\", "fotoR.jpeg"));
                }


                if (File.Exists(System.IO.Path.Combine(ruta, "firma.bmp")))
                {
                    File.Delete(System.IO.Path.Combine(ruta, "firma.bmp"));
                }

                if (File.Exists(System.IO.Path.Combine(ruta, "finger.json")))
                {
                    File.Delete(System.IO.Path.Combine(ruta, "finger.json"));
                }

                if (File.Exists(System.IO.Path.Combine(ruta, "FingerLeft")))
                {
                    File.Delete(System.IO.Path.Combine(ruta, "FingerLeft"));
                }

                if (File.Exists(System.IO.Path.Combine(ruta, "FingerRight")))
                {
                    File.Delete(System.IO.Path.Combine(ruta, "FingerRight"));
                }

                if (File.Exists(System.IO.Path.Combine(ruta, "firma.jpg")))
                {
                    File.Delete(System.IO.Path.Combine(ruta, "firma.jpg"));
                }

                if (File.Exists(System.IO.Path.Combine(ruta, "firma.txt")))
                {
                    File.Delete(System.IO.Path.Combine(ruta, "firma.txt"));
                }

                if (File.Exists(System.IO.Path.Combine(ruta, "thumbs")))
                {
                    File.Delete(System.IO.Path.Combine(ruta, "thumbs"));
                }

                if (File.Exists(System.IO.Path.Combine(ruta, "huella.txt")))
                {
                    File.Delete(System.IO.Path.Combine(ruta, "huella.txt"));
                }


                string path = ruta + "/firma.bmp";
                // string path1 = ruta + "/thumb_6.png";
                string path2 = ruta + "/fotoRecort.jpeg";

                string path3 = ruta + "/FingerLeft_10.jpg";
                string path4 = ruta + "/FingerLeft_9.jpg";
                string path5 = ruta + "/FingerLeft_8.jpg";
                string path6 = ruta + "/FingerLeft_7.jpg";
                string path8 = ruta + "/FingerRight_5.jpg";
                string path11 = ruta + "/FingerRight_4.jpg";
                string path12 = ruta + "/FingerRight_3.jpg";
                string path13= ruta + "/FingerRight_2.jpg";
                string path14= ruta + "/thumbs_1.jpg";
                string path15 = ruta + "/thumbs_6.jpg";





                bool result = File.Exists(path);

                if (result == true)
                {
                   
          
                    File.Delete(path);
                   
                }
                bool result1 = File.Exists(path2);

                if (result1 == true)
                {


                    File.Delete(path2);

                }


                bool result2 = File.Exists(path3);

                if (result2== true)
                {


                    File.Delete(path3);

                }





                bool result3 = File.Exists(path4);

                if (result3 == true)
                {


                    File.Delete(path4);

                }



                bool result4 = File.Exists(path5);

                if (result4 == true)
                {


                    File.Delete(path5);

                }



                bool result5 = File.Exists(path6);

                if (result5 == true)
                {


                    File.Delete(path6);

                }


                bool result6 = File.Exists(path11);

                if (result6 == true)
                {


                    File.Delete(path11);

                }

                bool result7 = File.Exists(path8);

                if (result7 == true)
                {


                    File.Delete(path8);

                }

                bool result8 = File.Exists(path12);

                if (result8 == true)
                {


                    File.Delete(path12);

                }


                bool result9 = File.Exists(path13);

                if (result9 == true)
                {


                    File.Delete(path13);

                }


                bool result10 = File.Exists(path14);

                if (result10 == true)
                {


                    File.Delete(path14);

                }

                bool result11 = File.Exists(path15);

                if (result11 == true)
                {


                    File.Delete(path15);

                }


                string url = conect + "?pg=BorrarDirectorio&FolioSeg=" + BuscarTramites.Text;
                var response = new WebClient().DownloadString(url);

                jObject = JObject.Parse(response);

                loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " Limpiar " + "---Se limpió");
                return true;
            }
            catch (Exception ex)
            {
                loggerPrincipal.Error(DateTime.Now.ToString("yyyyMMddmmss") + " Limpiar " + "Error!!! Descrpción: " + ex.Message);
                return false;
            }

        }
        async private void Enter_GuardarTramite(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Error.Content = "";

                if (foliolasertext.Text != "")
                {
                    var clienttr = new HttpClient();

                    try
                    {
                        Dictionary<string, string> postParameters = new Dictionary<string, string>();
                        postParameters.Add("pg", "LicImpresa");
                        postParameters.Add("Modulo", Modulo.ToString());
                        // postParameters.Add("NumeroLicencia", licencia);
                        postParameters.Add("FolioSeguimiento",folioSegui);
                        postParameters.Add("Folio", foliolasertext.Text);
                        postParameters.Add("UsuarioId", UsuarioId.ToString());

                        var content = new FormUrlEncodedContent(postParameters);

                        var response = await clienttr.PostAsync(conect, content);

                        var responseString = await response.Content.ReadAsStringAsync();

                        var obj = JObject.Parse(responseString);

                        var success = Convert.ToBoolean((string)obj["success"].ToString());
                        var error = (string)obj["errores"].ToString();

                        if (!success)
                        {
                            loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " GuardarTramite " + "---Error!!!! descripcion: " + error);
                            Error.Content = error;
                            return;
                        }
                        loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " GuardarTramite " + "--- Se guardó folio laser a un trámite");
                        popup.IsOpen = false;
                        imprimeCard();
                        imprimir = true;
                    }
                    catch (Exception ex)
                    {
                        loggerPrincipal.Error(DateTime.Now.ToString("yyyyMMddmmss") + " GuardarTramite " + "---ERROR!!! Descripción " + ex.Message);
                    }

                }
                else
                {
                    loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " GuardarTramite " + "---Error!!! Descripción: Favor de escribir un Folio");
                    Error.Content = "Favor de escribir un Folio";
                }
            }
        }
        private void Enter_BuscarTramite(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                BuscarTramite("", e);
            }
        }
        private void BuscarTramite(object sender, RoutedEventArgs e)
        {
            loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " BuscarTramite " + "---Buscando trámite: " + NombreVal.Text);
            foliolasertext.Text = "";
            NombreVal.Text = "";
            CurpVal.Text = "";
            NacionalidadVal.Text = "";
            ExpedidaVal.Text = "";
            VigenciaVal.Text = "";
            CodigoVal.Text = "";
            TipoVal.Text = "";
            AntiguoVal.Text = "";
            DonadorVal.Text = "";
            Nombre = "";
            Curp = "";
            Nacionalidad = "";
            FechaExpedicion = "";
            FechaVencimiento = "";
            CodigoPostal = "";
            Modalidad = "";
            Antiguo = "";
            DonacionOrganos = true;
            loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " BuscarTramite " + "---Limpiando info actual");
            Limpiar();

            if (BuscarTramites.Text != "")
            {

                try
                {

                    string url = conect + "?pg=GetTramites&escritorio=true&NumLicencia=" + BuscarTramites.Text;
                    string response = "";
                    using (WebClient webClient = new WebClient())
                    {
                        webClient.Encoding = Encoding.UTF8;
                        response = webClient.DownloadString(url);
                    }

                    jObject = JObject.Parse(response);
                    string totalCount = Convert.ToString(jObject.SelectToken("totalCount"));

                    if (totalCount == "1")
                    {
                        tramitevalido = true;
                        licencia = Convert.ToString(jObject.SelectToken("data[0].NumeroLicencia"));

                        folioSegui = Convert.ToString(jObject.SelectToken("data[0].FolioSeguimiento"));
                        Nombre = Convert.ToString(jObject.SelectToken("data[0].NombreCompleto"));
                        Curp = Convert.ToString(jObject.SelectToken("data[0].Curp"));
                        Nacionalidad = Convert.ToString(jObject.SelectToken("data[0].Nacionalidad"));
                        FechaExpedicion = (string)jObject.SelectToken("data[0].FechaExpedicion");
                        FechaVencimiento = (string)jObject.SelectToken("data[0].FechaVencimiento");
                        CodigoPostal = Convert.ToString(jObject.SelectToken("data[0].CodigoPostal"));
                        Modalidad = (string)jObject.SelectToken("data[0].NombreLicencia");
                        Antiguo = Convert.ToString(jObject.SelectToken("data[0].FechaAntiguedad"));
                        DonacionOrganos = (bool)jObject.SelectToken("data[0].DonacionOrganos");
                        tipolic = (string)jObject.SelectToken("data[0].tipolic");
                        DateTime fech;
                        if (FechaVencimiento != null || !String.IsNullOrEmpty(FechaVencimiento))
                        {
                            fech = (DateTime)jObject.SelectToken("data[0].FechaVencimiento");
                            FechaVencimiento = fech.ToString().Substring(0, 10);
                        }
                        else
                        {
                            FechaVencimiento = "";
                        }

                        DateTime fechexp = (DateTime)jObject.SelectToken("data[0].FechaExpedicion");

                        NumeroLicencia.Content = "Folio:   " + BuscarTramites.Text;

                        EstatusTramite.Content = "Estatus: " + (string)jObject.SelectToken("data[0].Estatus");

                        string curpfile = Path.Combine(ruta, "CURP.txt");
                        string Usuario = Path.Combine(ruta, "Usuario.txt");
                       // string NoTramite = Path.Combine(ruta, "NumeroLicencia.txt");
                        string NoFolio = Path.Combine(ruta,"FolioSeguimiento.txt");

                        File.WriteAllText(curpfile, Curp);
                        File.WriteAllText(Usuario, UsuarioId.ToString());
                        //File.WriteAllText(NoTramite, licencia);
                        File.WriteAllText(NoFolio,folioSegui.ToString());

                    












                        NombreVal.Text = Nombre;
                        CurpVal.Text = Curp;

                        if (Nacionalidad == null)
                        {
                            Nacionalidad = "";
                        }

                        NacionalidadVal.Text = Nacionalidad;

                        if (FechaExpedicion == null)
                        {
                            FechaExpedicion = "";
                        }
                        else
                        {
                            FechaExpedicion = fechexp.ToString().Substring(0, 10);
                        }

                        ExpedidaVal.Text = FechaExpedicion;

                        VigenciaVal.Text = FechaVencimiento == "" ? "Permanente" : FechaVencimiento;

                        CodigoVal.Text = CodigoPostal;

                        if (Modalidad == null)
                        {
                            Modalidad = "";
                        }
                        else
                        {
                            TipoVal.Text = Modalidad;
                        }



                        if (Antiguo == null)
                        {
                            Antiguo = "";
                        }
                        else
                        {
                            AntiguoVal.Text = Antiguo.ToString().Substring(0, 10);
                        }


                        dona = "NO";

                        if (DonacionOrganos)
                        {
                            dona = "SI";
                        }

                        DonadorVal.Text = dona;

                        #region QR
                        //Borramos el contenido previo de la carpeta QR
                        System.IO.DirectoryInfo di = new DirectoryInfo(ruta + "\\QR");

                        foreach (FileInfo file in di.GetFiles())
                        {
                            file.Delete();
                        }
                        foreach (DirectoryInfo dir in di.GetDirectories())
                        {
                            dir.Delete(true);
                        }
                        #endregion

                        //QR NUEVO
                        string liga = Path.Combine(ruta+ "\\QR" , "QRI" +".png");
                       // var almacen = @"imagenes/" + licencia + ".jpg";
                        string c = GenerateCode(licencia,Nombre,FechaExpedicion,FechaVencimiento, liga);
                        ///Redimensionar imagen
                 
                        //QR VIEJO
                        // MessageBox.Show(redi);
                        // string s = PueblaQR.GeneraQR(licencia, ruta + "\\QR\\" + licencia + "\\", "dFZ5dN+thedjEAo9UNhHew==");

                        if (c!="OK")
                        {
                            loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " BuscarTramite " + "---No se pudo generar el QR: " + BuscarTramites.Text);
                            MessageBoxResult result = MessageBox.Show("No se pudo generar el QR", "Aviso Importante");
                            tramitevalido = false;
                            licencia = "";
                            NumeroLicencia.Content = "Licencia ";
                            EstatusTramite.Content = "Estatus:  ";
                        }
                        
                        //TRAEMOS LOS BIOMÉTRICOS
                        //string urlBio = conect + "?pg=GetTramiteBiometricos&NumLicencia=" + BuscarTramites.Text;
                        string urlBio = conect + "?pg=GetTramiteBiometricos&folioAsignado=" + BuscarTramites.Text;
                        string responseBio = "";
                        using (WebClient webClient = new WebClient())
                        {
                            webClient.Encoding = Encoding.UTF8;
                            responseBio = webClient.DownloadString(urlBio);
                        }

                        JObject biometricosObj = JObject.Parse(responseBio);
                        string totalCountBio = Convert.ToString(biometricosObj.SelectToken("totalCount"));

                        string FotoLic = (string)biometricosObj.SelectToken("data[0].FotoLic");

                       /// string fotoPath = Path.Combine(ruta, "fotoRecort.jpeg");
                       /// 



                        if (FotoLic != null)
                        {
                            string fotoPath = Path.Combine(ruta, "fotoRecort.jpeg");
                            File.WriteAllBytes(fotoPath, Convert.FromBase64String(FotoLic));

                        /*   byte[] bytes = Convert.FromBase64String(FotoLic);
                              using (MemoryStream ms = new MemoryStream(bytes))
                                  {
                                  BitmapImage bitmap = new BitmapImage();

                                  Bitmap img = (Bitmap)System.Drawing.Image.FromStream(ms);


                                BitmapImage.Source = img;
                              }*/



                        }

                        string thumbs_1 = (string)biometricosObj.SelectToken("data[0].thumbs_1");
                        if (thumbs_1 != null)
                        {
                            string thumbs_1Path = Path.Combine(ruta, "thumbs_1.bmp");
                            File.WriteAllBytes(thumbs_1Path, Convert.FromBase64String(thumbs_1));
                        }

                        string thumbs_6 = (string)biometricosObj.SelectToken("data[0].thumbs_6");
                        if (thumbs_6 != null)
                        {
                            string thumbs_6Path = Path.Combine(ruta, "thumbs_6.bmp");
                            File.WriteAllBytes(thumbs_6Path, Convert.FromBase64String(thumbs_6));
                        }

                        string FirmaLic = (string)biometricosObj.SelectToken("data[0].FirmaLic");
                        if (FirmaLic != null)
                        {
                            string firmaPathBmp = Path.Combine(ruta, "firma.bmp");
                            File.WriteAllBytes(firmaPathBmp, Convert.FromBase64String(FirmaLic));
                            string firmaPathJpg = Path.Combine(ruta, "firma.jpg");
                            File.WriteAllBytes(firmaPathJpg, Convert.FromBase64String(FirmaLic));
                            string firmaPathTxt = Path.Combine(ruta, "firma.txt");
                            File.WriteAllText(firmaPathTxt, FirmaLic);
                        }

                    }
                    else
                    {
                        loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " BuscarTramite " + "---No se encontró el trámite: " + BuscarTramites.Text);
                        MessageBoxResult result = MessageBox.Show("No se encontró el trámite", "Aviso Importante");
                        tramitevalido = false;
                        licencia = "";
                        NumeroLicencia.Content = "Licencia ";
                        EstatusTramite.Content = "Estatus:  ";
                    }

                }
                catch (Exception ex)
                {
                    loggerPrincipal.Error(DateTime.Now.ToString("yyyyMMddmmss") + " BuscarTramite " + "---ERROR!!! Descripción: " + ex.Message);
                    MessageBoxResult result = MessageBox.Show("Hay problema de conexión", "Aviso Importante");
                    return;
                }


            }
            else
            {
                loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " BuscarTramite " + "---No se encontraron datos o hay más de un trámite con " + BuscarTramites.Text);
                MessageBoxResult result = MessageBox.Show("No se encontraron datos", "Aviso Importante");
                return;
            }
        }
        private bool IsProcessRunning(string sProcessName)
        {
            System.Diagnostics.Process[] proc = System.Diagnostics.Process.GetProcessesByName(sProcessName);
            if (proc.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void botones_Click(object sender, RoutedEventArgs e)
        {
            loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " botones_Click " + "--- Función para activar algún módulo del proceso de enrolamiento");
            Button btn = (Button)sender;
            loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " botones_Click " + "---Se oprimió botón " + btn.Name);
            switch (btn.Name)
            {
                case btnHuellas:
                    //Finger old
                    if (tramitevalido)
                    {
                        var allProcesses = Process.GetProcesses();

                        if (!IsProcessRunning("Finger"))
                        {
                            loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " botones_Click " + "--- trámite válido, se activará la función para las huellas");
                            ProcessStartInfo pro = new ProcessStartInfo();
                            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
                            pro.FileName = Path.Combine(path, "Huella", "Finger.exe");
                            //pro.Arguments = "-uid " + UsuarioId.ToString() + " -lic " + licencia;
                            pro.Arguments = "-uid " + UsuarioId.ToString() + " -lic " + folioSegui;
                            processABIS = Process.Start(pro);
                        }
                        else
                        {
                            handle = processABIS.MainWindowHandle;
                            SetForegroundWindow(handle);
                        }
                    }
                    else
                    {
                        loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " botones_Click " + "--- trámite no válido");
                        MessageBoxResult result = MessageBox.Show("No hay datos para enrolar", "Aviso Importante");
                    }
                    
                    break;
                case Firmaelectronica:
                    if (tramitevalido)
                    {
                        loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " botones_Click " + "--- trámite válido, se activará la función para la firma");
                        sigPlusNET_wpfDemo.Window1 Firma = new sigPlusNET_wpfDemo.Window1(folioSegui, conect, UsuarioId);
                        Firma.ShowDialog();
                    }
                    else
                    {
                        loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " botones_Click " + "--- trámite no válido");
                        MessageBoxResult result = MessageBox.Show("No hay datos para enrolar", "Aviso Importante");
                    }
                    break;
                case ImprimirLicencia:
                    string fullPhotoPath = ruta + "\\fotoRecort.jpeg";
                    imprimir = true;

                    if (imprimir & tramitevalido && File.Exists(System.IO.Path.Combine(ruta, "firma.bmp")) && File.Exists(System.IO.Path.Combine(ruta, "fotoRecort.jpeg")))
                    {
                        loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " botones_Click " + "--- Ya previsualizó, se activará la función para guardar licencia a trámite");
                        popup.IsOpen = true;
                        foliolasertext.Focus();
                    }
                    else
                    {
                        loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " botones_Click " + "--- trámite no es válido o falta la foto o la firma");
                        MessageBoxResult result = MessageBox.Show("Faltan pasos para imprimir (firma o foto)", "Aviso Importante");
                    }
                    break;
                case PrevisualizarLicencia:
                    if (true & tramitevalido && File.Exists(System.IO.Path.Combine(ruta, "firma.bmp")) && File.Exists(System.IO.Path.Combine(ruta, "fotoRecort.jpeg")))
                    {
                       
                        loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " botones_Click " + "--- se activará la función para previsualizar");
                     
                        
                        MuestraPrevisualizacion(licencia);
                    }
                    else
                    {
                        loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " botones_Click " + "--- trámite no es válido o falta la foto o la firma");
                        MessageBoxResult result = MessageBox.Show("Faltan pasos para previsualizar (firma, foto o grabar)", "Aviso Importante");
                    }
                    break;
                case GrabarChip:
                    if (tramitevalido && File.Exists(System.IO.Path.Combine(ruta, "fotoRecort.jpeg")))
                    {
                        loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " botones_Click " + "---se activará la función para grabar chip");
                        GrabaChip();

                    }
                    else
                    {
                        loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " botones_Click " + "--- el trámite no es válido");
                        MessageBoxResult result = MessageBox.Show("faltan pasos para grabar chip", "Aviso Importante");
                    }
                    break;

                case btnFotografias:
                    if (tramitevalido)
                    {
                        loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " botones_Click " + "--- trámite válido, se activara la función de la cámara");
                        /*Process[] namePFoto = Process.GetProcessesByName("EDSDKWrapper.UI");
                        if (namePFoto.Length >= 0)
                        {
                            ProcessStartInfo prof = new ProcessStartInfo();
                            string pathf = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
                            prof.FileName = Path.Combine(pathf, "Camara", "EDSDKWrapper.UI.exe");
                            Process processf = Process.Start(prof);
                        }*/
                        ProcessStartInfo info = new ProcessStartInfo();
                        info.UseShellExecute = true;
                        info.FileName = "EDSDKWrapper.UI.exe";
                        //info.WorkingDirectory = @"C:\inetpub\wwwroot\Proyecto_Chihuaha\TLX HORIZONTAL\Camara\EDSDKWrapper.UI\bin\Debug";
                        info.WorkingDirectory = @"C:\inetpub\wwwroot\Camara\EDSDKWrapper.UI\bin\Debug";
                        //info.WorkingDirectory = ruta + @"Camara\EDSDKWrapper.UI\bin\Debug";

                        Process.Start(info);
                    }
                    else
                    {
                        loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " botones_Click " + "--- el trámite no es válido");
                        MessageBoxResult result = MessageBox.Show("No hay datos para enrolar", "Aviso Importante");
                    }
                    break;

                case MostrarFoto:

                    Process.Start(ruta + "/Debug/VistaBio.exe");

                    break;
                case guardarFoto:

                SqlConnection sqlconn = new SqlConnection(@"Data Source=10.169.3.25;Initial Catalog=LicenciasCHRes;User ID=sa;Password=DC_2022$");
        

                     sqlconn.Open();

                    byte[] imgData = File.ReadAllBytes(ruta + "/thumbs_6.bmp");

                    string insertXMLQuery = "UPDATE dtTramites SET thumbs_6=@thumb_6 WHERE FolioSeguimiento=@FolioSegui";

                    SqlCommand insertCommand = new SqlCommand(insertXMLQuery,sqlconn);
                    insertCommand.Parameters.Add(new SqlParameter("@thumb_6", imgData));
                    insertCommand.Parameters.Add(new SqlParameter("@FolioSegui",folioSegui));

                    insertCommand.ExecuteNonQuery();

                    sqlconn.Close();


                    sqlconn.Open();

                    byte[] imgData1 = File.ReadAllBytes(ruta + "/firma.bmp");
                    string base64String = Convert.ToBase64String(imgData1);

                    string insertXMLQuery1 = "UPDATE dtTramites SET FirmaLic=@FirmaLic WHERE FolioSeguimiento=@FolioSegui";

                    SqlCommand insertCommand1 = new SqlCommand(insertXMLQuery1, sqlconn);
                    insertCommand1.Parameters.Add(new SqlParameter("@FirmaLic", base64String));
                    insertCommand1.Parameters.Add(new SqlParameter("@FolioSegui", folioSegui));

                    insertCommand1.ExecuteNonQuery();

                    sqlconn.Close();


                    sqlconn.Open();

                    byte[] imgData2 = File.ReadAllBytes(ruta + "/fotoRecort.jpeg");

                    string insertXMLQuery2 = "UPDATE dtTramites SET FotoLic=@FotoLic WHERE FolioSeguimiento=@FolioSegui";

                    SqlCommand insertCommand2 = new SqlCommand(insertXMLQuery2, sqlconn);
                    insertCommand2.Parameters.Add(new SqlParameter("@FotoLic", imgData2));
                    insertCommand2.Parameters.Add(new SqlParameter("@FolioSegui", folioSegui));

                    insertCommand2.ExecuteNonQuery();

                    sqlconn.Close();

                    // SqlParameter sqlParam = insertCommand.Parameters.AddWithValue("@thumb_6", imgData);


                    string fotosruta = ruta + "\\fotoRecort.jpeg";
                    /*var nomimg = "fotoR";
                    var originalFile = fotosruta;

                    //RUTA DEL DIRECTORIO TEMPORAL

                    string DirTemp = Path.Combine(ruta + "\\QR\\", nomimg + ".jpg");

                    //   String DirTemp = Path.GetTempPath() + @"\" + "nombre.jpg"; ;
                    //IMAGEN ORIGINAL A REDIMENSIONAR
                    Bitmap imagen = new Bitmap(originalFile);
                    //CREAMOS UN MAPA DE BIT CON LAS DIMENSIONES QUE QUEREMOS PARA LA NUEVA IMAGEN
                    Bitmap nuevaImagen = new Bitmap(80, 80);

                    //CREAMOS UN NUEVO GRAFICO
                    Graphics gr = Graphics.FromImage(nuevaImagen);
                    //DIBUJAMOS LA NUEVA IMAGEN
                    gr.DrawImage(imagen, 0, 0, nuevaImagen.Width, nuevaImagen.Height);
                    //LIBERAMOS RECURSOS
                    gr.Dispose();
                    //GUARDAMOS LA NUEVA IMAGEN ESPECIFICAMOS LA RUTA Y EL FORMATO
                    nuevaImagen.Save(DirTemp, System.Drawing.Imaging.ImageFormat.Jpeg);
                    //LIBERAMOS RECURSOS
                    nuevaImagen.Dispose();
                    imagen.Dispose();

                    string traer = ruta + "\\QR\\"+ "fotoR.jpg";

                    string imageRe = Imagen_A_Bytes(traer);


                     //MessageBoxResult res = MessageBox.Show(imageRe);
                    */
                    string liga2 = Path.Combine(ruta + "\\QR", licencia + ".png");
                    //string traer = ruta + "fotorecort.jpeg";
                    string imgqr = Imagen_A_Bytes(fotosruta);
                    //MessageBox.Show(codigoQRImg);
                    string url = "https://qirsolutions.com.mx/api/v1/qrs?test=false&resolution=500";

                    //string cookie = Obtenercookie();
                    //MessageBox.Show(cookie);
                    //string resultado = peticionCodeImg(url,imgqr,Nombre,tipolic,licencia,Curp,FechaExpedicion,FechaVencimiento,cookie);

                    //string respuesta = respuestaqr(resultado, liga2);

                    //MessageBox.Show(respuesta);

                    if (tramitevalido && File.Exists(System.IO.Path.Combine(ruta, "firma.bmp")) && File.Exists(System.IO.Path.Combine(ruta, "fotoRecort.jpeg")) && File.Exists(System.IO.Path.Combine(ruta, "thumbs_1.bmp")))
                    {
                        loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " botones_Click " + "--- trámite válido, se guardará la foto del ciudadano en la DB");
                        string path = ruta + "\\fotoRecort.jpeg";

                      

                        ///Redimensionar foto y 
                        ///
                        //bool success1 = SaveImageBD("thumbs_6", "thumbs_6", System.Drawing.Image.FromFile(path1));



                        bool success = SaveImageBD("FotoLic", "fotoRecort", System.Drawing.Image.FromFile(path));
                        if (success)
                        {
                           
                            MessageBoxResult result = MessageBox.Show("Guardado Exitoso.");
                        }
                        else
                        {
                            MessageBoxResult result = MessageBox.Show("Error en el proceso de guardado. Favor de intentarlo de nuevo.");
                        }
                    }
                    else
                    {
                        loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " botones_Click " + "--- el trámite no es válido");
                        MessageBoxResult result = MessageBox.Show("Faltan pasos para imprimir (firma o foto)", "Aviso Importante");
                    }
                    break;
            }
        }
        static byte[] ImageToByteArray(System.Drawing.Image image)
        {
            loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " ImageToByteArray " + "---");
            using (var ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                return ms.ToArray();
            }
        }
        public bool SaveImageBD(string fields, string fileName, System.Drawing.Image image)
        {
            loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " SaveImageBD " + "--- Guardando Foto");
            using (MemoryStream ms = new MemoryStream())
            {
                using (var client = new HttpClient())
                {
                    string status = "";
                    try
                    {
                        // Convert Image to byte[]
                        image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] imageBytes = ms.ToArray();
                        string ruta = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                        using (FileStream fs = new FileStream(System.IO.Path.Combine(ruta, fileName + ".jpeg"), FileMode.Open, FileAccess.Read))
                        {
                            byte[] data = new byte[fs.Length];
                            fs.Read(data, 0, data.Length);
                            fs.Close();

                            Dictionary<string, object> postParameters = new Dictionary<string, object>();
                            postParameters.Add("pg", "uploadfiledb");
                            postParameters.Add("field", fields);
                            postParameters.Add("usrid", UsuarioId.ToString());
                            //postParameters.Add("NumeroLicencia", licencia);
                            postParameters.Add("FolioSeguimiento",folioSegui);
                            postParameters.Add("Filedata", new FormUpload.FormUpload.FileParameter(data, fileName + ".jpeg", "image/jpeg"));

                            string userAgent = "tlaxcala";
                            HttpWebResponse webResponse = FormUpload.FormUpload.MultipartFormDataPost(conect, userAgent, postParameters);

                            if (webResponse.StatusCode == HttpStatusCode.OK)
                            {
                                status = "OK";
                                loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " SaveImageBD " + "--- Foto Guardada");
                                string url = conect + "?pg=InsertarImagenFoto&usuarioId=" + UsuarioId.ToString() + "&FolioSeg=" + folioSegui;
                                var response = new WebClient().DownloadString(url);

                                JObject jObject = JObject.Parse(response);
                                bool success = (bool)jObject.SelectToken("success");

                                if (!success)
                                {
                                    loggerPrincipal.Error(DateTime.Now.ToString("yyyyMMddmmss") + " SaveImageBD " + "--- ERROR!!! Descripción: Hubo un problema en el proceso");
                                    MessageBoxResult result = MessageBox.Show("Hubo un problema en el proceso", "Aviso Importante");
                                    return false;

                                }

                            }
                            if (webResponse.StatusCode == HttpStatusCode.Forbidden)
                            {
                                loggerPrincipal.Error(DateTime.Now.ToString("yyyyMMddmmss") + " SaveImageBD " + "--- ERROR!!! Descripción: Hubo un problema en el proceso (Forbidden)");
                                status = "Forbidden";
                                MessageBoxResult result = MessageBox.Show("Hubo un problema en el proceso", "Aviso Importante");
                                return false;
                            }
                            StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                            string fullResponse = responseReader.ReadToEnd();
                            webResponse.Close();
                            string sEvent = fullResponse;
                            return true;
                        }
                    }
                    catch (Exception ex)
                    {
                        loggerPrincipal.Error(DateTime.Now.ToString("yyyyMMddmmss") + " SaveImageBD " + "--- ERROR!!! Descripción: Hubo un problema en el proceso (Forbidden)");
                        status = "Forbidden";
                        MessageBoxResult result = MessageBox.Show("Hubo un problema en el proceso", "Aviso Importante");
                        return false;
                    }
                }
            }
        }
        void GrabaChip()
        {

            loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " GrabaChip " + "--- Función para grabar chip");

            Desfire g = new Desfire();

            string error;
            List<string> displist = new List<string>();

            displist = g.listaLectores();
            ChipInfo = "";

            if (displist.Count() == 1)
            {
                loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " GrabaChip " + "--- No se encontró lector alguno");
                MessageBoxResult result = MessageBox.Show("No se encontró lector alguno", "Aviso Importante");
                return;
            }

            string comun = g.getUID(displist[1]);

            if (comun == "CARD NOT PRESENT")
            {
                loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " GrabaChip " + "--- No se lee ningún chip");
                MessageBoxResult result = MessageBox.Show("Favor de colocar una tarjeta con chip", "Aviso Importante");
                return;
            }



            loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " GrabaChip " + "--- validación para grabar chip: ¿Seguro quieres grabar el chip?");
            MessageBoxResult messageBoxResultChip = System.Windows.MessageBox.Show("¿Seguro quieres grabar el chip?", "Aviso Importante", System.Windows.MessageBoxButton.YesNo);

            if (messageBoxResultChip == MessageBoxResult.No)
            {
                loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " GrabaChip " + "---  Se declinó");
                return;
            }
            loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " GrabaChip " + "---  Se aceptó");
            //Crea estructura en tarjeta virgen
            bool str = g.crearEstructura();


            if (!str)
            {
                loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " GrabaChip " + "--- Validación para sobreescribir una licencia que ya tiene estructura");
                MessageBoxResult sobreescribir = System.Windows.MessageBox.Show("Esta licencia ya tiene información, ¿Quieres sobreescribirla?", "Aviso Importante", System.Windows.MessageBoxButton.YesNo);
                if (sobreescribir == MessageBoxResult.No)
                {
                    loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " GrabaChip " + "--- Se declinó");
                    return;
                }
            }

            if (messageBoxResultChip == MessageBoxResult.Yes)
            {
                loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " GrabaChip " + "---  Se aceptó");
                ChipInfo = "||NOMBRE=" + Nombre;
                ChipInfo += "|NO_LICENCIA=" + licencia;
                ChipInfo += "|FECHA_EXPEDICION=" + FechaExpedicion;
                ChipInfo += "|FECHA_VENCIMIENTO=" + FechaVencimiento;
                ChipInfo += "||";

                bool p = g.escribirInfo(ChipInfo);

                if (!p)
                {

                    error = g.getError();
                    loggerPrincipal.Error(DateTime.Now.ToString("yyyyMMddmmss") + " GrabaChip " + "---  ERROR!!!! Descripción " + error);
                    MessageBoxResult result = MessageBox.Show(error, "Aviso Importante");
                    return;
                }

                string fullPhotoPath = ruta + "\\fotoRecort.jpeg";

                //SaveImageBD("fotoRecort", System.Drawing.Image.FromFile(fullPhotoPath));

                loggerPrincipal.Info(DateTime.Now.ToString("yyyyMMddmmss") + " GrabaChip " + "--- Se grabó la licencia");
                MessageBoxResult resultFinish = MessageBox.Show("¡LICENCIA GRABADA!", "Aviso Importante");
                imprimir = true;
                popup.IsOpen = false;
            }
        }
    }
}
