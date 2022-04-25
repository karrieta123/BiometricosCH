namespace Entidades
{
    public class Scanner
    {
        public decimal dpi { get; set; }
        public string color { get; set; }
        public decimal sides { get; set; }
    }
    public class ImageDocs
    {
        public int id { get; set; }
        public System.Drawing.Image imagen { get; set; }
    }

    public class EntiDocumentos
    {
        public int ID_DOCUMENTO { get; set; }
        public string NOMBRE { get; set; }
    }
    public class EntiDocumentosR
    {
        public int id { get; set; }
        public string nombre { get; set; }
    }

}
