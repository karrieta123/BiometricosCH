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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities1 : DbContext
    {
        public Entities1()
            : base("name=Entities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<BIOMETRICOS> BIOMETRICOS { get; set; }
        public DbSet<CIUDADANO> CIUDADANO { get; set; }
        public DbSet<COLONIAS> COLONIAS { get; set; }
        public DbSet<DATOS_CONTRIBUYENTE> DATOS_CONTRIBUYENTE { get; set; }
        public DbSet<DOCUMENTOS> DOCUMENTOS { get; set; }
        public DbSet<ENTIDADES> ENTIDADES { get; set; }
        public DbSet<IDENTIFICA> IDENTIFICA { get; set; }
        public DbSet<IMGBIOMETRICOS> IMGBIOMETRICOS { get; set; }
        public DbSet<NACIONALIDAD> NACIONALIDAD { get; set; }
        public DbSet<NUMERO_FOLIO> NUMERO_FOLIO { get; set; }
        public DbSet<NUMERO_LICENCIA> NUMERO_LICENCIA { get; set; }
        public DbSet<SCANNER> SCANNER { get; set; }
        public DbSet<SCANNER_PROPERTIES> SCANNER_PROPERTIES { get; set; }
        public DbSet<TIPO_CABELLO> TIPO_CABELLO { get; set; }
        public DbSet<TIPO_LICENCIAS> TIPO_LICENCIAS { get; set; }
        public DbSet<TIPO_TRAMITE> TIPO_TRAMITE { get; set; }
        public DbSet<TIPOS_SANGRE> TIPOS_SANGRE { get; set; }
        public DbSet<TRAMITE> TRAMITE { get; set; }
        public DbSet<TRAMITES> TRAMITES { get; set; }
    }
}
