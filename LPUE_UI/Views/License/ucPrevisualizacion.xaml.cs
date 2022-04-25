using FastMember;
using log4net;
using Microsoft.Reporting.WinForms;
using Newtonsoft.Json.Linq;
using PUE.Logger;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace PUE.Views.License
{
    /// <summary>
    /// Lógica de interacción para ucPrevisualizacion.xaml
    /// </summary>
    public partial class ucPrevisualizacion : UserControl
    {
        static string _numeroTramite = "";
        JObject _jObject = null;
        static string cnn = "";
        static int user = 0;
        private static readonly ILog logger = LogManager.GetLogger(typeof(ucPrevisualizacion));

        public ucPrevisualizacion(string id, JObject jDataLicense, string conection, int UsrId)
        {
            string LogFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "log", "Previsualizar");
            if (!Directory.Exists(LogFile))
            {
                Directory.CreateDirectory(LogFile);
            }
            string LogFileName = Path.Combine(LogFile, "LogFile_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".log");
            PUELogger.Setup(LogFileName);

            logger.Info(DateTime.Now.ToString("yyyyMMddmmss") + " ucPrevisualizacion " + "---Extrayendo datos para previsualizar...");
            InitializeComponent();
            _numeroTramite = id;
            _jObject = jDataLicense;
            cnn = conection;
            user = UsrId;

        }

        private System.Drawing.Image convierteAImage(byte[] imgOrac)
        {
            logger.Info(DateTime.Now.ToString("yyyyMMddmmss") + " convierteAImage " + "---");
            using (var ms = new MemoryStream(imgOrac))
            {
                return System.Drawing.Image.FromStream(ms);
            }
        }

        static byte[] ImageToByteArray(System.Drawing.Image image)
        {
            logger.Info(DateTime.Now.ToString("yyyyMMddmmss") + " ImageToByteArray " + "---");
            using (var ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                return ms.ToArray();
            }
        }

        void EncodeWithString(string inputFileName, string outputFileName)
        {
            logger.Info(DateTime.Now.ToString("yyyyMMddmmss") + " EncodeWithString " + "---");
            FileStream inFile;//=new FileStream();
            byte[] binaryData = new byte[1];

            try
            {
                inFile = new System.IO.FileStream(inputFileName, FileMode.Open, FileAccess.Read);
                binaryData = new byte[inFile.Length];
                long bytesRead = inFile.Read(binaryData, 0, int.Parse(inFile.Length.ToString()));
                inFile.Close();
            }
            catch (Exception ex)
            {
                logger.Error(DateTime.Now.ToString("yyyyMMddmmss") + " EncodeWithString " + "---ERROR!!! Descripción " + ex.Message);
            }

            // Convert the binary input into Base64 UUEncoded output.
            string base64String = string.Empty;
            try
            {
                base64String = Convert.ToBase64String(binaryData, 0, binaryData.Length);
            }
            catch (ArgumentException ex) { }


            // Write the UUEncoded version to the output file.
            StreamWriter outFile;//=new StreamWriter();
            try
            {
                outFile = new StreamWriter(outputFileName, false, Encoding.ASCII);
                outFile.Write(base64String);
                outFile.Close();
            }
            catch (Exception ex) { }

        }

        System.Drawing.Image bitmapfrombase64(string strBase64)
        {
            logger.Info(DateTime.Now.ToString("yyyyMMddmmss") + " bitmapfrombase64 " + "---");
            System.Drawing.Image _bmp;
            byte[] imageBytes;
            imageBytes = Convert.FromBase64String(strBase64.Trim());
            MemoryStream strm = new MemoryStream(imageBytes);
            _bmp = Bitmap.FromStream(strm);

            return _bmp;
        }

        string leerArchTexto(string rutaCompletaArch)
        {
            logger.Info(DateTime.Now.ToString("yyyyMMddmmss") + " leerArchTexto " + "---");
            StreamReader objReader = new StreamReader(rutaCompletaArch, Encoding.ASCII);
            string line = objReader.ReadToEnd();

            objReader.Close();
            return line;
        }

        //string ruta = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string ruta = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        static void SaveImageBD(string fields, System.Drawing.Image image)
        {

            logger.Info(DateTime.Now.ToString("yyyyMMddmmss") + " SaveImageBD " + "--- Guardando Foto");
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
                        byte[] data = new byte[fs.Length];
                        fs.Read(data, 0, data.Length);
                        fs.Close();

                        Dictionary<string, object> postParameters = new Dictionary<string, object>();
                        postParameters.Add("pg", "uploadfiledb");
                        postParameters.Add("field", fields);
                        postParameters.Add("usrid", user.ToString());
                        //postParameters.Add("NumeroLicencia", _numeroTramite);
                        postParameters.Add("FolioSeguimiento", _numeroTramite);
                        postParameters.Add("Filedata", new FormUpload.FormUpload.FileParameter(data, fields + ".jpeg", "image/jpeg"));

                        string userAgent = "tlaxcala";
                        HttpWebResponse webResponse = FormUpload.FormUpload.MultipartFormDataPost(cnn, userAgent, postParameters);

                        string status = "";
                        if (webResponse.StatusCode == HttpStatusCode.OK)
                        {
                            status = "OK";
                            logger.Info(DateTime.Now.ToString("yyyyMMddmmss") + " SaveImageBD " + "--- Foto Guardada");
                            string url = cnn + "?pg=InsertarImagenFoto&usuarioId=" + user.ToString() + "&NumeroLic=" + _numeroTramite;
                            var response = new WebClient().DownloadString(url);

                            JObject jObject = JObject.Parse(response);
                            bool success = (bool)jObject.SelectToken("success");

                            if (!success)
                            {
                                logger.Error(DateTime.Now.ToString("yyyyMMddmmss") + " SaveImageBD " + "--- ERROR!!! Descripción: Hubo un problema en el proceso");
                                MessageBoxResult result = MessageBox.Show("Hubo un problema en el proceso", "Aviso Importante");
                                return;

                            }

                        }
                        if (webResponse.StatusCode == HttpStatusCode.Forbidden)
                        {
                            logger.Error(DateTime.Now.ToString("yyyyMMddmmss") + " SaveImageBD " + "--- ERROR!!! Descripción: Hubo un problema en el proceso (Forbidden)");
                            status = "Forbidden";
                            MessageBoxResult result = MessageBox.Show("Hubo un problema en el proceso", "Aviso Importante");
                            return;
                        }
                        StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                        string fullResponse = responseReader.ReadToEnd();
                        webResponse.Close();
                        string sEvent = fullResponse;
                    }


                }


            }

        }


        private void ShowReport()
        {
            Controllers.Resultado res = new Controllers.Resultado();
            string rfcReturn = string.Empty;
            logger.Info(DateTime.Now.ToString("yyyyMMddmmss") + " ShowReport " + "--- Datos que previsualizar...");
            DataPUE.csPrevisual componente = new DataPUE.csPrevisual();

            bool showDomicilio = (bool)_jObject.SelectToken("data[0].ImprimeDir");
            //bool sinRestricciones = (bool)_jObject.SelectToken("data[0].SinRestricciones");
            /*bool lentes = (bool)_jObject.SelectToken("data[0].Lentes");
            bool protesis = (bool)_jObject.SelectToken("data[0].Protesis");
            bool auditivo = (bool)_jObject.SelectToken("data[0].Auditivo");
            bool lentesContacto = (bool)_jObject.SelectToken("data[0].LentesContacto");
            bool vehiculoAdaptado = (bool)_jObject.SelectToken("data[0].VehiculoAdaptado");
            bool vehiculoAutomatico = (bool)_jObject.SelectToken("data[0].VehiculoAutomatico");*/
            componente.NOMBRE = (string)_jObject.SelectToken("data[0].Nombre");
            componente.APATERNO = (string)_jObject.SelectToken("data[0].ApellidoPaterno");
            componente.AMATERNO = (string)_jObject.SelectToken("data[0].ApellidoMaterno");
            componente.APELLIDO = (string)_jObject.SelectToken("data[0].ApellidoPaterno") + " " + (string)_jObject.SelectToken("data[0].ApellidoMaterno");
            if (showDomicilio)
            {
                string dir = (string)_jObject.SelectToken("data[0].Calle") + " " +
                    (string)_jObject.SelectToken("data[0].NoExterior") + " " +
                    (string)_jObject.SelectToken("data[0].NoInterior") + " " +
                    (string)_jObject.SelectToken("data[0].ColoniaNombre") + ", " +
                    (string)_jObject.SelectToken("data[0].MunicipioNombre") + ", " +
                    (string)_jObject.SelectToken("data[0].EstadoNombre");
                componente.DIRECCION = dir;
            }
            else
            {
                componente.DIRECCION = "";
            }
            componente.Show_Domicilio = showDomicilio;
            /*if (!sinRestricciones)
            {
                if (lentes)
                {
                    componente.LENTES = "USA LENTES";
                }

                if (protesis)
                {
                    componente.PROTESIS = "USA PRÓTESIS";
                }

                if (auditivo)
                {
                    componente.AUDITIVO = "USA APARATO AUDITIVO";
                }

                if (lentesContacto)
                {
                    componente.LENTES_CONTACTO = "USA LENTES DE CONTACTO";
                }

                if (vehiculoAdaptado)
                {
                    componente.VEHICULO_ADAPTADO = "USA VEHÍCULO ADAPTADO";
                }

                if (vehiculoAutomatico)
                {
                    componente.VEHICULO_AUTOMATICO = "USA VEHÍCULO AUTOMÁTICO";
                }
            }
            else
            {
                componente.LENTES = "SIN RESTRICCIONES";
                componente.PROTESIS = "";
                componente.AUDITIVO = "";
                componente.LENTES_CONTACTO = "";
                componente.VEHICULO_ADAPTADO = "";
                componente.VEHICULO_AUTOMATICO = "";
            }*/

            componente.CURP = (string)_jObject.SelectToken("data[0].Curp");
            componente.RFC = (string)_jObject.SelectToken("data[0].RFC");
            componente.NACIONALIDAD = (string)_jObject.SelectToken("data[0].Nacionalidad");
            componente.numeroLicencia = (string)_jObject.SelectToken("data[0].NumeroLicencia");
            string FechaVencimiento = (string)_jObject.SelectToken("data[0].FechaVencimiento");
            DateTime fech;
            if (FechaVencimiento != null || !String.IsNullOrEmpty(FechaVencimiento))
            {
                fech = (DateTime)_jObject.SelectToken("data[0].FechaVencimiento");
                FechaVencimiento = fech.ToString().Substring(0, 10);
                componente.FECHA_VENCIMIENTO = FechaVencimiento;
                componente.seguro = "(INCLUYE SEGURO AP)";
            }
            else
            {
                componente.FECHA_VENCIMIENTO = "PERMANENTE";
                componente.seguro = "";
            }

            DateTime fechexp = (DateTime)_jObject.SelectToken("data[0].FechaExpedicion");

            DateTime fechanac = (DateTime)_jObject.SelectToken("data[0].FechaNacimiento");

            DateTime fechaAnt = (DateTime)_jObject.SelectToken("data[0].FechaAntiguedad");

            componente.FECHA_ANTIGUEDAD = fechaAnt.ToString().Substring(0, 10);
            componente.LicenciaLetra = (string)_jObject.SelectToken("data[0].TipoLicencia");
            componente.FECHA_NACIMIENTO = fechanac.ToString().Substring(0, 10);
            componente.FECHA_EXPEDICION = fechexp.ToString().Substring(0, 10);
            componente.ANIOS_VIGENCIA = (string)_jObject.SelectToken("data[0].AniosVigencia");
            componente.ExpedienteNum = (string)_jObject.SelectToken("data[0].FolioExpediente");
            componente.nombreSecre = "Nombre Secreto";
            componente.TIPO_LICENCIA = (string)_jObject.SelectToken("data[0].NombreLicencia");
            componente.SEXO = "Masculino";
            componente.CABELLO = "Castaño";
            componente.ESTATURA = "1.64";
            componente.TELEFONO = (string)_jObject.SelectToken("data[0].Telefono");
            componente.TIPO_SANGRE = (string)_jObject.SelectToken("data[0].TipoSangre");
            componente.ALERGIAS = (string)_jObject.SelectToken("data[0].Alergias");
            componente.SENAS_PARTICULARES = "Lunar cachete derecho";
            string dona = "Si";

            bool donador = (bool)_jObject.SelectToken("data[0].DonacionOrganos");

            if (!donador)
            {
                dona = "No";
            };

            componente.DONADOR_ORGANOS = dona;
            componente.IDfOLIO = "0001R";



            if (componente != null)
            {

                if (componente.NOMBRE != "0")
                {
                    List<DataPUE.csPrevisual> lstCom = new List<DataPUE.csPrevisual>();
                    string fullPhotoPath = ruta + "\\fotoRecort.jpeg";
                    string fullSignPath = ruta + "\\firma.bmp";
                    string fullSignTxtPath = ruta + "\\firma.txt";

                    if (File.Exists(fullPhotoPath))
                    {
                        try
                        {
                            componente.FOTO = ImageToByteArray(System.Drawing.Image.FromFile(fullPhotoPath));
                        }
                        catch (Exception ex)
                        {
                            logger.Error(DateTime.Now.ToString("yyyyMMddmmss") + " ShowReport " + "---ERROR!!! Descripcion " + ex.Message);
                            MessageBoxResult result = MessageBox.Show("No se pudo guardar la información");
                            return;
                        }

                        if (File.Exists(fullSignPath))
                        {
                            componente.FIRMA = ImageToByteArray(Bitmap.FromFile(fullSignPath));
                            EncodeWithString(fullSignPath, fullSignTxtPath);
                        }
                        else
                            componente.FIRMA = ImageToByteArray(bitmapfrombase64(leerArchTexto(fullSignTxtPath)));

                        if (componente.FIRMA != null)
                        {
                            File.WriteAllBytes(ruta + "\\firma.jpg", componente.FIRMA);

                            lstCom.Add(componente);
                            string rutaFoto = "file:///" + ruta + "\\fotoRecort.jpeg";
                            string rutaFirma = "file:///" + ruta + "\\FirmaSecreta.png";
                            string rutaHuella = "file:///" + ruta + "\\thumbs_6.bmp";
                            if (File.Exists(System.IO.Path.Combine(ruta, "thumbs_6.jpg")))
                            {
                                rutaHuella = "file:///" + ruta + "\\thumbs_6.jpg";
                            }
                            string rutaFirmaUsuario = "file:///" + ruta + "\\firma.bmp";
                            string rutaQR = "file:///" + ruta + "\\QR" + "\\" + componente.numeroLicencia + ".png";
                            string rutaQRI = "file:///" + ruta + "\\QR" + "\\" + componente.numeroLicencia + "QRI" + ".png";
                            System.Collections.Specialized.StringCollection mycollect = new System.Collections.Specialized.StringCollection();
                            ReportParameter[] paramers = new ReportParameter[6];
                            paramers[0] = new ReportParameter("ImageFoto", rutaFoto, true);
                            paramers[1] = new ReportParameter("ImageFirma", rutaFirma, true);
                            paramers[2] = new ReportParameter("ImageHuella", rutaHuella, true);
                            paramers[3] = new ReportParameter("ImageFirmaUsuario", rutaFirmaUsuario, true);
                            paramers[4] = new ReportParameter("ImageQR", rutaQR, true);
                            paramers[5] = new ReportParameter("ImageQRI", rutaQRI, true);
                            ReportDataSource reportDataSource = new ReportDataSource();
                            reportDataSource.Name = "dslic";
                            DataTable table = new DataTable();
                            using (var reader = ObjectReader.Create(lstCom))
                            {
                                table.Load(reader);
                            }
                            reportDataSource.Value = table;
                            //   reportViewer1.LocalReport.DataSources.Clear();
                            _reportViewer.LocalReport.DataSources.Clear();
                            _reportViewer.LocalReport.EnableExternalImages = true;
                            _reportViewer.LocalReport.ReportPath = @"Content\Formatos\rptPrevisual.rdlc";
                            _reportViewer.LocalReport.SetParameters(paramers);
                            _reportViewer.LocalReport.DataSources.Add(reportDataSource);
                            this._reportViewer.RefreshReport();
                            _reportViewer.SetDisplayMode(DisplayMode.PrintLayout);
                            _reportViewer.ZoomMode = ZoomMode.Percent;
                            _reportViewer.ZoomPercent = 100;
                        }
                        else
                        {
                            logger.Error(DateTime.Now.ToString("yyyyMMddmmss") + " ShowReport " + "---ERROR!!! Descripcion: No se pudo encontrar la imagen de la Firma ");
                            MessageBox.Show("No se pudo encontrar la imagen de la Firma");
                            var vari = Window.GetWindow(this);
                            vari.Close();
                        }
                    }
                    else
                    {
                        logger.Error(DateTime.Now.ToString("yyyyMMddmmss") + " ShowReport " + "---ERROR!!! Descripcion: No se pudo encontrar la imagen de la fotografia ");
                        MessageBox.Show("No se pudo encontrar la imagen de la fotografia");
                        var vari = Window.GetWindow(this);
                        vari.Close();
                    }

                }
            }
        }

        PUE.Views.Shared.LoadingPanel ucLoadingPanel;
        public System.Drawing.Bitmap ConvertToBitmap(string fileName)
        {
            logger.Info(DateTime.Now.ToString("yyyyMMddmmss") + " ConvertToBitmap " + "---");
            System.Drawing.Bitmap bitmap;
            using (Stream bmpStream = System.IO.File.Open(fileName, System.IO.FileMode.Open))
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(bmpStream);

                bitmap = new System.Drawing.Bitmap(image);

            }
            return bitmap;
        }

        Controllers.Rfc oDataCaptureInfo = new Controllers.Rfc();
        void DispatcherRequestFSBegin(PUE.Controllers.Resultado oResult, String tituloMsjOk, String detalleMsjOK, String tituloMsjERROR, bool esBusquedaParaModificacion = false, bool esCambioDeModulo = false)
        {
            logger.Info(DateTime.Now.ToString("yyyyMMddmmss") + " DispatcherRequestFSBegin " + "---");
            Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(delegate ()
            {
                if (oResult._Estatus == Controllers.Resultado.Estatus.ERROR)
                {
                    this.DataContext = new Controllers.CaptureInforLic();
                    BeforeSend(tituloMsjERROR, oResult._Detalles.ToString(), true, true);
                }
                else
                {
                    if (esBusquedaParaModificacion)
                    {
                        oDataCaptureInfo = new Controllers.Rfc();
                        oDataCaptureInfo = oResult._Detalles;

                        this.DataContext = new Controllers.CaptureInforLic();
                        this.DataContext = oDataCaptureInfo;
                    }

                    BeforeSend(tituloMsjOk, detalleMsjOK, false, true);
                }
            }));
        }

        void onCLickCloseBeforeSend(object sender, EventArgs e)
        {
            logger.Info(DateTime.Now.ToString("yyyyMMddmmss") + " onCLickCloseBeforeSend " + "---");
            pnlBottomMenuDetalles.Children.Clear();
        }

        void BeforeSend(String Message, String SubMessage, bool EsError, bool EsProcesoFinalizado)
        {
            logger.Info(DateTime.Now.ToString("yyyyMMddmmss") + " BeforeSend " + "---");
            pnlBottomMenuDetalles.Children.Clear();
            ucLoadingPanel = new Shared.LoadingPanel();
            ucLoadingPanel.onClickClose += new Shared.LoadingPanel.EventHandlerLoading(onCLickCloseBeforeSend);
            ucLoadingPanel.Mensaje = Message;
            ucLoadingPanel.SubMensaje = SubMessage;
            ucLoadingPanel.EsError = EsError;
            ucLoadingPanel.ProgressBar = EsProcesoFinalizado ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
            pnlBottomMenuDetalles.Children.Add(ucLoadingPanel);
        }

        private void seacabodecargar(object sender, RoutedEventArgs e)
        {
            logger.Info(DateTime.Now.ToString("yyyyMMddmmss") + " seacabodecargar " + "---");
            ShowReport();
        }


    }



}
