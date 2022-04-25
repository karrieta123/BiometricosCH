using Entidades;
using Microsoft.Reporting.WinForms;
using System;
using System.IO;
using System.Windows.Forms;

public class Format
{
    /// <summary>
    /// Genera arreglo de bytes para almacenar el formato
    /// </summary>
    /// <param name="DataSetInf">Nombre del dataset</param>
    /// <param name="Datos">Contenido del reporte</param>
    /// <param name="Reporte">Reporte a utilizar</param>
    /// <returns></returns>
    public static byte[] BytesReporte(String DataSetInf, dynamic Datos, byte[] Reporte)
    {
        ReportViewer reportViewer = new ReportViewer();
        byte[] pdfBytes = null;

        reportViewer.LocalReport.DataSources.Add(new ReportDataSource(DataSetInf, new BindingSource(Datos, null)));

        using (Stream resourceReportStream = new MemoryStream(Reporte))
        {
            reportViewer.LocalReport.LoadReportDefinition(resourceReportStream);
            pdfBytes = reportViewer.LocalReport.Render("PDF");
        }

        return pdfBytes;
    }

    public static EntiVersion CheckVersion(string version)
    {
        EntiVersion Ver = new EntiVersion();
        DataPUE.Versionamiento objVersion = new DataPUE.Versionamiento();
        //var versionAct = objVersion.VersionActual();
        //if (version != versionAct.NUMERO_VERSION.ToString())
        //{
        //    Ver.HayNuevaVersion = true;
        //    Ver.version = versionAct.NUMERO_VERSION.ToString();
        //}

        string vers = "10";
        if (version != vers)
        {
            Ver.HayNuevaVersion = true;
            Ver.version = vers;
        }

        return Ver;
    }
}
