using System;

namespace DataPUE
{
    public class I_ING_RFC
    {
        /// <summary>
        /// Método público para obtener el Registro de RFC en base al valor (rfc)
        /// </summary>
        /// <param name="rfc">RFC a buscar</param>
        /// <returns>Registro de Tipo RFCObj, si no se encuentra regresa NULL</returns>
        public static RFCObj obtenerRFC(string rfc)
        {
            if (rfc.ToUpper() != "CUPU800825569")
                return null;

            RFCObj nuevoRFC = new RFCObj()
            {
                ID_RFC = 1,
                RFC = "CUPU800825569",
                RFC_LETRA = "CUPU",
                RFC_NUM = "121212",
                RFC_HOMO = "569",
                CURP = "CUPU80082556",
                NOMBRE = "ULISES",
                NOMBRE_2 = "",
                APELLIDO_P = "CUEVAS",
                APELLIDO_M = "PÉREZ",
                CALLE = "Calle S/N",
                CALLE_PPAL = "",
                REFERENCIA1 = "",
                REFERENCIA2 = "",
                NO_EXTERIOR = "7",
                NO_INTERIOR = "",
                CVE_COLONIA = 8,
                CODIGO_POSTAL = "54888",
                TELEFONO = "5547878422",
                TIPO_CONTRIBUYENTE = "1",
                E_MAIL = "correo@gmail.com",
                TELEFONO_MOVIL = "5584845875",
                PAIS_ORIGEN = "MEXICO",
                PAIS_RESIDENCIA = "MEXICO",
                PENSIONADO = "1",
                CVE_EDO = "01",
                CVE_MPIO = "012",
                CVE_LOC = "2145",
                CVE_REC = "2214",
                CREADO_POR = 1,
                FECHA_CREACION = DateTime.Now,
                MODIFICADO_POR = 2,
                FECHA_MODIFICACION = DateTime.Now,
                FECHA_NACIM = DateTime.Now,
                NOMBRE_DEPURADO = "ULISES CUEVAS PÉREZ"
            };

            return nuevoRFC;
        }

        /// <summary>
        /// Método público para Agregar un Registro de RFC 
        /// </summary>
        /// <param name="rfc">RFC a buscar</param>
        /// <param name="rfcLetra">Letras del RFC</param>
        /// <param name="rfcNum">Valores Numericos RFC (Fecha Nac.)</param>
        /// <param name="rfcHomo">Homoclave RFC</param>
        /// <param name="curp">CURP</param>
        /// <param name="nombre">Nombre</param>
        /// <param name="nombre2">Segundo Nombre</param>
        /// <param name="apellidoP">Apellido Paterno</param>
        /// <param name="apellidoM">Apellido Materno</param>
        /// <param name="callePrincipal">Calle Principal</param>
        /// <param name="referencia1">Referencia 1</param>
        /// <param name="referencia2">Referencia 2</param>
        /// <param name="numExterior">Número Exterior</param>
        /// <param name="numInterior">Número Interior</param>
        /// <param name="claveColonia">Clave Colonia</param>
        /// <param name="codigoPostal">Código Postal</param>
        /// <param name="telefono">Teléfono</param>
        /// <param name="tipoContribuyente">Tipo de Contribuyente</param>
        /// <param name="eMail">E-Mail</param>
        /// <param name="telefonoMovil">Teléfono Movil</param>
        /// <param name="paisOrigen">País Origen</param>
        /// <param name="paisResidencia">País de Residencia</param>
        /// <param name="claveEstado">Clave de Estado (DEBERA SER "03")</param>
        /// <param name="pensionado">Valor que indica si es pensionado (0 = No, 1 = Si)</param>
        /// <param name="claveMunicipio">Clave de Municipio</param>
        /// <param name="claveLocalidad">Clave de Localidad</param>
        /// <param name="claveRecaudadora">Clave de recaudadora</param>
        /// <param name="usuarioCreo">Usuario que creó el registro</param>
        /// <param name="fechaNacimiento">Fecha de Nacimiento</param>
        /// <returns>Identificador del registroObj</returns>
        public static int anadirRFC(string rfc, string rfcLetra, string rfcNum, string rfcHomo, string curp, string nombre, string nombre2, string apellidoP, string apellidoM, string callePrincipal, string referencia1, string referencia2, string numExterior, string numInterior, int claveColonia, string codigoPostal, string telefono, string tipoContribuyente, string eMail, string telefonoMovil, string paisOrigen, string paisResidencia, string claveEstado, string pensionado, string claveMunicipio, string claveLocalidad, string claveRecaudadora, int usuarioCreo, DateTime fechaNacimiento)
        {
            //Se debera validar si existe un RFC con los mismos valores (rfc, rfcLetra, rfcNum, rfcHomo)


            RFCObj nuevoRFC = new RFCObj()
            {
                //------Asignando valor aleatorio
                ID_RFC = new Random().Next(1, 1000),
                //Asignando valor aleatorio-------
                RFC = rfc,
                RFC_LETRA = rfcLetra,
                RFC_NUM = rfcNum,
                RFC_HOMO = rfcHomo,
                CURP = curp,
                NOMBRE = (nombre + " " + nombre2 + " " + apellidoP + " " + apellidoM).Trim(),
                NOMBRE_2 = nombre2,
                APELLIDO_P = apellidoP,
                APELLIDO_M = apellidoM,
                CALLE = generaDomicilio(numExterior, numInterior, referencia1, referencia2, callePrincipal),
                CALLE_PPAL = callePrincipal,
                REFERENCIA1 = referencia1,
                REFERENCIA2 = referencia2,
                NO_EXTERIOR = numExterior,
                NO_INTERIOR = numInterior,
                CVE_COLONIA = claveColonia,
                CODIGO_POSTAL = codigoPostal,
                TELEFONO = telefono,
                TIPO_CONTRIBUYENTE = tipoContribuyente,
                E_MAIL = eMail,
                TELEFONO_MOVIL = telefonoMovil,
                PAIS_ORIGEN = paisOrigen,
                PAIS_RESIDENCIA = paisResidencia,
                PENSIONADO = pensionado,
                CVE_EDO = claveEstado,
                CVE_MPIO = claveMunicipio,
                CVE_LOC = claveLocalidad,
                CVE_REC = claveRecaudadora,
                CREADO_POR = usuarioCreo,
                FECHA_CREACION = DateTime.Now,
                FECHA_NACIM = DateTime.Now,
                NOMBRE_DEPURADO = nombre
            };

            return nuevoRFC.ID_RFC;
        }

        /// <summary>
        /// Método público para Editar un Registro de RFC 
        /// </summary>
        /// <param name="idRFC">Identificador del RFC a editar</param>
        /// <param name="rfc">RFC a buscar</param>
        /// <param name="rfcLetra">Letras del RFC</param>
        /// <param name="rfcNum">Valores Numericos RFC (Fecha Nac.)</param>
        /// <param name="rfcHomo">Homoclave RFC</param>
        /// <param name="curp">CURP</param>
        /// <param name="nombre">Nombre</param>
        /// <param name="nombre2">Segundo Nombre</param>
        /// <param name="apellidoP">Apellido Paterno</param>
        /// <param name="apellidoM">Apellido Materno</param>
        /// <param name="callePrincipal">Calle Principal</param>
        /// <param name="referencia1">Referencia 1</param>
        /// <param name="referencia2">Referencia 2</param>
        /// <param name="numExterior">Número Exterior</param>
        /// <param name="numInterior">Número Interior</param>
        /// <param name="claveColonia">Clave Colonia</param>
        /// <param name="codigoPostal">Código Postal</param>
        /// <param name="telefono">Teléfono</param>
        /// <param name="tipoContribuyente">Tipo de Contribuyente</param>
        /// <param name="eMail">E-Mail</param>
        /// <param name="telefonoMovil">Teléfono Movil</param>
        /// <param name="paisOrigen">País Origen</param>
        /// <param name="paisResidencia">País de Residencia</param>
        /// <param name="claveEstado">Clave de Estado (DEBERA SER "03")</param>
        /// <param name="pensionado">Valor que indica si es pensionado (0 = No, 1 = Si)</param>
        /// <param name="claveMunicipio">Clave de Municipio</param>
        /// <param name="claveLocalidad">Clave de Localidad</param>
        /// <param name="claveRecaudadora">Clave de recaudadora</param>
        /// <param name="usuarioCreo">Usuario que creó el registro</param>
        /// <param name="fechaNacimiento">Fecha de Nacimiento</param>
        /// <returns>Identificador del registro</returns>
        public static int anadirRFC(int idRFC, string rfc, string rfcLetra, string rfcNum, string rfcHomo, string curp, string nombre, string nombre2, string apellidoP, string apellidoM, string callePrincipal, string referencia1, string referencia2, string numExterior, string numInterior, int claveColonia, string codigoPostal, string telefono, string tipoContribuyente, string eMail, string telefonoMovil, string paisOrigen, string paisResidencia, string claveEstado, string pensionado, string claveMunicipio, string claveLocalidad, string claveRecaudadora, int usuarioCreo, DateTime fechaNacimiento)
        {
            //Se debera validar si existe un RFC con los mismos valores (rfc, rfcLetra, rfcNum, rfcHomo) y que sea diferente al idRFC a editar, si existe se regresará un error

            //Se modifican los valores
            RFCObj nuevoRFC = new RFCObj()
            {
                RFC = rfc,
                RFC_LETRA = rfcLetra,
                RFC_NUM = rfcNum,
                RFC_HOMO = rfcHomo,
                CURP = curp,
                NOMBRE = (nombre + " " + nombre2 + " " + apellidoP + " " + apellidoM).Trim(),
                NOMBRE_2 = nombre2,
                APELLIDO_P = apellidoP,
                APELLIDO_M = apellidoM,
                CALLE = generaDomicilio(numExterior, numInterior, referencia1, referencia2, callePrincipal),
                CALLE_PPAL = callePrincipal,
                REFERENCIA1 = referencia1,
                REFERENCIA2 = referencia2,
                NO_EXTERIOR = numExterior,
                NO_INTERIOR = numInterior,
                CVE_COLONIA = claveColonia,
                CODIGO_POSTAL = codigoPostal,
                TELEFONO = telefono,
                TIPO_CONTRIBUYENTE = tipoContribuyente,
                E_MAIL = eMail,
                TELEFONO_MOVIL = telefonoMovil,
                PAIS_ORIGEN = paisOrigen,
                PAIS_RESIDENCIA = paisResidencia,
                PENSIONADO = pensionado,
                CVE_EDO = claveEstado,
                CVE_MPIO = claveMunicipio,
                CVE_LOC = claveLocalidad,
                CVE_REC = claveRecaudadora,
                CREADO_POR = usuarioCreo,
                FECHA_CREACION = DateTime.Now,
                FECHA_NACIM = DateTime.Now,
                NOMBRE_DEPURADO = nombre
            };

            return nuevoRFC.ID_RFC;
        }

        /// <summary>
        /// Método público para generar el domicilio del RFC
        /// </summary>
        /// <returns>Domicilio</returns>
        private static string generaDomicilio(string numeroExterior, string numeroInterior, string referencia1, string referencia2, string calle)
        {
            //Este método debera SER MODIFICADO, tiene que llamar al procedimiento almacenado ING_CONCATENA_DOMICILIO
            //Regresando valores de prueba
            return calle + " " + numeroInterior;
        }
    }
}
