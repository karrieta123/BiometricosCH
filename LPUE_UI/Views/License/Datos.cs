using DataPUE;
using Entidades;
using System.Collections.Generic;

namespace Finger
{
    public class Datos
    {
        public List<EntiFinger> FillCausa()
        {
            I_FINGER finger = new I_FINGER();
            return finger.DatosCausas();
        }
    }
}
