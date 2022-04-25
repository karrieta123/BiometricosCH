using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PUE.Controllers
{
    public class PrintLic
    {

        public static string jsonImprime(JObject jDataLicense, string connect)
        {

            string strJson = string.Empty;

            List<DataPUE.csPrevisual> lstCom = new List<DataPUE.csPrevisual>();
            DataPUE.csPrevisual componente = new DataPUE.csPrevisual();
            componente.NOMBRE = (string)jDataLicense.SelectToken("data[0].Nombre");
            componente.APELLIDO = (string)jDataLicense.SelectToken("data[0].ApellidoPaterno") + " " + jDataLicense.SelectToken("data[0].ApellidoMaterno");
            componente.APATERNO = (string)jDataLicense.SelectToken("data[0].APaterno");
            componente.AMATERNO = (string)jDataLicense.SelectToken("data[0].AMaterno");
            componente.DIRECCION = (string)jDataLicense.SelectToken("data[0].DomicilioCompleto");
            componente.CURP = (string)jDataLicense.SelectToken("data[0].Curp");
            componente.DESTINO = (string)jDataLicense.SelectToken("data[0].DestinoId");
            componente.LicenciaLetra = (string)jDataLicense.SelectToken("data[0].TipoLicencia");

            componente.RFC = (string)jDataLicense.SelectToken("data[0].RFC");


            componente.NACIONALIDAD = (string)jDataLicense.SelectToken("data[0].Nacionalidad");

            componente.numeroLicencia = (string)jDataLicense.SelectToken("data[0].NumeroLicencia");

            DateTime fechexp = (DateTime)jDataLicense.SelectToken("data[0].FechaExpedicion");
            DateTime fechanac = (DateTime)jDataLicense.SelectToken("data[0].FechaNacimiento");

            DateTime fechaAnt = (DateTime)jDataLicense.SelectToken("data[0].FechaAntiguedad");

            componente.FANTIGUEDAD = "";

            if (!String.IsNullOrEmpty((string)jDataLicense.SelectToken("data[0].FechaAntiguedad")))
            {


                componente.FANTIGUEDAD = fechaAnt.ToString().Substring(0, 10);

            }


            componente.FECHA_NACIMIENTO = fechanac.ToString().Substring(0, 10);

            componente.FECHA_EXPEDICION = fechexp.ToString().Substring(0, 10);

            string FechaVencimiento = (string)jDataLicense.SelectToken("data[0].FechaVencimiento");
            DateTime fech;
            if (FechaVencimiento != null || !String.IsNullOrEmpty(FechaVencimiento))
            {
                fech = (DateTime)jDataLicense.SelectToken("data[0].FechaVencimiento");
                FechaVencimiento = fech.ToString().Substring(0, 10);
                componente.FECHA_VENCIMIENTO = FechaVencimiento;
            }
            else
            {
                componente.FECHA_VENCIMIENTO = "PERMANENTE";
            }

            componente.ANIOS_VIGENCIA = (string)jDataLicense.SelectToken("data[0].AniosVigencia");

            componente.ExpedienteNum = (string)jDataLicense.SelectToken("data[0].FolioExpediente");


            componente.nombreSecre = "Nombre Secreto";

            componente.TIPO_LICENCIA = (string)jDataLicense.SelectToken("data[0].NombreLicencia");

            componente.SEXO = "Masculino";

            componente.CABELLO = "Castaño";
            componente.ESTATURA = "1.64";


            string curp = (string)jDataLicense.SelectToken("data[0].Curp");

            componente.TELEFONO = (string)jDataLicense.SelectToken("data[0].Telefono");
            componente.TIPO_SANGRE = (string)jDataLicense.SelectToken("data[0].TipoSangre");

            componente.ALERGIAS = (string)jDataLicense.SelectToken("data[0].Alergias");

            componente.SENAS_PARTICULARES = "Lunar cachete derecho";
            string dona = "Si";

            bool donador = (bool)jDataLicense.SelectToken("data[0].DonacionOrganos");

            if (!donador)
            {
                dona = "No";
            };

            componente.DONADOR_ORGANOS = dona;
            componente.IDfOLIO = "0001R";

            //Address values
            componente.calle = (string)jDataLicense.SelectToken("data[0].Calle");
            componente.numero = (string)jDataLicense.SelectToken("data[0].NoExterior");
            componente.interior = (string)jDataLicense.SelectToken("data[0].NoInterior");
            componente.colonia = (string)jDataLicense.SelectToken("data[0].ColoniaNombre");
            componente.municipio = (string)jDataLicense.SelectToken("data[0].MunicipioNombre");
            componente.estado = (string)jDataLicense.SelectToken("data[0].EstadoNombre");

            componente.Show_Domicilio = (bool)jDataLicense.SelectToken("data[0].ImprimeDir");

            //componente.SIN_RESTRICCIONES = (bool)jDataLicense.SelectToken("data[0].SinRestricciones");

            /*if ((bool)jDataLicense.SelectToken("data[0].Lentes"))
                componente.LENTES = "Lentes";
            else
                componente.LENTES = "Sin restricciones";

            if ((bool)jDataLicense.SelectToken("data[0].Protesis"))
                componente.PROTESIS = "Prótesis";
            else
                componente.PROTESIS = "";

            if ((bool)jDataLicense.SelectToken("data[0].Auditivo"))
                componente.AUDITIVO = "Auditivo";
            else
                componente.AUDITIVO = "";

            if ((bool)jDataLicense.SelectToken("data[0].LentesContacto"))
                componente.LENTES_CONTACTO = "Lentes de contacto";
            else
                componente.LENTES_CONTACTO = "";

            if ((bool)jDataLicense.SelectToken("data[0].VehiculoAdaptado"))
                componente.VEHICULO_ADAPTADO = "VehiculoAdaptado";
            else
                componente.VEHICULO_ADAPTADO = "";

            if ((bool)jDataLicense.SelectToken("data[0].VehiculoAutomatico"))
                componente.VEHICULO_AUTOMATICO = "VehiculoAutomatico";
            else
                componente.VEHICULO_AUTOMATICO = "";*/

            /*bool van = (bool)jDataLicense.SelectToken("data[0].Van");
            bool bus = (bool)jDataLicense.SelectToken("data[0].Bus");
            bool taxi = (bool)jDataLicense.SelectToken("data[0].Taxi");

            componente.VAN = van == true ? "VAN" : "";
            componente.BUS = bus == true ? "BUS" : "";
            componente.TAXI = taxi == true ? "TAXI" : "";*/

            // lstCom.Add(componente);            
            strJson = DataPUE.Json.Serialize<DataPUE.csPrevisual>(componente);
            return strJson;

        }

        private static System.Drawing.Image convierteAImage(byte[] imgOrac)
        {
            using (var ms = new MemoryStream(imgOrac))
            {
                return System.Drawing.Image.FromStream(ms);
            }
        }

        public static string damedescripcionMunicipio(string cp, string idmun, string idcol, int dondeviene)
        {
            string desc = string.Empty;
            List<DataPUE.Colonia> listCol = new List<DataPUE.Colonia>();
            List<DataPUE.Muni> descMun = DataPUE.I_DATOS_CONTRIBUYENTE.lstBuscaCP(cp, out listCol, idcol, dondeviene, idmun);
            if (descMun.Count > 0)
            {
                var muni = descMun.Where(r => r.CVE_MPIO == idmun).FirstOrDefault();
                if (muni != null)
                {
                    desc = muni.Descripcion;
                    desc += "|" + DataPUE.I_DATOS_CONTRIBUYENTE.descripcionCOl(idcol, idmun);
                    //if (listCol.Count > 0)
                    //{
                    //    var coloni = listCol.Where(c => c.CVE_COL == idcol && c.CVE_MPIO == muni.CVE_MPIO).FirstOrDefault();
                    //    desc += "|" + coloni.Descripcion;
                    //}
                }
            }

            return desc;
        }

        public static List<DataPUE.Models.IMGBIOMETRICOS> dameIndentxTramite(int idTramite)
        {
            List<DataPUE.Models.IMGBIOMETRICOS> oIdentifica = DataPUE.I_IDENTIFICA.obtenerIdentificaPorTramite(idTramite);
            return oIdentifica;
        }
    }
}
