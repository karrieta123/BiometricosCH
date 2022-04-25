using System.Collections.Generic;

namespace PUELicencia_Huella
{
    public class Datos
    {
        public List<string> DatosCausas()
        {
            return GetCausas();
        }

        private List<string> GetCausas()
        {
            List<string> c = new List<string>();
            c.Add("Seleccione una opción.");
            c.Add("Vendado.");
            c.Add("Amputado.");
            c.Add("No información.");
            return c;
        }
    }

}
