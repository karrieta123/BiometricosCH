using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace DataPUE
{
    public class I_TRAMITE
    {

        public static int ingresaTramites(Models.TRAMITES tramites)
        {
            Models.EntitieLocal db = new Models.EntitieLocal();

            try
            {
                db.TRAMITES.Add(tramites);
                db.SaveChanges();

            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
            }

            //var tramite = db.TRAMITES.Where(i => i.CIUDADANO_ID == tramites.CIUDADANO_ID ).FirstOrDefault();
            //return (int)tramites.ID;

            List<Combos> id = (from r in db.TRAMITES
                               where r.CIUDADANO_ID == tramites.CIUDADANO_ID && r.TIPO_LICENCIA == tramites.TIPO_LICENCIA && r.TIPO_TRAMITE == tramites.TIPO_TRAMITE
                               select new Combos
                               {
                                   Identificador = r.ID,
                                   Descripcion = r.INCISO
                               }).OrderByDescending(n => n.Identificador).ToList();

            return (int)id[0].Identificador;
        }

        /// <summary>
        /// Método público para generar un tramite
        /// </summary>
        /// <param name="datosContribuyenteId">Identificador del registro Datos Contribuyente</param>
        /// <param name="estatus">Estatus de la licencia</param>
        /// <param name="tipoLicencia">Tipo de Licencia</param>
        /// <param name="fechaExpedicion">Fecha de expedición de la licencia</param>
        /// <param name="fechaVencimiento">Fecha de vencimiento de la licencia</param>
        /// <param name="movimiento">Descripción del movimiento</param>
        /// <param name="fechaMovimiento">Fecha del proceso/movimiento</param>
        /// <param name="numeroRecibo">Número de recibo</param>
        /// <param name="tipoPago">Tipo de pago</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="estadoId">Identificador del estado</param>
        /// <param name="municipio">Identificador del municipio</param>
        /// <param name="oficina">Oficina</param>
        /// <param name="conciliado">Conciliado</param>
        /// <param name="observaciones">Observaciones</param>
        /// <param name="usuario">Usuario que realiza el proceso</param>
        /// <param name="numeroPermiso">Número de permiso (Si aplica)</param>
        /// <param name="numeroExpediente">Número de Expediente (Si aplica)</param>
        /// <param name="tramiteAnteriorId">Identificador del tramite anterior (Si aplica)</param>
        /// <returns>Identificador de Tramite</returns>
        public static int generarTramite(int datosContribuyenteId, string estatus, string numeroLicencia, string tipoLicencia, DateTime fechaExpedicion, DateTime fechaVencimiento, string movimiento, DateTime? fechaMovimiento, string numeroRecibo, string tipoPago, string periodo, int estadoId, string municipio, string oficina, string conciliado, string observaciones, string usuario, decimal? numeroPermiso, string numeroExpediente, int? tramiteAnteriorId)
        {
            try
            {
                bool existe = true;
                Models.EntitieLocal db = new Models.EntitieLocal();
                //Obteniendo registro datos contribuyente
                Models.DATOS_CONTRIBUYENTE datos = db.DATOS_CONTRIBUYENTE.Where(x => x.DATOS_CONTRIBUYENTE_ID == datosContribuyenteId).SingleOrDefault();
                //Si no existe el registro de datos contribuyente se genera un error
                if (datos == null)
                    throw new Exception("El registro para Datos Contribuyente especificado no existe");

                //Validando existencia de Tramite
                Models.TRAMITE tramite = db.TRAMITE.Where(x => x.DATOS_CONTRIBUYENTE_ID == datosContribuyenteId).FirstOrDefault();
                if (tramite == null)
                {
                    tramite = new Models.TRAMITE();
                    //Relacionando tramite con datos de contribuyente
                    tramite.DATOS_CONTRIBUYENTE_ID = datosContribuyenteId;
                    existe = false;
                }
                //Generando datos
                tramite.ESTATUS = estatus;
                tramite.CURP = "";
                tramite.RFC = datos.RFC;
                tramite.NUMERO_LICENCIA = numeroLicencia;
                tramite.TIPO_LICENCIA = tipoLicencia;
                tramite.FECHA_EXPEDICION = fechaExpedicion;
                tramite.FECHA_VENCIMIENTO = fechaVencimiento;
                tramite.MOVIMIENTO = movimiento;
                tramite.FECHA_MOVIMIENTO = fechaMovimiento;
                tramite.NUMERO_RECIBO = numeroRecibo;
                tramite.TIPO_PAGO = tipoPago;
                tramite.PERIODO = periodo;
                tramite.ESTADO_ID = estadoId;
                tramite.MUNICIPIO = municipio;
                tramite.OFICINA = oficina;
                tramite.CONCILIADO = conciliado;
                tramite.OBSERVACIONES = observaciones;
                tramite.USUARIO = usuario;
                tramite.NUMERO_PERMISO = numeroPermiso;
                tramite.NUMERO_EXPEDIENTE = numeroExpediente;
                tramite.TRAMITE_ANTERIOR_ID = tramiteAnteriorId == 0 ? null : tramiteAnteriorId;
                tramite.DATOS_CONTRIBUYENTE_ID = datosContribuyenteId;
                //Si aun no existe un registro, se añade
                if (!existe)
                    db.TRAMITE.Add(tramite);
                db.SaveChanges();
                //Obteniendo Id
                decimal id = buscarTramitePorCURP("").TRAMITE_ID;
                //Regresando Id
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
        /// Método público para generar un tramite
        /// </summary>
        /// <param name="tramiteId">Identificador del registro Tramite a modificar</param>
        /// <param name="estatus">Estatus de la licencia</param>
        /// <param name="curp">CURP</param>
        /// <param name="rfc">RFC</param>
        /// <param name="tipoLicencia">Tipo de Licencia</param>
        /// <param name="fechaExpedicion">Fecha de expedición de la licencia</param>
        /// <param name="fechaVencimiento">Fecha de vencimiento de la licencia</param>
        /// <param name="movimiento">Descripción del movimiento</param>
        /// <param name="fechaMovimiento">Fecha del proceso/movimiento</param>
        /// <param name="numeroRecibo">Número de recibo</param>
        /// <param name="tipoPago">Tipo de pago</param>
        /// <param name="periodo">Periodo</param>
        /// <param name="estadoId">Identificador del estado</param>
        /// <param name="municipio">Identificador del municipio</param>
        /// <param name="oficina">Oficina</param>
        /// <param name="conciliado">Conciliado</param>
        /// <param name="observaciones">Observaciones</param>
        /// <param name="usuario">Usuario que realiza el proceso</param>
        /// <param name="numeroPermiso">Número de permiso (Si aplica)</param>
        /// <param name="numeroExpediente">Número de Expediente (Si aplica)</param>
        /// <param name="tramiteAnteriorId">Identificador del tramite anterior (Si aplica)</param>
        /// <returns>Identificador de Tramite</returns>
        public static int modificarTramite(int tramiteId, string estatus, string curp, string rfc, string numeroLicencia, string tipoLicencia, DateTime fechaExpedicion, DateTime fechaVencimiento, string movimiento, DateTime? fechaMovimiento, string numeroRecibo, string tipoPago, string periodo, int estadoId, string municipio, string oficina, string conciliado, string observaciones, string usuario, decimal? numeroPermiso, string numeroExpediente, int? tramiteAnteriorId)
        {
            Models.EntitieLocal db = new Models.EntitieLocal();
            //Obteniendo registro datos contribuyente
            Models.TRAMITE tramite = db.TRAMITE.Where(x => x.TRAMITE_ID == tramiteId).SingleOrDefault();
            //Si no existe el registro de tramite se genera un error
            if (tramite == null)
                throw new Exception("No existe ningún tramite con el id especificado");

            //Generando datos
            tramite.ESTATUS = estatus;
            tramite.CURP = curp;
            tramite.RFC = rfc;
            tramite.NUMERO_LICENCIA = numeroLicencia;
            tramite.TIPO_LICENCIA = tipoLicencia;
            tramite.FECHA_EXPEDICION = fechaExpedicion;
            tramite.FECHA_VENCIMIENTO = fechaVencimiento;
            tramite.MOVIMIENTO = movimiento;
            tramite.FECHA_MOVIMIENTO = fechaMovimiento;
            tramite.NUMERO_RECIBO = numeroRecibo;
            tramite.TIPO_PAGO = tipoPago;
            tramite.PERIODO = periodo;
            tramite.ESTADO_ID = estadoId;
            tramite.MUNICIPIO = municipio;
            tramite.OFICINA = oficina;
            tramite.CONCILIADO = conciliado;
            tramite.OBSERVACIONES = observaciones;
            tramite.USUARIO = usuario;
            tramite.NUMERO_PERMISO = numeroPermiso;
            tramite.NUMERO_EXPEDIENTE = numeroExpediente;
            tramite.TRAMITE_ANTERIOR_ID = tramiteAnteriorId;
            //Guardando cambios
            db.SaveChanges();
            //Regresando Id
            return Convert.ToInt32(tramite.TRAMITE_ID);
        }

        /// <summary>
        /// Método público para realizar la búsqueda de tramite en base al valor TramiteId
        /// </summary>
        /// <param name="tramiteId">Identificador del registro Tramite a buscar</param>
        /// <returns>Objeto de tipo Tramite, null si no lo encuentra</returns>
        public static Models.TRAMITE buscarTramitePorTramiteId(int tramiteId)
        {
            Models.EntitieLocal db = new Models.EntitieLocal();
            return db.TRAMITE.Find(tramiteId);
        }

        /// <summary>
        /// Método público para realizar la búsqueda del último tramite en base al RFC
        /// </summary>
        /// <param name="rfc">RFC registro Tramite a buscar</param>
        /// <returns>Objeto de tipo Tramite, null si no lo encuentra</returns>
        public static Models.TRAMITE buscarTramitePorRFC(string rfc)
        {
            Models.EntitieLocal db = new Models.EntitieLocal();
            return db.TRAMITE.Where(x => x.RFC == rfc).OrderByDescending(x => x.TRAMITE_ID).FirstOrDefault();
        }

        /// <summary>
        /// Método público para realizar la busqueda del tramite en base al número de licencia
        /// </summary>
        /// <param name="numeroDeLicencia">Licencia</param>
        /// <returns></returns>
        public static Models.TRAMITE buscarTramitePorNumeroDeLicencia(string numeroDeLicencia)
        {
            Models.EntitieLocal db = new Models.EntitieLocal();
            return db.TRAMITE.Where(x => x.NUMERO_LICENCIA == numeroDeLicencia).FirstOrDefault();
        }

        /// <summary>
        /// Método público para realizar la búsqueda del último tramite en base al CURP
        /// </summary>
        /// <param name="curp">CURP registro Tramite a buscar</param>
        /// <returns>Objeto de tipo Tramite, null si no lo encuentra</returns>
        public static Models.TRAMITE buscarTramitePorCURP(string curp)
        {
            Models.EntitieLocal db = new Models.EntitieLocal();
            return db.TRAMITE.Where(x => x.CURP == curp).OrderByDescending(x => x.TRAMITE_ID).FirstOrDefault();
        }

        /// <summary>
        /// Método público para obtener el registro de DatosContribuyente de un Tramite
        /// </summary>
        /// <param name="tramiteId">Identificador del Tramite a buscar</param>
        ///// <returns>Objeto de tipo DATOS_CONTRIBUYENTE, null si no lo encuentra</returns>
        //public static Models.DATOS_CONTRIBUYENTE obtenerDatosContribuyente(int tramiteId)
        //{
        //    Models.EntitieLocal db = new Models.EntitieLocal();
        //    var tramite = db.TRAMITE.Find(tramiteId);
        //    return tramite == null ? null : tramite.DATOS_CONTRIBUYENTE;
        //}

        /// <summary>
        /// Método público para obtener el registro de Aviso Entero de un Tramite
        /// </summary>
        /// <param name="tramiteId">Identificador del Tramite a buscar</param>
        /// <returns>Objeto de tipo Aviso Entero, null si no lo encuentra</returns>
        //public static Models.AVISO_ENTERO obtenerAvisoEntero(int tramiteId)
        //{
        //    Models.EntitieLocal db = new Models.EntitieLocal();
        //    var tramite = db.TRAMITE.Find(tramiteId);
        //    return tramite == null ? null : tramite.DATOS_CONTRIBUYENTE.AVISO_ENTERO;
        //}
    }
}
