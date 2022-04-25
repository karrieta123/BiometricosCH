using DataPUE.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace DataPUE
{
    public class I_ENTIDADES
    {
        /// <summary>
        /// Método público para agregar una Entidad
        /// </summary>
        /// <param name="clave">Clave para la entidad</param>
        /// <param name="descripcion">Descripción</param>
        /// <returns>Identificador del registro (Id)</returns>
        public static int agregarEntidad(int clave, string descripcion)
        {
            try
            {
                Models.EntitieLocal db = new Models.EntitieLocal();
                //Si ya existe una entidad con la misma clave
                if (db.ENTIDADES.Where(x => x.CLAVE == clave).FirstOrDefault() != null)
                {
                    throw new Exception("Ya existe una entidad con la misma clave especificada");
                }
                //Generando nuevo registro
                ENTIDADES entidad = new ENTIDADES()
                {
                    CLAVE = clave,
                    ENTIDAD = descripcion
                };
                //Agregando registro
                db.ENTIDADES.Add(entidad);
                //Guardando cambios
                db.SaveChanges();
                //ObteniendoId
                int entidadId = Convert.ToInt32(buscarEntidadPorClave(clave).ENTIDAD_ID);

                return entidadId;
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
        /// Método público para modificar una Entidad
        /// </summary>
        /// <param name="idEntidad">Identificador del registro a editar</param>
        /// <param name="clave">Clave para la entidad</param>
        /// <param name="descripcion">Descripción</param>
        /// <returns>Identificador del registro (Id)</returns>
        public static int modificarEntidad(int idEntidad, int clave, string descripcion)
        {
            try
            {
                Models.EntitieLocal db = new Models.EntitieLocal();

                ENTIDADES entidad = db.ENTIDADES.Find(idEntidad);

                //Si no existe una entidad con el id especificado
                if (entidad != null)
                {
                    throw new Exception("No existe una entidad con el id especificado");
                }
                //Si ya existe una entidad con la clave especificada
                if (db.ENTIDADES.Where(x => x.CLAVE == clave && x.ENTIDAD_ID != idEntidad).FirstOrDefault() != null)
                {
                    throw new Exception("Ya existe una entidad con la misma clave especificada");
                }
                //Modificando valores
                entidad.CLAVE = clave;
                entidad.ENTIDAD = descripcion;
                //Guardando cambios
                db.SaveChanges();

                return Convert.ToInt32(entidad.ENTIDAD_ID);
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
        /// Método público obtener el listado de Entidades
        /// </summary>
        /// <returns>Listado de tipo ENTIDADES</returns>
        public static List<ENTIDADES> obtenerEntidades()
        {
            Models.EntitieLocal db = new Models.EntitieLocal();
            //Realizando búsqueda
            return db.ENTIDADES.ToList();
        }

        /// <summary>
        /// Método público realizar la búsqueda de entidad en base al Id
        /// </summary>
        /// <param name="idEntidad">Identificador del registro</param>
        /// <returns>Registro de tipo ENTIDADES</returns>
        public static ENTIDADES buscarEntidadPorId(int idEntidad)
        {
            Models.EntitieLocal db = new Models.EntitieLocal();
            //Realizando búsqueda
            ENTIDADES entidad = db.ENTIDADES.Find(idEntidad);

            //Si no existe una entidad con el id especificado
            if (entidad == null)
            {
                throw new Exception("No existe una entidad con el Id Especificado");
            }

            return entidad;
        }

        /// <summary>
        /// Método público realizar la búsqueda de entidad en base a la clave
        /// </summary>
        /// <param name="clave">Clave para la entidad</param>
        /// <returns>Registro de tipo ENTIDADES</returns>
        public static ENTIDADES buscarEntidadPorClave(int clave)
        {
            Models.EntitieLocal db = new Models.EntitieLocal();
            //Realizando búsqueda
            ENTIDADES entidad = db.ENTIDADES.Where(x => x.CLAVE == clave).FirstOrDefault();

            //Si no existe una entidad con el id especificado
            if (entidad != null)
            {
                throw new Exception("No existe una entidad con la clave especificada");
            }

            return entidad;
        }

        /// <summary>
        /// Método público para obtener el listado de municipios para una entidad en base a la Clave
        /// </summary>
        /// <param name="clave">Clave de la entidad</param>
        /// <returns>Listado de Municipios</returns>
        //public static List<MUNICIPIO> obtenerMunicipiosEntidadPorClave(int clave)
        //{
        //    Models.EntitiesBCS db = new Models.EntitiesBCS();
        //    //Realizando búsqueda
        //    return db.MUNICIPIO.Where(x => x.ENTIDADES.CLAVE == clave).ToList();
        //}

        ///// <summary>
        ///// Método público para obtener el listado de municipios para una entidad en base al EntidadId
        ///// </summary>
        ///// <param name="idEntidad">Identificador de la entidad</param>
        ///// <returns>Listado de Municipios</returns>
        //public static List<MUNICIPIO> obtenerMunicipiosEntidadPorId(int idEntidad)
        //{
        //    Models.EntitiesBCS db = new Models.EntitiesBCS();
        //    //Realizando búsqueda
        //    return db.MUNICIPIO.Where(x => x.ENTIDADES.ENTIDAD_ID == idEntidad).ToList();
        //}
    }
}
