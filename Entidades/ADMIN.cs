namespace Entidades
{
    public class ADMIN
    {

    }
    public class Recaudaroda
    {
        public string CVE_REC { get; set; }
        public string DESCRIPCION { get; set; }

    }

    public class DGRecaudadora
    {
        public string CVE_REC { get; set; }
        public int Folio_Inicio { get; set; }
        public int Folio_Fin { get; set; }
        public int Cantidad { get; set; }
        public int Folio_Actual { get; set; }
        public string Activo { get; set; }
        public int Folio_Libres { get; set; }
    }
}
