using System;
using System.Data.Entity.Validation;
using System.Linq;

namespace DataPUE
{
    public class I_AVISO_ENTERO
    {
        /// <summary>
        /// Método público para generar un Aviso de Entero
        /// </summary>
        /// <param name="nombre">Nombre</param>
        /// <param name="apellidoPaterno">Apellido Paterno</param>
        /// <param name="apellidoMaterno">Apellido Materno</param>
        /// <param name="curp">CURP</param>
        /// <param name="sexo">Sexo</param>
        /// <param name="edad">Edad</param>
        /// <param name="fechaNacimiento">Fecha de Nacimiento</param>
        /// <param name="entidadNacimiento">Id Entidad de Nacimiento</param>
        /// <param name="calle">Calle de la dirección</param>
        /// <param name="entreCalle">Entre Calle de la dirección</param>
        /// <param name="entreCalle2">Segundo valor Entre Calle de la dirección</param>
        /// <param name="numero">Número exterior de la dirección</param>
        /// <param name="concepto">Concepto</param>
        /// <param name="inciso">Inciso</param>
        /// <param name="importe">Importe</param>
        /// <param name="tipoPago">Tipo de Pago</param>
        /// <param name="observaciones">Observaciones</param>
        /// <param name="tipoTramite">Tipo de Tramite</param>
        /// <param name="usuario">Usuario que realiza el registro</param>
        /// <param name="fecha">Fecha en la que se realiza el registro</param>
        /// <param name="estatus">Estatus</param>
        /// <param name="recibo">Recibo</param>
        public static int generarAvisoEntero(string nombre, string apellidoPaterno, string apellidoMaterno, string curp, Sexo sexo, decimal edad, DateTime fechaNacimiento, int entidadNacimiento, string calle, string entreCalle, string entreCalle2, string numero, string concepto, string inciso, decimal importe, string tipoPago, string observaciones, string tipoTramite, string usuario, DateTime fecha, int estatus, string recibo)
        {
            try
            {
                Models.EntitieLocal db = new Models.EntitieLocal();
                //Generando nuevo aviso
                Models.CIUDADANO aviso = new Models.CIUDADANO()
                {
                    NOMBRE = nombre,
                    APELLIDO_PATERNO = apellidoPaterno,
                    APELLIDO_MATERNO = apellidoMaterno,
                    CURP = curp,
                    SEXO = sexo == Sexo.Hombre ? "H" : "M",
                    //  EDAD = edad,
                    FECHA_NACIMIENTO = fechaNacimiento,
                    // ENTIDAD_NACIMIENTO = Convert.ToInt32(entidadNacimiento),
                    DIRECCION = calle,
                    ENTRE_CALLE1 = entreCalle,
                    ENTRE_CALLE2 = entreCalle2,
                    NUM_INT = numero.ToString(),
                    EMAIL = "",
                    ALERGIAS = "",
                    SENAS_PARTICULARES = "",
                    PROFESION = "",
                    CABELLO = 1,
                    TIPO_SANGRE = 1,
                    ALTURA = "",
                    USA_LENTES = "",
                    DONADOR_ORGANOS = "",
                    MUNICIPIO = 1,
                    COLONIA = 1,
                    CODIGO_POSTAL = 1,
                    TELEFONO = "1"




                };
                //Añadiendo aviso
                db.CIUDADANO.Add(aviso);
                //Guardando cambios
                db.SaveChanges();


                //Obteniendo Número de Aviso Generado en base al curp
                decimal? numeroAvisoGenerado = 0; //= buscarAvisoEnteroPorCurp(aviso.CURP);

                //Regresndo numero de aviso generado
                return Convert.ToInt32(numeroAvisoGenerado);
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
        /// Método público para modificar los datos de un Aviso de Entero
        /// </summary>
        /// <param name="numeroAvisoEntero">Número de Aviso Entero a modificar</param>
        /// <param name="nombre">Nombre</param>
        /// <param name="apellidoPaterno">Apellido Paterno</param>
        /// <param name="apellidoMaterno">Apellido Materno</param>
        /// <param name="curp">CURP</param>
        /// <param name="sexo">Sexo</param>
        /// <param name="edad">Edad</param>
        /// <param name="fechaNacimiento">Fecha de Nacimiento</param>
        /// <param name="entidadNacimiento">Id Entidad de Nacimiento</param>
        /// <param name="calle">Calle de la dirección</param>
        /// <param name="entreCalle">Entre Calle de la dirección</param>
        /// <param name="entreCalle2">Segundo valor Entre Calle de la dirección</param>
        /// <param name="numero">Número exterior de la dirección</param>
        /// <param name="concepto">Concepto</param>
        /// <param name="inciso">Inciso</param>
        /// <param name="importe">Importe</param>
        /// <param name="tipoPago">Tipo de Pago</param>
        /// <param name="observaciones">Observaciones</param>
        /// <param name="tipoTramite">Tipo de Tramite</param>
        /// <param name="usuario">Usuario que realiza el registro</param>
        /// <param name="fecha">Fecha en la que se realiza el registro</param>
        /// <param name="estatus">Estatus</param>
        /// <param name="recibo">Recibo</param>
        //public static int modificarAvisoEntero(int numeroAvisoEntero, string nombre, string apellidoPaterno, string apellidoMaterno, string curp, Sexo sexo, decimal edad, DateTime fechaNacimiento, int entidadNacimiento, string calle, string entreCalle, string entreCalle2, string numero, string concepto, string inciso, decimal importe, string tipoPago, string observaciones, string tipoTramite, string usuario, DateTime fecha, int estatus, string recibo)
        //{
        //    try
        //    {
        //        Models.EntitieLocal db = new Models.EntitieLocal();
        //        //Buscando aviso a modificar
        //        Models.AVISO_ENTERO aviso = db.AVISO_ENTERO.Where(x => x.NUMERO_AVISO == numeroAvisoEntero).FirstOrDefault();
        //        //Modificando datos
        //        aviso.NOMBRE = nombre;
        //        aviso.APELLIDO_PATERNO = apellidoPaterno;
        //        aviso.APELLIDO_MATERNO = apellidoMaterno;
        //        aviso.CURP = curp;
        //        aviso.SEXO = sexo == Sexo.Hombre ? "H" : "M" ;
        //        aviso.EDAD = edad;
        //        aviso.FECHA_NACIMIENTO = fechaNacimiento;
        //        aviso.ENTIDAD_NACIMIENTO = Convert.ToInt32(entidadNacimiento);
        //        aviso.CALLE = calle;
        //        aviso.ENTRE_CALLE = entreCalle;
        //        aviso.ENTRE_CALLE2 = entreCalle2;
        //        aviso.NUMERO = numero;
        //        aviso.CONCEPTO = concepto;
        //        aviso.INCISO = inciso;
        //        aviso.IMPORTE = importe;
        //        aviso.TIPO_PAGO = tipoPago;
        //        aviso.OBSERVACIONES = observaciones;
        //        aviso.TIPO_TRAMITE = tipoTramite;
        //        aviso.USUARIO = usuario;
        //        aviso.FECHA = fecha;
        //        aviso.ESTATUS = estatus;
        //        aviso.RECIBO = recibo;
        //        //Guardando cambios
        //        db.SaveChanges();
        //        //Regresndo numero de aviso generado
        //        return Convert.ToInt32(aviso.NUMERO_AVISO);
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
        /// Método público para obtener la información del último Aviso de Entero en base al CURP
        /// </summary>
        /// <param name="curp">CURP a buscar</param>
        public static Models.CIUDADANO buscarAvisoEnteroPorCurp(string curp)
        {
            Models.EntitieLocal db = new Models.EntitieLocal();
            //Realizando búsqueda
            Models.CIUDADANO aviso = db.CIUDADANO.Where(x => x.CURP == curp).OrderByDescending(x => x.ID).FirstOrDefault();
            //Regresando valor encontrado
            return aviso;
        }

        /// <summary>
        /// Método público para obtener la información del Aviso de Entero en base al Número de Aviso
        /// </summary>
        /// <param name="numeroAviso">Número de Aviso a buscar</param>
        public static Models.CIUDADANO buscarAvisoEnteroPorNumero(int numeroAviso)
        {
            Models.EntitieLocal db = new Models.EntitieLocal();
            //Realizando búsqueda
            Models.CIUDADANO aviso = db.CIUDADANO.Where(x => x.ID == numeroAviso).FirstOrDefault();
            //Regresando valor encontrado
            return aviso;
        }

        /// <summary>
        /// Método público para validar si se ha realizado el pago de la licencia
        /// </summary>
        /// <param name="numeroAviso">Número de Aviso a validar</param>
        /// <returns>True si el Aviso de Entero esta pagado, de lo contrario False</returns>
        public static bool avisoEnteroPagado(int numeroAviso)
        {
            //Código por implementar

            return true;
        }
    }
}
