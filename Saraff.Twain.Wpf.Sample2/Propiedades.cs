using DataPUE;
using Entidades;
using System;
using System.Collections.Generic;

namespace Saraff.Twain.Wpf.Sample2
{
    public class Propiedades
    {
        //dpi
        public string color { get; set; }
        public bool sided { get; set; }
        public float resolucion { get; set; }
        public object Ptype { get; set; }
        //public string JsonSettings = null;
        public static string JsonDocumentos { get; set; }
        public Propiedades()
        {
            I_SCANNER scan = new I_SCANNER();
            var settings = scan.Resolucion();
            foreach (var item in settings)
            {
                Ptype = Digitalizacion(item.color);
                sided = item.sides == 0 ? false : true;
                resolucion = Convert.ToInt32(item.dpi);
            }

        }
        //tipo de gitalizacion
        public object Digitalizacion(string name)
        {
            object escan = null;
            switch (name)
            {
                case "RGB":
                    escan = TwPixelType.RGB;
                    break;
                case "BW":
                    escan = TwPixelType.BW;
                    break;
                case "GRAY":
                    escan = TwPixelType.Gray;
                    break;
            }
            return escan;
        }
        public List<EntiDocumentos> DocumentosV(List<string> DocumentosRemove = null)
        {
            try
            {
                I_DOCUMENTOS doc = new I_DOCUMENTOS();
                List<EntiDocumentos> d = new List<EntiDocumentos>();
                DataPUE.I_SCANNER scan = new I_SCANNER();
                return scan.GetScanner();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
    public class EntiScannerProperties
    {
        public string color { get; set; }
        public int sided { get; set; }
        public int resolucion { get; set; }
    }
}