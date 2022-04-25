using DataPUE.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace DataPUE
{
    public class I_MUNICIPIO
    {
        /// <summary>
        /// Método público para agregar un Municipio
        /// </summary>
        /// <param name="clave">Clave para el Municipio</param>
        /// <param name="descripcion">Descripción</param>
        /// <param name="entidadId">Identificador de la entidad a la que pertenece el municipio</param>
        /// <returns>Identificador del registro (Id)</returns>
        public static int agregarMunicipio(string clave, string descripcion, int entidadId)
        {
            try
            {

                //Models.EntitiesBCS db = new Models.EntitiesBCS();
                ////Si ya existe una entidad con la misma clave
                //if (db.MUNICIPIO.Where(x => x.CVE_MUNICIPIO == clave).FirstOrDefault() != null)
                //{
                //    throw new Exception("Ya existe un municipio con la misma clave especificada");
                //}
                ////Generando nuevo registro
                //MUNICIPIO entidad = new MUNICIPIO()
                //{
                //    CVE_MUNICIPIO = clave,
                //    DESCRIPCION = descripcion,
                //    ID_ENTIDAD = Convert.ToDecimal(entidadId)
                //};
                ////Agregando registro
                //db.MUNICIPIO.Add(entidad);
                ////Guardando cambios
                //db.SaveChanges();
                //ObteniendoId
                int municipioId = 0;// Convert.ToInt32(buscarMunicipioPorClave(clave).ID_MUNICIPIO);

                return municipioId;
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
        /// Método público para modificar un Municipio
        /// </summary>
        /// <param name="idMunicipio">Identificador del registro a editar</param>
        /// <param name="clave">Clave para la entidad</param>
        /// <param name="descripcion">Descripción</param>
        /// <param name="entidadId">Identificador de la entidad</param>
        /// <returns>Identificador del registro (Id)</returns>
        public static int modificarMunicipio(int idMunicipio, string clave, string descripcion, int entidadId)
        {
            try
            {
                //Models.EntitiesBCS db = new Models.EntitiesBCS();

                //MUNICIPIO municipio = db.MUNICIPIO.Find(idMunicipio);

                ////Si no existe un municipio con el id especificado
                //if (municipio != null)
                //{
                //    throw new Exception("No existe un municipio con el id especificado");
                //}
                ////Si ya existe una entidad con la clave especificada
                //if (db.MUNICIPIO.Where(x => x.CVE_MUNICIPIO == clave && x.ID_MUNICIPIO != idMunicipio).FirstOrDefault() != null)
                //{
                //    throw new Exception("Ya existe un municipio con la misma clave especificada");
                //}
                ////Modificando valores
                //municipio.CVE_MUNICIPIO = clave;
                //municipio.DESCRIPCION = descripcion;
                //municipio.ID_ENTIDAD = entidadId;
                ////Guardando cambios
                //db.SaveChanges();

                return 0;// Convert.ToInt32(municipio.ID_MUNICIPIO);
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
        /// Método público obtener el listado de todos los Municipios
        /// </summary>
        /// <returns>Listado de tipo MUNICIPIO</returns>
        public static List<Muni> obtenerMunicipios()
        {
            Models.Entities1 db = new Models.Entities1();
            List<Muni> lst = new List<Muni>();
            //lst.Add(new Muni { CVE_MPIO = "001", Descripcion = "MULEGE" });
            //lst.Add(new Muni { CVE_MPIO = "002", Descripcion = "COMONDU" });
            //lst.Add(new Muni { CVE_MPIO = "003", Descripcion = "LA PAZ" });
            //lst.Add(new Muni { CVE_MPIO = "004", Descripcion = "LOS CABOS" });
            //lst.Add(new Muni { CVE_MPIO = "005", Descripcion = "LORETO" });
            //lst.Add(new Muni { CVE_MPIO = "006", Descripcion = "OTRO" });
            lst = db.Database.SqlQuery<Muni>("select CVE_MPIO, DESCRIPCION from ING_MUNICIPIOS WHERE CVE_EDO='03' order by DESCRIPCION asc").ToList();
            lst.Add(new Muni { Descripcion = "---Seleccione---", CVE_MPIO = "0" });
            return lst.OrderBy(s => s.CVE_MPIO).ToList();
            //Realizando búsqueda

        }

        /// <summary>
        /// Método público obtener el listado de Municipios ligados a un Estado
        /// </summary>
        /// <param name="idEntidad">Identificador de la entidad (Estado) a búscar</param>
        /// <returns>Listado de tipo MUNICIPIO</returns>
        public static List<Muni> obtenerMunicipiosPorEstadoId(string idEntidad)
        {
            Models.Entities1 db = new Models.Entities1();
            //Realizando búsqueda
            List<Muni> lst = new List<Muni>();
            lst = db.Database.SqlQuery<Muni>("select CVE_MPIO, DESCRIPCION from ING_MUNICIPIOS WHERE CVE_EDO='" + idEntidad + "'").ToList();
            lst.Add(new Muni { Descripcion = "---Seleccione---", CVE_MPIO = "0" });
            return lst.OrderBy(s => s.CVE_MPIO).ToList();
        }

        /// <summary>
        /// Método público realizar la búsqueda de municipio en base al Id
        /// </summary>
        /// <param name="idMunicipio">Identificador del registro</param>
        /// <returns>Registro de tipo MUNICIPIO</returns>
        //public static MUNICIPIO buscarMunicipioPorId(int idMunicipio)
        //{
        //    Models.EntitiesBCS db = new Models.EntitiesBCS();
        //    //Realizando búsqueda
        //    MUNICIPIO municipio = db.MUNICIPIO.Find(idMunicipio);

        //    //Si no existe una entidad con el id especificado
        //    if (municipio == null)
        //    {
        //        throw new Exception("No existe un municipio con el Id Especificado");
        //    }

        //    return municipio;
        //}

        /// <summary>
        /// Método público realizar la búsqueda de municipio en base a la clave
        /// </summary>
        /// <param name="clave">Clave del registro</param>
        /// <returns>Registro de tipo MUNICIPIO</returns>
        //public static MUNICIPIO buscarMunicipioPorClave(string clave)
        //{
        //    Models.EntitiesBCS db = new Models.EntitiesBCS();
        //    //Realizando búsqueda
        //    MUNICIPIO municipio = db.MUNICIPIO.Where(x => x.CVE_MUNICIPIO == clave).FirstOrDefault();
        //    //Si no existe un municipio con la clave especificada
        //    if (municipio != null)
        //    {
        //        throw new Exception("No existe un municipio con la clave Especificado");
        //    }

        //    return municipio;
        //}


        public static string dameCPporCOlonia(string colonia)
        {
            string strCP = string.Empty;
            Entities1 db = new Entities1();
            strCP = db.Database.SqlQuery<string>("select CODIGO_POSTAL FROM ING_COLONIAS WHERE CVE_COL='" + colonia + "'").FirstOrDefault();

            return strCP;
        }

        /// <summary>
        /// Método público para obtener el listado de colonias para una Municipio en base a la Clave
        /// </summary>
        /// <param name="clave">Clave del Municipio</param>
        /// <returns>Listado de Colonias</returns>
        public static List<Colonia> obtenerColoniasMunicipioPorClave(string clave, string strcp)
        {
            Models.Entities1 db = new Models.Entities1();
            List<Colonia> lst = new List<Colonia>();
            // if (strcp==string.Empty)
            lst = db.Database.SqlQuery<Colonia>("select CVE_COL, DESCRIPCION from ING_COLONIAS WHERE CVE_MPIO=" + clave + " AND CVE_EDO='03' order by DESCRIPCION asc").ToList();
            // lst = lst.Distinct().ToList();
            //else
            //    lst = db.Database.SqlQuery<Colonia>("select CVE_COL, DESCRIPCION from ING_COLONIAS WHERE CVE_MPIO=" + clave + " and CODIGO_POSTAL=" + strcp + " order by DESCRIPCION asc").ToList();
            lst.Add(new Colonia { Descripcion = "---Seleccione---", CVE_COL = "0" });
            return lst;//.OrderBy(s => s.CVE_COL).ToList();

        }

        /// <summary>
        /// Método público para obtener el listado de colonias para una Municipio en base al MunicipioId
        /// </summary>
        /// <param name="idMunicipio">Identificador del Municipio</param>
        /// <returns>Listado de Colonias</returns>
        //public static List<COLONIAS> obtenerColoniasMunicipioPorId(int idMunicipio)
        //{
        //    Models.Entities db = new Models.Entities();
        //    //Realizando búsqueda
        //    return db.COLONIAS.Where(x => x.MUNICIPIO.ID_MUNICIPIO == idMunicipio).ToList();
        //}

        public static List<LOcalidad> obtieneLocalidadesPorMunicipio(string claveMun)
        {
            Models.Entities1 db = new Entities1();
            List<LOcalidad> lst = db.Database.SqlQuery<LOcalidad>("select CVE_LOC, DESCRIPCION from ING_LOCALIDADES WHERE CVE_MPIO=" + claveMun + "AND CVE_EDO='03'     order by DESCRIPCION asc").ToList();
            lst.Add(new LOcalidad { Descripcion = "---Seleccione---", CVE_LOC = "0" });
            return lst;//.OrderBy(s => s.CVE_LOC).ToList();
        }
    }
}
