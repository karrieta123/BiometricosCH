using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ZXing;
using System.Web;
using System.IO;
using System.Drawing.Imaging;
using System.Security.Cryptography;

namespace ZacatecasQR
{
    public class Class1
    {


        public static string GenerateCodeZacatecas(string numeroLicemcia, string Nombre, string FechaExpedicion, string FechaVencimiento, string Path)
        {

            var writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;

            var result = writer.Write(" El numero de licencia es:" + numeroLicemcia + "\n" + "El nombre es :" + Nombre + "\n" + "Fecha de expedicion:" + FechaExpedicion + "\n" + "Fecha de vencimiento:" + FechaVencimiento);

          //  string destino = @"C: \Users\DC SOLUCIONES\Documents\CODIGOS" + numeroLicemcia + ".Jpeg";
            var barcodeBitmap = new Bitmap(result);


            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = new FileStream(Path, FileMode.Create, FileAccess.ReadWrite))
                {
                    barcodeBitmap.Save(memory, ImageFormat.Jpeg);

                    byte[] bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
            }

            return "OK";
        }

    }
}
