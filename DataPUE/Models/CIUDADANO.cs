//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataPUE.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CIUDADANO
    {
        public CIUDADANO()
        {
            this.TRAMITES = new HashSet<TRAMITES>();
        }
    
        public decimal ID { get; set; }
        public string NOMBRE { get; set; }
        public string APELLIDO_PATERNO { get; set; }
        public string APELLIDO_MATERNO { get; set; }
        public System.DateTime FECHA_NACIMIENTO { get; set; }
        public string SEXO { get; set; }
        public string EMAIL { get; set; }
        public string ALERGIAS { get; set; }
        public string SENAS_PARTICULARES { get; set; }
        public string PROFESION { get; set; }
        public Nullable<decimal> CABELLO { get; set; }
        public Nullable<decimal> TIPO_SANGRE { get; set; }
        public string ALTURA { get; set; }
        public string USA_LENTES { get; set; }
        public string DONADOR_ORGANOS { get; set; }
        public Nullable<decimal> MUNICIPIO { get; set; }
        public Nullable<decimal> COLONIA { get; set; }
        public string NUM_EXT { get; set; }
        public string NUM_INT { get; set; }
        public Nullable<decimal> CODIGO_POSTAL { get; set; }
        public string DIRECCION { get; set; }
        public string ENTRE_CALLE1 { get; set; }
        public string ENTRE_CALLE2 { get; set; }
        public string TELEFONO { get; set; }
        public string CURP { get; set; }
        public string LOCALIDAD { get; set; }
        public string NACIONALIDAD { get; set; }

        public virtual ICollection<TRAMITES> TRAMITES { get; set; }
    }
}
