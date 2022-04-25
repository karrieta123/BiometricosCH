using DataPUE.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace DataPUE
{
    public class I_NACIONALIDAD
    {
        /// <summary>
        /// Método público para agregar una Nacionalidad
        /// </summary>
        /// <param name="pais">Pais</param>
        /// <param name="descripcion">Descripción</param>
        /// <returns>Identificador del registro (Id)</returns>
        public static int agregarNacionalidad(string pais, string descripcion)
        {
            try
            {
                Models.EntitieLocal db = new Models.EntitieLocal();
                //Si ya existe una Nacionalidad con los mismos valores
                if (db.NACIONALIDAD.Where(x => x.PAIS == pais && x.DESCRIPCION == descripcion).FirstOrDefault() != null)
                {
                    throw new Exception("Ya existe una Nacionalidad con los mismos valores");
                }
                //Generando nuevo registro
                NACIONALIDAD entidad = new NACIONALIDAD()
                {
                    PAIS = pais,
                    DESCRIPCION = descripcion
                };
                //Agregando registro
                db.NACIONALIDAD.Add(entidad);
                //Guardando cambios
                db.SaveChanges();
                //ObteniendoId
                int nacionalidadId = Convert.ToInt32(obtenerNacionalidadPorValores(pais, descripcion).NACIONALIDAD_ID);

                return nacionalidadId;
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
        /// Método público para modificar una Nacionalidad
        /// </summary>
        /// <param name="idNacionalidad">Identificador del registro a editar</param>
        /// <param name="pais">Pais</param>
        /// <param name="descripcion">Descripción</param>
        /// <returns>Identificador del registro (Id)</returns>
        public static int modificarNacionalidad(int idNacionalidad, string pais, string descripcion)
        {
            try
            {
                Models.EntitieLocal db = new Models.EntitieLocal();

                NACIONALIDAD nacionalidad = db.NACIONALIDAD.Find(idNacionalidad);

                //Si no existe una Nacionalidad con el id especificado
                if (nacionalidad != null)
                {
                    throw new Exception("No existe una nacionalidad con el id especificado");
                }
                //Si ya existe una entidad con la clave especificada
                if (db.NACIONALIDAD.Where(x => x.PAIS == pais && x.DESCRIPCION == descripcion && x.NACIONALIDAD_ID != idNacionalidad).FirstOrDefault() != null)
                {
                    throw new Exception("Ya existe una Nacionalidad con los mismos valores especificados");
                }
                //Modificando valores
                nacionalidad.PAIS = pais;
                nacionalidad.DESCRIPCION = descripcion;
                //Guardando cambios
                db.SaveChanges();

                return Convert.ToInt32(nacionalidad.NACIONALIDAD_ID);
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
        /// Método público obtener el listado de todos las Nacionalidades
        /// </summary>
        /// <returns>Listado de tipo NACIONALIDAD</returns>
        public static List<NACIONALIDAD> obtenerNacionalidades()
        {
            Models.EntitieLocal db = new Models.EntitieLocal();
            //Realizando búsqueda
            return db.NACIONALIDAD.ToList();
        }

        /// <summary>
        /// Método público obtener el registro de Nacionalidad en base a los valores
        /// </summary>
        /// <param name="pais">Paid</param>
        /// <param name="descripcion">Descripcion</param>
        /// <returns>Registro de tipo NACIONALIDAD</returns>
        public static NACIONALIDAD obtenerNacionalidadPorValores(string pais, string descripcion)
        {
            Models.EntitieLocal db = new Models.EntitieLocal();
            //Realizando búsqueda
            return db.NACIONALIDAD.Where(x => x.PAIS == pais && x.DESCRIPCION == descripcion).FirstOrDefault();
        }
    }
}
