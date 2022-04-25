using System;
using System.Data.Entity.Validation;

namespace DataPUE
{
    public class I_RECAUDADORA
    {
        /// <summary>
        /// Método público para agregar un registro de Recaudadora
        /// </summary>
        /// <param name="clave">Clave para la Recaudadora</param>
        /// <param name="caja">Caja</param>
        /// <param name="oficina">Oficina</param>
        /// <param name="direccion">Dirección</param>
        /// <returns>Identificador del registro (Id)</returns>
        public static int agregarRecaudadora(string clave, string caja, string oficina, string direccion, int municipioId)
        {
            //try
            //{
            //    Models.EntitiesBCS db = new Models.EntitiesBCS();
            //    //Si ya existe una recaudadora con la misma clave
            //    if (db.RECAUDADORA.Where(x => x.CLAVE_RECAUDADORA == clave).FirstOrDefault() != null)
            //    {
            //        throw new Exception("Ya existe una Recaudadora con la misma clave especificada");
            //    }
            //    //Generando nuevo registro
            //    RECAUDADORA recaudadora = new RECAUDADORA()
            //    {
            //        CLAVE_RECAUDADORA = clave,
            //        CAJA = caja,
            //        OFICINA = oficina,
            //        DIRECCION = direccion,
            //        ID_MUNICIPIO = municipioId
            //    };
            //    //Agregando registro
            //    db.RECAUDADORA.Add(recaudadora);
            //    //Guardando cambios
            //    db.SaveChanges();
            //    //ObteniendoId
            //    int recaudadoraId = Convert.ToInt32(buscarRecaudadoraPorClave(clave).RECAUDADORA_ID);

            return 0;
            //}
            //catch (DbEntityValidationException dbEx)
            //{
            //    string listadoErrores = "Se generaron los siguientes errores al guardar los cambios: \n";
            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //    {
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //        {
            //            listadoErrores += validationError.ErrorMessage + "\n";
            //        }
            //    }
            //    throw new Exception(listadoErrores);
            //}
        }

        /// <summary>
        /// Método público para modificar un registro de Recaudadora
        /// </summary>
        /// <param name="idRecaudadora">Identificador del registro</param>
        /// <param name="clave">Clave para la Recaudadora</param>
        /// <param name="caja">Caja</param>
        /// <param name="oficina">Oficina</param>
        /// <param name="direccion">Dirección</param>
        /// <returns>Identificador del registro (Id)</returns>
        public static int modificarRecaudadora(int idRecaudadora, string clave, string caja, string oficina, string direccion, int municipioId)
        {
            try
            {

                Models.EntitieLocal db = new Models.EntitieLocal();

                //RECAUDADORA recaudadora = db.RECAUDADORA.Find(idRecaudadora);

                ////Si no existe una recaudadora con el id especificado
                //if (recaudadora != null)
                //{
                //    throw new Exception("No existe una Recaudadora con el id especificado");
                //}
                ////Si ya existe una RECAUDADORA con la clave especificada
                //if (db.RECAUDADORA.Where(x => x.CLAVE_RECAUDADORA == clave && x.RECAUDADORA_ID != idRecaudadora).FirstOrDefault() != null)
                //{
                //    throw new Exception("Ya existe una recaudadora con la misma clave especificada");
                //}
                ////Modificando valores
                //recaudadora.CLAVE_RECAUDADORA = clave;
                //recaudadora.CAJA = caja;
                //recaudadora.OFICINA = oficina;
                //recaudadora.DIRECCION = direccion;
                //recaudadora.ID_MUNICIPIO = municipioId;
                ////Guardando cambios
                //db.SaveChanges();

                //return Convert.ToInt32(recaudadora.RECAUDADORA_ID);
                return 0;
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
        /// Método público obtener el listado de todas las Recaudadoras
        /// </summary>
        /// <returns>Listado de tipo RECAUDADORA</returns>
        //public static List<RECAUDADORA> obtenerRecaudadoras()
        //{
        //    Models.EntitiesBCS db = new Models.EntitiesBCS();
        //    //Realizando búsqueda
        //    return db.RECAUDADORA.ToList();
        //}

        ///// <summary>
        ///// Método público obtener el listado de Recaudadoras ligadas a un Municipio
        ///// </summary>
        ///// <param name="idMunicipio">Identificador del Municipio a búscar</param>
        ///// <returns>Listado de tipo RECAUDADORA</returns>
        //public static List<RECAUDADORA> obtenerRecaudadorasPorMunicipioId(int idMunicipio)
        //{
        //    Models.EntitiesBCS db = new Models.EntitiesBCS();
        //    //Realizando búsqueda
        //    return db.RECAUDADORA.Where(x => x.ID_MUNICIPIO == idMunicipio).ToList();
        //}

        ///// <summary>
        ///// Método público para obtener el listado de Recaudadoras para una Municipio en base a la Clave
        ///// </summary>
        ///// <param name="clave">Clave del Municipio</param>
        ///// <returns>Listado de Recaudadoras</returns>
        //public static List<RECAUDADORA> obtenerRecaudadorasMunicipioPorClave(string clave)
        //{
        //    Models.EntitiesBCS db = new Models.EntitiesBCS();
        //    //Realizando búsqueda
        //    return db.RECAUDADORA.Where(x => x.MUNICIPIO.CVE_MUNICIPIO == clave).ToList();
        //}

        ///// <summary>
        ///// Método público realizar la búsqueda de una Recaudadora en base al Id
        ///// </summary>
        ///// <param name="idRecaudadora">Identificador del registro</param>
        ///// <returns>Registro de tipo RECAUDADORA</returns>
        //public static RECAUDADORA buscarRecaudadoraPorId(int idRecaudadora)
        //{
        //    Models.EntitiesBCS db = new Models.EntitiesBCS();
        //    //Realizando búsqueda
        //    RECAUDADORA recaudadora = db.RECAUDADORA.Find(idRecaudadora);

        //    //Si no existe una recaudadora con el id especificado
        //    if (recaudadora != null)
        //    {
        //        throw new Exception("No existe una Recaudadora con el Id Especificado");
        //    }

        //    return recaudadora;
        //}

        ///// <summary>
        ///// Método público realizar la búsqueda de una Recaudadora en base a la clave
        ///// </summary>
        ///// <param name="clave">Clave del registro</param>
        ///// <returns>Registro de tipo RECAUDADORA</returns>
        //public static RECAUDADORA buscarRecaudadoraPorClave(string clave)
        //{
        //    Models.EntitiesBCS db = new Models.EntitiesBCS();
        //    //Realizando búsqueda
        //    RECAUDADORA recaudadora = db.RECAUDADORA.Where(x => x.CLAVE_RECAUDADORA == clave).FirstOrDefault();
        //    //Si no existe una recaudadora con la clave especificada
        //    if (recaudadora != null)
        //    {
        //        throw new Exception("No existe una Recaudadora con la clave Especificado");
        //    }

        //    return recaudadora;
        //}
    }
}
