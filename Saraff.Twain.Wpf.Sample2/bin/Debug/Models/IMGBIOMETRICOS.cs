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
    
    public partial class IMGBIOMETRICOS
    {
        public decimal ID_IMGBIOMETRICOS { get; set; }
        public byte[] IMG { get; set; }
        public decimal ID_TRAMITES { get; set; }
        public decimal ID_BIOMETRICOS { get; set; }
    
        public virtual BIOMETRICOS BIOMETRICOS { get; set; }
        public virtual TRAMITES TRAMITES { get; set; }
    }
}