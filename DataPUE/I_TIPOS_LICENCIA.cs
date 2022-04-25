using DataPUE.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace DataPUE
{
    public class I_TIPOS_LICENCIA
    {
        /// <summary>
        /// Método público para agregar un Tipo de Licencia
        /// </summary>
        /// <param name="descripcion">Descripción</param>
        /// <param name="inciso">Inciso</param>
        /// <param name="subinciso">SubInciso</param>
        /// <param name="nivelSubinciso">Nivel de SubInciso</param>
        /// <returns>Identificador del registro (Id)</returns>
        public static int agregarTipoLicencia(string descripcion, string inciso, string subinciso, string nivelSubInciso)
        {
            try
            {
                Models.EntitieLocal db = new Models.EntitieLocal();
                //Si ya existe una Tipo de Licencia con los mismos valores
                // if (db.TIPO_LICENCIAS.Where(x => x.INCISO == inciso && x.SUBINCISO == subinciso && x.NIVEL_SUBINCISO == nivelSubInciso).FirstOrDefault() != null)
                if (false)
                {
                    throw new Exception("Ya existe un Tipo de Licencia con el mismo inciso, subinciso y nivel subinciso especificados");
                }
                //Generando nuevo registro
                TIPO_LICENCIAS tipoLicencia = new TIPO_LICENCIAS()
                {
                    DESCRIPCION = descripcion,
                    //INCISO = inciso,
                    //SUBINCISO = subinciso,
                    //NIVEL_SUBINCISO = nivelSubInciso
                };
                //Agregando registro
                db.TIPO_LICENCIAS.Add(tipoLicencia);
                //Guardando cambios
                db.SaveChanges();
                //ObteniendoId
                int tipoLicenciaId = Convert.ToInt32(buscarTipoLicenciaPorInciso(inciso, subinciso, nivelSubInciso).ID);

                return tipoLicenciaId;
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
        /// Método público para modificar un Tipo de Licencia
        /// </summary>
        /// <param name="idTipoLicencia">Identificador del registro</param>
        /// <param name="descripcion">Descripción</param>
        /// <param name="inciso">Inciso</param>
        /// <param name="subinciso">SubInciso</param>
        /// <param name="nivelSubinciso">Nivel de SubInciso</param>
        /// <returns>Identificador del registro (Id)</returns>
        public static int modificarTipoLicencia(int idTipoLicencia, string descripcion, string inciso, string subinciso, string nivelSubInciso)
        {
            try
            {
                Models.EntitieLocal db = new Models.EntitieLocal();

                TIPO_LICENCIAS tipoLicencia = db.TIPO_LICENCIAS.Find(idTipoLicencia);

                //Si no existe un Tipo de Licencia con el id especificado
                if (tipoLicencia != null)
                {
                    throw new Exception("No existe un Tipo de Licencia con el id especificado");
                }
                //Si ya existe un tipo de licencia con los incisos especificados
                // if (db.TIPO_LICENCIAS.Where(x => x.INCISO == inciso && x.SUBINCISO == subinciso && x.NIVEL_SUBINCISO == nivelSubInciso && x.TIPO_LICENCIA_ID != idTipoLicencia).FirstOrDefault() != null)
                if (false)
                {
                    throw new Exception("Ya existe un Tipo de Licencia con los mismos valores especificados");
                }
                //Modificando valores
                tipoLicencia.DESCRIPCION = descripcion;
                //tipoLicencia.INCISO = inciso;
                //tipoLicencia.NIVEL_SUBINCISO = nivelSubInciso;
                //tipoLicencia.SUBINCISO = subinciso;

                //Guardando cambios
                db.SaveChanges();

                return Convert.ToInt32(tipoLicencia.ID);
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
        /// Método público obtener el listado de los tipos de Licencias
        /// </summary>
        /// <returns>Listado de tipo TIPOS_LICENCIAS</returns>
        public static List<Combos> obtenerTiposLicencias()
        {
            Models.EntitieLocal db = new Models.EntitieLocal();
            //Realizando búsqueda
            return db.TIPO_LICENCIAS.ToList().Select(x => new Combos() { Identificador = x.ID, Descripcion = x.DESCRIPCION }).ToList();
        }

        /// <summary>
        /// Método público realizar la búsqueda de un Tipo de Licencia en base al Id
        /// </summary>
        /// <param name="idTipoLicencia">Identificador del registro</param>
        /// <returns>Registro de tipo TIPOS_LICENCIAS</returns>
        public static TIPO_LICENCIAS buscarTipoLicenciaPorId(int idTipoLicencia)
        {
            Models.EntitieLocal db = new Models.EntitieLocal();
            //Realizando búsqueda
            TIPO_LICENCIAS tipoLicencia = db.TIPO_LICENCIAS.Find(idTipoLicencia);

            //Si no existe un Tipo de Licencia con el id especificado
            if (idTipoLicencia != null)
            {
                throw new Exception("No existe un Tipo de Licencia con el Id Especificado");
            }

            return tipoLicencia;
        }

        /// <summary>
        /// Método público realizar la búsqueda de un Tipo de Licencia en base al Inciso, SubInciso y Nivel de SubInciso
        /// </summary>
        /// <param name="inciso">Inciso</param>
        /// <param name="subInciso">SubInciso</param>
        /// <param name="nivelSubInciso">Nivel de SubInciso</param>
        /// <returns>Registro de tipo TIPOS_LICENCIAS</returns>
        public static TIPO_LICENCIAS buscarTipoLicenciaPorInciso(string inciso, string subInciso, string nivelSubInciso)
        {
            Models.EntitieLocal db = new Models.EntitieLocal();
            //Realizando búsqueda
            TIPO_LICENCIAS tipoLicencia = new TIPO_LICENCIAS(); //db.TIPO_LICENCIAS.Where(x => x.INCISO == inciso && x.SUBINCISO == subInciso && x.NIVEL_SUBINCISO == nivelSubInciso).FirstOrDefault();

            //Si no existe un Tipo de Licencia con el id especificado
            if (tipoLicencia != null)
            {
                throw new Exception("No existe un Tipo de Licencia con el Id Especificado");
            }

            return tipoLicencia;
        }
    }


    public class Combos
    {
        public decimal Identificador { get; set; }
        public String Descripcion { get; set; }
    }
}
