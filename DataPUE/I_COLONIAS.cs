namespace DataPUE
{
    public class I_COLONIAS
    {
        /// <summary>
        /// Método público para agregar una colonia
        /// </summary>
        /// <param name="clave">Clave para la Colonia</param>
        /// <param name="descripcion">Descripción</param>
        /// <param name="cp">Código Postal</param>
        /// <param name="tipoAsentamiento">Tipo de Asentamiento</param>
        /// <param name="ciudad">Ciudad</param>
        /// <param name="municipioId">Identificador del Municipio al que pertenece la Colonia</param>
        /// <returns>Identificador del registro (Id)</returns>
        //public static int agregarColonia(int clave, string descripcion, string cp, string tipoAsentamiento, string ciudad, int municipioId)
        //{
        //    try
        //    {
        //        Models.EntitiesBCS db = new Models.EntitiesBCS();
        //        //Si ya existe una colonia con la misma clave
        //        if (db.COLONIAS.Where(x => x.CLAVE == clave).FirstOrDefault() != null)
        //        {
        //            throw new Exception("Ya existe una Colonia con la misma clave especificada");
        //        }
        //        //Generando nuevo registro
        //        COLONIAS colonia = new COLONIAS()
        //        {
        //            CLAVE = clave,
        //            COLONIA = descripcion,
        //            CP = cp,
        //            TIPO_ASENTAMIENTO = tipoAsentamiento,
        //            CIUDAD = ciudad,
        //            ID_MUNICIPIO = municipioId
        //        };
        //        //Agregando registro
        //        db.COLONIAS.Add(colonia);
        //        //Guardando cambios
        //        db.SaveChanges();
        //        //ObteniendoId
        //        int coloniaId = Convert.ToInt32(buscarColoniaPorClave(clave).COLONIA_ID);

        //        return coloniaId;
        //    }
        //    catch (DbEntityValidationException dbEx)
        //    {
        //        string listadoErrores = "Se generaron los siguientes errores al guardar los cambios: \n";
        //        foreach (var validationErrors in dbEx.EntityValidationErrors)
        //        {
        //            foreach (var validationError in validationErrors.ValidationErrors)
        //            {
        //                listadoErrores += validationError.ErrorMessage + "\n";
        //            }
        //        }
        //        throw new Exception(listadoErrores);
        //    }
        //}

        /// <summary>
        /// Método público para modificar los datos de una colonia
        /// </summary>
        /// <param name="idColonia">Identificador de la Colonia a modificar</param>
        /// <param name="clave">Clave para la Colonia</param>
        /// <param name="descripcion">Descripción</param>
        /// <param name="cp">Código Postal</param>
        /// <param name="tipoAsentamiento">Tipo de Asentamiento</param>
        /// <param name="ciudad">Ciudad</param>
        /// <param name="municipioId">Identificador del Municipio al que pertenece la Colonia</param>
        /// <returns>Identificador del registro (Id)</returns>
        //public static int modificarColonia(int idColonia, int clave, string descripcion, string cp, string tipoAsentamiento, string ciudad, int municipioId)
        //{
        //    try
        //    {
        //        Models.EntitiesBCS db = new Models.EntitiesBCS();

        //        COLONIAS colonia = db.COLONIAS.Find(idColonia);

        //        //Si no existe una colonia con el id especificado
        //        if (colonia != null)
        //        {
        //            throw new Exception("No existe una colonia con el id especificado");
        //        }
        //        //Si ya existe una COLONIA con la clave especificada
        //        if (db.COLONIAS.Where(x => x.CLAVE == clave && x.COLONIA_ID != idColonia).FirstOrDefault() != null)
        //        {
        //            throw new Exception("Ya existe una colonia con la misma clave especificada");
        //        }
        //        //Modificando valores
        //        colonia.CLAVE = clave;
        //        colonia.COLONIA = descripcion;
        //        colonia.CP = cp;
        //        colonia.TIPO_ASENTAMIENTO = tipoAsentamiento;
        //        colonia.CIUDAD = ciudad;
        //        colonia.ID_MUNICIPIO = municipioId;
        //        //Guardando cambios
        //        db.SaveChanges();

        //        return Convert.ToInt32(colonia.COLONIA_ID);
        //    }
        //    catch (DbEntityValidationException dbEx)
        //    {
        //        string listadoErrores = "Se generaron los siguientes errores al guardar los cambios: \n";
        //        foreach (var validationErrors in dbEx.EntityValidationErrors)
        //        {
        //            foreach (var validationError in validationErrors.ValidationErrors)
        //            {
        //                listadoErrores += validationError.ErrorMessage + "\n";
        //            }
        //        }
        //        throw new Exception(listadoErrores);
        //    }
        //}

        /// <summary>
        /// Método público obtener el listado de todas las Colonias
        /// </summary>
        /// <returns>Listado de tipo COLONIAS</returns>
        //public static List<COLONIAS> obtenerColonias()
        //{
        //    Models.EntitiesBCS db = new Models.EntitiesBCS();
        //    //Realizando búsqueda
        //    return db.COLONIAS.ToList();
        //}

        /// <summary>
        /// Método público obtener el listado de Colonias ligadas a un Municipio
        /// </summary>
        /// <param name="idMunicipio">Identificador del Municipio a búscar</param>
        /// <returns>Listado de tipo COLONIAS</returns>
        //public static List<COLONIAS> obtenerColoniasPorMunicipioId(int idMunicipio)
        //{
        //    Models.EntitiesBCS db = new Models.EntitiesBCS();
        //    //Realizando búsqueda
        //    return db.COLONIAS.Where(x => x.ID_MUNICIPIO == idMunicipio).ToList();
        //}

        /// <summary>
        /// Método público para obtener el listado de colonias para una Municipio en base a la Clave
        /// </summary>
        /// <param name="clave">Clave del Municipio</param>
        /// <returns>Listado de Colonias</returns>
        //public static List<COLONIAS> obtenerColoniasMunicipioPorClave(string clave)
        //{
        //    Models.EntitiesBCS db = new Models.EntitiesBCS();
        //    //Realizando búsqueda
        //    return db.COLONIAS.Where(x => x.MUNICIPIO.CVE_MUNICIPIO == clave).ToList();
        //}

        /// <summary>
        /// Método público realizar la búsqueda de una Colonia en base al Id
        /// </summary>
        /// <param name="idColonia">Identificador del registro</param>
        /// <returns>Registro de tipo COLONIAS</returns>
        //public static COLONIAS buscarColoniaPorId(int idColonia)
        //{
        //    Models.EntitiesBCS db = new Models.EntitiesBCS();
        //    //Realizando búsqueda
        //    COLONIAS colonia = db.COLONIAS.Find(idColonia);

        //    //Si no existe una colonia con el id especificado
        //    if (colonia == null)
        //    {
        //        throw new Exception("No existe una Colonia con el Id Especificado");
        //    }

        //    return colonia;
        //}

        /// <summary>
        /// Método público realizar la búsqueda de una colonia en base a la clave
        /// </summary>
        /// <param name="clave">Clave del registro</param>
        /// <returns>Registro de tipo COLONIAS</returns>
        //public static COLONIAS buscarColoniaPorClave(int clave)
        //{
        //    Models.EntitiesBCS db = new Models.EntitiesBCS();
        //    //Realizando búsqueda
        //    COLONIAS colonia = db.COLONIAS.Where(x => x.CLAVE == clave).FirstOrDefault();
        //    //Si no existe una colonia con la clave especificada
        //    if (colonia == null)
        //    {
        //        throw new Exception("No existe una Colonia con la clave Especificado");
        //    }

        //    return colonia;
        //}
    }
}
