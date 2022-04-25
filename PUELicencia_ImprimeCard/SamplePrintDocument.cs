using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Printing;
using System.Xml;
using dxp01sdk;
using System.Text;
using log4net;
using PUE.Logger;
using ThoughtWorks.QRCode;
using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;

namespace PUELicencia_ImprimeCard
{
    class SamplePrintDocument : PrintDocument
    {
        private CommandLineOptions _commandLineOptions;
        private bool _frontOfCard = true;
        private int _sizeFactor = 3;
        private static readonly ILog loggerSample = LogManager.GetLogger(typeof(SamplePrintDocument));


        public SamplePrintDocument(CommandLineOptions commandLineOptions)
        {

            string LogFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "log", "Impresora");
            if (!Directory.Exists(LogFile))
            {
                Directory.CreateDirectory(LogFile);
            }
            string LogFileName = Path.Combine(LogFile, "LogFile_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".log");
            PUELogger.Setup(LogFileName);

            loggerSample.Info(DateTime.Now.ToString("yyyyMMddmmss") + " SamplePrintDocument " + "---Extrayendo datos para imprimir...");
            _commandLineOptions = commandLineOptions;
            PrinterSettings.PrinterName = commandLineOptions.printerName;
            DocumentName = "XPS Driver SDK c# print sample";
        }



        public void OnBeginPrint(object sender, PrintEventArgs printEventArgs)
        {

            loggerSample.Info(DateTime.Now.ToString("yyyyMMddmmss") + " OnBeginPrint " + "---Config impresora");

            // prepare the PrintTicket for the entire print job.
            PrintQueue printQueue = new PrintQueue(new LocalPrintServer(), PrinterSettings.PrinterName);
            PrintTicket deltaPrintTicket = new PrintTicket();
            deltaPrintTicket.Duplexing =
               _commandLineOptions.twoPages ? Duplexing.TwoSidedLongEdge : Duplexing.OneSided;
            deltaPrintTicket.CopyCount = _commandLineOptions.numCopies;
            deltaPrintTicket.PageOrientation =
               _commandLineOptions.portraitFront ? PageOrientation.Portrait : PageOrientation.Landscape;

            ValidationResult validationResult = printQueue.MergeAndValidatePrintTicket(
               printQueue.UserPrintTicket,
               deltaPrintTicket);

            string xmlString = PrintTicketXml.Prefix;

            xmlString += _commandLineOptions.rotateFront ?
               PrintTicketXml.FlipFrontFlipped : PrintTicketXml.FlipFrontNone;

            switch (_commandLineOptions.disablePrinting)
            {
                case CommandLineOptions.DisablePrinting.All:
                    xmlString += PrintTicketXml.DisablePrintingAll;
                    break;
                case CommandLineOptions.DisablePrinting.Off:
                    xmlString += PrintTicketXml.DisablePrintingOff;
                    break;
                case CommandLineOptions.DisablePrinting.Front:
                    xmlString += PrintTicketXml.DisablePrintingFront;
                    break;
                case CommandLineOptions.DisablePrinting.Back:
                    xmlString += PrintTicketXml.DisablePrintingBack;
                    break;
            }

            if (_commandLineOptions.twoPages)
            {
                xmlString += _commandLineOptions.rotateBack ?
                   PrintTicketXml.FlipBackFlipped : PrintTicketXml.FlipBackNone;
            }

            xmlString += GetTopcoatBlockingPrintTicketXml();
            xmlString += PrintTicketXml.Suffix;

            // prepare to merge our PrintTicket xml into an actual PrintTicket:
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlString);
            MemoryStream memoryStream = new MemoryStream();
            xmlDocument.Save(memoryStream);
            memoryStream.Position = 0;
            deltaPrintTicket = new PrintTicket(memoryStream);

            validationResult = printQueue.MergeAndValidatePrintTicket(
               validationResult.ValidatedPrintTicket,
               deltaPrintTicket);

            printQueue.UserPrintTicket = validationResult.ValidatedPrintTicket;

            if (_commandLineOptions.showXml)
            {
                Util.DisplayPrintTicket(validationResult.ValidatedPrintTicket);
            }

            // IMPORTANT: this Commit() call sets the driver's 'Printing Preferences'
            // on this machine:
            printQueue.Commit();
        }

        string base64Data = string.Empty;
        int CardWidth = 204;
        int CardHeight = 322;

        public void EncodeWithString(string inputFileName, string outputFileName)
        {
            loggerSample.Info(DateTime.Now.ToString("yyyyMMddmmss") + " EncodeWithString " + "---Config impresora");
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

        private Image bitmapfrombase64(string strBase64)
        {
            loggerSample.Info(DateTime.Now.ToString("yyyyMMddmmss") + " bitmapfrombase64 " + "---BitImagen a base64");
            Image _bmp;
            byte[] imageBytes;
            imageBytes = Convert.FromBase64String(strBase64.Trim());
            MemoryStream strm = new MemoryStream(imageBytes);
            _bmp = Bitmap.FromStream(strm);

            return _bmp;
        }
        private string leerArchTexto(string rutaCompletaArch)
        {
            loggerSample.Info(DateTime.Now.ToString("yyyyMMddmmss") + " leerArchTexto " + "---Leyendo ruta...");
            StreamReader objReader = new StreamReader(rutaCompletaArch, Encoding.ASCII);
            string line = objReader.ReadToEnd();

            objReader.Close();
            return line;
        }

        private Bitmap loadImg(string urlImg)
        {
            loggerSample.Info(DateTime.Now.ToString("yyyyMMddmmss") + " loadImg " + "---Cargando imágen");
            int extLen = urlImg.Length;
            string ext = urlImg.Substring(extLen - 3, 3).ToUpper();
            string[] filters = { "BMP", "JPG", "GIF", "PNG", "BMP", "JPG", "GIF", "PNG" };
            bool isPic = false;
            foreach (string filter in filters)
            {
                if (ext == filter)
                    isPic = true;

            }
            if (isPic == false)
                return null;

            // ' load bitmap into this:
            Image loadedBM;
            loadedBM = System.Drawing.Bitmap.FromFile(urlImg);
            //'initialise sourceBM at the correct size and force it to be 32bppARGB
            Bitmap sourceBM = new Bitmap(loadedBM.Width, loadedBM.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            //'get a graphics surface on the bitmap:
            Graphics g = Graphics.FromImage(sourceBM);
            // 'draw the image onto it
            g.DrawImage(loadedBM, new Rectangle(0, 0, loadedBM.Width, loadedBM.Height));

            return sourceBM;
        }

        private Image insertWaterMarkToImg(Image urlImg)
        {
            loggerSample.Info(DateTime.Now.ToString("yyyyMMddmmss") + " insertWaterMarkToImg " + "---Cargando imágen");
            Image sourceBM = urlImg;
            if (sourceBM == null)
            {
                //MessageBox.Show("Isn't load the picture"
                return null;
            }


            Graphics g = Graphics.FromImage(sourceBM);
            Rectangle rect = new Rectangle(0, 0, sourceBM.Width, sourceBM.Height);
            g.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.White)), rect);
            return sourceBM;
        }



        private Bitmap delFondoFoto(Bitmap _bmpFoto, bool mascara)
        {
            loggerSample.Info(DateTime.Now.ToString("yyyyMMddmmss") + " delFondoFoto " + "---Cargando imágen");
            Color colort;
            Color colorp;
            int fila;


            Bitmap _bmpClon = (Bitmap)_bmpFoto.Clone();
            Rectangle rect = new Rectangle(0, 0, _bmpFoto.Width, _bmpFoto.Height);
            Graphics g = Graphics.FromImage(_bmpFoto);

            if (mascara)
            {
                for (int i = 0; i < 30; i++)
                {
                    g.FillRectangle(new SolidBrush(Color.FromArgb(2, Color.Cyan)), rect);
                    g.FillRectangle(new SolidBrush(Color.FromArgb(2, Color.Magenta)), rect);
                    g.FillRectangle(new SolidBrush(Color.FromArgb(1, Color.Yellow)), rect);
                }
            }

            colort = _bmpFoto.GetPixel(5, 5);
            colorp = _bmpFoto.GetPixel(5, 5);
            fila = 0;
            int colorTolera = 20;
            if (mascara) colorTolera = 5;
            for (int j = 0; j < _bmpFoto.Height; j++)
            {
                for (int l = 0; l < _bmpFoto.Width; l++)
                {
                    colorp = _bmpFoto.GetPixel(l, j);
                    if (colorp.R <= colort.R + colorTolera && colorp.R >= colort.R - colorTolera)
                    {

                        if (colorp.G <= colort.G + colorTolera && colorp.G >= colort.G - colorTolera)
                        {

                            if (colorp.B <= colort.B + colorTolera && colorp.B >= colort.B - colorTolera)

                                _bmpFoto.SetPixel(l, j, Color.White);

                            else
                                if (fila == 0) fila = j;
                        }
                    }
                }
            }

            for (int k = 0; k < _bmpFoto.Height; k++)
            {
                for (int t = _bmpFoto.Width; t == 0; t--)
                {
                    colorp = _bmpFoto.GetPixel(t, k);
                    if (colorp.R <= colort.R + colorTolera && colorp.R >= colort.R - colorTolera)
                    {

                        if (colorp.G <= colort.G + colorTolera && colorp.G >= colort.G - colorTolera)
                        {

                            if (colorp.B <= colort.B + colorTolera && colorp.B >= colort.B - colorTolera)
                                _bmpFoto.SetPixel(t, k, Color.White);
                            else
                                if (fila == 0) fila = k;
                            t = _bmpFoto.Width;

                        }
                        else
                            t = _bmpFoto.Width;
                    }
                    else
                        t = _bmpFoto.Width;
                }
            }


            for (int h = 0; h < _bmpFoto.Height; h++)
            {
                for (int u = 0; u < _bmpFoto.Width; u++)
                {
                    colorp = _bmpFoto.GetPixel(u, h);
                    if (colorp.R == Color.White.R && colorp.G == Color.White.G && colorp.B == Color.White.B)

                        _bmpClon.SetPixel(u, h, Color.White);

                    else
                        u = _bmpFoto.Width;
                }
            }

            for (int m = 0; m < _bmpFoto.Height; m++)
            {
                for (int e = _bmpFoto.Width; e == 0; e--)
                {
                    colorp = _bmpFoto.GetPixel(e, m);
                    if (colorp.R == Color.White.R && colorp.G == Color.White.G && colorp.B == Color.White.B)
                        _bmpClon.SetPixel(e, m, Color.White);

                    else
                        e = 0;

                }
            }

            return _bmpClon;
        }

        private Font font45;
        private Font font45Telefono;

        private Font font65Bold;

        private Font font65BoldTelefono;

        private string[] infoLic = new string[22];
        Single FotoWidth;
        Image Foto;
        Image Firma;
        Image FirmaSecretario;
        Image huella;
        Single FotoHeight;
        Image firmaSec;
        string[] strCadena;
        Font font6Bold;
        SizeF mstr;
        Font font5Bold;
        Font font85Bold;
        Font font85BoldName;
        string licNumber;

        public static string jsonNodes;
        public static csPrevisual component = new csPrevisual();

        public static Font ninefontBold = new Font("Helvetica", 9F, FontStyle.Bold);
        public static Font eightfontBold = new Font("Helvetica", 8F, FontStyle.Bold);
        public static Font sevenfontBold = new Font("Helvetica", 7F, FontStyle.Bold);
        public static Font sixfontBold = new Font("Helvetica", 6F, FontStyle.Bold);
        public static Font fivefontBold = new Font("Helvetica", 5F, FontStyle.Bold);
        public static Font fourfontBold = new Font("Helvetica", 4F, FontStyle.Bold);
        public static Font threefontBold = new Font("Helvetica", 3F, FontStyle.Bold);
        public static Font twofontBold = new Font("Helvetica", 2F, FontStyle.Bold);
        public static Font onefontBold = new Font("Helvetica", 1F, FontStyle.Bold);

        public static Font ninefont = new Font("Helvetica", 9F, FontStyle.Regular);
        public static Font eightfont = new Font("Helvetica", 8F, FontStyle.Regular);
        public static Font sevenfont = new Font("Helvetica", 7F, FontStyle.Regular);
        public static Font sixfont = new Font("Helvetica", 6F, FontStyle.Regular);
        public static Font fivefont = new Font("Helvetica", 5F, FontStyle.Regular);

        public static string workPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        public static Brush uvBrush = new SolidBrush(Color.FromArgb(255, 217, 217, 217));

        //blic static Brush licenseTxtBrush = new SolidBrush(Color.FromArgb(255, 217, 217, 217));

        //public static Color uvBrush = Color.FromArgb(217, 217, 217);

        //NOTA: CAMBIOS A PARÁMETROS PARA OBJETOS E IMÁGENES PARA PRUEBA DE IMPRESIÓN
        public void DrawCardFront(PrintPageEventArgs pageEventArgs)
        {
            loggerSample.Info(DateTime.Now.ToString("yyyyMMddmmss") + " DrawCardFront " + "---Datos de frente para imprimir");
            pageEventArgs.Graphics.TranslateTransform(340.0F, 210.5F);
            pageEventArgs.Graphics.RotateTransform(180);
            component = Json.Deserialise<csPrevisual>(jsonNodes);

            //= new Single();
            //=new Single();
            FotoWidth = 80.0F; //'px/*2.3cm; original : 89.8F;
            FotoHeight = 90.0F;// 'px/*2.7cm original : 106.0F;  
            //Image Foto=convierteAImage(component.FOTO);

            if (File.Exists(workPath + "\\fotoRecort.jpeg"))
            {
                Foto = Bitmap.FromFile(workPath + "\\fotoRecort.jpeg");

                this.EncodeWithString(workPath + "\\fotoRecort.jpeg", workPath + "\\fotoRecort.jpeg");

            }
            else
                Foto = bitmapfrombase64(leerArchTexto(workPath + "\\foto.txt"));

            Image foto2 = Foto;

            if (File.Exists(workPath + "\\firma.bmp"))
            {
                Firma = Bitmap.FromFile(workPath + "\\firma.bmp");
                this.EncodeWithString(workPath + "\\firma.bmp", workPath + "\\firma.txt");
            }
            else
                Firma = bitmapfrombase64(leerArchTexto(workPath + "\\firma.txt"));

            if (File.Exists(workPath + "\\FirmaSecreta.png"))
            {
                FirmaSecretario = Bitmap.FromFile(workPath + "\\FirmaSecreta.png");
                this.EncodeWithString(workPath + "\\FirmaSecreta.bmp", workPath + "\\FirmaSecreta.txt");
            }
            else
                FirmaSecretario = bitmapfrombase64(leerArchTexto(workPath + "\\FirmaSecreta.txt"));

            byte[] imageBytes;
            imageBytes = Convert.FromBase64String(base64Data);
            MemoryStream str = new MemoryStream(imageBytes);

            Pen blackPen = new Pen(Color.Black, 3);
            //'Create a rectangle
            Rectangle R = new Rectangle(0, 0, CardHeight, CardWidth);

            //'/* Set fonts to Arial  */

            font45 = new System.Drawing.Font("Myriad", 6F, FontStyle.Bold);
            Font font5 = new System.Drawing.Font("Helvetica", 5, FontStyle.Regular);
            font5Bold = new System.Drawing.Font("Helvetica", 5, FontStyle.Bold);
            Font font6 = new System.Drawing.Font("Helvetica", 6);
            font6Bold = new System.Drawing.Font("Helvetica", 6, FontStyle.Bold);
            font65Bold = new System.Drawing.Font("Myriad", 7.5F, FontStyle.Bold);

            font65BoldTelefono = new System.Drawing.Font("Myriad", 6F, FontStyle.Bold);
            font45Telefono = new System.Drawing.Font("Myriad", 4.5F, FontStyle.Bold);

            Font font65BoldBold = new System.Drawing.Font("Helvetica", 7, FontStyle.Bold);
            Font font7 = new System.Drawing.Font("Helvetica", 7);
            Font font7Bold = new System.Drawing.Font("Helvetica", 7, FontStyle.Bold);
            Font font75Bold = new System.Drawing.Font("Helvetica", 7, FontStyle.Bold);
            Font font8 = new System.Drawing.Font("Helvetica", 8);
            Font font8Bold = new System.Drawing.Font("Helvetica", 8, FontStyle.Bold);
            font85Bold = new System.Drawing.Font("Myriad", 5F, FontStyle.Bold);
            font85BoldName = new System.Drawing.Font("Myriad", 5.5F, FontStyle.Bold);
            Font font100Bold = new System.Drawing.Font("Helvetica", 10, FontStyle.Bold);

            //'Is necessary to put the logic of each page on a Select Case, because it needs to control the number of pages contained in our print job

            pageEventArgs.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            pageEventArgs.Graphics.TextContrast = 1;
            pageEventArgs.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            Single xFront = 94.0F;

            Font numLic = new Font("Helvetica", 10F, FontStyle.Bold);
            Font TipoLicLetra = new Font("Helvetica", 40F, FontStyle.Bold);

            Font Expediente = new Font("Helvetica", 10F, FontStyle.Bold);
            Font SubtitleFont = new Font("Helvetica", 7.5F, FontStyle.Bold);
            Font JobFont = new Font("Helvetica Extended", 6F, FontStyle.Bold);
            Font JobFonLarget = new Font("Helvetica Extended", 5F, FontStyle.Bold);

            //'e.Graphics.DrawRectangle(blackPen, R)

            //'/*Photo
            //Single fotoX = 21.0F;
            Single fotoX = 9F;
            //Single fotoY = 59.0F;
            Single fotoY = 70.0F;
            Single fotoYmod = fotoY - 10.0F;

            pageEventArgs.Graphics.DrawImage(Foto, fotoX, fotoY - 10.0F, FotoWidth, FotoHeight);

            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            RectangleF rectModaliad1 = new RectangleF(xFront + 52F, fotoYmod + 72.0F, 140.0F, 45.0F);
            RectangleF rectModaliad2 = new RectangleF(xFront - 128.0F, fotoYmod + 80.0F, 155.0F, 45.0F);
            if (component.TIPO_LICENCIA == "AUTOMIVILISTA")
            {
                //pageEventArgs.Graphics.DrawString("A", TipoLicLetra, Brushes.Green, xFront + 145, fotoYmod + 7.0F);                
            }
            else if (component.TIPO_LICENCIA == "MOTOCICLISTA")
            {
                //pageEventArgs.Graphics.DrawString("M", TipoLicLetra, Brushes.Green, xFront + 145, fotoYmod + 7.0F);
            }
            else if (component.TIPO_LICENCIA == "PROVISIONAL AUTOMOVILISTA")
            {
                //pageEventArgs.Graphics.DrawString("PROVISIONAL\r\nAUTOMOVILISTA", eightfontBold, Brushes.Black, rectModaliad2, sf);
            }
            else if (component.TIPO_LICENCIA == "PROVISIONAL MOTOCICLISTA")
            {
                //pageEventArgs.Graphics.DrawString("PROVISIONAL\r\nMOTOCICLISTA", eightfontBold, Brushes.Black, rectModaliad2, sf);
            }
            else if (component.TIPO_LICENCIA == "CHOFER PARTICULAR")
            {
                //pageEventArgs.Graphics.DrawString("CHOFER\r\nPARTICULAR", eightfontBold, Brushes.Black, rectModaliad1, sf);
            }
            else if (component.TIPO_LICENCIA == "TRANSPORTE MERCANTIL")
            {
                //pageEventArgs.Graphics.DrawString("TRANSPORTE\r\nMERCANTIL", eightfontBold, Brushes.Black, rectModaliad1, sf);
            }
            else
            {
                pageEventArgs.Graphics.DrawString(component.TIPO_LICENCIA, sixfontBold, Brushes.Black, rectModaliad2, sf);
            }

            //'NOMBRE / Name
            //'Valor
            Font namefont = sevenfontBold;
            int nombreLen = component.NOMBRE.Length + component.APELLIDO.Length;

            if (nombreLen >= 22)
            {
                namefont = sixfontBold;
            }

            if (nombreLen >= 27)
            {
                namefont = fivefontBold;
            }

            if (nombreLen >= 40)
            {
                namefont = fourfontBold;
            }

            if (nombreLen > 60)
            {
                namefont = threefontBold;
            }

            if (nombreLen > 80)
            {
                namefont = twofontBold;
            }

            if (nombreLen > 95)
            {
                namefont = onefontBold;
            }

            RectangleF rectNombre = new RectangleF(xFront, fotoYmod - 1F, 128.0F, 25.0F);
            pageEventArgs.Graphics.DrawString(component.NOMBRE.ToUpper() + " " + component.APELLIDO.ToUpper(), eightfont, Brushes.Black, rectNombre);

            //'LETRA
            //'Etiqueta
            pageEventArgs.Graphics.DrawString("TIPO", sevenfontBold, Brushes.Black, xFront + 160, fotoYmod - 1F);
            //'Valor
            pageEventArgs.Graphics.DrawString(component.LicenciaLetra, TipoLicLetra, Brushes.Green, xFront + 145, fotoYmod + 7.0F);

            //'RFC / Population Registry
            //'Etiqueta
            pageEventArgs.Graphics.DrawString("CURP", sixfontBold, Brushes.Black, xFront, fotoYmod + 24F);
            //'Valor
            pageEventArgs.Graphics.DrawString(component.CURP, sevenfontBold, Brushes.Black, xFront, fotoYmod + 32F);

            float domicilioY = 65F;
            Font domiciliofont = fivefontBold;
            int domicilioLen = (component.calle + " " + component.numero + " " + component.colonia + ", " + component.municipio + ", " + component.estado).Length;

            if (domicilioLen > 45)
            {
                domiciliofont = fourfontBold;
            }

            if (component.Show_Domicilio)
            {
                //'DOMICILIO / ADDRESS
                //'Etiqueta
                //pageEventArgs.Graphics.DrawString("DOMICILIO", fivefontBold, Brushes.Black, xFront, fotoYmod + 40.0F);
                //'Valor
                //RectangleF rectDomicilio = new RectangleF(xFront, fotoYmod + 47.0F, 170, 20);
                //pageEventArgs.Graphics.DrawString(component.calle.ToUpper() + " " + component.numero.ToUpper() + " " + component.interior.ToUpper() + "\r\n" + component.colonia.ToUpper() +
                //    ", " + component.municipio.ToUpper() + ", " + component.estado.ToUpper(), domiciliofont, Brushes.Black, rectDomicilio);
            }
            else
            {
                domicilioY = 40.0F;
            }

            //'Etiqueta
            pageEventArgs.Graphics.DrawString("NACIONALIDAD", sixfontBold, Brushes.Black, xFront, fotoYmod + 42.0F);
            //'Valor
            pageEventArgs.Graphics.DrawString(component.NACIONALIDAD, sevenfontBold, Brushes.Black, xFront, fotoYmod + 50.0F);

            //'EXPEDICION / Issued 
            //'Etiqueta
            pageEventArgs.Graphics.DrawString("EXPEDIDA", sixfontBold, Brushes.Black, xFront, fotoYmod + 60.0F);
            // 'Valor
            pageEventArgs.Graphics.DrawString(component.FECHA_EXPEDICION, sevenfontBold, Brushes.Black, xFront, fotoYmod + 68.0F);

            //'VENCIMIENTO / Expire
            //'Etiqueta
            pageEventArgs.Graphics.DrawString("VENCIMIENTO", sixfontBold, Brushes.Black, xFront + 75.0F, fotoYmod + 60.0F);
            //'Valor
            string fechaVencimiento = "";
            if (!String.IsNullOrEmpty(component.FECHA_VENCIMIENTO) && component.FECHA_VENCIMIENTO != "PERMANENTE")
            {
                fechaVencimiento = component.FECHA_VENCIMIENTO;
                //pageEventArgs.Graphics.DrawString("(INCLUYE SEGURO AP)", fivefontBold, Brushes.Black, xFront + 146F, fotoYmod + 20.0F);
            }
            else
            {
                fechaVencimiento = "PERMANENTE";
            }
            pageEventArgs.Graphics.DrawString(fechaVencimiento, sevenfontBold, Brushes.Black, xFront + 75.0F, fotoYmod + 68.0F);

            // 'VIGENCIA 
            //'Etiqueta
            pageEventArgs.Graphics.DrawString("VIGENCIA", sixfontBold, Brushes.Black, xFront, fotoYmod + 78.0F);
            // 'Valor
            pageEventArgs.Graphics.DrawString(component.ANIOS_VIGENCIA + " " + "AÑOS", sevenfontBold, Brushes.Black, xFront, fotoYmod + 86.0F);

            //'NUMERO DE LICENCIA
            //'Etiqueta
            //pageEventArgs.Graphics.DrawString("NUMERO DE LICENCIA", fivefontBold, Brushes.Black, xFront, fotoYmod + domicilioY + 36.0F);
            pageEventArgs.Graphics.DrawString(component.numeroLicencia, ninefontBold, Brushes.Black, xFront + 150.0F, fotoYmod + domicilioY + 58.0F);

            //'TIPO DE LICENCIA
            //'Etiqueta
            //pageEventArgs.Graphics.DrawString("TIPO DE LICENCIA", fivefontBold, Brushes.Black, xFront + 100F, fotoYmod + domicilioY + 18F);

            //UV
            pageEventArgs.Graphics.DrawString(component.NOMBRE + " " + component.APELLIDO, namefont, uvBrush, xFront, fotoYmod + domicilioY + 65.0F);
            pageEventArgs.Graphics.DrawString(component.numeroLicencia, sevenfontBold, uvBrush, xFront, fotoYmod + domicilioY + 75.0F);

            //Single firmafactor;

            //firmafactor = float.Parse(((FotoWidth * 0.5 * 2.5) / (Firma.Width)).ToString());
            //if (firmafactor * Firma.Height > FotoHeight * 0.5F)
            //    firmafactor = (FotoHeight * 0.5F) / Firma.Height;

            //pageEventArgs.Graphics.DrawImage(Firma, fotoX + 25.0F, fotoY + 85.0F, Firma.Width * firmafactor * 1.2F, Firma.Height * firmafactor * 1.5F);


            mstr = pageEventArgs.Graphics.MeasureString("" + ("00" + ""), font100Bold);

            Image FotoWaterMark;
            FotoWaterMark = insertWaterMarkToImg(foto2);
            FotoWaterMark = delFondoFoto((Bitmap)FotoWaterMark, false);
            FotoWaterMark = delFondoFoto((Bitmap)FotoWaterMark, true);

            pageEventArgs.Graphics.DrawImage(FotoWaterMark, xFront + 75.0F, fotoYmod + 72.0F, 36F, 42F);
            pageEventArgs.Graphics.RotateTransform(180);
        }


        private static System.Drawing.Image convierteAImage(byte[] imgOrac)
        {
            using (var ms = new MemoryStream(imgOrac))
            {
                loggerSample.Info(DateTime.Now.ToString("yyyyMMddmmss") + " convierteAImage " + "---Datos de mes");
                return System.Drawing.Image.FromStream(ms);
            }
        }

        public void DrawCardBack(PrintPageEventArgs pageEventArgs)
        {
            pageEventArgs.Graphics.TranslateTransform(339.5F, 211.5F);
            pageEventArgs.Graphics.RotateTransform(180);
            Single x1Back = 26.0F;
            Single x2Back = 90.0F;
            loggerSample.Info(DateTime.Now.ToString("yyyyMMddmmss") + " DrawCardBack " + "---Datos reverso licencia");
            //'/*finger key
            Single fotoX = 20F;
            Single fotoY = 20.0F;
            Single fotoYmod = fotoY - 10.0F;

            //'Etiqueta
            pageEventArgs.Graphics.DrawString("DONADOR:", sixfontBold, Brushes.Black, fotoX + 12.0F, fotoY + 84.0F);
            //'Valor                
            pageEventArgs.Graphics.DrawString(component.DONADOR_ORGANOS.ToUpper(), sevenfontBold, Brushes.Black, fotoX + 12.0F, fotoY + 92.0F);

            //'Etiqueta
            pageEventArgs.Graphics.DrawString("ALERGIAS:", sixfontBold, Brushes.Black, fotoX + 12.0F, fotoY + 106.0F);
            //'Valor                
            pageEventArgs.Graphics.DrawString(component.ALERGIAS, sevenfontBold, Brushes.Black, fotoX + 12.0F, fotoY + 114.0F);

            //'Etiqueta
            pageEventArgs.Graphics.DrawString("TIPO SANGUINEO:", sixfontBold, Brushes.Black, fotoX + 88.0F, fotoY + 9.0f);
            //'Valor                
            pageEventArgs.Graphics.DrawString(component.TIPO_SANGRE, sevenfontBold, Brushes.Black, fotoX + 88.0F, fotoY + 17.0f);

            //'Etiqueta
            pageEventArgs.Graphics.DrawString("NACIMIENTO:", sixfontBold, Brushes.Black, fotoX + 185.0F, fotoY + 9.0F);
            //'Valor
            pageEventArgs.Graphics.DrawString(component.FECHA_NACIMIENTO, sevenfontBold, Brushes.Black, fotoX + 185.0F, fotoY + 17.0F);

            //'Etiqueta
            pageEventArgs.Graphics.DrawString("TELEFONO:", sixfontBold, Brushes.Black, fotoX + 88.0F, fotoY + 35.0f);
            //'Valor                
            pageEventArgs.Graphics.DrawString(component.TELEFONO, sevenfontBold, Brushes.Black, fotoX + 88.0F, fotoY + 43.0f);

            //ETIQUETA
            pageEventArgs.Graphics.DrawString("ANTIGÜEDAD:", sixfontBold, Brushes.Black, fotoX + 170.0F, fotoY + 35.0f);
            //'Valor                
            pageEventArgs.Graphics.DrawString(component.FANTIGUEDAD, sevenfontBold, Brushes.Black, fotoX + 170.0F, fotoY + 43.0f);

            //'Etiqueta
            //pageEventArgs.Graphics.DrawString("TLX SEGURO:", sixfontBold, Brushes.Black, fotoX + 93.0F, fotoY + 80.0f);
            //'Valor
            //pageEventArgs.Graphics.DrawString("TLX SEGURO", sevenfontBold, Brushes.Black, fotoX + 93.0F, fotoY + 88.0F);

            float yCertificacion = 7.0F;

            /*string restriccionesStr = "";
            pageEventArgs.Graphics.DrawString("RESTRICCIONES", fivefontBold, Brushes.Black, fotoX + 85.0F, fotoY + 100.0F);
            if (!component.SIN_RESTRICCIONES)
            {
                if (component.LENTES == "Lentes")
                {
                    restriccionesStr += "USA LENTES\r\n";
                }
                if (component.PROTESIS != "")
                {
                    restriccionesStr += "USA PRÓTESIS\r\n";
                    yCertificacion += 7.0F;
                }
                if (component.AUDITIVO != "")
                {
                    restriccionesStr += "USA APARATO AUDITIVO\r\n";
                    yCertificacion += 7.0F;
                }
                if (component.LENTES_CONTACTO != "")
                {
                    restriccionesStr += "USA LENTES DE CONTACTO\r\n";
                    yCertificacion += 7.0F;
                }
                if (component.VEHICULO_ADAPTADO != "")
                {
                    restriccionesStr += "USA VEHÍCULO ADAPTADO\r\n";
                    yCertificacion += 7.0F;
                }
                if (component.VEHICULO_AUTOMATICO != "")
                {
                    restriccionesStr += "USA VEHÍCULO AUTOMÁTICO";
                    yCertificacion += 7.0F;
                }
            }
            else
            {
                restriccionesStr = "SIN RESTRICCIONES";
            }

            pageEventArgs.Graphics.DrawString(restriccionesStr, fivefontBold, Brushes.Black, fotoX + 85.0F, fotoY + 107.0F);*/

            //VAN,BUS,TAXI
            /*string certificacionStr = "";
            if (component.VAN != "")
            {
                certificacionStr += "VAN\r\n";
            }
            if (component.BUS != "")
            {
                certificacionStr += "BUS\r\n";
            }
            if (component.TAXI != "")
            {
                certificacionStr += "TAXI";
            }

            if (certificacionStr != "")
            {
                pageEventArgs.Graphics.DrawString("CONDUCTOR CERTIFICADO\r\nEN:", fivefontBold, Brushes.Black, fotoX + 195.0F, fotoY + 44.0F + yCertificacion);
                pageEventArgs.Graphics.DrawString(certificacionStr, fivefontBold, Brushes.Black, fotoX + 195.0F, fotoY + 58.0F + yCertificacion);
            }*/

            //ETIQUETA
            pageEventArgs.Graphics.DrawString("FIRMA DEL TITULAR", sixfontBold, Brushes.Black, fotoX + 160.0F, fotoY + 110.0F);
            pageEventArgs.Graphics.DrawImage(Firma, fotoX + 180.0F, fotoY + 70.0F, 57, 42);

            Single firmafactor;

            firmafactor = float.Parse(((FotoWidth * 0.5 * 2.5) / (Firma.Width)).ToString());
            if (firmafactor * Firma.Height > FotoHeight * 0.5F)
                firmafactor = (FotoHeight * 0.5F) / Firma.Height;

            //pageEventArgs.Graphics.DrawImage(Firma, 240.0F, 86.0F, Firma.Width * firmafactor * 2F, Firma.Height * firmafactor * 2.8F);

            pageEventArgs.Graphics.DrawImage(FirmaSecretario, fotoX + 100.0F, fotoY + 100.0F, 60, 25);

            //Secretario
            pageEventArgs.Graphics.DrawString("LICDA. LUZ MARIA VÁZQUEZ AVILA", fivefontBold, Brushes.Black, fotoX + 65.0F, fotoY + 125.0F);
            pageEventArgs.Graphics.DrawString("SECRETARIA DE", fivefontBold, Brushes.Black, fotoX + 90.0F, fotoY + 133.0F);
            pageEventArgs.Graphics.DrawString("MOVILIDAD Y TRANSPORTE", fivefontBold, Brushes.Black, fotoX + 75.0F, fotoY + 141.0F);
            //pageEventArgs.Graphics.DrawString("", fivefontBold, Brushes.Black, fotoX + 107.0F, fotoY + 132.0F);
        
            //QR DATOS
            Image fotoQR;
            //string qrPath = "C:/wamp64/www/licenciadigital1/programs/diseno/qr/" + component.numeroLicencia + "\\" + "qr.png";
            string qrPath = workPath + "\\QR\\" + component.numeroLicencia + "QRI" + ".png";
            if (File.Exists(qrPath))
            {
                fotoQR = Bitmap.FromFile(qrPath);

                this.EncodeWithString(qrPath, qrPath);

                pageEventArgs.Graphics.DrawImage(fotoQR, fotoX - 15F, fotoY  -2F, 75F, 75F);
            }

            //QR DE IMAGEN
            //string qrPath = "C:/wamp64/www/licenciadigital1/programs/diseno/qr/" + component.numeroLicencia + "\\" + "qr.png";
            /*Image fotoQR1;
            string qrPath2 = workPath + "\\QR\\" + component.numeroLicencia + "QRI" + ".jpg";
            if (File.Exists(qrPath))
            {
                fotoQR1 = Bitmap.FromFile(qrPath);

                this.EncodeWithString(qrPath, qrPath);

                pageEventArgs.Graphics.DrawImage(fotoQR1, fotoX, fotoY + 85.0F, 60.0F, 90.0F);
            }*/

            Image fotoHuella;
            if (File.Exists(workPath + "\\thumbs_6.bmp"))
            {
                fotoHuella = Bitmap.FromFile(workPath + "\\thumbs_6.bmp");

                this.EncodeWithString(workPath + "\\thumbs_6.bmp", workPath + "\\thumbs_6.bmp");

                //pageEventArgs.Graphics.DrawImage(fotoHuella, fotoX, fotoY + 120.0F, 40.0F, 60.0F);
            }
            else
            {
                if (File.Exists(workPath + "\\thumbs_6.jpg"))
                {
                    fotoHuella = Bitmap.FromFile(workPath + "\\thumbs_6.jpg");

                    this.EncodeWithString(workPath + "\\thumbs_6.jpg", workPath + "\\thumbs_6.jpg");

                   // pageEventArgs.Graphics.DrawImage(fotoHuella, fotoX, fotoY + 120.0F, 40.0F, 60.0F);
                }
            }


            #region codigo anterior comentado
            //using (Font font = new Font("Courier New", 8))
            //using (SolidBrush brush = new SolidBrush(Color.Red))
            //using (Pen pen = new Pen(Color.YellowGreen))
            //{
            //    // pageEventArgs.Graphics.DrawString("card back", font, brush, 10, 10);
            //    Bitmap bitm = new Bitmap("C:\\Users\\drivas\\Documents\\Visual Studio 2013\\Projects\\PruebasDispositivos\\PruebasDispositivos\\bin\\Debug\\nuevo.bmp");
            //    Image.GetThumbnailImageAbort pequeña = null;
            //    bitm.GetThumbnailImage(20, 30, pequeña, IntPtr.Zero);
            //    Image img = (Image)bitm;
            //    Point punto = new Point();
            //    punto.X = 20;
            //    punto.Y = 30;
            //    pageEventArgs.Graphics.DrawImage(img, new Rectangle(0, 0, 180, 220));//(img, punto);
            //}

            //Bitmap colorBitmap = new Bitmap("color.bmp");
            //pageEventArgs.Graphics.DrawImage(colorBitmap, 25, 50, colorBitmap.Width / _sizeFactor, colorBitmap.Height / _sizeFactor);

            //Bitmap kBitmap = new Bitmap("mono.bmp");
            //pageEventArgs.Graphics.DrawImage(kBitmap, 130, 50, kBitmap.Width / _sizeFactor, kBitmap.Height / _sizeFactor);

            //Bitmap UVBitmap = new Bitmap("uv.bmp");
            //pageEventArgs.Graphics.DrawImage(UVBitmap, 235, 50, UVBitmap.Width / _sizeFactor, UVBitmap.Height / _sizeFactor);

            // WriteCustomTopcoatBlockingEscapesBack(pageEventArgs.Graphics);
            #endregion
        }

        private string EliminaEspacios(string cadena)
        {
            loggerSample.Info(DateTime.Now.ToString("yyyyMMddmmss") + " EliminaEspacios " + "---");
            if (cadena != null)
            {
                while (cadena.IndexOf("  ") != -1)
                    cadena = cadena.Replace("  ", " ");
            }
            else
                cadena = string.Empty;
            return cadena;
        }

        private string BitmapToString(Bitmap bImage)
        {
            loggerSample.Info(DateTime.Now.ToString("yyyyMMddmmss") + " BitmapToString " + "---");
            try
            {
                string data;
                MemoryStream ms = new MemoryStream();
                bImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                data = Convert.ToBase64String(ms.ToArray());
                return data;
            }
            catch (Exception ex)
            {
                loggerSample.Error(DateTime.Now.ToString("yyyyMMddmmss") + " ERROR!!! Descripción " + ex.Message);
                return string.Empty;
            }

        }

        private bool writeImagesToBase64(string workPath, Bitmap Foto, Bitmap Firma, Bitmap HuellaIzq, Bitmap HuellaDer)
        {
            loggerSample.Info(DateTime.Now.ToString("yyyyMMddmmss") + " writeImagesToBase64 " + "---");
            StreamWriter sw = new System.IO.StreamWriter(workPath + "\\data.txt");
            string data = BitmapToString(Foto);
            sw.WriteLine(data);
            data = BitmapToString(Firma);
            sw.WriteLine(data);
            data = BitmapToString(HuellaIzq);
            sw.WriteLine(data);
            data = BitmapToString(HuellaDer);
            sw.WriteLine(data);
            sw.Close();
            return true;
        }

        private string[] CortaCadenaGraficos(string cadena, PrintPageEventArgs objGrafico, int maxAnchoCad, Font fuente)
        {
            loggerSample.Info(DateTime.Now.ToString("yyyyMMddmmss") + " CortaCadenaGraficos " + "---");
            string[] cadenaArray = cadena.Split(' ');
            string[] testCadena = new string[1];
            int intCadena;
            string strMeasure = string.Empty;
            //'Dimensiones de la cadena de texto en el método gráfico
            System.Drawing.SizeF Sdimensioncad = new System.Drawing.SizeF(0, 0);


            intCadena = 0;
            Array.Resize(ref testCadena, intCadena);


            for (int ele = 0; ele < cadenaArray.Length; ele++)
            {
                strMeasure = (testCadena[intCadena] + " " + cadenaArray[ele]).Trim();
                Sdimensioncad = objGrafico.Graphics.MeasureString(strMeasure, fuente);
                Sdimensioncad = objGrafico.Graphics.MeasureString(strMeasure, fuente);
                if (Sdimensioncad.Width <= maxAnchoCad)
                    testCadena[intCadena] += " " + cadenaArray[ele].Trim();
                else
                {
                    intCadena = intCadena + 1;
                    Array.Resize(ref testCadena, intCadena);
                    testCadena[intCadena] = " " + cadenaArray[ele].Trim();
                }
            }

            return testCadena;

        }

        private Bitmap _recortaBMP(Bitmap bmp)
        {
            loggerSample.Info(DateTime.Now.ToString("yyyyMMddmmss") + " _recortaBMP " + "---");
            //'Define las cordenadas de la imagen
            int imgCol = 0, imgCol2 = 0, imgRow = 0, imgRow2 = 0;
            //'Define el color que se debe recortar
            Color color = bmp.GetPixel(0, 0);
            //'Define el alto y ancho de la imagen original
            int alto = bmp.Height;
            int ancho = bmp.Width;

            //'Rectangulo utilizado de la imagen
            Rectangle rec;


            for (int i = 0; i < ancho; i++)
            {
                for (int j = 0; j < alto; j++)
                {
                    if (color.Name.ToString().Substring(2, 6) != bmp.GetPixel(i, j).Name.ToString().Substring(2, 6))
                    {
                        imgCol = i;
                        j = alto;
                    }
                }
                if (imgCol > 0) i = ancho;
            }

            for (int i = 0; i < ancho; i++)
            {
                for (int j = 0; j < alto; j++)
                {
                    if (color.Name.ToString().Substring(2, 6) != bmp.GetPixel(i, j).Name.ToString().Substring(2, 6))
                    {
                        imgCol2 = i;
                        j = alto;
                    }
                }
                if (imgCol2 > 0)
                    i = ancho;
            }

            for (int j = 0; j < alto; j++)
            {
                for (int i = 0; i < ancho; i++)
                {
                    if (color.Name.ToString().Substring(2, 6) != bmp.GetPixel(i, j).Name.ToString().Substring(2, 6))
                    {
                        imgRow = j;
                        i = ancho;
                    }
                }
                if (imgRow > 0) j = alto;
            }

            for (int j = alto; j < imgRow; imgRow--)
            {
                for (int i = 0; i < ancho; i++)
                {
                    if (color.Name.ToString().Substring(2, 6) != bmp.GetPixel(i, j).Name.ToString().Substring(2, 6))
                    {
                        imgRow2 = j;
                        i = ancho;
                    }
                }
                if (imgRow2 > 0) j = imgRow;
            }


            //'Recorta la imagen original y la coloca en el rectangulo especificado
            rec = new Rectangle(imgCol, imgRow, imgCol2 - imgCol, imgRow2 - imgRow);
            System.Drawing.Imaging.PixelFormat format = bmp.PixelFormat;
            Bitmap clonBMP = bmp.Clone(rec, format);

            return clonBMP;
        }

        public void OnPrintPage(object sender, PrintPageEventArgs pageEventArgs)
        {
            loggerSample.Info(DateTime.Now.ToString("yyyyMMddmmss") + " OnPrintPage " + "---");
            if (_frontOfCard)
            {
                DrawCardFront(pageEventArgs);
                _frontOfCard = false;
                pageEventArgs.HasMorePages = _commandLineOptions.twoPages;
            }
            else
            {
                DrawCardBack(pageEventArgs);
            }
        }

        public void OnQueryPageSettings(object sender, QueryPageSettingsEventArgs queryEventArgs)
        {
            loggerSample.Info(DateTime.Now.ToString("yyyyMMddmmss") + " OnQueryPageSettings " + "---");
            // use this opportunity to adjust the orientation for the back side of the card:
            if (!_frontOfCard)
            {
                queryEventArgs.PageSettings.Landscape = _commandLineOptions.portraitBack ? false : true;
            }
        }

        private void WriteMagstripeEscapes(Graphics graphics)
        {
            loggerSample.Info(DateTime.Now.ToString("yyyyMMddmmss") + " WriteMagstripeEscapes " + "---");
            // emit some plain track 1, 2, 3 data. Assume IAT track configuration.
            string track1Escape = "~1ABC 123";
            string track2Escape = "~2456";
            string track3Escape = "~3789";

            using (Font font = new Font("Courier New", 6))
            using (SolidBrush brush = new SolidBrush(Color.CadetBlue))
            using (Pen pen = new Pen(Color.Red))
            {
                graphics.DrawString(track1Escape, font, brush, 50, 50);
                graphics.DrawString(track2Escape, font, brush, 50, 50);
                graphics.DrawString(track3Escape, font, brush, 50, 50);
            }
        }

        private string GetTopcoatBlockingPrintTicketXml()
        {
            string topcoatBlockingXml = string.Empty;
            loggerSample.Info(DateTime.Now.ToString("yyyyMMddmmss") + " GetTopcoatBlockingPrintTicketXml " + "--- _commandLineOptions.topcoatBlockingFront " + _commandLineOptions.topcoatBlockingFront);
            // front:
            switch (_commandLineOptions.topcoatBlockingFront)
            {
                case "":
                    // use the current driver settings.
                    break;
                case "custom":
                    // We will generate topcoat and blocking escapes for the card front
                    // in this application. Escapes override the PrintTicket settings.
                    break;
                case "all":
                    topcoatBlockingXml += PrintTicketXml.FrontTopcoatBlockingPreset_All;
                    break;

                // we need the 'exception' markup for the remaining settings:

                case "chip":
                    topcoatBlockingXml += PrintTicketXml.FrontTopcoatBlockingPreset_Except;
                    topcoatBlockingXml += PrintTicketXml.FrontTopcoatBlockingPreset_ISO_7816;
                    break;
                case "magjis":
                    topcoatBlockingXml += PrintTicketXml.FrontTopcoatBlockingPreset_Except;
                    topcoatBlockingXml += PrintTicketXml.FrontTopcoatBlockingPreset_JIS;
                    break;
                case "mag2":
                    topcoatBlockingXml += PrintTicketXml.FrontTopcoatBlockingPreset_Except;
                    topcoatBlockingXml += PrintTicketXml.FrontTopcoatBlockingPreset_ISO_2Track;
                    break;
                case "mag3":
                    topcoatBlockingXml += PrintTicketXml.FrontTopcoatBlockingPreset_Except;
                    topcoatBlockingXml += PrintTicketXml.FrontTopcoatBlockingPreset_ISO_3Track;
                    break;
            }

            // back:
            switch (_commandLineOptions.topcoatBlockingBack)
            {
                case "":
                    // use the current driver settings.
                    break;
                case "custom":
                    // We will generate topcoat and blocking escapes for the card back
                    // in this application. Escapes override the PrintTicket settings.
                    break;
                case "all":
                    topcoatBlockingXml += PrintTicketXml.BackTopcoatBlockingPreset_All;
                    break;

                // we need the 'exception' markup for the remaining settings:

                case "chip":
                    topcoatBlockingXml += PrintTicketXml.BackTopcoatBlockingPreset_Except;
                    topcoatBlockingXml += PrintTicketXml.BackTopcoatBlockingPreset_ISO_7816;
                    break;
                case "magjis":
                    topcoatBlockingXml += PrintTicketXml.BackTopcoatBlockingPreset_Except;
                    topcoatBlockingXml += PrintTicketXml.BackTopcoatBlockingPreset_JIS;
                    break;
                case "mag2":
                    topcoatBlockingXml += PrintTicketXml.BackTopcoatBlockingPreset_Except;
                    topcoatBlockingXml += PrintTicketXml.BackTopcoatBlockingPreset_ISO_2Track;
                    break;
                case "mag3":
                    topcoatBlockingXml += PrintTicketXml.BackTopcoatBlockingPreset_Except;
                    topcoatBlockingXml += PrintTicketXml.BackTopcoatBlockingPreset_ISO_3Track;
                    break;
            }

            return topcoatBlockingXml;
        }

        private void WriteCustomTopcoatBlockingEscapesFront(Graphics graphics)
        {
            loggerSample.Info(DateTime.Now.ToString("yyyyMMddmmss") + " WriteCustomTopcoatBlockingEscapesFront " + "---");

            if (_commandLineOptions.topcoatBlockingFront != "custom")
                return;

            // a 'topcoat Add' escape will force topcoat OFF for the entire card side.

            // units are millimeters; landscape basis; top left width height:
            // a rectangle one inch down; two inches wide; 1 cm high.
            // units are millimeters; top left width height:
            string topCoatAddEsc = "~TA%25.4 0 50.8 10;";
            topCoatAddEsc += "40 60 7 7?";  // add a square, lower

            using (Font font = new Font("Courier New", 6))
            using (SolidBrush brush = new SolidBrush(Color.Black))
            using (Pen pen = new Pen(Color.Black))
            {
                graphics.DrawString(topCoatAddEsc, font, brush, 10, 10);
            }

            // a 'blocking' escape will override the driver settings:
            string blockingEsc = "~PB% 0 19 3 54;";

            using (Font font = new Font("Courier New", 6))
            using (SolidBrush brush = new SolidBrush(Color.Black))
            using (Pen pen = new Pen(Color.Black))
            {
                graphics.DrawString(blockingEsc, font, brush, 10, 20);
            }
        }

        private void WriteCustomTopcoatBlockingEscapesBack(Graphics graphics)
        {
            loggerSample.Info(DateTime.Now.ToString("yyyyMMddmmss") + " WriteCustomTopcoatBlockingEscapesBack " + "---");
            if (_commandLineOptions.topcoatBlockingBack != "custom")
                return;

            string topCoatAddEsc = "~TA%25.4 10 50.8 20;";

            using (Font font = new Font("Courier New", 6))
            using (SolidBrush brush = new SolidBrush(Color.Black))
            using (Pen pen = new Pen(Color.Black))
            {
                graphics.DrawString(topCoatAddEsc, font, brush, 10, 10);
            }

            // a 'blocking' escape will override the driver settings:
            string blockingEsc = "~PB% 0 23 3 54;";

            using (Font font = new Font("Times New Roman", 6))
            using (SolidBrush brush = new SolidBrush(Color.Pink))
            using (Pen pen = new Pen(Color.SeaShell))
            {
                graphics.DrawString(blockingEsc, font, brush, 10, 20);
            }
        }
    }



}
