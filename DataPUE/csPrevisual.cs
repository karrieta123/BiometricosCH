using System.Runtime.Serialization;

namespace DataPUE
{
    [DataContract]
    public class csPrevisual
    {
        [DataMember]
        public string NOMBRE { get; set; }
        [DataMember]
        public string APELLIDO { get; set; }
        [DataMember]
        public string DIRECCION { get; set; }
        [DataMember]
        public string CURP { get; set; }
        [DataMember]
        public string FECHA_EXPEDICION { get; set; }
        [DataMember]
        public string FECHA_ANTIGUEDAD { get; set; }
        [DataMember]
        public string FECHA_VENCIMIENTO { get; set; }
        [DataMember]
        public string ANIOS_VIGENCIA { get; set; }
        [DataMember]
        public string TIPO_LICENCIA { get; set; }
        [DataMember]
        public string LicenciaLetra { get; set; }
        [DataMember]
        public string SEXO { get; set; }
        [DataMember]
        public string TELEFONO { get; set; }
        [DataMember]
        public string CABELLO { get; set; }
        [DataMember]
        public string ESTATURA { get; set; }
        [DataMember]
        public string FANTIGUEDAD { get; set; }
        [DataMember]
        public string FECHA_NACIMIENTO { get; set; }
        [DataMember]
        public string TIPO_SANGRE { get; set; }
        [DataMember]
        public string ALERGIAS { get; set; }
        [DataMember]
        public string SENAS_PARTICULARES { get; set; }
        [DataMember]
        public string DONADOR_ORGANOS { get; set; }
        [DataMember]
        public string IDfOLIO { get; set; }
        [DataMember]
        public string IMPORTE { get; set; }
        [DataMember]
        public string RFC { get; set; }
        [DataMember]
        public string calle { get; set; }
        [DataMember]
        public string numero { get; set; }
        [DataMember]
        public string interior { get; set; }
        [DataMember]
        public string colonia { get; set; }
        [DataMember]
        public string municipio { get; set; }
        [DataMember]
        public string estado { get; set; }
        [DataMember]
        public string codigopost { get; set; }
        [DataMember]
        public string numeroLicencia { get; set; }
        [DataMember]
        public string profesion { get; set; }
        [DataMember]
        public string nacion { get; set; }
        [DataMember]
        public string concepto { get; set; }
        [DataMember]
        public string inciso { get; set; }
        [DataMember]
        public string quiencaptura { get; set; }
        [DataMember]
        public byte[] FOTO { get; set; }
        [DataMember]
        public byte[] HUELLA { get; set; }
        [DataMember]
        public byte[] FIRMA { get; set; }
        [DataMember]
        public string CINTILLO { get; set; }
        [DataMember]
        public byte[] BARCODE { get; set; }
        [DataMember]
        public byte[] QR { get; set; }
        [DataMember]
        public byte[] FIRMA_DIRECTOR { get; set; }
        [DataMember]
        public byte[] cintillo { get; set; }
        [DataMember]
        public string nombreSecre { get; set; }
        [DataMember]
        public string ExpedienteNum { get; set; }
        [DataMember]
        public string Localidad { get; set; }
        [DataMember]
        public string NACIONALIDAD { get; set; }
        [DataMember]
        public bool Show_Domicilio { get; set; }
        [DataMember]
        public bool SIN_RESTRICCIONES { get; set; }
        [DataMember]
        public string LENTES { get; set; }
        [DataMember]
        public string PROTESIS { get; set; }
        [DataMember]
        public string AUDITIVO { get; set; }
        [DataMember]
        public string LENTES_CONTACTO { get; set; }
        [DataMember]
        public string VEHICULO_ADAPTADO { get; set; }
        [DataMember]
        public string VEHICULO_AUTOMATICO { get; set; }
        [DataMember]
        public string seguro { get; set; }
        [DataMember]
        public string VAN { get; set; }
        [DataMember]
        public string BUS { get; set; }
        [DataMember]
        public string TAXI { get; set; }
        public string APATERNO { get; set; }
        public string AMATERNO { get; set; }
        public string DESTINO { get; set; }
    }


    [DataContract]
    public class ResumenFolio
    {
        [DataMember]
        public string RECAUDADORA { get; set; }
        [DataMember]
        public string FECHA_CREACION { get; set; }
        [DataMember]
        public string NO_AVISO { get; set; }
        [DataMember]
        public string NO_LASER { get; set; }
        [DataMember]
        public string NO_LICENCIA { get; set; }
        [DataMember]
        public string USUARIO { get; set; }

        [DataMember]
        public string TOTAL_FOLIO { get; set; }
    }
}
