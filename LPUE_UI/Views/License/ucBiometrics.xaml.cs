using Entidades;
using Finger;
using log4net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;


namespace PUE.Views.License
{
    /// <summary>
    /// Lógica de interacción para ucBiometrics.xaml
    /// </summary>
    public partial class ucBiometrics : UserControl
    {
        private string dedo { get; set; }
        JsonFinger jsonSettings;
        string id_finger = null;
        FileSystemWatcher fs;
        static private string ruta = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private PUE.Controllers.CaptureInforLic controller = new Controllers.CaptureInforLic();
        static string tramiteid = "";
        static string cnn;
        static int usrid = 0;
        private static readonly ILog loggerBio = LogManager.GetLogger(typeof(ucBiometrics));
        PUE.Controllers.Biometrics dataBiometricos;
        //byte[] FotoImgn = null,
        //       HuellaImg = null,
        //       FirmaImg = null,
        //       Huella2Img = null;

        private List<EntiBiometricos> LisBiomertricos = new List<EntiBiometricos>();

        public ucBiometrics(string id, string conn, int userId)
        {

            string LogFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "log", "Huellas");
            if (!Directory.Exists(LogFile))
            {
                Directory.CreateDirectory(LogFile);
            }
            string LogFileName = Path.Combine(LogFile, "LogFile_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".log");
            PUE.Controllers.PUELogger.Setup(LogFileName);

            loggerBio.Info(DateTime.Now.ToString("yyyyMMddmmss") + " ucBiometrics " + "---Constructor ventana biométricos del tramite " + id);

            InitializeComponent();
            usrid = userId;
            dataBiometricos = new Controllers.Biometrics();
            //the folder to be watched
            string watchingFolder = ruta; // System.IO.Path.GetTempPath();
            //initialize the filesystem watcher
            fs = new FileSystemWatcher(watchingFolder);
            //fs.Filter = "*.bmp"; //"rol.bmp*.*pulgares.bmp*.*DedosIzquierda.bmp*.*DedosDerecha.bmp";
            fs.EnableRaisingEvents = true;
            fs.IncludeSubdirectories = true;
            //This event will check for  new files added to the watching folder
            fs.Created += new FileSystemEventHandler(newfile);
            // fs.Deleted += new FileSystemEventHandler(newfile);
            cnn = conn;
            fs.Changed += new FileSystemEventHandler(newfile);
            tramiteid = id;
        }



        /// <summary>
        /// Evento disparado para iniciar biometrico
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onBiometric_MouseDown(object sender, MouseButtonEventArgs e)
        {
            loggerBio.Info(DateTime.Now.ToString("yyyyMMddmmss") + " onBiometric_MouseDown " + "---Ventana para captura de huellas ");
            try
            {
                Process[] namePHuella = Process.GetProcessesByName("Finger");
                if (namePHuella.Length == 0)
                {
                    ProcessStartInfo pro = new ProcessStartInfo();
                    string path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
                    pro.FileName = Path.Combine(path, "Huella", "Finger.exe");
                    Process process = Process.Start(pro);
                }
            }
            catch (Exception ex)
            {
                loggerBio.Error(DateTime.Now.ToString("yyyyMMddmmss") + " onBiometric_MouseDown " + "---ERROR!!! Descripción " + ex.Message);
            }
        }


        static void SaveImageBD(string fields, System.Drawing.Image image)
        {
            loggerBio.Info(DateTime.Now.ToString("yyyyMMddmmss") + " SaveImageBD " + "---Guardando imagen " + fields);
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {

                    using (var client = new HttpClient())
                    {
                        // Convert Image to byte[]
                        image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] imageBytes = ms.ToArray();

                        using (FileStream fs = new FileStream(System.IO.Path.Combine(ruta, fields + ".bmp"), FileMode.Open, FileAccess.Read))
                        {
                            try
                            {
                                byte[] data = new byte[fs.Length];
                                fs.Read(data, 0, data.Length);
                                fs.Close();

                                Dictionary<string, object> postParameters = new Dictionary<string, object>();
                                postParameters.Add("pg", "uploadfiledb");
                                postParameters.Add("field", fields);
                                postParameters.Add("usrid", usrid.ToString());
                                postParameters.Add("NumeroLicencia", tramiteid);
                                postParameters.Add("FolioSeguimiento", tramiteid);
                                postParameters.Add("Filedata", new FormUpload.FormUpload.FileParameter(data, fields + ".jpeg", "image/jpeg"));

                                string userAgent = "tlaxcala";
                                HttpWebResponse webResponse = FormUpload.FormUpload.MultipartFormDataPost(cnn, userAgent, postParameters);

                                string status = "";
                                if (webResponse.StatusCode == HttpStatusCode.OK)
                                {
                                    loggerBio.Info(DateTime.Now.ToString("yyyyMMddmmss") + " SaveImageBD " + "---Se guardó " + fields);
                                    status = "OK";
                                }
                                if (webResponse.StatusCode == HttpStatusCode.Forbidden)
                                {
                                    loggerBio.Error(DateTime.Now.ToString("yyyyMMddmmss") + " SaveImageBD " + "---ERROR!! Descripción: no se pudo guardar " + fields);
                                    status = "Forbidden";

                                }
                                StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                                string fullResponse = responseReader.ReadToEnd();
                                webResponse.Close();
                                string sEvent = fullResponse;
                            }
                            catch (Exception ex)
                            {
                                loggerBio.Error(DateTime.Now.ToString("yyyyMMddmmss") + " SaveImageBD " + "---ERROR!! Descripción: " + ex.Message);
                            }
                        }


                    }


                }
            }
            catch (Exception ex)
            {
                loggerBio.Error(DateTime.Now.ToString("yyyyMMddmmss") + " SaveImageBD " + "---ERROR!! Descripción: " + ex.Message);

            }

        }

        private bool ArchivosHuellas()
        {

            if (!File.Exists(System.IO.Path.Combine(ruta, "FingerLeft_10.bmp")))
                return false;

            if (!File.Exists(System.IO.Path.Combine(ruta, "FingerLeft_9.bmp")))
                return false;

            if (!File.Exists(System.IO.Path.Combine(ruta, "FingerLeft_8.bmp")))
                return false;

            if (!File.Exists(System.IO.Path.Combine(ruta, "FingerLeft_7.bmp")))
                return false;

            if (!File.Exists(System.IO.Path.Combine(ruta, "thumbs_6.bmp")))
                return false;

            if (!File.Exists(System.IO.Path.Combine(ruta, "FingerRight_5.bmp")))
                return false;

            if (!File.Exists(System.IO.Path.Combine(ruta, "FingerRight_4.bmp")))
                return false;

            if (!File.Exists(System.IO.Path.Combine(ruta, "FingerRight_3.bmp")))
                return false;

            if (!File.Exists(System.IO.Path.Combine(ruta, "FingerRight_2.bmp")))
                return false;

            if (!File.Exists(System.IO.Path.Combine(ruta, "thumbs_1.bmp")))
                return false;

            return true;

        }

        void newfile(object fscreated, FileSystemEventArgs Eventocc)
        {
            loggerBio.Info(DateTime.Now.ToString("yyyyMMddmmss") + " newfile " + "---" + Eventocc.Name);

            string CreatedFileName = Eventocc.Name;
            string file = Path.GetExtension(CreatedFileName);
            if (Regex.IsMatch(file, @"\.bmp|\.jpeg|\.json", RegexOptions.IgnoreCase))
            {
                ruta = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                loggerBio.Info(DateTime.Now.ToString("yyyyMMddmmss") + " newfile " + "Se guardará el archivo " + CreatedFileName);
                switch (CreatedFileName)
                {

                    case ("FingerLeft_10.bmp"):
                        this.Dispatcher.Invoke((Action)(() =>
                        {

                            if (LisBiomertricos.Exists(exi => exi.id == 10))
                            {
                                LisBiomertricos.RemoveAll(r => r.id == 10);
                            }
                            if (File.Exists(System.IO.Path.Combine(ruta, "FingerLeft_10.bmp")))
                            {
                                try
                                {
                                    System.Drawing.Image img = ConvertToBitmap(System.IO.Path.Combine(ruta, "FingerLeft_10.bmp"));
                                    Fingerten.Source = render.ToWpfImage(img);
                                    SaveImageBD("FingerLeft_10", img);
                                    LisBiomertricos.Add(new EntiBiometricos { id = 10, imagen = controller.ConevrtImageBytes(img) });
                                }
                                catch (Exception ex)
                                {
                                    loggerBio.Error(DateTime.Now.ToString("yyyyMMddmmss") + " newfile " + "ERROR!! Descripción: " + ex.Message);
                                    MessageBoxResult result = MessageBox.Show("No se pudo guardar la información");
                                    return;
                                }

                            }


                        }));
                        break;
                    case ("FingerLeft_9.bmp"):
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            if (LisBiomertricos.Exists(exi => exi.id == 9))
                            {
                                LisBiomertricos.RemoveAll(r => r.id == 9);
                            }
                            if (File.Exists(System.IO.Path.Combine(ruta, "FingerLeft_9.bmp")))
                            {
                                try
                                {
                                    System.Drawing.Image img = ConvertToBitmap(System.IO.Path.Combine(ruta, "FingerLeft_9.bmp"));
                                    Fingernine.Source = render.ToWpfImage(img);


                                    SaveImageBD("FingerLeft_9", img);
                                    LisBiomertricos.Add(new EntiBiometricos { id = 9, imagen = controller.ConevrtImageBytes(img) });
                                }
                                catch (Exception ex)
                                {
                                    loggerBio.Error(DateTime.Now.ToString("yyyyMMddmmss") + " newfile " + "ERROR!! Descripción: " + ex.Message);
                                    MessageBoxResult result = MessageBox.Show("No se pudo guardar la información");
                                    return;
                                }

                            }

                        }));
                        break;
                    case ("FingerLeft_8.bmp"):
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            if (LisBiomertricos.Exists(exi => exi.id == 8))
                            {
                                LisBiomertricos.RemoveAll(r => r.id == 8);
                            }
                            if (File.Exists(System.IO.Path.Combine(ruta, "FingerLeft_8.bmp")))
                            {
                                try
                                {
                                    System.Drawing.Image img = ConvertToBitmap(System.IO.Path.Combine(ruta, "FingerLeft_8.bmp"));
                                    Fingereight.Source = render.ToWpfImage(img);


                                    SaveImageBD("FingerLeft_8", img);
                                    LisBiomertricos.Add(new EntiBiometricos { id = 8, imagen = controller.ConevrtImageBytes(img) });
                                }
                                catch (Exception ex)
                                {
                                    loggerBio.Error(DateTime.Now.ToString("yyyyMMddmmss") + " newfile " + "ERROR!! Descripción: " + ex.Message);
                                    MessageBoxResult result = MessageBox.Show("No se pudo guardar la información");
                                    return;
                                }

                            }
                        }));
                        break;
                    case ("FingerLeft_7.bmp"):
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            if (LisBiomertricos.Exists(exi => exi.id == 7))
                            {
                                LisBiomertricos.RemoveAll(r => r.id == 7);
                            }
                            if (File.Exists(System.IO.Path.Combine(ruta, "FingerLeft_7.bmp")))
                            {
                                try
                                {
                                    System.Drawing.Image img = ConvertToBitmap(System.IO.Path.Combine(ruta, "FingerLeft_7.bmp"));
                                    Fingerseven.Source = render.ToWpfImage(img);
                                    LisBiomertricos.Add(new EntiBiometricos { id = 7, imagen = controller.ConevrtImageBytes(img) });


                                    SaveImageBD("FingerLeft_7", img);
                                    LisBiomertricos.Add(new EntiBiometricos { id = 8, imagen = controller.ConevrtImageBytes(img) });
                                }
                                catch (Exception ex)
                                {
                                    loggerBio.Error(DateTime.Now.ToString("yyyyMMddmmss") + " newfile " + "ERROR!! Descripción: " + ex.Message);
                                    MessageBoxResult result = MessageBox.Show("No se pudo guardar la información");
                                    return;
                                }
                            }

                        }));
                        break;
                    case ("thumbs_6.bmp"):
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            if (LisBiomertricos.Exists(exi => exi.id == 6))
                            {
                                LisBiomertricos.RemoveAll(r => r.id == 6);
                            }
                            if (File.Exists(System.IO.Path.Combine(ruta, "thumbs_6.bmp")))
                            {
                                try
                                {
                                    System.Drawing.Image img = ConvertToBitmap(System.IO.Path.Combine(ruta, "thumbs_6.bmp"));
                                    Fingersix.Source = render.ToWpfImage(img);
                                    LisBiomertricos.Add(new EntiBiometricos { id = 6, imagen = controller.ConevrtImageBytes(img) });


                                    SaveImageBD("thumbs_6", img);
                                    LisBiomertricos.Add(new EntiBiometricos { id = 8, imagen = controller.ConevrtImageBytes(img) });
                                }
                                catch (Exception ex)
                                {
                                    loggerBio.Error(DateTime.Now.ToString("yyyyMMddmmss") + " newfile " + "ERROR!! Descripción: " + ex.Message);
                                    MessageBoxResult result = MessageBox.Show("No se pudo guardar la información");
                                    return;
                                }

                            }
                        }));
                        break;
                    case ("FingerRight_5.bmp"):
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            if (LisBiomertricos.Exists(exi => exi.id == 5))
                            {
                                LisBiomertricos.RemoveAll(r => r.id == 5);
                            }
                            if (File.Exists(System.IO.Path.Combine(ruta, "FingerRight_5.bmp")))
                            {
                                try
                                {
                                    System.Drawing.Image img = ConvertToBitmap(System.IO.Path.Combine(ruta, "FingerRight_5.bmp"));
                                    Fingerfive.Source = render.ToWpfImage(img);
                                    LisBiomertricos.Add(new EntiBiometricos { id = 5, imagen = controller.ConevrtImageBytes(img) });


                                    SaveImageBD("FingerRight_5", img);
                                    LisBiomertricos.Add(new EntiBiometricos { id = 5, imagen = controller.ConevrtImageBytes(img) });
                                }
                                catch (Exception ex)
                                {
                                    loggerBio.Error(DateTime.Now.ToString("yyyyMMddmmss") + " newfile " + "ERROR!! Descripción: " + ex.Message);
                                    MessageBoxResult result = MessageBox.Show("No se pudo guardar la información");
                                    return;
                                }
                            }

                        }));
                        break;
                    case ("FingerRight_4.bmp"):
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            if (LisBiomertricos.Exists(exi => exi.id == 4))
                            {
                                LisBiomertricos.RemoveAll(r => r.id == 4);
                            }
                            if (File.Exists(System.IO.Path.Combine(ruta, "FingerRight_4.bmp")))
                            {
                                try
                                {
                                    System.Drawing.Image img = ConvertToBitmap(System.IO.Path.Combine(ruta, "FingerRight_4.bmp"));
                                    Fingerfour.Source = render.ToWpfImage(img);
                                    LisBiomertricos.Add(new EntiBiometricos { id = 4, imagen = controller.ConevrtImageBytes(img) });


                                    SaveImageBD("FingerRight_4", img);
                                    LisBiomertricos.Add(new EntiBiometricos { id = 5, imagen = controller.ConevrtImageBytes(img) });
                                }
                                catch (Exception ex)
                                {
                                    loggerBio.Error(DateTime.Now.ToString("yyyyMMddmmss") + " newfile " + "ERROR!! Descripción: " + ex.Message);
                                    MessageBoxResult result = MessageBox.Show("No se pudo guardar la información");
                                    return;
                                }

                            }
                        }));
                        break;
                    case ("FingerRight_3.bmp"):
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            if (LisBiomertricos.Exists(exi => exi.id == 3))
                            {
                                LisBiomertricos.RemoveAll(r => r.id == 3);
                            }
                            if (File.Exists(System.IO.Path.Combine(ruta, "FingerRight_3.bmp")))
                            {
                                try
                                {
                                    System.Drawing.Image img = ConvertToBitmap(System.IO.Path.Combine(ruta, "FingerRight_3.bmp"));
                                    Fingertree.Source = render.ToWpfImage(img);
                                    LisBiomertricos.Add(new EntiBiometricos { id = 3, imagen = controller.ConevrtImageBytes(img) });

                                    SaveImageBD("FingerRight_3", img);
                                    LisBiomertricos.Add(new EntiBiometricos { id = 3, imagen = controller.ConevrtImageBytes(img) });
                                }
                                catch (Exception ex)
                                {
                                    loggerBio.Error(DateTime.Now.ToString("yyyyMMddmmss") + " newfile " + "ERROR!! Descripción: " + ex.Message);
                                    MessageBoxResult result = MessageBox.Show("No se pudo guardar la información");
                                    return;
                                }

                            }
                        }));
                        break;
                    case ("FingerRight_2.bmp"):
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            if (LisBiomertricos.Exists(exi => exi.id == 2))
                            {
                                LisBiomertricos.RemoveAll(r => r.id == 2);
                            }
                            if (File.Exists(System.IO.Path.Combine(ruta, "FingerRight_2.bmp")))
                            {
                                try
                                {
                                    System.Drawing.Image img = ConvertToBitmap(System.IO.Path.Combine(ruta, "FingerRight_2.bmp"));
                                    Fingertwo.Source = render.ToWpfImage(img);
                                    LisBiomertricos.Add(new EntiBiometricos { id = 2, imagen = controller.ConevrtImageBytes(img) });


                                    SaveImageBD("FingerRight_2", img);
                                    LisBiomertricos.Add(new EntiBiometricos { id = 2, imagen = controller.ConevrtImageBytes(img) });
                                }
                                catch (Exception ex)
                                {
                                    loggerBio.Error(DateTime.Now.ToString("yyyyMMddmmss") + " newfile " + "ERROR!! Descripción: " + ex.Message);
                                    MessageBoxResult result = MessageBox.Show("No se pudo guardar la información");
                                    return;
                                }
                            }

                        }));
                        break;

                    case ("thumbs_1.bmp"):
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            if (LisBiomertricos.Exists(exi => exi.id == 1))
                            {
                                LisBiomertricos.RemoveAll(r => r.id == 1);
                            }
                            if (File.Exists(System.IO.Path.Combine(ruta, "thumbs_1.bmp")))
                            {
                                try
                                {
                                    System.Drawing.Image img = ConvertToBitmap(System.IO.Path.Combine(ruta, "thumbs_1.bmp"));
                                    fingerOne.Source = render.ToWpfImage(img);
                                    btnHuella.Source = render.ToWpfImage(img);


                                    SaveImageBD("thumbs_1", img);
                                    LisBiomertricos.Add(new EntiBiometricos { id = 1, imagen = controller.ConevrtImageBytes(img) });
                                }
                                catch (Exception ex)
                                {
                                    loggerBio.Error(DateTime.Now.ToString("yyyyMMddmmss") + " newfile " + "ERROR!! Descripción: " + ex.Message);
                                    MessageBoxResult result = MessageBox.Show("No se pudo guardar la información");
                                    return;
                                }

                            }

                        }));
                        break;

                    case ("finger.json"):
                        this.Dispatcher.Invoke((Action)(() =>
                        {
                            ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "finger.json");
                            if (File.Exists(ruta))
                            {
                                JArray o1 = JArray.Parse(File.ReadAllText(ruta));
                                dedo = o1[0]["id_finger"].ToString();
                                    // File.Delete(ruta);
                                }

                        }
                            ));
                        break;
                }


            }
        }
        /// <summary>
        /// Evento disparado por los botones guardar y cerrar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void botonesBiometricos_Click(object sender, RoutedEventArgs e)
        {
            loggerBio.Info(DateTime.Now.ToString("yyyyMMddmmss") + " botonesBiometricos_Click " + "---Función para guardar huellas");

            var vari = Window.GetWindow(this);


            MessageBoxResult respuesta = MessageBox.Show("¿Quieres guardar las huellas capturadas actualmente?",
                             "Aviso", MessageBoxButton.OKCancel, MessageBoxImage.Information);
            loggerBio.Info(DateTime.Now.ToString("yyyyMMddmmss") + " botonesBiometricos_Click " + "---Validacion: ¿Quieres guardar las huellas capturadas actualmente? ");
            if (respuesta == MessageBoxResult.OK)
            {
                string causa = "";
                if (!ArchivosHuellas())
                {
                    fingerless fic = new fingerless();
                    fic.ShowDialog();
                    causa = fic.causa;

                    if (causa == null)
                        return;
                }

                loggerBio.Info(DateTime.Now.ToString("yyyyMMddmmss") + " botonesBiometricos_Click " + "---Se aceptó ");
                loggerBio.Info(DateTime.Now.ToString("yyyyMMddmmss") + " botonesBiometricos_Click " + "---Función para guardar las imágenes que se subieron");
                try
                {
                    string url = cnn + "?pg=InsertarImagenOpti&Comentario=" + causa + "&usuarioId=" + usrid + "&NumeroLic=" + tramiteid;
                    var response = new WebClient().DownloadString(url);
                }
                catch (Exception ex)
                {

                    loggerBio.Error(DateTime.Now.ToString("yyyyMMddmmss") + " botonesBiometricos_Click " + "---ERROR!!! Descripcion:" + ex.Message);
                    MessageBoxResult result = MessageBox.Show("Hubo un problema en el proceso", "Aviso Importante");
                    return;
                }


                vari.Close();
            }
        }
        public Bitmap ConvertToBitmap(string fileName)
        {
            loggerBio.Info(DateTime.Now.ToString("yyyyMMddmmss") + " ConvertToBitmap " + "---Función para convertir imágen a un mapa de bits");
            Bitmap bitmap;
            using (Stream bmpStream = System.IO.File.Open(fileName, System.IO.FileMode.Open))
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(bmpStream);


                bitmap = new Bitmap(image);

            }
            return bitmap;
        }


        private void Cerrar(object sender, RoutedEventArgs e)
        {
            loggerBio.Info(DateTime.Now.ToString("yyyyMMddmmss") + " Cerrar " + "---Cerrando ventana");
            //Metodo para Cerrar procesos
            try
            {
                Process[] namePFoto = Process.GetProcessesByName("EDSDKWrapper.UI");
                foreach (var item in namePFoto)
                {
                    try
                    {
                        item.CloseMainWindow();
                        item.WaitForExit();
                    }
                    catch (Exception ex)
                    {
                        string t = ex.Message;
                        throw;
                    }
                }
                Process[] namePHuella = Process.GetProcessesByName("Finger");
                foreach (var item in namePHuella)
                {
                    item.CloseMainWindow();
                    item.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                loggerBio.Error(DateTime.Now.ToString("yyyyMMddmmss") + " ConvertToBitmap " + "---ERROR!!! Descripción: " + ex.Message);
            }
        }
    }
    public static class render
    {
        public static BitmapImage ToWpfImage(this System.Drawing.Image img)
        {
            BitmapImage ix = new BitmapImage();
            try
            {
                MemoryStream ms = new MemoryStream();  // no using here! BitmapImage will dispose the stream after loading
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);


                ix.BeginInit();
                ix.CacheOption = BitmapCacheOption.OnLoad;
                ix.StreamSource = ms;
                ix.EndInit();
            }
            catch
            {

            }
            return ix;

        }
    }
}
