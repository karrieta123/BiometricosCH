using log4net;
using PUE.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Media.Imaging;

namespace sigPlusNET_wpfDemo
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public static System.Drawing.Image ImgFirma { get; set; }
        static string tramiteid = "";
        static string conne;
        static int usrid = 0;
        private static readonly ILog loggerFirma = LogManager.GetLogger(typeof(Window1));

        public Window1(string id, string cnn, int us)
        {
            string LogFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "log", "Firma");
            if (!Directory.Exists(LogFile))
            {
                Directory.CreateDirectory(LogFile);
            }
            string LogFileName = Path.Combine(LogFile, "LogFile_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".log");
            PUELogger.Setup(LogFileName);

            loggerFirma.Info(DateTime.Now.ToString("yyyyMMddmmss") + " Window1/Firma " + "---Constructor página principal");

            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
            sigPlusNET1.SetTabletState(1);
            tramiteid = id;
            conne = cnn;
            usrid = us;
        }

        async static void SaveImageBD(string field, string image)
        {
            loggerFirma.Info(DateTime.Now.ToString("yyyyMMddmmss") + " SaveImageBD " + "---Se guadará la firma");
            try
            {
                IEnumerable<KeyValuePair<string, string>> queries = new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>("pg","SaveImg"),
                    new KeyValuePair<string, string>("field",field),
                    new KeyValuePair<string, string>("usrid",usrid.ToString()),
                    new KeyValuePair<string, string>("img64",image),
                   // new KeyValuePair<string, string>("NumeroLicencia",tramiteid),
                    new KeyValuePair<string, string>("FolioSeguimiento",tramiteid)
                };

                HttpContent q = new FormUrlEncodedContent(queries);

                using (HttpClient client = new HttpClient())
                {

                    using (HttpResponseMessage response = await client.PostAsync(conne, q))
                    {

                        using (HttpContent content = response.Content)
                        {

                            string mycontent = await content.ReadAsStringAsync();
                            Console.WriteLine(mycontent);

                        }

                    }

                }
            }
            catch (Exception ex)
            {
                loggerFirma.Error(DateTime.Now.ToString("yyyyMMddmmss") + " SaveImageBD " + "---ERROR!!! Descripción : " + ex.Message);
            }

        }


        private void cmdSign_Click(object sender, RoutedEventArgs e)
        {

            loggerFirma.Info(DateTime.Now.ToString("yyyyMMddmmss") + "cmdSign_Click " + "--- Cuadro para firmar");

            var vista = Window.GetWindow(this);
            sigPlusNET1.SetImagePenWidth(35);
            sigPlusNET1.SetImageXSize(900);
            sigPlusNET1.SetImageYSize(700);
            //ImgFirma = sigPlusNET1.GetSigImage();
            //Obtener la imagen del dispositivo
            sigPlusNET1.SetLCDCaptureMode(0);
            ImgFirma = sigPlusNET1.GetSigImage();
            try
            {
                loggerFirma.Info(DateTime.Now.ToString("yyyyMMddmmss") + "cmdSign_Click " + "--- Guardando firma...");
                using (MemoryStream ms = new MemoryStream())
                {
                    // Convert Image to byte[]
                    ImgFirma.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] imageBytes = ms.ToArray();

                    // Convert byte[] to Base64 String
                    string base64String = Convert.ToBase64String(imageBytes);

                    SaveImageBD("FirmaLic", base64String);


                    string ruta = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "firma.bmp");
                    ImgFirma.Save(ruta, System.Drawing.Imaging.ImageFormat.Bmp);

                    MessageBoxResult result = MessageBox.Show("Guardado Exitoso");
                }

            }
            catch (Exception ex)
            {
                loggerFirma.Error(DateTime.Now.ToString("yyyyMMddmmss") + "cmdSign_Click " + "--- ERROR!!! Descripción " + ex.Message);
                MessageBoxResult result = MessageBox.Show("No se pudo guardar la información");
            }

            vista.Close();
        }

        private void cmdClear_Click(object sender, RoutedEventArgs e)
        {
            loggerFirma.Info(DateTime.Now.ToString("yyyyMMddmmss") + "cmdClear_Click " + "--- Limpiando...");
            sigPlusNET1.ClearTablet();
            image1.Source = null;
        }

        private void cmdSigString_Click(object sender, RoutedEventArgs e)
        {
            loggerFirma.Info(DateTime.Now.ToString("yyyyMMddmmss") + "cmdSigString_Click " + "---");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            loggerFirma.Info(DateTime.Now.ToString("yyyyMMddmmss") + "Window_Closing " + "--- Cerrando");
            sigPlusNET1.SetTabletState(0);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            loggerFirma.Info(DateTime.Now.ToString("yyyyMMddmmss") + "dispatcherTimer_Tick " + "---Escribiendo...");

            if (sigPlusNET1.NumberOfTabletPoints() > 0)
            {
                sigPlusNET1.SetImageXSize(1000);
                sigPlusNET1.SetImageYSize(300);
                System.Drawing.Image myImg = sigPlusNET1.GetSigImage();

                MemoryStream ms = new MemoryStream();
                myImg.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                BitmapImage sigImg = new BitmapImage();
                sigImg.BeginInit();
                //sigImg.CacheOption = BitmapCacheOption.OnLoad;
                sigImg.StreamSource = ms;
                sigImg.EndInit();

                image1.Source = sigImg;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Create a timer
            loggerFirma.Info(DateTime.Now.ToString("yyyyMMddmmss") + "Window_Loaded " + "---");
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            dispatcherTimer.Start();

            sigPlusNET1.SetImageXSize((int)image1.Width);
            sigPlusNET1.SetImageYSize((int)image1.Height);
        }
    }
}
