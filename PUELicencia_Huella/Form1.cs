using Newtonsoft.Json;
using RealScan;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PUELicencia_Huella
{
    public partial class Form1 : Form
    {
        #region Variables
        int[] IdProcess = new int[3] { 4, 5, 4 };
        List<json> Json_ = new List<json>();
        int Cont_Json = 0;
        int NumDedos = 0;
        int vuelta = 0;//Nos indica en que proceso esta
        int m_result = 0;//resultado de la DLL
        string m_errorMsg = " ";// Muestra el error
        int deviceHandle = 0;//Número de Dispositivo
        bool m_prevStopped = true;
        private int i = 0;// se tiene en valor 0 es el que controle la iteracion de la captura
        RSPreviewDataCallback previewCallback;
        RSAdvPreviewCallback advPreviewCallback;
        private PrevMode _selectedPrevMode;
        CaptureDataCallback rawCaptureCallback;

        int m_numOfTargets;
        enum PrevMode
        {
            directDraw,
            callbackDraw,
            advCallbackDraw
        }
        enum callbackMode
        {
            none,
            save,
            segment,
            enlarge,
            saveNseg,
            seqCheck
        }

        int m_captureMode = 0;
        int m_slapType = 0;
        int m_fingerCount = 0;
        int m_captureDir = 0;
        bool m_bCaptureModeSelected = false;

        byte[][] m_pSeqCheckTargetImages = new byte[5][];
        int[] m_nSeqCheckTargetWidths = new int[5];
        int[] m_nSeqCheckTargetHeights = new int[5];
        int[] m_nSeqCheckTargetSlapTypes = new int[5];
        int m_numOfTargerFingers = 0;

        #endregion

        public Form1()
        {
            InitializeComponent();
            StartSDK();
            // StartDevice();
            //el número cero indica el dispositivo que ejecutara
            m_result = RealScanSDK.RS_InitDevice(0, ref deviceHandle);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MessageBox.Show("El dispositivo se encuentra desconectado. Por favor verifiquelo.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                i = 3;
            }
            StopDevice();
        }

        #region Mandamos a llamar a todos los metodos
        private void AllMethod(int l, bool EnabledStop = true)
        {
            bool fallo = true;
            fallo = StartDevice();//Iniciamos el dispositivo    
            fallo = fallo == true ? StartProcess(IdProcess[l]) : false;
            fallo = fallo == true ? PrintHuella(IdProcess[l]) : false;//Le indicamos cuantos dedos como minimo puede leer
            if (EnabledStop) StopDevice();
            if (!fallo)
            {
                MessageBox.Show("El dispositivo a sido desconectado. De clic en aceptar para reintentar la captura.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                StopDevice();
                StartSDK();
                AllMethod(vuelta, true);
            }
        }
        #endregion

        #region Load
        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            try
            {
                if (i != 3)
                {
                    StartSDK();
                    #region Proceso de Captura
                    for (i = 0; i < 3; i++)
                    {
                        vuelta = i;
                        AllMethod(i);
                    }

                    MessageBox.Show("Proceso finalizado.");
                }
                this.Close();

                //MessageBox.Show("Proceso terminado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //this.Close();
                #endregion

            }
            catch (DllNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        #endregion

        #region Metodos del SDK

        #region Inicializamos el SDK
        private void StartSDK()
        {
            int numOfDevice = 0;
            RSSDKInfo sdkInfo = new RSSDKInfo();

            m_result = RealScanSDK.RS_InitSDK(null, 0, ref numOfDevice);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MessageBox.Show("La dll RS_SDK no se encuentra instalada.");
            }
            //Tomamos la versión y la mostramos
            m_result = RealScanSDK.RS_GetSDKInfo(ref sdkInfo);
            if (m_result == RealScanSDK.RS_SUCCESS)
            {
                lblversion.Text = System.Text.Encoding.ASCII.GetString(sdkInfo.version);
            }
        }
        #endregion

        #region Inicia el dispositivo
        private bool StartDevice()
        {
            bool res = true;
            //el número cero indica el dispositivo que ejecutara
            m_result = RealScanSDK.RS_InitDevice(0, ref deviceHandle);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {

                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                MessageBox.Show("El dispositivo se encuentra desconectado. Por favor verifiquelo.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                StartDevice();
            }
            if (!m_prevStopped)
            {
                if (_selectedPrevMode == PrevMode.directDraw) RealScanSDK.RS_StopViewWindow(deviceHandle);
            }
            m_prevStopped = true;
            return res;
        }
        #endregion

        #region Se detiene el dispositivo
        private void StopDevice()
        {
            if (!m_prevStopped)
            {
                if (_selectedPrevMode == PrevMode.directDraw)
                {
                    m_result = RealScanSDK.RS_StopViewWindow(deviceHandle);
                    if (m_result != RealScanSDK.RS_SUCCESS)
                    {
                        //RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                        //lblerrores.Text = m_errorMsg;
                        return;
                    }
                }
                m_prevStopped = true;
            }
            //Salimos del dispositivo
            m_result = RealScanSDK.RS_ExitDevice(deviceHandle);

            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                //RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                // lblerrores.Text = m_errorMsg;
                return;
            }
        }
        #endregion

        #region Iniciamos la captura
        private bool StartProcess(int id_process)
        {
            if (!ValidateDevice())
            {
                return false;
            }
            CaptureMode(id_process);//Número que se requiere para saber que proceso es, PUEDE SER 4-4-2

            m_result = RealScanSDK.RS_RegisterCaptureDataCallback(deviceHandle, (CaptureDataCallback)null);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                //RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                //lblerrores.Text = m_errorMsg;
            }
            m_result = RealScanSDK.RS_RemoveAllOverlay(deviceHandle);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                //RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                //lblerrores.Text = m_errorMsg;
            }

            m_result = RealScanSDK.RS_StartCapture(deviceHandle, false, 0);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                if (m_result == -201)
                {
                    return false;
                }
                else
                {
                    int ret = RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                    string men = "Intente limpiar el cristal del dispositivo. \nColoque los dedos después de iniciar el dispositivo con el cristal en verde cuando se le indique.. espere";
                    MessageBox.Show(men, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    StartProcess(id_process);
                }
            }
            if (!ValidateDevice())
            {
                return false;
            }
            return true;
        }
        #endregion

        #region Metodo que nos identifica el tipo de captura de Huellas
        /// <summary>
        /// 
        /// </summary>
        /// <param name="NumDevice">Es el número que identifica el Proceso de toma de huella que deseamos</param>
        private void CaptureMode(int NumDevice)
        {
            switch (vuelta)
            {
                case 0:
                    m_captureMode = 4;
                    break;
                case 1:
                    m_captureMode = 5;
                    break;
                case 2:
                    m_captureMode = 3;
                    break;
            }

            switch (m_captureMode)
            {
                case RealScanSDK.RS_CAPTURE_FLAT_TWO_FINGERS:
                case RealScanSDK.RS_CAPTURE_FLAT_TWO_FINGERS_EX:
                    m_slapType = RealScanSDK.RS_SLAP_TWO_THUMB;
                    m_fingerCount = 2;
                    break;
                case RealScanSDK.RS_CAPTURE_FLAT_LEFT_FOUR_FINGERS:
                    m_slapType = RealScanSDK.RS_SLAP_LEFT_FOUR;
                    m_fingerCount = 4;
                    break;

                case RealScanSDK.RS_CAPTURE_FLAT_RIGHT_FOUR_FINGERS:
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
                //RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                //lblerrores.Text = m_errorMsg;
                return;
            }

            int imageWidth = 0;
            int imageHeight = 0;

            m_result = RealScanSDK.RS_GetImageSize(deviceHandle, ref imageWidth, ref imageHeight);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                //RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                //lblerrores.Text = m_errorMsg;
                m_bCaptureModeSelected = false;
                return;
            }

            m_bCaptureModeSelected = true;
        }
        #endregion

        #region Tomamos la Huella
        private bool PrintHuella(int NumProcess)
        {
            if (!ValidateDevice())
            {
                return false;
            }
            MessageBox.Show(GetMenssage(vuelta), "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Minium_Finger(vuelta);
            int imageWidth = 0;
            int imageHeight = 0;
            IntPtr imageData = (IntPtr)0;
            if (!ValidateDevice())
            {
                return false;
            }
            m_result = RealScanSDK.RS_TakeCurrentImageData(deviceHandle, 10000, ref imageData, ref imageWidth, ref imageHeight);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                if (m_result != RealScanSDK.RS_SUCCESS)
                {
                    RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                    lblerrores.Text = m_errorMsg;
                }
                if (imageData != (IntPtr)0)
                {
                    RealScanSDK.RS_FreeImageData(imageData);
                }
            }
            //Aqui ya fue capturada la huella
            SaveSeqCheckTargetProcess(imageData, imageWidth, imageHeight);

            int numOfFingers = 0;
            IntPtr[] ImageBuffer = new IntPtr[4];
            int[] ImageWidth = new int[4];
            int[] ImageHeight = new int[4];
            RSSlapInfoArray slapInfoA = new RSSlapInfoArray();

            if (SegmentCaptureProcess(imageData, imageWidth, imageHeight, deviceHandle, ref slapInfoA, ref numOfFingers, ref ImageBuffer, ref ImageWidth, ref ImageHeight))
            {
                if (4 == (int)callbackMode.saveNseg)
                {
                    SegmentSaveImageCaptureProcess(imageData, imageWidth, imageHeight, numOfFingers, slapInfoA, ImageBuffer, ImageWidth, ImageHeight);
                }
                for (int i = 0; i < 4; i++)
                {
                    RealScanSDK.RS_FreeImageData(ImageBuffer[i]);
                }
                if (!ValidateDevice())
                {
                    return false;
                }

            }
            return true;
        }
        #endregion

        private void SaveSeqCheckTargetProcess(IntPtr imageData, int imageWidth, int imageHeight)
        {
            if (m_numOfTargerFingers + m_fingerCount <= 10 && m_slapType < RealScanSDK.RS_SLAP_ONE_FINGER)
            {
                bool bIsOverlapped = false;
                for (int i = 0; i < m_numOfTargets; i++)
                {
                    if (m_nSeqCheckTargetSlapTypes[i] == m_slapType)
                    {
                        Marshal.Copy(imageData, m_pSeqCheckTargetImages[i], 0, imageWidth * imageHeight);
                        m_nSeqCheckTargetWidths[i] = imageWidth;
                        m_nSeqCheckTargetHeights[i] = imageHeight;
                        bIsOverlapped = true;
                        break;
                    }
                }

                if (!bIsOverlapped)
                {
                    m_pSeqCheckTargetImages[m_numOfTargets] = new byte[imageWidth * imageHeight];
                    Marshal.Copy(imageData, m_pSeqCheckTargetImages[m_numOfTargets], 0, imageWidth * imageHeight);
                    m_nSeqCheckTargetWidths[m_numOfTargets] = imageWidth;
                    m_nSeqCheckTargetHeights[m_numOfTargets] = imageHeight;
                    m_nSeqCheckTargetSlapTypes[m_numOfTargets] = m_slapType;
                    m_numOfTargets++;
                    m_numOfTargerFingers += m_fingerCount;
                }
            }
        }

        #region Procesos de Segmentación de Huella
        private bool SegmentCaptureProcess(IntPtr imageData, int imageWidth, int imageHeight, int deviceHandle, ref RSSlapInfoArray slapInfo,
                                          ref int numOfFingers, ref IntPtr[] ImageBuffer, ref int[] ImageWidth, ref int[] ImageHeight)
        {
            bool mano_diferente = true;
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

            if (m_captureDir != RealScanSDK.RS_CAPTURE_DIRECTION_DEFAULT)
            {
                int captureDir = RealScanSDK.RS_CAPTURE_DIRECTION_DEFAULT;
                m_result = RealScanSDK.RS_GetCaptureModeWithDir(deviceHandle, ref captureMode, ref captureDir, ref captureOption);
            }
            else
                m_result = RealScanSDK.RS_GetCaptureMode(deviceHandle, ref captureMode, ref captureOption);

            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                lblerrores.Text = m_errorMsg;
            }
            switch (captureMode)
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
                m_result = RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                // MsgPanel.Text = m_errorMsg;
                if (m_errorMsg == "The hand type is differnt from the specified\0")
                {
                    MessageBox.Show("La mano es diferente a la especificada. Intentelo de nuevo por favor.");
                    StopDevice();
                    StartSDK();
                    AllMethod(vuelta, true);
                    mano_diferente = false;
                }
            }
            if (mano_diferente)//Esta condicion es para verificar que solo de una vuelta al ejecutarse el error, error la mano no es la correcta
            {
                slapInfoA = (RSSlapInfoArray)Marshal.PtrToStructure(slapInfoArray, typeof(RSSlapInfoArray));
                RealScanSDK.RS_FreeImageData(slapInfoArray);

                int overlayHandle = -1;
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

                    RealScan.RSOverlayQuadrangle quad = new RSOverlayQuadrangle();
                    quad.pos = new RSPoint[4];
                    quad.color = 0x00ff0000;

                    RSRect rect = new RSRect();
                    RealScanSDK.GetClientRect(pbhuellas.Handle, ref rect);

                    quad.pos[0].x = slapInfoA.RSSlapInfoA[i].fingerPosition[0].x * rect.right / imageWidth;
                    quad.pos[0].y = slapInfoA.RSSlapInfoA[i].fingerPosition[0].y * rect.bottom / imageHeight;
                    quad.pos[1].x = slapInfoA.RSSlapInfoA[i].fingerPosition[1].x * rect.right / imageWidth;
                    quad.pos[1].y = slapInfoA.RSSlapInfoA[i].fingerPosition[1].y * rect.bottom / imageHeight;
                    quad.pos[2].x = slapInfoA.RSSlapInfoA[i].fingerPosition[3].x * rect.right / imageWidth;
                    quad.pos[2].y = slapInfoA.RSSlapInfoA[i].fingerPosition[3].y * rect.bottom / imageHeight;
                    quad.pos[3].x = slapInfoA.RSSlapInfoA[i].fingerPosition[2].x * rect.right / imageWidth;
                    quad.pos[3].y = slapInfoA.RSSlapInfoA[i].fingerPosition[2].y * rect.bottom / imageHeight;

                    m_result = RealScanSDK.RS_AddOverlayQuadrangle(deviceHandle, ref quad, ref overlayHandle);
                    m_result = RealScanSDK.RS_ShowOverlay(overlayHandle, true);
                    if (m_result != RealScanSDK.RS_SUCCESS)
                    {
                        lblerrores.Text = "No se puede sobreporner" + m_result;
                    }
                }
                NumDedos = numOfFingers;
                //for (int i = 0; i < numOfFingers; i++)
                //{
                //    lblerrores.Text += "[" + slapInfoA.RSSlapInfoA[i].fingerType + ":" + slapInfoA.RSSlapInfoA[i].imageQuality + "]\n Dedos:" + numOfFingers;
                //}

            }
            return mano_diferente;
        }
        #endregion

        #region Se guarda la huella Segmentada
        private void SegmentSaveImageCaptureProcess(IntPtr imageData, int imageWidth, int imageHeight, int numOfFingers, RSSlapInfoArray slapInfo, IntPtr[] ImageBuffer, int[] ImageWidth, int[] ImageHeight)
        {
            int quality = 0;
            bool bandera = false;
            string[] txtJson = new string[6];
            string Mensajes = "";
            string ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), NameImage(vuelta));//Ruta donde vamos a guardar las imagenes
            m_result = RealScanSDK.RS_SaveBitmap(imageData, imageWidth, imageHeight, ruta);

            #region Bloque de código para mostrar la imagen en panatalla

            int finger = vuelta < 2 ? 4 : 2;
            if (numOfFingers == finger)//Comparamos el # de dedos con el minimo que se debe de capturar de lo contra
            {

                RealScanSDK.RS_GetQualityScore(imageData, imageWidth, imageHeight, ref quality);
                if (quality >= 2)
                {
                    MessageBox.Show("La calidad de la imagen no es la adecuada, se intentara de nuevo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    pbhuellas.Image = null;
                    StopDevice();//Paramos el proceso del dispositivo
                    AllMethod(vuelta, false);//mandamos a llamar a todos los metodos para empezar el proceso
                }
                else
                {
                    Bitmap bs = new Bitmap(ruta);

                    pbhuellas.Image = (Image)bs;
                    bandera = true;
                    MessageBox.Show("Ya puede retirar la mano, Gracias.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                string mensaje = String.Format("¿El dispositivo solo detecto {0} de {1} dedos esta de acuerdo?", numOfFingers, finger);
                if (MessageBox.Show(mensaje, "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    pbhuellas.Image = null;
                    StopDevice();//Paramos el proceso del dispositivo
                    AllMethod(vuelta, false);//mandamos a llamar a todos los metodos para empezar el proceso                    }
                }
                else
                {
                    //Mensaje ms = new Mensaje();
                    //ms.ShowDialog();
                    //Mensajes = ms.mensaje;
                    //ms.Dispose();
                    bandera = true;
                }
            }
            #endregion

            if (bandera)
            {
                txtJson[Cont_Json] = ruta;
                Cont_Json++;
                if (m_result != RealScanSDK.RS_SUCCESS)
                {
                    RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                    lblerrores.Text = m_errorMsg;
                }
                for (int i = 0; i < numOfFingers; i++)
                {
                    string ruta_huella = ruta + "_" + slapInfo.RSSlapInfoA[i].fingerType + ".bmp";
                    if (NameImage(vuelta) == "thumbs")
                    {
                        ruta_huella = (i == 1 ? ruta + "_1.bmp" : ruta + "_6.bmp");
                    }
                    m_result = RealScanSDK.RS_SaveBitmap(ImageBuffer[i], ImageWidth[i], ImageHeight[i], ruta_huella);
                    if (m_result != RealScanSDK.RS_SUCCESS)
                    {
                        RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                        lblerrores.Text = m_errorMsg;
                    }
                    txtJson[Cont_Json] = ruta_huella;

                    Cont_Json++;
                }
                txtJson[5] = Mensajes;
                Json_.Add(new json()
                {
                    imagen = txtJson[0],
                    imagen_uno = txtJson[1],
                    imagen_dos = txtJson[2],
                    imagen_tres = txtJson[3],
                    imagen_cuatro = txtJson[4],
                    mensaje = txtJson[5]
                });
                if (vuelta == 2)
                {
                    SaveJson(Json_);
                }
                Cont_Json = 0;
            }
            //Generamos los datos de las imagenes
        }
        #endregion

        #region Metodo para generar y guardar el JSON
        private void SaveJson(List<json> Json)
        {
            string _Json = JsonConvert.SerializeObject(Json.ToArray());
            System.IO.File.WriteAllText(Path.GetTempPath() + "Nombre.json", _Json);
        }
        #endregion

        #region Metodo para poner el minimo de los dedos que puede detectar
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t">Indica el Número de vuelta que nos indica en el For</param>
        private void Minium_Finger(int t)
        {
            int m_minCount = t < 2 ? 4 : 2;//Condición para poner el minimo de dedos que puede detectar
            m_result = RealScanSDK.RS_SetMinimumFinger(deviceHandle, m_minCount);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                RealScanSDK.RS_GetErrString(m_result, ref m_errorMsg);
                lblerrores.Text = m_errorMsg;
            }
        }
        #endregion

        #endregion

        #region Metodo para traer los mensajes correspondientes
        private string GetMenssage(int t)
        {
            string mensaje = "";
            switch (t)
            {
                case 0:
                    mensaje = "Posicione los dedos de la mano izquierda firmemente sobre el cristal, por favor.";
                    break;
                case 1:
                    mensaje = "Posicione los dedos de la mano derecha firmemente sobre el cristal, por favor.";
                    break;
                case 2:
                    mensaje = "Posicione los pulgares de ambas manos firmemente sobre el cristal, por favor.";
                    break;
            }
            return mensaje;
        }

        private string NameImage(int t)
        {
            string mensaje = "";
            switch (t)
            {
                case 0:
                    mensaje = "FingerLeft";
                    break;
                case 1:
                    mensaje = "FingerRight";
                    break;
                case 2:
                    mensaje = "thumbs";
                    break;
            }
            return mensaje;
        }
        #endregion

        #region Metodo para validar el dispositivo si se encuentra conectado
        private bool ValidateDevice()
        {
            bool res = true;
            RSDeviceInfo deviceInfo = new RSDeviceInfo();
            m_result = RealScanSDK.RS_GetDeviceInfo(deviceHandle, ref deviceInfo);
            if (m_result != RealScanSDK.RS_SUCCESS)
            {
                res = false;
            }
           ;
            return res;
        }

        #endregion

    }
}
