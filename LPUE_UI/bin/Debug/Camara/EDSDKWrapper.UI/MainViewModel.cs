using EDSDKLib;
using EDSDKWrapper.Framework.Managers;
using EDSDKWrapper.Framework.Objects;
using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;


//using EDSDKWrapper.Framework.Enums;

namespace EDSDKWrapper.UI
{

    public class MainViewModel : DependencyObject, IDisposable
    {
        private double factorX = 0;
        public static double RecttX { get; set; }
        public static double RecttY { get; set; }
        public bool initialCamara { get; set; }
        string RutaApp = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        string tramiteid = "";
        private static readonly ILog loggerCam = LogManager.GetLogger(typeof(MainViewModel));

        string LogFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "log", "Camara");


        #region Tomamos las variables del Config
        int Config_widthMax = Convert.ToInt32(ConfigurationManager.AppSettings["widthMax"].ToString());
        int Config_widthMin = Convert.ToInt32(ConfigurationManager.AppSettings["widthMin"].ToString());
        int Config_XMax = Convert.ToInt32(ConfigurationManager.AppSettings["XMax"].ToString());
        int Config_XMin = Convert.ToInt32(ConfigurationManager.AppSettings["XMin"].ToString());
        int Config_YMax = Convert.ToInt32(ConfigurationManager.AppSettings["YMax"].ToString());
        int Config_YMin = Convert.ToInt32(ConfigurationManager.AppSettings["YMin"].ToString());
        int Config_HeightMax = Convert.ToInt32(ConfigurationManager.AppSettings["HeightMax"].ToString());
        int Config_HeightMin = Convert.ToInt32(ConfigurationManager.AppSettings["HeightMin"].ToString());
        #region Valdia si es correcto la posicion de la foto y lo marca con rojo si no lo es y verde si lo es
        private void ValPhoto()
        {
            loggerCam.Info(DateTime.Now.ToString("yyyyMMddmmss") + " ValPhoto " + "---");
            if ((AnchoD > Config_widthMin && AnchoD < Config_widthMax) && (ValorX > Config_XMin && ValorX < Config_XMax) && (ValorY < Config_YMax && ValorY > Config_YMin) && (AltoD < Config_HeightMax && AltoD > Config_HeightMin))
            {
                ColorCircle = "Green";
            }
            else
            {
                ColorCircle = "Red";
            }
        }
        #endregion
        #endregion

        //   public List<Marco> lstdondequedo { get; set; }

        #region Properties
        public string error_camera;//Propiedad para ver si al camara esta prendida
        TaskScheduler UITaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        public FrameworkManager FrameworkManager { get; set; }
        public Task LiveViewCapturingTask { get; set; }
        public Camera Camera { get; set; }
        IntPtr camlist = new IntPtr();//Dispositivo
        private int _numDispFoto;
        private EDSDK.EdsDeviceInfo GetInfo;
        public EDSDK.EdsRect recFoco;
        public int altImg;
        public int ancImg;
        public int xImg;
        public int yImg;


        #region ImageSource



        public string label
        {
            get { return (string)GetValue(labelProperty); }
            set { SetValue(labelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for label.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty labelProperty =
            DependencyProperty.Register("labelabel", typeof(string), typeof(MainViewModel));



        public string ColorCircle
        {
            get { return (string)GetValue(ColorCircleProperty); }
            set { SetValue(ColorCircleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColorCircle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorCircleProperty =
            DependencyProperty.Register("ColorCircle", typeof(string), typeof(MainViewModel));


        public double AltoD
        {
            get { return (double)GetValue(AltoDProperty); }
            set { SetValue(AltoDProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AltoD.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AltoDProperty =
            DependencyProperty.Register("AltoD", typeof(double), typeof(MainViewModel));

        public double AnchoD
        {
            get { return (double)GetValue(AnchoDProperty); }
            set { SetValue(AnchoDProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AnchoD.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnchoDProperty =
            DependencyProperty.Register("AnchoD", typeof(double), typeof(MainViewModel));

        public double ValorX
        {
            get { return (double)GetValue(ValorXProperty); }
            set { SetValue(ValorXProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ValorX.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValorXProperty =
            DependencyProperty.Register("ValorX", typeof(double), typeof(MainViewModel));


        public double ValorY
        {
            get { return (double)GetValue(ValorYProperty); }
            set { SetValue(ValorYProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ValorY.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValorYProperty =
            DependencyProperty.Register("ValorY", typeof(double), typeof(MainViewModel));

        public double Ancho
        {
            get { return (double)GetValue(AnchoProperty); }
            set { SetValue(AnchoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Ancho.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnchoProperty =
            DependencyProperty.Register("Ancho", typeof(double), typeof(MainViewModel));

        public double Alto
        {
            get { return (double)GetValue(AltoProperty); }
            set { SetValue(AltoProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Alto.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AltoProperty =
            DependencyProperty.Register("Alto", typeof(double), typeof(MainViewModel));

        public string on_off
        {
            get { return (string)GetValue(on_offProperty); }
            set { SetValue(on_offProperty, value); }
        }

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public int Xcord
        {
            get { return (int)GetValue(XProperty); }
            set { SetValue(XProperty, value); }
        }

        public int Ycordi
        {
            get { return (int)GetValue(YProperty); }
            set { SetValue(YProperty, value); }
        }

        // Using a DependencyProperty as the backing store for on_off.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty on_offProperty =
            DependencyProperty.Register("on_off", typeof(string), typeof(MainViewModel));

        // Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(MainViewModel));

        public static readonly DependencyProperty XProperty =
            DependencyProperty.Register("Xcord", typeof(int), typeof(MainViewModel));

        public static readonly DependencyProperty YProperty =
            DependencyProperty.Register("Ycordi", typeof(int), typeof(MainViewModel));


        #endregion

        #endregion

        #region Methods

        #region Instance

        public static void Setup(string filepath)
        {

            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
            hierarchy.Root.RemoveAllAppenders();

            FileAppender fileAppender = new FileAppender();
            fileAppender.AppendToFile = true;
            fileAppender.AppendToFile = true;
            fileAppender.LockingModel = new FileAppender.MinimalLock();
            fileAppender.File = filepath;
            PatternLayout pl = new PatternLayout();
            pl.ConversionPattern = "%date %-5level - %message;; %newline";
            pl.ActivateOptions();
            fileAppender.Layout = pl;
            fileAppender.ActivateOptions();

            log4net.Config.BasicConfigurator.Configure(fileAppender);

        }

        public MainViewModel(string id)
        {
            if (!Directory.Exists(LogFile))
            {
                Directory.CreateDirectory(LogFile);
            }
            string LogFileName = Path.Combine(LogFile, "LogFile_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".log");
            Setup(LogFileName);

            loggerCam.Info(DateTime.Now.ToString("yyyyMMddmmss") + " ucMenu " + "---Constructor Cámara");


            tramiteid = id;
            this.FrameworkManager = new FrameworkManager();

            try
            {
                this.FrameworkManager.Dispose();
                this.FrameworkManager = new FrameworkManager();
            }
            catch (Exception ex)
            {
                loggerCam.Error(DateTime.Now.ToString("yyyyMMddmmss") + " MainViewModel " + "---ERROR!! --> " + ex.Message);
            }
        }

        #endregion

        #endregion

        #region Commands

        #region StartCapturingCommand

        public ICommand StartCapturingCommand
        {
            get
            {
                loggerCam.Info(DateTime.Now.ToString("yyyyMMddmmss") + " ICommand StartCapturingCommand " + "---");
                return new RelayCommand(StartCapturingCommand_Executed, StartCapturingCommand_CanExecute);
            }
        }

        public bool StartCapturingCommand_CanExecute()
        {
            loggerCam.Info(DateTime.Now.ToString("yyyyMMddmmss") + " StartCapturingCommand_CanExecute " + "---");
            return true;
        }

        public void StartCapturingCommand_Executed()
        {
            loggerCam.Info(DateTime.Now.ToString("yyyyMMddmmss") + " StartCapturingCommand_Executed " + "---");
            On_camera();
            initialCamara = true;
            this.FrameworkManager = new FrameworkManager();

            try
            {
                this.FrameworkManager.Dispose();
                this.FrameworkManager = new FrameworkManager();
            }
            catch (Exception ex)
            {
                loggerCam.Error(DateTime.Now.ToString("yyyyMMddmmss") + " StartCapturingCommand_CanExecute " + "---ERROR!!! Descripcion: " + ex.Message);
            }
            this.LiveViewCapturingTask = Task.Factory.StartNew(() =>
            {
                this.Camera = this.FrameworkManager.Cameras.First();
                if (!On_camera())//Validamos si la camara esta Conectada
                {
                    loggerCam.Info(DateTime.Now.ToString("yyyyMMddmmss") + " StartCapturingCommand_CanExecute " + "---Error se desconecto la camara");
                    System.Windows.Forms.MessageBox.Show("Error se desconecto la camara.", "Error");
                    return;
                }
                this.Camera.StartLiveView();
                this.Camera.LiveViewAutoFocusMode = Framework.Enums.LiveViewAutoFocusMode.LiveFace;

                int exceptionCount = 0;

                EDSDK.EdsGetDeviceInfo(camlist, out GetInfo);

                while (Camera.LiveViewEnabled)
                {
                    if (!On_camera())
                    {
                        System.Windows.Forms.MessageBox.Show("Error se desconecto la camara.", "Error");
                        return;
                    }
                    exceptionCount = 1;

                    try
                    {
                        var rectang = this.Camera.FocusInformation.ImageRectangle;
                        EDSDK.EdsSendCommand(camlist, EDSDK.CameraCommand_DriveLensEvf, 0);

                        var stream = this.Camera.GetLiveViewImage();

                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = stream;
                        bitmapImage.EndInit();
                        bitmapImage.Freeze();

                        Dispatcher.BeginInvoke(new Action(() =>
                        {
                            this.ImageSource = bitmapImage;
                            //this.Xcord = (rectang.X / 1000000);
                            //this.Ycordi = (rectang.Width / 1000000);
                            #region Focus

                            //MainViewModel MVM = new MainViewModel();
                            FocusInfo();
                            //Focus.Height = (MVM.recFoco.height * .10F);
                            //Focus.Width = (MVM.recFoco.width * .10F);
                            var settin = Properties.Settings.Default.ratio;
                            #endregion
                        }));

                        exceptionCount = 0;

                    }
                    catch (Exception ex)
                    {
                        loggerCam.Error(DateTime.Now.ToString("yyyyMMddmmss") + " StartCapturingCommand_CanExecute " + "---ERROR!!!" + ex.Message);
                        Thread.Sleep(100);
                        if (++exceptionCount > 10)
                        {

                            throw;
                        }
                    }
                }
                // System.Windows.Forms.MessageBox.Show("Error se desconecto la camara.", "Error");
                if (!On_camera())
                {
                    loggerCam.Info(DateTime.Now.ToString("yyyyMMddmmss") + " StartCapturingCommand_CanExecute " + "---Error se desconecto la camara");
                    System.Windows.Forms.MessageBox.Show("Error se desconecto la camara.", "Error");
                    return;
                }
            }).ContinueWith((previewsTask) =>
            {
            },
                UITaskScheduler);
        }

        #endregion

        #region StopCapturingCommand

        public ICommand StopCapturingCommand { get { return new RelayCommand(StopCapturingCommand_Executed, StopCapturingCommand_CanExecute); } }

        private bool StopCapturingCommand_CanExecute()
        {
            loggerCam.Info(DateTime.Now.ToString("yyyyMMddmmss") + " StopCapturingCommand_CanExecute " + "---");
            return true;
        }
        private void StopCapturingCommand_Executed()
        {
            //StartCapturingCommand_Executed();
            loggerCam.Info(DateTime.Now.ToString("yyyyMMddmmss") + " StopCapturingCommand_Executed " + "---");

            if (factorX != 0)
            {
                if (initialCamara)
                {
                    if (!On_camera())
                    {
                        loggerCam.Info(DateTime.Now.ToString("yyyyMMddmmss") + " StopCapturingCommand_Executed " + "---Error La camara no se encuentra conectada On_Camera() = false");
                        System.Windows.Forms.MessageBox.Show("La camara no se encuentra conectada. Verifiquela por favor", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    this.Camera.LiveViewAutoFocusMode = Framework.Enums.LiveViewAutoFocusMode.LiveFace;
                    //this.Camera.AEMode = Framework.Enums.AEMode.CloseUp;
                    this.Camera.AEMode = Framework.Enums.AEMode.Portrait;
                    // this.Camera.ApertureValue = Framework.Enums.ApertureValue.AV_00;
                    //this.Camera.
                    this.Camera.LiveViewAutoFocusMode = Framework.Enums.LiveViewAutoFocusMode.LiveFace;
                    this.Camera.ShutterPressed(5);
                    this.Camera.TakePhoto();

                    Stream str = this.Camera.GetLiveViewImage();
                    System.Drawing.Image img = System.Drawing.Image.FromStream(str);

                    img.Save(Path.Combine(RutaApp, "myImage.Jpeg"));//, ImageFormat.Jpeg);
                    //this.Camera.SaveTo="C:\\Users\\Public\\Pictures";
                    this.Camera.ImageSaveDirectory = RutaApp;
                    this.Camera.StopLiveView();
                    this.FrameworkManager.Dispose();
                    //corta(xY);
                    Recortar();
                }
                else
                {
                    loggerCam.Info(DateTime.Now.ToString("yyyyMMddmmss") + " StopCapturingCommand_Executed " + "---Por favor inicializar la camara y tomar la fotografia initialCamara == false");
                    System.Windows.MessageBox.Show("Por favor inicializar la camara y tomar la fotografia. Gracias", "Advertencia", MessageBoxButton.OK);
                }
            }
            else
            {
                loggerCam.Info(DateTime.Now.ToString("yyyyMMddmmss") + " StopCapturingCommand_Executed " + "---No se a podido enfocar algun rostro, intentelo de nuevo por factorX == 0");
                System.Windows.MessageBox.Show("No se a podido enfocar algun rostro, intentelo de nuevo por favor.", "Advertencia", MessageBoxButton.OK);
            }
        }

        #endregion

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            loggerCam.Info(DateTime.Now.ToString("yyyyMMddmmss") + " Dispose " + "---");
            FrameworkManager.Dispose();
        }

        #endregion

        private Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            // BitmapImage bitmapImage = new BitmapImage(new Uri("../Images/test.png", UriKind.Relative));
            loggerCam.Info(DateTime.Now.ToString("yyyyMMddmmss") + " BitmapImage2Bitmap " + "---");
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }
        public ICommand RecortarCapturingCommand { get { return new RelayCommand(RecortarCapturingCommand_Executed, RecortarCapturingCommand_CanExecute); } }

        private bool RecortarCapturingCommand_CanExecute()
        {
            loggerCam.Info(DateTime.Now.ToString("yyyyMMddmmss") + " RecortarCapturingCommand_CanExecute " + "---");
            return true;
        }
        private void RecortarCapturingCommand_Executed()
        {
            loggerCam.Info(DateTime.Now.ToString("yyyyMMddmmss") + " RecortarCapturingCommand_Executed " + "---");
            Recortar();
        }
        private void Recortar()
        {
            loggerCam.Info(DateTime.Now.ToString("yyyyMMddmmss") + " Recortar " + "---");
            if (File.Exists(Path.Combine(RutaApp, "myImage.Jpeg")))
            {
                if (ValorX == 0.0)
                {
                    string xY = Properties.Settings.Default.ratio;
                    string[] sep = xY.Split('_');
                    ValorX = Convert.ToInt32(sep[0]);
                    ValorY = Convert.ToInt32(sep[1]);
                    Ancho = Convert.ToInt32(sep[2]);
                    Alto = Convert.ToInt32(sep[3]);
                }
                RectangleF recOrigen; //Rectángulo que será recortado del la fotografía"
                RectangleF recDestino; //Rectángulo de destino de la imagen            "
                Bitmap bmp; //Bitmap para el manejo delas imagenes                    "

                //Factor de escala de la imagen
                double dobEscalaAncho;
                double dobEscalaAlto;


                Byte[] ByteBMP = File.ReadAllBytes(Path.Combine(RutaApp, "myImage.Jpeg"));

                Bitmap bmp1 = (Bitmap)System.Drawing.Image.FromStream(new MemoryStream(ByteBMP));
                //bmp1.SetResolution(368.0F, 368.0F);
                ByteBMP = new Byte[0];
                int resp = (bmp1.Width / 2);
                int rest = (bmp1.Width / 2) - 95;
                //if (posX > 280 && posX < 350)
                //rest = (bmp1.Width / 2);
                dobEscalaAncho = ((double)bmp1.Width / 410);
                dobEscalaAlto = ((double)bmp1.Height / 300);
                //Crea el rectángulo de origen con las dimensiones especificadas
                //recOrigen = new Rectangle((int)Math.Round((posX + 2) * dobEscalaAncho, 0), (int)Math.Round(((posY) * dobEscalaAlto), 0), (int)Math.Round(200 * dobEscalaAncho, 0), (int)Math.Round((250) * dobEscalaAlto, 0));
                recOrigen = new RectangleF((float)ValorX, (float)ValorY, (float)Ancho, (float)Alto);
                //recOrigen = new Rectangle(ValorX,/*y*/ValorY,/*ancho*/Ancho,/*alto*/Alto);
                //Crea el rectángulo de destino con las dimensiones espcificadas
                recDestino = new RectangleF(0,/*y*/0,/*ancho*/recOrigen.Width,/*alto*/recOrigen.Height);//600,750);

                //Instancia los elementos graficos para la manipulación
                bmp = new Bitmap((int)recDestino.Width, (int)recDestino.Height);

                //bmp.SetResolution(368.0F, 368.0F);
                Graphics g = System.Drawing.Graphics.FromImage((System.Drawing.Image)bmp);
                //Bitmap bmpCrop = g.Clone(cropArea, bmpImage.PixelFormat);
                //Recorta la imagen
                //g.DrawImage()
                g.DrawImage((System.Drawing.Image)bmp1, recDestino, recOrigen, GraphicsUnit.Pixel);
                System.Drawing.Image img = (System.Drawing.Image)bmp;
                img.Save(Path.Combine(RutaApp, "fotoRecort.jpeg"));

            }
            else
            {
                loggerCam.Info(DateTime.Now.ToString("yyyyMMddmmss") + " Recortar " + "---Error Aun no hay fotografia para poder recortar; No existe myImage.Jpeg");
                System.Windows.MessageBox.Show("Aun no hay fotografia para poder recortar.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public ICommand RecortarDosCapturingCommand { get { return new RelayCommand(RecortarDosCapturingCommand_Executed, RecortarDosCapturingCommand_CanExecute); } }

        private bool RecortarDosCapturingCommand_CanExecute()
        {
            loggerCam.Info(DateTime.Now.ToString("yyyyMMddmmss") + " RecortarDosCapturingCommand_CanExecute " + "---");
            return true;
        }
        private void RecortarDosCapturingCommand_Executed()
        {
            loggerCam.Info(DateTime.Now.ToString("yyyyMMddmmss") + " RecortarDosCapturingCommand_Executed " + "---");
            RecortarDos();
        }
        private void RecortarDos()
        {
            loggerCam.Info(DateTime.Now.ToString("yyyyMMddmmss") + " RecortarDos " + "---");
            int i = 0;
            if (File.Exists(Path.Combine(RutaApp, "myImage.Jpeg")))
            {
                MainView mv = new MainView();

                string xY = Properties.Settings.Default.ratio;
                string[] str = xY.Split('_');
                int posX = 0;
                int posY = 0;
                int ancho = 0;
                int alto = 0;

                posX = int.Parse(str[0]);
                posY = int.Parse(str[1]);
                ancho = int.Parse(str[2]);
                alto = int.Parse(str[3]);
                if (posX == -600)
                {
                    label = "Recorte Manual";
                    RecttX = 0;
                }
                else
                {
                    Rectangle recOrigen; //Rectángulo que será recortado del la fotografía"
                    Rectangle recDestino; //Rectángulo de destino de la imagen            "
                    Bitmap bmp; //Bitmap para el manejo delas imagenes                    "
                    Byte[] ByteBMP = File.ReadAllBytes(Path.Combine(RutaApp, "myImage.Jpeg"));
                    Bitmap bmp1 = (Bitmap)System.Drawing.Image.FromStream(new MemoryStream(ByteBMP));
                    //bmp1.SetResolution(368.0F, 368.0F);
                    ByteBMP = new Byte[0];
                    recOrigen = new Rectangle(posX,/*y*/ posY,/*ancho*/ ancho,/*alto*/ alto);
                    //Crea el rectángulo de destino con las dimensiones especificadas
                    recDestino = new Rectangle(0, 0, ancho, alto);//600,750);

                    //Instancia los elementos graficos para la manipulación
                    bmp = new Bitmap(recDestino.Width, recDestino.Height);

                    //bmp.SetResolution(368.0F, 368.0F);
                    Graphics g = System.Drawing.Graphics.FromImage((System.Drawing.Image)bmp);
                    //Bitmap bmpCrop = g.Clone(cropArea, bmpImage.PixelFormat);
                    //Recorta la imagen
                    //g.DrawImage()
                    g.DrawImage((System.Drawing.Image)bmp1, recDestino, recOrigen, GraphicsUnit.Pixel);
                    System.Drawing.Image img = (System.Drawing.Image)bmp;
                    img.Save(Path.Combine(RutaApp, "fotoRecort.jpeg"));//, System.Drawing.Imaging.ImageFormat.Jpeg);


                }

            }
            else
            {
                loggerCam.Info(DateTime.Now.ToString("yyyyMMddmmss") + " RecortarDos " + "---Error aun no hay fotografia para poder recortar;NO existe myImage.Jpeg");
                System.Windows.MessageBox.Show("Aun no hay fotografia para poder recortar.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        #region Metodo para validar si la camara esta conectada
        private bool On_camera()
        {
            loggerCam.Info(DateTime.Now.ToString("yyyyMMddmmss") + " On_camera " + "---");
            IntPtr listDevice;
            int contador;
            EDSDK.EdsGetCameraList(out listDevice);//Obetiene la lista de los dispositivos conectados
            EDSDK.EdsGetChildCount(listDevice, out contador);
            if (contador == 0)
            {
                initialCamara = false;
                return false;
            }
            EDSDK.EdsGetChildAtIndex(listDevice, this._numDispFoto, out camlist);
            if (camlist == IntPtr.Zero)
            {
                initialCamara = false;
                return false;
            }
            return true;
            //EDSDK.EdsGetChildCount(camlist, out GetCameras);
            //return GetCameras > 0 ? true : false;
        }
        #endregion



        #region Focus

        public void FocusInfo()
        {

            loggerCam.Info(DateTime.Now.ToString("yyyyMMddmmss") + " FocusInfo " + "---");

            uint error = EDSDK.EDS_ERR_OK;
            IntPtr proData;
            int tamProp;
            var focusInfo = new EDSDK.EdsFocusInfo();

            factorX = 0;
            double factorY = 0;
            double factorW = 0;
            double factorH = 0;
            bool enfoque = false;
            double difAncho = 0;
            double difAlto = 0;

            //asignados espacios de memoria no asignados
            tamProp = Marshal.SizeOf(focusInfo);
            proData = Marshal.AllocHGlobal(tamProp);
            //damos los valores a los parametros de las coordenadas
            recFoco.height = 0;
            recFoco.width = 0;
            recFoco.x = 0;
            recFoco.y = 0;

            error = EDSDK.EdsGetPropertyData(camlist, EDSDK.PropID_FocusInfo, 0, tamProp, proData);
            if (error == EDSDK.EDS_ERR_OK)
            {
                focusInfo = (EDSDK.EdsFocusInfo)Marshal.PtrToStructure(proData, typeof(EDSDK.EdsFocusInfo));
                for (int i = 0; i < focusInfo.focusPoint.Length; i++)
                {
                    if (focusInfo.focusPoint[i].valid == 1)
                    {
                        enfoque = true;
                        recFoco = focusInfo.focusPoint[i].rect;
                        altImg = focusInfo.imageRect.height;
                        ancImg = focusInfo.imageRect.width;
                        xImg = focusInfo.imageRect.x;
                        yImg = focusInfo.imageRect.y;

                        factorX = ((double)recFoco.x / (double)ancImg);
                        factorY = ((double)recFoco.y / (double)altImg);
                        ValorX = factorX * 1056;
                        ValorY = factorY * 704;
                        factorW = ((double)recFoco.width / (double)ancImg);
                        factorH = ((double)recFoco.height / (double)ancImg);
                        AnchoD = factorW * recFoco.width;//Ancho nativo
                        AltoD = factorH * recFoco.height;//alto nativo

                        ValorX = ValorX - 150;
                        ValorY = ValorY - 270;

                        if (ValorY > 88)
                        {
                            ValorY = 88;
                        }
                        else if (ValorY < 3)
                        {
                            ValorY = 3;
                        }
                        if (ValorX > 538)
                        {
                            ValorX = 538;
                        }
                        else if (ValorX < 10)
                        {
                            ValorX = 10;
                        }
                        Alto = 600;
                        Ancho = 510;
                        RecttX = ValorX;
                        RecttY = ValorY;
                        ValPhoto();
                        break;
                    }


                }

            }
        }

        #endregion

    }
}