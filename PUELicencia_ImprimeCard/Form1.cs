using dxp01sdk;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace PUELicencia_ImprimeCard
{
    public partial class Form1 : Form
    {
        public Form1(string[] args)
        {
            InitializeComponent();

            if (args.Count() > 0)
            {
                string[] argumentos = new string[4];
                for (int i = 0; i < 4; i++)
                {
                    argumentos[i] = args[i];
                }
                SamplePrintDocument.jsonNodes = args[4];
                this.FormBorderStyle = FormBorderStyle.None;
                CommandLineOptions commandLineOptions = CommandLineOptions.CreateFromArguments(argumentos);
                commandLineOptions.Validate();
                pBar.Increment(20);
                pBar.Refresh();
                lblMens.Text = "Cargando Parametros";
                lblMens.Refresh();
                // System.Threading.Thread.Sleep(1500);

                BidiSplWrap bidiSpl = null;
                int printerJobID = 0;

                try
                {
                    bidiSpl = new BidiSplWrap();
                    bidiSpl.BindDevice(commandLineOptions.printerName);

                    pBar.Increment(40);
                    pBar.Refresh();
                    lblMens.Text = "Parametros cargados";
                    lblMens.Refresh();

                    Image img;
                    //RenderReport();
                    //byte[] byteReport = RenderReport();
                    //using (var ms = new MemoryStream(byteReport))
                    //{
                    //    img = Image.FromStream(ms);
                    //    img.Save(Application.StartupPath + "\\nuevo.bmp", ImageFormat.Bmp);
                    //}

                    string driverVersionXml = bidiSpl.GetPrinterData(strings.SDK_VERSION);
                    Console.WriteLine(Environment.NewLine + "driver version: " + Util.ParseDriverVersionXML(driverVersionXml) + Environment.NewLine);

                    string printerOptionsXML = bidiSpl.GetPrinterData(strings.PRINTER_OPTIONS2);
                    PrinterOptionsValues printerOptionsValues = Util.ParsePrinterOptionsXML(printerOptionsXML);

                    pBar.Increment(50);
                    pBar.Refresh();
                    lblMens.Text = "Reconociendo Dispositivo";
                    lblMens.Refresh();
                    //System.Threading.Thread.Sleep(1500);

                    if ("Ready" != printerOptionsValues._printerStatus && "Busy" != printerOptionsValues._printerStatus)
                    {
                        throw new Exception(commandLineOptions.printerName + " is not ready. status: " + printerOptionsValues._printerStatus);

                        lblError.Text = "la impresora no esta lista. estatus: " + printerOptionsValues._printerStatus;
                        lblError.Refresh();
                    }

                    string hopperID = string.Empty;
                    if (commandLineOptions.checkSupplies || commandLineOptions.jobCompletion)
                    {
                        printerJobID = Util.StartJob(
                            bidiSpl,
                            commandLineOptions.checkSupplies,
                            hopperID);
                    }
                    pBar.Increment(70);
                    pBar.Refresh();
                    lblMens.Text = "Imprimiendo...";
                    lblMens.Refresh();
                    // System.Threading.Thread.Sleep(1500);

                    SamplePrintDocument printDocument = new SamplePrintDocument(commandLineOptions);
                    printDocument.PrintController = new StandardPrintController();
                    printDocument.BeginPrint += new PrintEventHandler(printDocument.OnBeginPrint);
                    printDocument.QueryPageSettings += new QueryPageSettingsEventHandler(printDocument.OnQueryPageSettings);
                    printDocument.PrintPage += new PrintPageEventHandler(printDocument.OnPrintPage);
                    printDocument.Print();

                    if (0 != printerJobID)
                    {
                        // wait for the print spooling to finish and then issue an EndJob():
                        Util.WaitForWindowsJobID(bidiSpl, commandLineOptions.printerName);
                        bidiSpl.SetPrinterData(strings.ENDJOB);
                    }

                    if (commandLineOptions.jobCompletion)
                    {
                        Util.PollForJobCompletion(bidiSpl, printerJobID);
                    }
                }
                catch (BidiException ex)
                {
                    Console.WriteLine(ex.Message);
                    Util.CancelJob(bidiSpl, ex.PrinterJobID, ex.ErrorCode);
                    lblError.Text = "se detecto el siguiente error: " + ex.Message;
                    lblError.Refresh();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    if (0 != printerJobID)
                    {
                        Util.CancelJob(bidiSpl, printerJobID, 0);
                    }

                    lblError.Text = "Se detecto el siguiente error: " + ex.Message;
                    lblError.Refresh();
                }
                finally
                {
                    bidiSpl.UnbindDevice();
                }
            }

            pBar.Increment(95);
            pBar.Refresh();
            lblMens.Text = "Se mando a imprimir correctamente...";
            lblMens.Refresh();
            //System.Threading.Thread.Sleep(1500);
            // this.Close();

        }

        private static byte[] GeneraBytesReporteTotales(string nombreDataSet, ReportDataSource contenidoReporte, byte[] resource)
        {
            ReportViewer reportViewer = new ReportViewer();
            byte[] pdfBytes = null;

            reportViewer.LocalReport.DataSources.Add(new ReportDataSource(nombreDataSet, new BindingSource(contenidoReporte, null)));


            using (Stream resourceReportStream = new MemoryStream(resource))
            {
                reportViewer.LocalReport.LoadReportDefinition(resourceReportStream);
                pdfBytes = reportViewer.LocalReport.Render("PDF");
            }

            return pdfBytes;
        }



        private void RenderReport()
        {

            LocalReport localReport = new LocalReport();

            localReport.ReportPath = @"content\Formatos\RptCompleto.rdlc";
            //localReport.ReportPath ="\\192.168.100.203\\datosg\\TI\\Proyectos\\BCS\\Solución\\BCSLicencias\\PUELicencia_ImprimeCard\\Resources\\Report1.rdlc";

            DataSet1TableAdapters.sp_PruebasTableAdapter obj = new DataSet1TableAdapters.sp_PruebasTableAdapter();
            DataTable dt = obj.GetData();
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Value = dt;
            reportDataSource.Name = "DataSet1";
            localReport.DataSources.Add(reportDataSource);

            //ReportViewer viewer = new ReportViewer();
            //viewer.ProcessingMode = ProcessingMode.Local;
            //viewer.LocalReport.ReportPath = localReport.ReportPath;
            //viewer.LocalReport.DataSources.Add(reportDataSource);

            //File.WriteAllBytes("C:\\Users\\drivas\\Documents\\visual studio 2013\\Projects\\PruebasDispositivos\\pruebas\\reporte.pdf",
            //    GeneraBytesReporteTotales("DataSet1",reportDataSource, Resource1.Report1));

            string deviceInfo =
        "<DeviceInfo>" +
        "  <OutputFormat>bmp</OutputFormat>" +
        "  <PageWidth>8.55cm</PageWidth>" +
        "  <PageHeight>5.4cm</PageHeight>" +
        "</DeviceInfo>";
            string reportType = "Image";
            string mimeType;
            string encoding;
            string fileNameExtension;
            Warning[] warnings;

            string[] streams;

            byte[] renderedBytes;

            m_streams = new List<Stream>();
            //Render the report

            //renderedBytes = localReport.Render("Image", deviceInfo, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            localReport.Render("Image", deviceInfo, CreateStream, out warnings);
            //byte[] bytes = viewer.LocalReport.Render("PDF", string.Empty, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);



            //  File.WriteAllBytes("C:\\Users\\drivas\\Documents\\visual studio 2013\\Projects\\PruebasDispositivos\\pruebas\\reporte.pdf", bytes);   
            for (int i = 0; i < m_streams.Count; i++)
            {
                m_streams[i].Flush();
                m_streams[i].Close();
                m_streams[i].Dispose();
            }

            localReport.Dispose();
        }

        private IList<Stream> m_streams;
        private String tmpPath = "C:\\Users\\drivas\\Documents\\Visual Studio 2013\\Projects\\PruebasDispositivos\\PruebasDispositivos\\bin\\Debug\\";

        private Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            Stream stream = new FileStream(tmpPath + name + "." + fileNameExtension, FileMode.Create);

            m_streams.Add(stream);

            return stream;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
