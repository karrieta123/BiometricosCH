using Entidades;
using Newtonsoft.Json;
using RealScan;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace Finger
{
    public partial class Form1 : Form
    {
        private List<Thread> hilos = new List<Thread>();
        public int activoBoton = 1;//1 activo 0 inactivo
        public int dedoActual { get; set; }
        private int m_captureDir = 0;
        private int m_captureMode = 0;
        private int m_result = 0;//resultado de la DLL
        private string m_errorMsg = "";// Muestra el error
        private int deviceHandle = 0;//Número de Dispositivo
        private bool m_prevStopped = true;
        private int m_slapType = 0;
        private int m_fingerCount = 0;
        private bool error = true;
        private PrevMode _selectedPrevMode;
        static int UsuarioId;
        static string licencia = "";
        static string conect = "";
        private IntPtr PictureBoxHeight = IntPtr.Zero;
        static string comentarios = "";
        public int vuelta { get; set; }
        enum PrevMode
        {
            directDraw,
            callbackDraw,
            advCallbackDraw
        }
        RSPreviewDataCallback previewCallback;
        RSAdvPreviewCallback advPreviewCallback;
        CaptureDataCallback rawCaptureCallback;
        int[] fingers = new int[] { 4, 5, 2 };
        public Form1(string id, string conn, int userId)
        {
            licencia = id;
            conect = conn;
            UsuarioId = userId;
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = true;
        }

        #region Metodos Privados

        #region Metodo que nos tiene los metodos principales
        private void FullMethods(int f)
        {
            StartSDK();
            StartDevice();
            m_result = RealScanSDK.RS_SetAutomaticCalibrate(deviceHandle, false);
            SetPreview();
            CaptureMode_SelectedIndexChanged(f);
            InitViewLive();
            if (!error)
            {
                StopDevice();
                activoBoton = 1;
                error = true;
            }
        }
        #endregion

        #region Metodo que inicia el SDK DEL G-10
        private void StartSDK()
        {
            int NumeroDispositivo = 0;
            //RSSDKInfo info = new RSSDKInfo();
            m_result = RealScanSDK.RS_InitSDK(null, 0, ref NumeroDispositivo);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                System.Windows.Forms.MessageBox.Show("La dll RS_SDK no se encuentra instalada.");
            }
        }
        #endregion

        #region Inicia el dispositivo
        private void StartDevice()
        {
            //_selectedPrevMode = PrevMode.directDraw;
            //el número cero indica el dispositivo que ejecutara
            m_result = RealScanSDK.RS_InitDevice(0, ref deviceHandle);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {

                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                Errores(m_result);
                //StartDevice();
            }
            if (!m_prevStopped)
            {
                if (_selectedPrevMode == PrevMode.directDraw) RealScanSDK.RS_StopViewWindow(deviceHandle);
            }
            m_prevStopped = true;

        }
        #endregion

        #region Iniciamos la captura en Vivo
        private void InitViewLive()
        {
            //RealScanSDK.RS_RegisterCaptureDataCallback(deviceHandle, (CaptureDataCallback)null);
            RealScanSDK.RS_RegisterCaptureDataCallback(deviceHandle, capturaHuellas);

            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                //MsgPanel.Text = m_errorMsg;
            }
            m_result = RealScanSDK.RS_RemoveAllOverlay(deviceHandle);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                //MsgPanel.Text = m_errorMsg;
            }

            m_result = RealScanSDK.RS_StartCapture(deviceHandle, true, 0);//establecemos la vista en vivo con el TRUE
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                Errores(m_result);
                error = false;
            }
        }
        private void capturaHuellas(int deviceHandle, int errorCode, IntPtr imageData, int imageWidth, int imageHeight)
        {

            IntPtr slapInfoArray;
            if (errorCode != RealScanSDK.RS_SUCCESS)
            {

            }
            int numOfFingers = 0;
            IntPtr[] ImageBuffer = new IntPtr[4];
            int[] ImageWidth = new int[4];
            int[] ImageHeight = new int[4];
            RSSlapInfoArray slapInfoA = new RSSlapInfoArray();

            SegmentCaptureProcess(imageData, imageWidth, imageHeight, deviceHandle, ref slapInfoA, ref numOfFingers, ref ImageBuffer, ref ImageWidth, ref ImageHeight);
            if (SegmentSaveImageCaptureProcess(imageData, imageWidth, imageHeight, numOfFingers, slapInfoA, ImageBuffer, ImageWidth, ImageHeight))
            {
                for (int i = 0; i < 4; i++)
                {
                    RealScanSDK.RS_FreeImageData(ImageBuffer[i]);
                }
            }
            StopDevice();
            foreach (Thread item in hilos)
            {
                if (item.IsAlive)
                {
                    item.Abort();
                }
            }
            System.Windows.Forms.MessageBox.Show(GetMenssage(vuelta), "Aviso", MessageBoxButtons.OK);
            activoBoton = 1;

        }
        public static void GIF()
        {
            //Thread.Sleep(10);
            Work w = new Work();
            w.ShowDialog();
        }
        private bool SegmentSaveImageCaptureProcess(IntPtr imageData, int imageWidth, int imageHeight, int numOfFingers, RSSlapInfoArray slapInfoA, IntPtr[] ImageBuffer, int[] ImageWidth, int[] ImageHeight)
        {

            bool imprimir = false;
            bool ban = true;
            //RealScanSDK.RS_SaveBitmap(imageData, imageWidth, imageHeight, saveDialog.FileName + ".bmp");
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                //MsgPanel.Text = m_errorMsg;
            }

            if ((vuelta == 4 ? vuelta : vuelta - 1) == numOfFingers)
            {
                imprimir = true;
            }
            else
            {
                string mensaje = String.Format("¿El dispositivo solo detecto {0} de {1} dedos esta de acuerdo?", numOfFingers, (vuelta == 4 ? vuelta : vuelta - 1));
                if (System.Windows.Forms.MessageBox.Show(mensaje, "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    fingerless fic = new fingerless();
                    fic.ShowDialog();
                    string causa = fic.causa;
                    imprimir = true;
                    if (causa != "")
                    {
                        comentarios = causa;
                    }
                }
                else
                {
                    imprimir = false;
                }
            }
            if (imprimir)
            {
                for (int i = 0; i < numOfFingers; i++)
                {
                    string name = NameImage(vuelta);
                    string ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), name);//Ruta donde vamos a guardar las imagenes                
                    ruta = ruta + "_" + slapInfoA.RSSlapInfoA[i].fingerType + ".bmp";
                    string imgName = name + "_" + slapInfoA.RSSlapInfoA[i].fingerType;
                    if (name == "thumbs")
                    {
                        ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), name);
                        ruta = (i == 1 ? ruta + "_1.bmp" : ruta + "_6.bmp");
                        imgName = (i == 1 ? name + "_1" : name + "_6");
                    }
                    //string rutaTemp = System.IO.Path.Combine(ruta, "Finger_" + slapInfoA.RSSlapInfoA[i].fingerType + ".bmp");
                    RealScanSDK.RS_SaveBitmap(ImageBuffer[i], ImageWidth[i], ImageHeight[i], ruta);

                    if (m_result != RealScanSDK.RS_SUCCESS)
                    {
                        RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                        //MsgPanel.Text = m_errorMsg;
                    }

                    if (File.Exists(ruta))
                    {
                        try
                        {
                            System.Drawing.Image img = ConvertToBitmap(ruta);
                            SaveImageBD(imgName, img);
                        }
                        catch (Exception ex)
                        {
                            MessageBoxResult result = System.Windows.MessageBox.Show("No se pudo guardar la información");
                        }

                    }
                }
            }
            return ban;
        }

        public Bitmap ConvertToBitmap(string fileName)
        {
            Bitmap bitmap;
            using (Stream bmpStream = System.IO.File.Open(fileName, System.IO.FileMode.Open))
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(bmpStream);


                bitmap = new Bitmap(image);

            }
            return bitmap;
        }

        static void SaveImageBD(string fields, System.Drawing.Image image)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (var client = new HttpClient())
                    {
                        // Convert Image to byte[]
                        image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] imageBytes = ms.ToArray();
                        string ruta = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                        using (FileStream fs = new FileStream(System.IO.Path.Combine(ruta, fields + ".jpeg"), FileMode.Open, FileAccess.Read))
                        {
                            try
                            {
                                byte[] data = new byte[fs.Length];
                                fs.Read(data, 0, data.Length);
                                fs.Close();

                                string base64String = Convert.ToBase64String(imageBytes);

                                Dictionary<string, object> postParameters = new Dictionary<string, object>();
                                postParameters.Add("pg", "uploadfiledb");
                                postParameters.Add("field", fields);
                                postParameters.Add("usrid", UsuarioId.ToString());
                                postParameters.Add("NumeroLicencia", licencia);
                                postParameters.Add("Comentarios", comentarios);
                                postParameters.Add("Filedata", new FormUpload.FormUpload.FileParameter(data, fields + ".jpeg", "image/jpeg"));

                                string userAgent = "tlaxcala";
                                HttpWebResponse webResponse = FormUpload.FormUpload.MultipartFormDataPost(conect, userAgent, postParameters);

                                string status = "";
                                if (webResponse.StatusCode == HttpStatusCode.OK)
                                {
                                    status = "OK";
                                }
                                if (webResponse.StatusCode == HttpStatusCode.Forbidden)
                                {
                                    status = "Forbidden";
                                }
                                StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                                string fullResponse = responseReader.ReadToEnd();
                                webResponse.Close();
                                string sEvent = fullResponse;
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        #region Metodo para crear el archivo Json
        private void CreateJson(string causa)
        {
            string ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "finger.json");
            List<JsonFinger> li = new List<JsonFinger>();
            li.Add(new JsonFinger { id_finger = causa });
            string l = JsonConvert.SerializeObject(li);
            File.WriteAllText(ruta, l);
        }
        #endregion
        private void SegmentCaptureProcess(IntPtr imageData, int imageWidth, int imageHeight, int deviceHandle, ref RSSlapInfoArray slapInfo, ref int numOfFingers, ref IntPtr[] ImageBuffer, ref int[] ImageWidth, ref int[] ImageHeight)
        {
            Thread t = new Thread(GIF);
            t.Start();
            hilos.Add(t);

            RSSlapInfoArray slapInfoA = new RSSlapInfoArray();
            IntPtr slapInfoArray;

            int captureMode = 0;
            int captureOption = 0;
            int slapType = 1;

            for (int i = 0; i < 4; i++)
            {
                ImageBuffer[i] = (IntPtr)0;
                ImageWidth[i] = 0;
                ImageHeight[i] = 0;
            }

            int _size = Marshal.SizeOf(typeof(RSSlapInfoArray));
            slapInfoArray = Marshal.AllocHGlobal(_size);
            Marshal.StructureToPtr(slapInfoA, slapInfoArray, true);

            int fingerType = 0;
            int[] missingFingerArray = new int[] { 0, 0, 0, 0 };

            int n = 0;
            m_result = RealScanSDK.RS_GetCaptureMode(deviceHandle, ref captureMode, ref captureOption);
            switch (vuelta)
            {
                case RealScanSDK.RS_CAPTURE_FLAT_TWO_FINGERS:
                case RealScanSDK.RS_CAPTURE_FLAT_TWO_FINGERS_EX:
                    slapType = RealScanSDK.RS_SLAP_TWO_FINGER;
                    break;
                case RealScanSDK.RS_CAPTURE_FLAT_LEFT_FOUR_FINGERS:
                    slapType = RealScanSDK.RS_SLAP_LEFT_FOUR;
                    fingerType = RealScanSDK.RS_FGP_LEFT_LITTLE;
                    break;
                case RealScanSDK.RS_CAPTURE_FLAT_RIGHT_FOUR_FINGERS:
                    slapType = RealScanSDK.RS_SLAP_RIGHT_FOUR;
                    fingerType = RealScanSDK.RS_FGP_RIGHT_INDEX;
                    break;

            }
            m_result = RealScanSDK.RS_Segment4(imageData, imageWidth, imageHeight, slapType, ref numOfFingers, ref slapInfoArray, ref ImageBuffer[0], ref ImageWidth[0],
                                             ref ImageHeight[0], ref ImageBuffer[1], ref ImageWidth[1], ref ImageHeight[1], ref ImageBuffer[2], ref ImageWidth[2],
                                             ref ImageHeight[2], ref ImageBuffer[3], ref ImageWidth[3], ref ImageHeight[3]);

            if (m_result != RealScanSDK.RS_SUCCESS)//aQUI SE VERIFICA QUE SEA LA MANO CORRECTA
            {
                Errores(m_result);
            }

            //MsgPanel.Text = "Quality:";

            slapInfoA = (RSSlapInfoArray)Marshal.PtrToStructure(slapInfoArray, typeof(RSSlapInfoArray));
            RealScanSDK.RS_FreeImageData(slapInfoArray);
            int j = 0;
            for (int i = 0; i < numOfFingers; i++)
            {
                if (slapInfoA.RSSlapInfoA[i].fingerType == RealScanSDK.RS_FGP_UNKNOWN)
                {
                    if (slapType == RealScanSDK.RS_SLAP_LEFT_FOUR)
                    {
                        while (fingerType == missingFingerArray[j])
                        {
                            fingerType--;
                            j++;
                        }

                        slapInfoA.RSSlapInfoA[i].fingerType = fingerType--;
                    }
                    else if (slapType == RealScanSDK.RS_SLAP_RIGHT_FOUR)
                    {
                        while (fingerType == missingFingerArray[j])
                        {
                            fingerType++;
                            j++;
                        }

                        slapInfoA.RSSlapInfoA[i].fingerType = fingerType++;
                    }
                }
                slapInfo = slapInfoA;
            }
        }
        #endregion

        #region Metodo que muestra la vista en vivo
        private void SetPreview()
        {
            //RealScanSDK.RS_StopViewWindow(deviceHandle);

            switch (this._selectedPrevMode)
            {
                case PrevMode.directDraw:
                    RSRect rect = new RSRect();
                    RealScanSDK.GetClientRect(pbHuella.Handle, ref rect);//modificacion de pbHuella.Handle a IntPr
                    m_result = RealScanSDK.RS_SetViewWindow(this.deviceHandle, pbHuella.Handle, rect, true);//modificacion de pbHuella.Handle a IntPr
                    if (m_result != RealScanSDK.RS_SUCCESS)
                    {
                        RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                        // MsgPanel.Text = m_errorMsg;
                        return;
                    }
                    m_prevStopped = false;
                    pbHuella.Invalidate();
                    pbHuella.Show();
                    break;
                case PrevMode.callbackDraw:
                    m_result = RealScanSDK.RS_RegisterPreviewCallback(deviceHandle, previewCallback);
                    if (m_result != RealScanSDK.RS_SUCCESS)
                    {
                        RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                        //MsgPanel.Text = m_errorMsg;
                    }
                    break;
                case PrevMode.advCallbackDraw:
                    m_result = RealScanSDK.RS_RegisterAdvPreviewCallback(deviceHandle, advPreviewCallback);
                    if (m_result != RealScanSDK.RS_SUCCESS)
                    {
                        RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                        //MsgPanel.Text = m_errorMsg;
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion


        #endregion

        private void CaptureMode_SelectedIndexChanged(int numeroIdCaptureFinger)
        {
            int[] tmpCaptureModes = new int[11] {
                RealScanSDK.RS_CAPTURE_DISABLED,RealScanSDK.RS_CAPTURE_ROLL_FINGER,RealScanSDK.RS_CAPTURE_FLAT_SINGLE_FINGER,
                                                RealScanSDK.RS_CAPTURE_FLAT_TWO_FINGERS,RealScanSDK.RS_CAPTURE_FLAT_LEFT_FOUR_FINGERS,RealScanSDK.RS_CAPTURE_FLAT_RIGHT_FOUR_FINGERS,
                                                RealScanSDK.RS_CAPTURE_FLAT_LEFT_PALM,RealScanSDK.RS_CAPTURE_FLAT_RIGHT_PALM,RealScanSDK.RS_CAPTURE_FLAT_SINGLE_FINGER_EX,
                                                RealScanSDK.RS_CAPTURE_FLAT_TWO_FINGERS_EX, RealScanSDK.RS_CAPTURE_FLAT_MANUAL};
            m_captureMode = numeroIdCaptureFinger;
            switch (m_captureMode)
            {
                //    case RealScanSDK.RS_CAPTURE_FLAT_SINGLE_FINGER:
                //    case RealScanSDK.RS_CAPTURE_FLAT_SINGLE_FINGER_EX:
                //        m_slapType = RealScanSDK.RS_SLAP_ONE_FINGER;
                //        m_fingerCount = 1;
                //        break;

                case RealScanSDK.RS_CAPTURE_FLAT_TWO_FINGERS:
                case RealScanSDK.RS_CAPTURE_FLAT_TWO_FINGERS_EX:
                    m_slapType = RealScanSDK.RS_SLAP_TWO_THUMB;
                    m_fingerCount = 2;
                    break;

                case RealScanSDK.RS_CAPTURE_FLAT_LEFT_FOUR_FINGERS:
                    m_slapType = RealScanSDK.RS_SLAP_LEFT_FOUR;
                    m_fingerCount = 4;
                    break;

                case 5:
                    m_slapType = RealScanSDK.RS_SLAP_RIGHT_FOUR;
                    m_fingerCount = 4;
                    break;

                default:
                    break;
            }

            int[] nCaptDir = new int[3] { RealScanSDK.RS_CAPTURE_DIRECTION_DEFAULT, RealScanSDK.RS_CAPTURE_DIRECTION_LEFT, RealScanSDK.RS_CAPTURE_DIRECTION_RIGHT };

            m_result = RealScanSDK.RS_SetCaptureModeWithDir(deviceHandle, m_captureMode, nCaptDir[m_captureDir], 0, true);

            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
            }
        }
        #region Se detiene el dispositivo
        private void StopDevice()
        {
            m_result = RealScanSDK.RS_AbortCapture(deviceHandle);
            m_result = RealScanSDK.RS_StopViewWindow(deviceHandle);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                //RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                //lblerrores.Text = m_errorMsg;
                return;
            }
            m_prevStopped = true;
            //Salimos del dispositivo
            m_result = RealScanSDK.RS_ExitDevice(deviceHandle);

            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
            }
        }
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        }

        #region Botones
        private void BtnInicio_Click(object sender, EventArgs e)
        {
            if (activoBoton == 1)
            {
                try
                {
                    activoBoton = 0;
                    vuelta = 4;
                    FullMethods(4);
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
        }
        private void btnDerecha_Click(object sender, EventArgs e)
        {
            if (activoBoton == 1)
            {
                activoBoton = 0;
                vuelta = 5;
                FullMethods(5);
            }
        }

        private void btnPulgares_Click(object sender, EventArgs e)
        {
            if (activoBoton == 1)
            {
                activoBoton = 0;
                vuelta = 3;
                FullMethods(3);
            }
        }
        #endregion

        #region Metodo para traer los mensajes correspondientes
        private string GetMenssage(int t)
        {
            string mensaje = "";
            switch (t)
            {
                case 4:
                    mensaje = "Las huellas de la mano izquierda se han tomado correctamente. Gracias.";
                    break;
                case 5:
                    mensaje = "Las huellas de la mano derecha se han tomado correctamente. Gracias.";
                    break;
                case 3:
                    mensaje = "Las huellas de los pulgares se han tomado correctamente. Gracias.";
                    break;
            }
            return mensaje;
        }

        private string NameImage(int t)
        {
            string mensaje = "";
            switch (t)
            {
                case 4:
                    mensaje = "FingerLeft";
                    break;
                case 5:
                    mensaje = "FingerRight";
                    break;
                case 3:
                    mensaje = "thumbs";
                    break;
            }
            return mensaje;
        }
        #endregion

        #region Metodo que nos mande el mensaje Correspondiente
        private void Mensaje(string cadena)
        {
            System.Windows.Forms.MessageBox.Show(cadena, "Aviso", MessageBoxButtons.OK);
        }
        #endregion

        #region Metodo de Calibración del Scaner
        private void btnCalibrar_Click(object sender, EventArgs e)
        {

            if (activoBoton == 1)
            {
                StartSDK();
                StartDevice();
                activoBoton = 0;
                m_result = RealScanSDK.RS_Calibrate(deviceHandle);
                m_result = RealScanSDK.RS_SetAutomaticCalibrate(deviceHandle, false);
                if (m_result != RealScanSDK.RS_SUCCESS)
                {
                    RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                    Mensaje(m_errorMsg);
                }
                else
                {
                    Mensaje("Calibración Satisfactoria.");
                }
                StopDevice();
                activoBoton = 1;
            }
        }
        #endregion

        private void btncerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Catalogo de Errores
        private void Errores(int error)
        {
            string mensaje = "";
            switch (error)
            {
                case -10:
                    mensaje = "El SDK no se inicio.";
                    break;
                case -11:
                    mensaje = "El SDK ya esta inicializado.";
                    break;
                case -13:
                    mensaje = "Uno de los parametros es incorrecto.";
                    break;
                case -14:
                    mensaje = "No se puede asignar memoria.";
                    break;
                case -15:
                    mensaje = "La función no se ha implementado.";
                    break;
                case -16:
                    mensaje = "No puede se abri el archivo especificado.";
                    break;
                case -17:
                    mensaje = "No puede se leer el archivo especificado.";
                    break;
                case -18:
                    mensaje = "No puede se escribir el archivo especificado.";
                    break;
                case -100:
                    mensaje = "El dispositivo no esta conectado.";
                    break;
                case -101:
                    mensaje = "El dispositivo no es valido.";
                    break;
                case -102:
                    mensaje = "El identificador del dispositivo es valido.";
                    break;
                case -103:
                    mensaje = "No se pudo inciar el dispositivo.";
                    break;
                case -105:
                    mensaje = "No se a detectado el dispositivo.";
                    break;
                case -106:
                    mensaje = "El dispositivo ya esta inicializado.";
                    break;
                case -107:
                    mensaje = "No se tiene acceso al dispositivo.";
                    break;
                case -108:
                    mensaje = "No puede se escribir en el dispositivo.";
                    break;
                case -109:
                    mensaje = "No se puede escribir.";
                    break;
                case -110:
                    mensaje = "No se puede leer desde le dispositivo.";
                    break;
                case -111:
                    mensaje = "Lectura fuera de tiempo.";
                    break;
                case -112:
                    mensaje = "El modelo de la camara no es soportado.";
                    break;
                case -113:
                    mensaje = "El formato del archivo no es soportado.";
                    break;
                case -114:
                    mensaje = "El comando no es soportado.";
                    break;
                case -115:
                    mensaje = "Intente limpiar el cristal del dispositivo. \nColoque los dedos después de iniciar el dispositivo con el cristal en verde cuando se le indique.. espere.";
                    break;
                case -116:
                    mensaje = "los dedos se colocan en el sensor antes de capturar aperturas. Este error se devolverá sólo si la calibración automática está activada";
                    break;
                case -117:
                    mensaje = "la luz exterior es demasiado fuerte para capturar imágenes.";
                    break;
                case -218:
                    mensaje = "la mano izquierda es capturado por la mano derecha, o viceversa";
                    break;
            }

            if (mensaje != "")
            {
                System.Windows.Forms.MessageBox.Show(mensaje, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        #endregion
    }
}
