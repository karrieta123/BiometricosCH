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
    
    public partial class DEDOS_AMP
    {
        public decimal ID_DEDOS_AMP { get; set; }
        public decimal ID_TRAMITE { get; set; }
        public decimal ID_DEDO_CAUSA { get; set; }
    
        public virtual DEDOS_CAUSA DEDOS_CAUSA { get; set; }
        public virtual TRAMITES TRAMITES { get; set; }
    }
}
