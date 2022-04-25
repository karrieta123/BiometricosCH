using DataPUE.Models;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;


namespace DataPUE
{
    public class I_BIOMETRICOS
    {

        public void SETBiometricos(List<EntiBiometricos> bio, int idtramite)
        {
            //InsertBiometricos(bio, idtramite);
        }

        private void InsertBiometricos(List<EntiBiometricos> bio, int id_tramite)
        {

            //I_SCANNER scanner = new I_SCANNER();//mandamos a llamar a la clase para ocupar el metodo para hacer la imagen en bytes
            EntitieLocal loc = new EntitieLocal();

            IMGBIOMETRICOS Biometricos = loc.IMGBIOMETRICOS.Where(w => w.ID_TRAMITES == id_tramite).FirstOrDefault();
            if (Biometricos == null)
            {
                foreach (var item in bio)
                {
                    EntitieLocal local = new EntitieLocal();
                    IMGBIOMETRICOS imgb = new IMGBIOMETRICOS()
                    {
                        ID_BIOMETRICOS = item.id,
                        IMG = item.imagen,
                        ID_TRAMITES = id_tramite
                    };
                    local.IMGBIOMETRICOS.Add(imgb);
                    local.SaveChanges();
                }
            }
            else
            {
                using (EntitieLocal BioLocal = new EntitieLocal())
                {
                    var bios = BioLocal.IMGBIOMETRICOS.Where(w => w.ID_TRAMITES == id_tramite);
                    foreach (var item in bios)
                    {
                        var delete = BioLocal.IMGBIOMETRICOS.Where(w => w.ID_TRAMITES == id_tramite && w.ID_IMGBIOMETRICOS == item.ID_IMGBIOMETRICOS).First();
                        BioLocal.IMGBIOMETRICOS.Attach(delete);
                        BioLocal.IMGBIOMETRICOS.Remove(delete);
                        BioLocal.SaveChanges();
                    }
                }
                foreach (var item in bio)
                {
                    EntitieLocal local = new EntitieLocal();
                    IMGBIOMETRICOS imgb = new IMGBIOMETRICOS()
                    {
                        ID_BIOMETRICOS = item.id,
                        IMG = item.imagen,
                        ID_TRAMITES = id_tramite
                    };
                    local.IMGBIOMETRICOS.Add(imgb);
                    local.SaveChanges();
                }
            }
        }


        #region Metodo para poder meter los datos para el registro de dedos que no se encuentran
        //public void SetFingerless(int Id_soli, JsonFinger jsonSettings)
        //{
        //   Fingerless(Id_soli, jsonSettings);
        //}

        public void SetFingerless(int Id_soli, string jsonSettings)
        {
            Fingerless(Id_soli, jsonSettings);
        }


        public void Fingerless(int Id_soli, string jsonSettings)
        {
            int dd = Convert.ToInt32(jsonSettings);
            EntitieLocal local = new EntitieLocal();
            if (local.DEDOS_AMP.Any(w => w.ID_DEDO_CAUSA == dd && w.ID_TRAMITE == Id_soli))
            {
                var res = local.DEDOS_AMP.Where(w => w.ID_DEDO_CAUSA == dd && w.ID_TRAMITE == Id_soli).First();
                res.ID_DEDO_CAUSA = dd;
                local.DEDOS_AMP.Add(res);
                local.SaveChanges();
            }
            else
            {
                DEDOS_AMP de = new DEDOS_AMP()
                {
                    ID_DEDO_CAUSA = Convert.ToInt32(jsonSettings),
                    ID_TRAMITE = Id_soli
                };
                local.DEDOS_AMP.Add(de);
                local.SaveChanges();
            }



        }

        //public void Fingerless(int Id_soli, JsonFinger jsonSettings)
        //{

        //} 
        #endregion
    }
}
