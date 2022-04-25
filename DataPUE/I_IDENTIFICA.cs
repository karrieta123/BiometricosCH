using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace DataPUE
{
    public class I_IDENTIFICA
    {
        /// <summary>
        /// Método público para generar un registro de Identifica
        /// </summary>
        /// <param name="foto">Fotografia de la persona</param>
        /// <param name="huella">Huella de la persona</param>
        /// <param name="huella2">Huella de la persona</param>
        /// <param name="firma">Firma de la persona</param>
        /// <param name="tramiteId">Identificador del nuevo registro</param>
        public static int agregarRegistroIdentifica(byte[] foto, byte[] huella, byte[] firma, int tramiteId, byte[] huella2 = null)
        {
            try
            {
                bool existe = true;
                Models.EntitieLocal db = new Models.EntitieLocal();
                //Validando existencia del tramite
                Models.TRAMITE tramite = db.TRAMITE.Find(tramiteId);
                //Si no existe el tramite
                if (tramite == null)
                    throw new Exception("El tramite especificado no existe");
                Models.IDENTIFICA identifica = db.IDENTIFICA.Where(x => x.TRAMITE_ID == tramiteId).FirstOrDefault();
                //Si aún no existe el registro
                if (identifica == null)
                {
                    identifica = new Models.IDENTIFICA();
                    identifica.TRAMITE_ID = tramiteId;
                    existe = false;
                }
                //Guardando valores
                identifica.FOTO = foto;
                identifica.HUELLA = huella;
                identifica.HUELLA2 = huella2;
                identifica.FIRMA = firma;
                if (!existe)
                    db.IDENTIFICA.Add(identifica);
                //Guardando cambios
                db.SaveChanges();
                //Obteniendo Id
                decimal id = buscarRegistroIdentificaPorCurp(tramite.CURP).IDENTIFICA_ID;
                //Regresando identificador del registro
                return Convert.ToInt32(id);
            }
            catch (DbEntityValidationException dbEx)
            {
                string listadoErrores = "Se generaron los siguientes errores al guardar los cambios: \n";
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        listadoErrores += validationError.ErrorMessage + "\n";
                    }
                }
                throw new Exception(listadoErrores);
            }
        }

        /// <summary>
        /// Método público para modificar un registro de Identifica
        /// </summary>
        /// <param name="identificaId">Identificador del registro</param>
        /// <param name="foto">Fotografia de la persona</param>
        /// <param name="huella">Huella de la persona</param>
        /// <param name="huella2">Huella de la persona</param>
        /// <param name="firma">Firma de la persona</param>
        /// <param name="tramiteId">Identificador del registro</param>
        public static int modificarRegistroIdentifica(int identificaId, byte[] foto, byte[] huella, byte[] firma, int tramiteId, byte[] huella2 = null)
        {
            try
            {
                Models.EntitieLocal db = new Models.EntitieLocal();

                Models.IDENTIFICA identifica = db.IDENTIFICA.Find(identificaId);
                //Si aún no existe el registro
                if (identifica == null)
                    throw new Exception("El registro especificado no existe");
                //Guardando valores
                identifica.FOTO = foto;
                identifica.HUELLA = huella;
                identifica.HUELLA2 = huella2;
                identifica.FIRMA = firma;
                //Guardando cambios
                db.SaveChanges();
                //Regresando identificador del registro
                return Convert.ToInt32(identifica.IDENTIFICA_ID);
            }
            catch (DbEntityValidationException dbEx)
            {
                string listadoErrores = "Se generaron los siguientes errores al guardar los cambios: \n";
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        listadoErrores += validationError.ErrorMessage + "\n";
                    }
                }
                throw new Exception(listadoErrores);
            }
        }

        /// <summary>
        /// Método público para obtener el último registro de Identifica en base al CURP
        /// </summary>
        /// <param name="curp">CURP</param>
        /// <returns>Objeto de tipo Identifica, null si no lo encuentra</returns>
        public static Models.IDENTIFICA buscarRegistroIdentificaPorCurp(string curp)
        {
            Models.EntitieLocal db = new Models.EntitieLocal();
            var identifica = db.IDENTIFICA.Where(x => x.TRAMITE.CURP == curp).OrderByDescending(x => x.IDENTIFICA_ID).FirstOrDefault();
            return identifica == null ? null : identifica;
        }

        /// <summary>
        /// Método público para obtener el último registro de Identifica en base al RFC
        /// </summary>
        /// <param name="rfc">RFC</param>
        /// <returns>Objeto de tipo Identifica, null si no lo encuentra</returns>
        public static Models.IDENTIFICA buscarRegistroIdentificaPorRFC(string rfc)
        {
            Models.EntitieLocal db = new Models.EntitieLocal();
            var identifica = db.IDENTIFICA.Where(x => x.TRAMITE.RFC == rfc).OrderByDescending(x => x.IDENTIFICA_ID).FirstOrDefault();
            return identifica == null ? null : identifica;
        }

        /// <summary>
        /// Método público para obtener el registro de Identifica en base al Id
        /// </summary>
        /// <param name="identificaId">Identificador del registro a buscar</param>
        /// <returns>Objeto de tipo Identifica, null si no lo encuentra</returns>
        public static Models.IDENTIFICA obtenerIdentifica(int identificaId)
        {
            Models.EntitieLocal db = new Models.EntitieLocal();
            var identifica = db.IDENTIFICA.Find(identificaId);
            return identifica == null ? null : identifica;
        }

        /// <summary>
        /// Método público para obtener el registro de Identifica en base al número de tramite
        /// </summary>
        /// <param name="tramite">Número de tramite</param>
        /// <returns></returns>
        public static List<Models.IMGBIOMETRICOS> obtenerIdentificaPorTramite(int tramite)
        {
            Models.EntitieLocal db = new Models.EntitieLocal();
            List<Models.IMGBIOMETRICOS> identifica = db.IMGBIOMETRICOS.Where(x => x.ID_TRAMITES == tramite).ToList();
            return identifica;
        }

        /// <summary>
        /// Método público para obtener el registro de Tramite de un Registro Identifica
        /// </summary>
        /// <param name="identificaId">Identificador del registro a buscar</param>
        /// <returns>Objeto de tipo Tramite, null si no lo encuentra</returns>
        public static Models.TRAMITE obtenerTramite(int identificaId)
        {
            Models.EntitieLocal db = new Models.EntitieLocal();
            var identifica = db.IDENTIFICA.Find(identificaId);
            return identifica == null ? null : identifica.TRAMITE;
        }

        /// <summary>
        /// Método público para obtener el registro de DatosContribuyente de un Registro Identifica
        /// </summary>
        /// <param name="identificaId">Identificador del registro a buscar</param>
        /// <returns>Objeto de tipo DATOS_CONTRIBUYENTE, null si no lo encuentra</returns>
        ////public static Models.DATOS_CONTRIBUYENTE obtenerDatosContribuyente(int identificaId)
        //{
        //    Models.EntitieLocal db = new Models.EntitieLocal();
        //    var identifica = db.IDENTIFICA.Find(identificaId);
        //    return identifica == null ? null : identifica.TRAMITE.DATOS_CONTRIBUYENTE;
        //}

        /// <summary>
        /// Método público para obtener el registro de Aviso Entero de un Registro Identifica
        /// </summary>
        /// <param name="identificaId">Identificador del registro a buscar</param>
        /// <returns>Objeto de tipo Aviso Entero, null si no lo encuentra</returns>
        //public static Models.CIUDADANO obtenerAvisoEntero(int identificaId)
        //{
        //    Models.EntitieLocal db = new Models.EntitieLocal();

        //    var g = (from a in db.IDENTIFICA
        //                 where a.IDENTIFICA_ID==0
        //                 select a);

        //    var identifica = db.IDENTIFICA.Find(identificaId);
        //    return identifica == null ? null : db.CIUDADANO;
        //}
    }
}
