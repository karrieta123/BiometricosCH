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
    
    public partial class IDENTIFICA
    {
        public decimal IDENTIFICA_ID { get; set; }
        public byte[] FOTO { get; set; }
        public byte[] HUELLA { get; set; }
        public byte[] HUELLA2 { get; set; }
        public byte[] FIRMA { get; set; }
        public decimal TRAMITE_ID { get; set; }
    
        public virtual TRAMITE TRAMITE { get; set; }
    }
}
