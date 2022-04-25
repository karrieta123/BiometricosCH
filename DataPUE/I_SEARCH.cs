using System.Collections.Generic;
using System.Linq;

namespace DataPUE
{
    public class I_SEARCH
    {
        private Models.EntitieLocal local = new Models.EntitieLocal();
        public List<Colonia> Colonias(string Municipio, string Localidad)
        {
            //Models.EntitieLocal local = new Models.EntitieLocal();
            //var res = (from r in local.Database.SqlQuery(,);
            string QUERY = "SELECT CVE_COL, Descripcion,CVE_MPIO FROM ING_COLONIAS WHERE CVE_EDO='03' AND CVE_MPIO='" + Municipio + "' ORDER BY DESCRIPCION";
            var resa = local.Database.SqlQuery<Colonia>(QUERY).ToList<Colonia>();
            return resa;
        }

        public List<Colonia> SearchForColonia(string Municipio, string Localidad, string word)
        {
            string QUERY = "SELECT CVE_COL, Descripcion,CODIGO_POSTAL FROM ING_COLONIAS WHERE CVE_EDO='03' AND CVE_MPIO='" + Municipio + "' AND DESCRIPCION LIKE '%" + word.ToUpper() + "%' ORDER BY DESCRIPCION";
            var resa = local.Database.SqlQuery<Colonia>(QUERY).ToList<Colonia>();
            return resa;
        }
    }
}
