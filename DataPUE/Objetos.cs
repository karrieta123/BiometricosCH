using System;
using System.Linq;

namespace DataPUE
{
    public enum Sexo
    {
        Hombre,
        Mujer
    }

    public enum Busqueda
    {
        CURP,
        NoAviso
    }


    public class Muni
    {
        public string CVE_MPIO { get; set; }
        public string Descripcion { get; set; }
    }
    public class LOcalidad
    {
        public string CVE_LOC { get; set; }
        public string Descripcion { get; set; }
    }

    public class Colonia
    {
        public string CVE_COL { get; set; }
        public string Descripcion { get; set; }
        public string CVE_MPIO { get; set; }
        public string CODIGO_POSTAL { get; set; }
    }
    public class SearchEnti
    {
        public string CVE_COL { get; set; }
        public string Descripcion { get; set; }
        public string CODIGO_POSTAL { get; set; }
    }


    public class Catalogo
    {
        public string Id { get; set; }
        public string Descripcion { get; set; }
    }

    public class RFCObj
    {
        public int ID_RFC { get; set; }
        public string RFC { get; set; }
        public string RFC_LETRA { get; set; }
        public string RFC_NUM { get; set; }
        public string RFC_HOMO { get; set; }
        public string CURP { get; set; }
        public string NOMBRE { get; set; }
        public string NOMBRE_2 { get; set; }
        public string APELLIDO_P { get; set; }
        public string APELLIDO_M { get; set; }
        public string CALLE { get; set; }
        public string CALLE_PPAL { get; set; }
        public string REFERENCIA1 { get; set; }
        public string REFERENCIA2 { get; set; }
        public string NO_EXTERIOR { get; set; }
        public string NO_INTERIOR { get; set; }
        public int CVE_COLONIA { get; set; }
        public string CODIGO_POSTAL { get; set; }
        public string TELEFONO { get; set; }
        public string TIPO_CONTRIBUYENTE { get; set; }
        public string E_MAIL { get; set; }
        public string TELEFONO_MOVIL { get; set; }
        public string PAIS_ORIGEN { get; set; }
        public string PAIS_RESIDENCIA { get; set; }
        public string PENSIONADO { get; set; }
        public string CVE_EDO { get; set; }
        public string CVE_MPIO { get; set; }
        public string CVE_LOC { get; set; }
        public string CVE_REC { get; set; }
        public int CREADO_POR { get; set; }
        public DateTime FECHA_CREACION { get; set; }
        public int MODIFICADO_POR { get; set; }
        public DateTime FECHA_MODIFICACION { get; set; }
        public DateTime FECHA_NACIM { get; set; }
        public string NOMBRE_DEPURADO { get; set; }
    }
    public class Versionamiento
    {
        public Models.VERSIONAMIENTO VersionActual()
        {
            Models.EntitieLocal db = new Models.EntitieLocal();
            var versionActual = db.VERSIONAMIENTO.Where(r => r.ES_ACTIVO == 1).OrderByDescending(v => v.FECHA_CREACION).FirstOrDefault();
            return versionActual;
        }
    }

}
