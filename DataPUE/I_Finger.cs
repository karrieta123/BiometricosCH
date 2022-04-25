using DataPUE.Models;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataPUE
{
    public class I_FINGER
    {
        public List<EntiFinger> DatosCausas()
        {
            return GetCausas();
        }

        private List<EntiFinger> GetCausas()
        {
            EntitieLocal local = new EntitieLocal();
            List<EntiFinger> causas = null;
            try
            {
                causas = new List<EntiFinger>();
                //causas = local.DEDOS_CAUSA.Select(s => new EntiFinger { ID_DEDOS_CAUSA = s.ID_DEDOS_CAUSA, NOMBRE = s.NOMBRE }).ToList();
                //causas.Add(new EntiFinger { ID_DEDOS_CAUSA = 0, NOMBRE = "1.Seleccione una opción." });
            }
            catch (Exception ex)
            {

                throw;
            }

            return causas.OrderBy(o => o.NOMBRE).ToList();
        }
    }
}
