using FastMember;
using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace PUE.Views.License
{
    /// <summary>
    /// Interaction logic for Resume.xaml
    /// </summary>
    public partial class Resume : UserControl
    {
        static string fechaInit = string.Empty;
        static string fechaFin = string.Empty;
        static string recaudadora = string.Empty;

        public Resume(string[] args)
        {
            InitializeComponent();
            recaudadora = args[0];
            fechaInit = args[1];
            fechaFin = args[2];
        }

        private void seacabodecargar(object sender, RoutedEventArgs e)
        {
            ShowReport();
        }

        private void ShowReport()
        {
            Controllers.Resultado res = new Controllers.Resultado();
            DateTime dtini = DateTime.Parse(fechaInit);
            DateTime dtFin = DateTime.Parse(fechaFin);
            //fechaInit = dtini.ToString("dd/MM/yy");
            //fechaFin = dtFin.ToString("dd/MM/yy");
            //recaudadora = "0102";

            var tram = DataPUE.I_ADMIN.getDataResumen(recaudadora, dtini, dtFin);
            try
            {
                ReportParameter[] paramers = new ReportParameter[5];
                paramers[0] = new ReportParameter("PARAM_HOY", DateTime.Now.ToString(), true);
                paramers[1] = new ReportParameter("FECHA_INIT", fechaInit, true);
                paramers[2] = new ReportParameter("FECHA_FIN", fechaFin, true);
                paramers[3] = new ReportParameter("REC_PARAM", recaudadora, true);
                paramers[4] = new ReportParameter("TOTAL_FOLIOS", tram.Count.ToString(), true);

                ReportDataSource reportDataSource = new ReportDataSource();
                reportDataSource.Name = "DSResumen";
                DataTable table = new DataTable();
                using (var reader = ObjectReader.Create(tram))
                {
                    table.Load(reader);
                }
                reportDataSource.Value = table;

                _reportViewer.LocalReport.DataSources.Clear();
                _reportViewer.LocalReport.ReportPath = @"Content\Formatos\rptResumen.rdlc";
                _reportViewer.LocalReport.SetParameters(paramers);
                _reportViewer.LocalReport.DataSources.Add(reportDataSource);

                this._reportViewer.RefreshReport();
                _reportViewer.SetDisplayMode(DisplayMode.PrintLayout);
                _reportViewer.ZoomMode = ZoomMode.Percent;
                _reportViewer.ZoomPercent = 100;
            }
            catch (Exception ex)
            { }

        }
    }
}
