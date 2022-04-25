namespace PUE.Controllers
{
    using Entidades;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Data;


    public class CaptureInforLic : IDataErrorInfo, IValueConverter, INotifyPropertyChanged
    {
        public static List<ImageDocs> documentos = null;

        #region OBJETOS
        Resultado oResultado;
        #endregion

        #region ENUMERABLES
        public enum TipoPantalla { Registro, Expedicion, Licencia, Administracion, NoExiste, Consulta }
        public enum Sexo { FEMENINO, MASCULINO }
        #endregion!

        #region VARIABLES
        private String _Estado = "";
        private String _NumeroSolicitud = "";
        private String _Curp = "";
        private String _NombreCiudadano = "";
        private String _ApellidoPaterno = "";
        private String _ApellidoMaterno = "";
        private String _FechaNacimiento = "";
        private Sexo _SexoCiudadano = Sexo.FEMENINO;
        private String _Email = "";
        private String _Alergias = "";
        private String _SeñasParticulares = "";
        private String _OficioProfesion = "";
        private String _Cabello = "";
        private String _TipoSangre = "";
        private String _Altura = "";
        private string _UsaLentes;
        private string _EsDonador;
        private String _Municipio = "";
        private String _Colonia = "";
        private String _DescColonia = "";
        private String _Localidad = "";
        private String _DescLocal = "";
        private String _NumeroExterior = "";
        private String _NumeroInterior = "";
        private String _CodigoPostal = "";
        private String _Direccion = "";
        private String _EntreCalle1 = "";
        private String _EntreCalle2 = "";
        private String _LadaTelefono = "";
        private String _Telefono = "";
        private String _TipoTramite = "";
        private String _TipoLicencia = "";
        public String _Estatus = "";
        private String _NumeroLicencia = "";
        private String _NumeroRecibo = "";
        private String _FechaExpedicion = "";
        private String _FechaVencimiento = "";
        private String _LicenciaAnterior = "";
        private String _NumeroPermiso = "";
        private String _NumeroExpediente = "";
        private String _Inciso = "";
        private String _Concepto = "";
        private String _Importe = "";
        private String _TotalPagar = "";
        private String _IDFolio = "";
        private String _RFC = "";
        private String _Nacionalidad = "";

        #endregion

        #region PROPIEDADES

        public string Error { get { throw new NotImplementedException(); } }
        public string this[string propertyName]
        {
            get
            {
                //string result = IsValid(propertyName);

                //if (result != string.Empty && propertyName != "NumeroDeAviso")
                //	Errors.Add(propertyName, result);
                return "";
            }
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string parameterString = parameter as string;
            if (parameterString == null)
                return DependencyProperty.UnsetValue;

            if (Enum.IsDefined(value.GetType(), value) == false)
                return DependencyProperty.UnsetValue;

            object parameterValue = Enum.Parse(value.GetType(), parameterString.ToUpper());

            return parameterValue.Equals(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string parameterString = parameter as string;
            if (parameterString == null)
                return DependencyProperty.UnsetValue;

            return Enum.Parse(targetType, parameterString.ToUpper());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public String NumeroSolicitud { get { return _NumeroSolicitud; } set { _NumeroSolicitud = value; NotifyPropertyChanged(); } }
        public String IDFolio { get { return _IDFolio; } set { _IDFolio = value; NotifyPropertyChanged(); } }
        public String Curp { get { return _Curp; } set { _Curp = value; NotifyPropertyChanged(); } }
        public String NombreCiudadano { get { return _NombreCiudadano; } set { _NombreCiudadano = value; NotifyPropertyChanged(); } }
        public String ApellidoPaterno { get { return _ApellidoPaterno; } set { _ApellidoPaterno = value; NotifyPropertyChanged(); } }
        public String ApellidoMaterno { get { return _ApellidoMaterno; } set { _ApellidoMaterno = value; NotifyPropertyChanged(); } }
        public String FechaNacimiento { get { return _FechaNacimiento; } set { _FechaNacimiento = value; NotifyPropertyChanged(); } }
        public Sexo SexoCiudadano { get { return _SexoCiudadano; } set { _SexoCiudadano = value; NotifyPropertyChanged(); } }
        public String Email { get { return _Email; } set { _Email = value; NotifyPropertyChanged(); } }
        public String Alergias { get { return _Alergias; } set { _Alergias = value; NotifyPropertyChanged(); } }
        public String SeñasParticulares { get { return _SeñasParticulares; } set { _SeñasParticulares = value; NotifyPropertyChanged(); } }
        public String OficioProfesion { get { return _OficioProfesion; } set { _OficioProfesion = value; NotifyPropertyChanged(); } }
        public String Cabello { get { return _Cabello; } set { _Cabello = value; NotifyPropertyChanged(); } }
        public String TipoSangre { get { return _TipoSangre; } set { _TipoSangre = value; NotifyPropertyChanged(); } }
        public String Altura { get { return _Altura; } set { _Altura = value; NotifyPropertyChanged(); } }
        public string UsaLentes { get { return _UsaLentes; } set { _UsaLentes = value; NotifyPropertyChanged(); } }
        public string EsDonador { get { return _EsDonador; } set { _EsDonador = value; NotifyPropertyChanged(); } }
        public String Municipio { get { return _Municipio; } set { _Municipio = value; NotifyPropertyChanged(); } }
        public String Colonia { get { return _Colonia; } set { _Colonia = value; NotifyPropertyChanged(); } }
        public String Localidad { get { return _Localidad; } set { _Localidad = value; NotifyPropertyChanged(); } }
        public String NumeroExterior { get { return _NumeroExterior; } set { _NumeroExterior = value; NotifyPropertyChanged(); } }
        public String NumeroInterior { get { return _NumeroInterior; } set { _NumeroInterior = value; NotifyPropertyChanged(); } }
        public String CodigoPostal { get { return _CodigoPostal; } set { _CodigoPostal = value; NotifyPropertyChanged(); } }
        public String Direccion { get { return _Direccion; } set { _Direccion = value; NotifyPropertyChanged(); } }
        public String EntreCalle1 { get { return _EntreCalle1; } set { _EntreCalle1 = value; NotifyPropertyChanged(); } }
        public String EntreCalle2 { get { return _EntreCalle2; } set { _EntreCalle2 = value; NotifyPropertyChanged(); } }
        public String LadaTelefono { get { return _LadaTelefono; } set { _LadaTelefono = value; NotifyPropertyChanged(); } }
        public String Telefono { get { return _Telefono; } set { _Telefono = value; NotifyPropertyChanged(); } }
        public String TipoTramite { get { return _TipoTramite; } set { _TipoTramite = value; NotifyPropertyChanged(); } }
        public String TipoLicencia { get { return _TipoLicencia; } set { _TipoLicencia = value; NotifyPropertyChanged(); } }
        public String NumeroLicencia { get { return _NumeroLicencia; } set { _NumeroLicencia = value; NotifyPropertyChanged(); } }
        public String NumeroRecibo { get { return _NumeroRecibo; } set { _NumeroRecibo = value; NotifyPropertyChanged(); } }
        public String FechaExpedicion { get { return _FechaExpedicion; } set { _FechaExpedicion = value; NotifyPropertyChanged(); } }
        public String FechaVencimiento { get { return _FechaVencimiento; } set { _FechaVencimiento = value; NotifyPropertyChanged(); } }
        public String LicenciaAnterior { get { return _LicenciaAnterior; } set { _LicenciaAnterior = value; NotifyPropertyChanged(); } }
        public String NumeroPermiso { get { return _NumeroPermiso; } set { _NumeroPermiso = value; NotifyPropertyChanged(); } }
        public String NumeroExpediente { get { return _NumeroExpediente; } set { _NumeroExpediente = value; NotifyPropertyChanged(); } }
        public String Inciso { get { return _Inciso; } set { _Inciso = value; NotifyPropertyChanged(); } }
        public String Concepto { get { return _Concepto; } set { _Concepto = value; NotifyPropertyChanged(); } }
        public String Importe { get { return _Importe; } set { _Importe = value; NotifyPropertyChanged(); } }
        public String TotalPagar { get { return _TotalPagar; } set { _TotalPagar = value; NotifyPropertyChanged(); } }
        public String RFC { get { return _RFC; } set { _RFC = value; NotifyPropertyChanged(); } }
        public String Nacionalidad { get { return _Nacionalidad; } set { _Nacionalidad = value; NotifyPropertyChanged(); } }
        #endregion

        #region PÚBLICOS
        //private int validame(string tel, string codigo, string FECHA_NACIMIENTO,)
        public static string clave = string.Empty;
        //public static string clave = string.Empty;
        //public static string clave = string.Empty;
        //public static string clave = string.Empty;
        public Resultado actualiza(int idSol)
        {
            var ingresaCiudadano = new DataPUE.Models.CIUDADANO
            {
                NOMBRE = NombreCiudadano,
                APELLIDO_PATERNO = ApellidoPaterno,
                APELLIDO_MATERNO = ApellidoMaterno,
                FECHA_NACIMIENTO = DateTime.Parse(FechaNacimiento),
                SEXO = SexoCiudadano.ToString(),
                EMAIL = Email,
                ALERGIAS = Alergias,
                SENAS_PARTICULARES = SeñasParticulares,
                PROFESION = OficioProfesion,
                CABELLO = int.Parse(string.IsNullOrEmpty(Cabello) ? "0" : Cabello),
                TIPO_SANGRE = int.Parse(string.IsNullOrEmpty(TipoSangre) ? "0" : TipoSangre),
                ALTURA = Altura,
                USA_LENTES = UsaLentes.ToString(),
                DONADOR_ORGANOS = EsDonador.ToString(),
                MUNICIPIO = int.Parse(Municipio),
                COLONIA = int.Parse(string.IsNullOrEmpty(Colonia) ? "0" : Colonia),
                NUM_EXT = string.IsNullOrEmpty(NumeroExterior) ? "0" : NumeroExterior,
                NUM_INT = string.IsNullOrEmpty(NumeroInterior) ? "0" : NumeroInterior,
                CODIGO_POSTAL = int.Parse(string.IsNullOrEmpty(CodigoPostal) ? "0" : CodigoPostal),
                DIRECCION = Direccion,
                ENTRE_CALLE1 = EntreCalle1,
                ENTRE_CALLE2 = EntreCalle2,
                TELEFONO = string.IsNullOrEmpty(Telefono) ? "0" : Telefono,
                CURP = Curp,
                LOCALIDAD = Localidad
            };
            if (documentos != null)
                FormatImage(documentos, idSol);//Se guardan los documentos
            if (DataPUE.I_DATOS_CONTRIBUYENTE.actualizaCiudadano(ingresaCiudadano, idSol, 0))
                return new Resultado() { _Estatus = Resultado.Estatus.OK, _Detalles = "se Actualizó, correctamente el registro" };
            else
                return new Resultado() { _Estatus = Resultado.Estatus.ERROR, _Detalles = "Ocurrió un error al actualizar la información" };
        }

        public static void CsCierre(string foliLaser)
        {
            //obj = new DataPUE.I_DATOS_CONTRIBUYENTE();
            DataPUE.I_DATOS_CONTRIBUYENTE.cierraClico(idSoli.ToString(), foliLaser);
            string curpi = DataPUE.I_DATOS_CONTRIBUYENTE.dameCurp(idSoli);
            if (curpi != string.Empty)
                DataPUE.I_DATOS_CONTRIBUYENTE.cierraReimpresion(curpi, idSoli, DataPUE.CATALOGOS.id_Usuario);
        }

        /// <summary>
        /// Método público para registrar ciudadano
        /// </summary>
        /// <returns></returns>
        public Resultado RegistrarCiudadano()
        {
            if (this.NombreCiudadano == "")
                //--Regresamos detalles de error en caso de que falten datos obligatorios ó se presente un error en el guardado
                return new Resultado() { _Estatus = Resultado.Estatus.ERROR, _Detalles = "El campo nombre es un valor obligatorio." };
            else
            {
                int idCiudadano = 0;

                DataPUE.Models.CIUDADANO mydata = DataPUE.I_DATOS_CONTRIBUYENTE.buscarDatosContribuyentePorCURP(Curp);
                if (EsDonador == null) EsDonador = "false";
                else if (EsDonador == "SI") EsDonador = "true";
                else if (EsDonador == "NO") EsDonador = "false";

                if (UsaLentes == null) UsaLentes = "false";
                else if (UsaLentes == "SI") UsaLentes = "true";
                else if (UsaLentes == "NO") UsaLentes = "false";
                if (string.IsNullOrEmpty(ApellidoMaterno))
                    ApellidoMaterno = ".";
                var ingresaCiudadano = new DataPUE.Models.CIUDADANO
                {
                    NOMBRE = NombreCiudadano,
                    APELLIDO_PATERNO = ApellidoPaterno,
                    APELLIDO_MATERNO = ApellidoMaterno,
                    FECHA_NACIMIENTO = DateTime.Parse(FechaNacimiento),
                    SEXO = SexoCiudadano.ToString(),
                    EMAIL = Email,
                    ALERGIAS = Alergias,
                    SENAS_PARTICULARES = SeñasParticulares,
                    PROFESION = OficioProfesion,
                    CABELLO = int.Parse(string.IsNullOrEmpty(Cabello) ? "0" : Cabello),
                    TIPO_SANGRE = int.Parse(string.IsNullOrEmpty(TipoSangre) ? "0" : TipoSangre),
                    ALTURA = Altura,
                    USA_LENTES = bool.Parse(UsaLentes) ? "SI" : "NO",
                    DONADOR_ORGANOS = bool.Parse(EsDonador) ? "SI" : "NO",
                    MUNICIPIO = int.Parse(Municipio),
                    COLONIA = int.Parse(string.IsNullOrEmpty(Colonia) ? "0" : Colonia),
                    NUM_EXT = string.IsNullOrEmpty(NumeroExterior) ? "0" : NumeroExterior,
                    NUM_INT = string.IsNullOrEmpty(NumeroInterior) ? "0" : NumeroInterior,
                    CODIGO_POSTAL = int.Parse(string.IsNullOrEmpty(CodigoPostal) ? "0" : CodigoPostal),
                    DIRECCION = Direccion,
                    ENTRE_CALLE1 = EntreCalle1,
                    ENTRE_CALLE2 = EntreCalle2,
                    TELEFONO = string.IsNullOrEmpty(Telefono) ? "0" : Telefono,
                    CURP = Curp,
                    NACIONALIDAD = Nacionalidad,
                    LOCALIDAD = string.IsNullOrEmpty(Localidad) ? "0" : Localidad

                };
                if (mydata == null)
                {
                    //--Si el registro es éxitoso regresamos el id generado
                    //hay q guardar en ciudadano, datos contribuye y en tramites
                    try
                    {
                        //bool usalen=UsaLentes??false;
                        //bool donante = EsDonador ?? false;                       
                        idCiudadano = DataPUE.I_DATOS_CONTRIBUYENTE.guardaCiudadano(ingresaCiudadano);
                    }
                    catch (Exception ex)
                    {
                        return new Resultado() { _Estatus = Resultado.Estatus.ERROR, _Detalles = "se encontro un error al guardar los datos" + ex.Message.ToString() };
                    }

                }
                else
                {
                    idCiudadano = int.Parse(mydata.ID.ToString());
                    DataPUE.I_DATOS_CONTRIBUYENTE.actualizaCiudadano(ingresaCiudadano, 0, idCiudadano);
                }

                try
                {

                    if (Rfc.isLocal == "0")
                    {  //se va a guardar con el rfc Generico y a el Rfc.idDatosContribuyente se le asigna el id del registro del generico que seria 0 pa qitarme de pedos
                       //guardaRFCGenerico();
                        Rfc.idDatosContribuyente = 1;
                        Rfc.isLocal = "XAXX010101000";
                    }
                    RFC = Rfc.isLocal;

                    var tramites = new DataPUE.Models.TRAMITES
                    {
                        CIUDADANO_ID = idCiudadano,
                        CONTRIBUYENTE_ID = Rfc.idDatosContribuyente == 0 ? 1 : Rfc.idDatosContribuyente,
                        IDENTIFICA_ID = 2,
                        TIPO_TRAMITE = int.Parse(string.IsNullOrEmpty(TipoTramite) ? "0" : TipoTramite),
                        TIPO_LICENCIA = int.Parse(string.IsNullOrEmpty(TipoLicencia) ? "0" : TipoLicencia),
                        NUM_LICENCIA = NumeroLicencia,
                        LICENCIA_ANTERIOR = LicenciaAnterior,
                        FECHA_EXPEDICION = DateTime.Parse(FechaExpedicion + " 00:00:00"),
                        FECHA_MOVIMIENTO = DateTime.Parse(FechaVencimiento + " 00:00:00"),
                        INCISO = Inciso,
                        CONCEPTO_PAGO = Concepto,
                        IMPORTE = Importe,
                        TOTAL_PAGAR = TotalPagar,
                        NUM_PERMISO = NumeroPermiso,
                        RECIBO = NumeroRecibo,
                        NUM_EXPEDIENTE = NumeroExpediente,
                        STATUS = "REGISTRADO",
                        IDUSUARIO = DataPUE.CATALOGOS.id_Usuario,
                        RFC_IS_LOCAL = Rfc.isLocal.ToString(),
                        RECAUDADORA = DataPUE.CATALOGOS.cv_recaudadora,
                        NUM_OFICIO = FolioExce
                    };

                    int idTramite = DataPUE.I_TRAMITE.ingresaTramites(tramites);

                    if (tramites.TIPO_TRAMITE != 5)
                    {
                        if (tramites.TIPO_TRAMITE != 6)
                        {
                            var datosFaltantes = DataPUE.I_DATOS_CONTRIBUYENTE.obtenValoresFaltantesCobro(tramites.CONCEPTO_PAGO, tramites.IMPORTE);
                            DataPUE.I_DATOS_CONTRIBUYENTE.RegistraCobro(int.Parse(DataPUE.CATALOGOS.id_Usuario), DataPUE.CATALOGOS.cv_recaudadora, tramites.CONTRIBUYENTE_ID.ToString(), tramites.INCISO, datosFaltantes.SUBINCISO, datosFaltantes.DET_SUBINCISO, idTramite.ToString(), double.Parse(TotalPagar));
                        }
                    }

                    idSoli = idTramite;
                    NumeroSolicitud = idTramite.ToString();

                    if (documentos != null)
                        FormatImage(documentos, idTramite);//Se guardan los documentos

                    if (!DataPUE.I_DATOS_CONTRIBUYENTE.sicayo(idTramite.ToString(), tramites.RFC_IS_LOCAL))
                        return new Resultado() { _Estatus = Resultado.Estatus.OK, _Detalles = "se creo correctamente el Numero de solicitud: " + idTramite };



                    return new Resultado() { _Estatus = Resultado.Estatus.OK, _Detalles = "se creo correctamente el Numero de solicitud: " + idTramite };
                    //}
                    //else
                    //    return new Resultado() { _Estatus = Resultado.Estatus.ERROR, _Detalles = "se debe, Scanear los documentos antes de guardar la Información" };
                }

                catch (Exception ex)
                {
                    return new Resultado() { _Estatus = Resultado.Estatus.ERROR, _Detalles = "se encontro un error al guardar los datos" + ex.Message.ToString() };
                }



            }
        }




        public int idFolioLaser(int idTramite, int folio)
        {
            int idLaser = 0;
            try
            {
                DataPUE.I_DATOS_CONTRIBUYENTE.ingresaFolio(idTramite, DataPUE.CATALOGOS.id_Usuario, DataPUE.CATALOGOS.cv_recaudadora, folio);
                //idLaser = DataPUE.I_DATOS_CONTRIBUYENTE.dameFolioxRecaudadora();

                //actualiza siguiente folio en inventario

                idfoli = idLaser;
            }
            catch (Exception ex)
            { }
            return idLaser;
        }


        public static int idSoli = 0;
        public static int idfoli = 0;
        public static string FolioExce = string.Empty;

        public int newIdFolio(int numTramite, string strDesc, int folix)
        {
            int idNewFolio = DataPUE.I_DATOS_CONTRIBUYENTE.UpgradeFolioReturnNew(numTramite, strDesc, DataPUE.CATALOGOS.id_Usuario, folix);
            this._IDFolio = idNewFolio.ToString();
            idfoli = idNewFolio;
            idSoli = numTramite;

            return idNewFolio;
        }

        private void guardaRFCGenerico()
        {
            //var rfctabla = new DataPUE.Models.DATOS_CONTRIBUYENTE
            //{
            //    RFC = "XAXX010101000",
            //    CURP = string.Empty,
            //    NOMBRE = string.Empty,
            //    APELLIDO_PATERNO = string.Empty,
            //    APELLIDO_MATERNO = string.Empty,
            //    PENSIONADO = 0,
            //    NACIONALIDAD_ID = 36,
            //    ESTADO_ID = 3,
            //    MUNICIPIO = 3,
            //    LOCALIDAD = 3,
            //    DIRECCION = string.Empty,
            //    CALLE_PRINCIPAL = string.Empty,
            //    ENTRE_CALLE1 = string.Empty,
            //    ENTRE_CALLE2 = string.Empty,
            //    COLONIA_ID = 3,                
            //    NUMERO_INTERIOR = "0",
            //    CODIGO_POSTAL = "0",
            //    EMAIL = string.Empty,
            //    TELEFONO = string.Empty,
            //    FISICA_MORAL = string.Empty
            //};

            // DataPUE.I_DATOS_CONTRIBUYENTE.insertacontribuyente(rfctabla);

            //debe de guardar tambien en la bd de gob
            CultureInfo ci = new CultureInfo("en-US");
            DataPUE.RECORD obj = new DataPUE.RECORD();
            obj.ID_RFC = 0;
            obj.RFC = "XAXX010101000";
            obj.RFC_LETRA = "XAXX";
            obj.RFC_NUM = DateTime.Now.ToString("dd/MMM/yyyy");
            obj.RFC_HOMO = "";
            obj.CURP = string.Empty;
            obj.NOMBRE = string.Empty;
            obj.NOMBRE_2 = string.Empty;
            obj.APELLIDO_P = string.Empty;
            obj.APELLIDO_M = string.Empty;
            obj.CALLE = string.Empty;
            obj.CALLE_PPAL = string.Empty;
            obj.REFERENCIA1 = string.Empty;
            obj.REFERENCIA2 = string.Empty;
            obj.NO_EXTERIOR = string.Empty;
            obj.NO_INTERIOR = string.Empty;
            obj.CVE_COLONIA = 0;
            obj.CODIGO_POSTAL = string.Empty;
            obj.TELEFONO = string.Empty;
            obj.TIPO_CONTRIBUYENTE = string.Empty;
            obj.E_MAIL = string.Empty;
            obj.TELEFONO_MOVIL = string.Empty;
            obj.PAIS_ORIGEN = string.Empty;
            obj.PAIS_RECIDENCIA = string.Empty;
            obj.PENSIONADO = "No";
            obj.CVE_EDO = string.Empty;
            obj.CVE_MPIO = string.Empty;
            obj.CVE_LOC = string.Empty;
            obj.CVE_REC = DataPUE.CATALOGOS.cv_recaudadora;
            obj.CREADO_POR = int.Parse(DataPUE.CATALOGOS.id_Usuario);
            obj.FECHA_CREACION = DateTime.Now.ToString("dd/MMM/yyyy");
            obj.MODIFICADO_POR = int.Parse(DataPUE.CATALOGOS.id_Usuario);

            DataPUE.I_DATOS_CONTRIBUYENTE.insertaContribuyenteGobierno(obj);
        }

        public Resultado consultaCP(string CP, out List<DataPUE.Muni> lstMun)
        {
            oResultado = new Resultado();
            List<DataPUE.Colonia> lst = new List<DataPUE.Colonia>();
            lstMun = DataPUE.I_DATOS_CONTRIBUYENTE.lstBuscaCP(CP, out lst, "0", 0, string.Empty);
            if (lstMun.Count > 0)
            {

                return new Resultado() { _Estatus = Resultado.Estatus.OK, _Detalles = this };
            }

            else
                return new Resultado() { _Estatus = Resultado.Estatus.ERROR, _Detalles = "No se encontro información para el Codigo postal ingresado" };

        }

        public Resultado conSultasCurp(string curp, out List<ConsultaOrden> lstin)
        {
            lstin = new List<ConsultaOrden>();
            List<DataPUE.Consultas> lst = new List<DataPUE.Consultas>();
            oResultado = new Resultado();
            lst = DataPUE.I_DATOS_CONTRIBUYENTE.ConsultaCurp(curp);

            for (int i = 0; i < lst.Count; i++)
            {
                ConsultaOrden obj = new ConsultaOrden();
                obj.Nombre = lst[i].Nombre;
                obj.Tipo_Licencia = switchTipoLic(int.Parse(lst[i].tipoLic.ToString()));
                obj.Tipo_Tramite = "Renovación";
                obj.Fecha_Expedicion = lst[i].FechaExpedicion.ToString("dd/MM/yy");
                obj.Fecha_Vencimiento = lst[i].FechaVencimiento.ToString();
                obj.Fecha_Vencimiento = DateTime.Parse(obj.Fecha_Vencimiento).ToString("dd/MM/yy");
                obj.Solicitud = lst[i].id.ToString();

                lstin.Add(obj);
            }

            return oResultado;

        }

        /// <summary>
        /// Buscar información de ciudadano por CURP
        /// </summary>
        /// <param name="Curp"></param>
        /// <returns></returns>
        public Resultado ConsultarCiudadanoPorCurp(String CURP)
        {
            oResultado = new Resultado();
            if (String.IsNullOrEmpty(CURP))//|| CURP != "MUCA7002HHGTRL01")
                return new Resultado() { _Estatus = Resultado.Estatus.ERROR, _Detalles = "No se encontro información para el Curp ingresado" };
            else
            {
                DataPUE.Models.CIUDADANO mydata = DataPUE.I_DATOS_CONTRIBUYENTE.buscarDatosContribuyentePorCURP(CURP);
                if (mydata != null && mydata.CURP != null)
                {
                    List<DataPUE.Models.TRAMITES> lstTrami = mydata.TRAMITES.ToList();

                    //List<DataPUE.Models.DATOS_CONTRIBUYENTE> lstDatos=.
                    this._NombreCiudadano = mydata.NOMBRE == null ? "" : mydata.NOMBRE;
                    this._ApellidoPaterno = mydata.APELLIDO_PATERNO == null ? "" : mydata.APELLIDO_PATERNO;
                    this._ApellidoMaterno = mydata.APELLIDO_MATERNO == null ? "" : mydata.APELLIDO_MATERNO;
                    this._Alergias = mydata.ALERGIAS == null ? "" : mydata.ALERGIAS;
                    this._Altura = mydata.ALTURA == null ? "" : mydata.ALTURA;
                    this._Cabello = mydata.CABELLO == null ? "" : mydata.CABELLO.ToString();
                    this._CodigoPostal = mydata.CODIGO_POSTAL == null ? "" : mydata.CODIGO_POSTAL.ToString();
                    this._Colonia = mydata.COLONIA == null ? "" : mydata.COLONIA.ToString();
                    this._Curp = CURP == null ? "" : Curp;
                    this._Direccion = mydata.DIRECCION == null ? "" : mydata.DIRECCION;
                    this._Email = mydata.EMAIL == null ? "" : mydata.EMAIL;
                    this._EntreCalle1 = mydata.ENTRE_CALLE1 == null ? "" : mydata.ENTRE_CALLE1;
                    this._EntreCalle2 = mydata.ENTRE_CALLE2 == null ? "" : mydata.ENTRE_CALLE2;
                    this._EsDonador = mydata.DONADOR_ORGANOS.ToUpper();// == "SI" ? true : false;
                    this._UsaLentes = mydata.USA_LENTES;// == "SI" ? true : false;
                    this._FechaNacimiento = mydata.FECHA_NACIMIENTO.ToString();
                    this._LadaTelefono = "";
                    this._LicenciaAnterior = "";

                    this._Localidad = string.IsNullOrEmpty(mydata.LOCALIDAD) ? string.Empty : mydata.LOCALIDAD.ToString();
                    this._Municipio = mydata.MUNICIPIO == null ? "" : mydata.MUNICIPIO.ToString();
                    this._OficioProfesion = mydata.PROFESION == null ? "" : mydata.PROFESION;
                    this._SeñasParticulares = mydata.SENAS_PARTICULARES == null ? "" : mydata.SENAS_PARTICULARES;
                    this._SexoCiudadano = mydata.SEXO.ToUpper() == "MASCULINO" ? Sexo.MASCULINO : Sexo.FEMENINO;
                    this._Telefono = mydata.TELEFONO == null ? "" : mydata.TELEFONO.ToString();
                    this._TipoSangre = mydata.TIPO_SANGRE == null ? "" : mydata.TIPO_SANGRE.ToString();
                    this._NumeroInterior = (mydata.NUM_INT == null ? "" : mydata.NUM_INT.ToString());
                    this._NumeroExterior = (mydata.NUM_EXT == null ? "" : mydata.NUM_EXT.ToString());
                    //this._Estado=mydata.
                    if (lstTrami.Count > 0)
                    {
                        this._TipoLicencia = lstTrami[0].TIPO_LICENCIA.ToString();

                        this._TipoTramite = lstTrami[0].TIPO_TRAMITE.ToString();
                        this._NumeroExpediente = lstTrami[0].NUM_EXPEDIENTE;
                        this._Estatus = lstTrami[0].STATUS;
                        this._NumeroLicencia = lstTrami[0].NUM_LICENCIA;
                        this._NumeroPermiso = lstTrami[0].NUM_PERMISO;
                        this._NumeroRecibo = lstTrami[0].RECIBO;
                        if (lstTrami[0].RFC_IS_LOCAL == "0" || lstTrami[0].RFC_IS_LOCAL == null)
                            this._RFC = lstTrami[0].RFC_IS_LOCAL;
                        else
                        {
                            Rfc buscaRfc = new Rfc();
                            var rfcGob = buscaRfc.BuscaPorRFC(lstTrami[0].RFC_IS_LOCAL);
                            this.RFC = rfcGob.RFC;
                        }
                        this._IDFolio = lstTrami[0].ID.ToString();
                    }
                    this._UsaLentes = mydata.USA_LENTES;// == "SI" ? true : false;
                    return new Resultado() { _Estatus = Resultado.Estatus.OK, _Detalles = this };
                }
                return new Resultado() { _Estatus = Resultado.Estatus.ERROR, _Detalles = "No se encontraron resultados con este CURP" };
            }
        }

        /// <summary>
        /// Consultar informacion de expediente por número de solicitud
        /// </summary>
        /// <param name="NumeroSolicitud"></param>
        /// <returns></returns>
		public Resultado ConsultarExpedientePorNumeroSolicitud(String NumeroDeSolicitud, bool pantalla)
        {
            oResultado = new Resultado();
            if (String.IsNullOrEmpty(NumeroDeSolicitud))
                return new Resultado() { _Estatus = Resultado.Estatus.ERROR, _Detalles = "No se encontro información para el número de solicitud ingresado" };
            else
            {
                try
                {
                    int id_solicitud = int.Parse(NumeroDeSolicitud);
                    string rfcRegresa = string.Empty;
                    DataPUE.Models.CIUDADANO ciud = DataPUE.I_DATOS_CONTRIBUYENTE.BuscaPorIDSolicitud(id_solicitud, pantalla, out rfcRegresa);
                    if (ciud != null)
                    {
                        if (ciud.NOMBRE == "0")
                            return new Resultado() { _Estatus = Resultado.Estatus.ERROR, _Detalles = "No se encontraron resultados, con la  solicitud ingresada" };

                        List<DataPUE.Models.TRAMITES> tramito = ciud.TRAMITES.Where(x => x.ID == id_solicitud).ToList();
                        this._NombreCiudadano = ciud.NOMBRE;
                        this._ApellidoPaterno = ciud.APELLIDO_PATERNO;
                        this._ApellidoMaterno = ciud.APELLIDO_MATERNO;
                        this._Alergias = ciud.ALERGIAS;
                        this._Altura = ciud.ALTURA;
                        this._Cabello = ciud.CABELLO.ToString();
                        this._CodigoPostal = ciud.CODIGO_POSTAL.ToString();
                        this._Colonia = ciud.COLONIA.ToString();
                        this._Concepto = tramito[0].CONCEPTO_PAGO;
                        this._Estatus = tramito[0].STATUS;
                        this._Curp = ciud.CURP;
                        this._Direccion = ciud.DIRECCION;
                        this._Email = ciud.EMAIL;
                        this._EntreCalle1 = ciud.ENTRE_CALLE1;
                        this._EntreCalle2 = ciud.ENTRE_CALLE2;
                        this._EsDonador = ciud.DONADOR_ORGANOS;//=="NO"?false:true ;
                        this._FechaExpedicion = tramito[0].FECHA_EXPEDICION.ToString("dd/MM/yyyy");
                        this._FechaNacimiento = ciud.FECHA_NACIMIENTO.ToString();
                        this._FechaVencimiento = tramito[0].FECHA_MOVIMIENTO.ToString();
                        this._FechaVencimiento = DateTime.Parse(this._FechaVencimiento).ToString("dd/MM/yyyy");
                        this._Importe = tramito[0].IMPORTE;
                        this._Inciso = tramito[0].INCISO;
                        this._LadaTelefono = ciud.TELEFONO.ToString();
                        this._LicenciaAnterior = "";

                        this._Localidad = string.IsNullOrEmpty(ciud.LOCALIDAD) ? string.Empty : ciud.LOCALIDAD.ToString();
                        this._Municipio = ciud.MUNICIPIO.ToString();
                        this._NumeroExpediente = tramito[0].NUM_EXPEDIENTE;
                        this._NumeroInterior = string.IsNullOrEmpty(ciud.NUM_INT) ? string.Empty : ciud.NUM_INT.ToString();
                        this._NumeroExterior = string.IsNullOrEmpty(ciud.NUM_EXT) ? string.Empty : ciud.NUM_EXT.ToString();
                        this._NumeroLicencia = tramito[0].NUM_LICENCIA;
                        this._NumeroPermiso = tramito[0].NUM_PERMISO;
                        this._NumeroRecibo = tramito[0].RECIBO;
                        //this._NumeroSolicitud = "123";
                        this._RFC = rfcRegresa;
                        this._OficioProfesion = ciud.PROFESION;
                        this._SeñasParticulares = ciud.SENAS_PARTICULARES;
                        this._SexoCiudadano = ciud.SEXO.ToUpper() == "FEMENINO" ? Sexo.FEMENINO : Sexo.MASCULINO;
                        this._Telefono = ciud.TELEFONO.ToString();

                        this._TipoLicencia = tramito[0].TIPO_LICENCIA.ToString();
                        this._TipoSangre = ciud.TIPO_SANGRE.ToString();
                        this._TipoTramite = tramito[0].TIPO_TRAMITE.ToString();
                        this._TotalPagar = tramito[0].TOTAL_PAGAR.ToString();
                        this._UsaLentes = ciud.USA_LENTES;//=="NO"?false:true;// false;
                        return new Resultado() { _Estatus = Resultado.Estatus.OK, _Detalles = this };
                    }
                    else
                        return new Resultado() { _Estatus = Resultado.Estatus.ERROR, _Detalles = "Esta Solicitud aun no esta pagada" };
                }
                catch (Exception ex)
                {
                    return new Resultado() { _Estatus = Resultado.Estatus.ERROR, _Detalles = "Se origino un error al buscar la solicitud" };
                }

            }
        }

        /// <summary>
        /// Imprimir solicitud
        /// </summary>
        /// <param name="RutaAlmacenamiento">Ruta en la que se almacenara el pdf</param>
        /// <returns></returns>
        public Resultado ImprimirSolicitud(String RutaAlmacenamiento)
        {
            if (this._NumeroSolicitud != "")
                return new Resultado() { _Estatus = Resultado.Estatus.OK, _Detalles = "Archivo generado correctamente." };
            else
                return new Resultado() { _Estatus = Resultado.Estatus.ERROR, _Detalles = "El número de solicitud es un dato obligatorio." };
        }


        public static string tipoSnagre(int idtipo)
        {
            string tiposangre = string.Empty;
            switch (idtipo)
            {
                case (1):
                    tiposangre = "A-";
                    break;
                case (2):
                    tiposangre = "A+";
                    break;
                case (3):
                    tiposangre = "B-";
                    break;
                case (4):
                    tiposangre = "B+";
                    break;
                case (5):
                    tiposangre = "O-";
                    break;
                case (6):
                    tiposangre = "O+";
                    break;
                case (7):
                    tiposangre = "AB-";
                    break;
                case (8):
                    tiposangre = "AB+";
                    break;

            }
            return tiposangre;
        }

        public static string getEntidad(string abrevia)
        {
            string strentida = string.Empty;
            strentida = DataPUE.CATALOGOS.getEstado(abrevia);
            return strentida;
        }

        public static string dameNombre(string strId)
        {
            string name = string.Empty;
            name = DataPUE.CATALOGOS.getNombre(strId);
            return name;
        }

        private string dameclave(string tipolICENCIA)
        {

            string str = DataPUE.CATALOGOS.getclavelic(int.Parse(tipolICENCIA));
            //switch (tipolICENCIA)
            //{
            //    case("1"):
            //        clave = "1024-16-A)";
            //        break;
            //    case ("2"):
            //        clave = "1024-16-B)";
            //        break;
            //    case ("3"):
            //        clave = "1024-16-C)";
            //        break;
            //    case ("4"):
            //        clave = "1024-17";
            //        break;

            //}
            return str;
        }

        private string dameclaveDescuento(int idDescuento)
        {
            string str = DataPUE.CATALOGOS.getClaveDescuento(idDescuento);
            return str;
        }

        public static string switchTipoLic(int id)
        {
            string typelic = string.Empty;
            switch (id)
            {
                case (1):
                    typelic = "CHOFER";
                    break;
                case (2):
                    typelic = "AUTOMOVILISTA";
                    break;
                case (3):
                    typelic = "MOTOCICLISTA";
                    break;
                case (4):
                    typelic = "PERMISO";
                    break;
            }

            return typelic;
        }

        private string dameclaveReposicion(string tipoTram, string tipoLic)
        {
            string claveReposicion = string.Empty;
            switch (tipoLic)
            {
                case ("1"):
                    if (tipoTram == "3")
                        claveReposicion = "1024-18-A";
                    else
                        claveReposicion = "1024-18-B";

                    break;
                case ("2"):
                    if (tipoTram == "3")
                        claveReposicion = "1024-18-C";
                    else
                        claveReposicion = "1024-18-D";
                    break;
                case ("3"):
                    if (tipoTram == "3")
                        claveReposicion = "1024-18-E";
                    else
                        claveReposicion = "1024-18-F";
                    break;

            }

            return claveReposicion;
        }

        /// <summary>
        /// Consultar Costo de licencia
        /// </summary>
        /// <param name="TipoLicencia"></param>
        /// <param name="Municipio"></param>
        /// <returns></returns>
        public CaptureInforLic ObtenerDatosPorTipoDeLicencia(String TipoLicencia, String Municipio, string strdescuento, string idtipoTram)
        {
            Municipio = DataPUE.CATALOGOS.cv_mpio;
            if (TipoLicencia != "0" && Municipio != string.Empty)
            {
                DataPUE.calificaCobro result = new DataPUE.calificaCobro();
                if (strdescuento != "0")
                {
                    if (idtipoTram == "3" || idtipoTram == "4")
                    {
                        string ClaveReposicion = dameclaveReposicion(idtipoTram, TipoLicencia);
                        result = DataPUE.I_DATOS_CONTRIBUYENTE.CalificaCobro(string.Empty, ClaveReposicion);
                    }
                    else
                        result = DataPUE.I_DATOS_CONTRIBUYENTE.CalificaCobro(string.Empty, dameclaveDescuento(int.Parse(strdescuento)));
                }
                else if (idtipoTram == "3" || idtipoTram == "4")
                {
                    string ClaveReposicion = dameclaveReposicion(idtipoTram, TipoLicencia);
                    result = DataPUE.I_DATOS_CONTRIBUYENTE.CalificaCobro(string.Empty, ClaveReposicion);
                }
                else
                    result = DataPUE.I_DATOS_CONTRIBUYENTE.CalificaCobro(string.Empty, dameclave(TipoLicencia));

                if (result != null)
                {
                    this._Inciso = result.INCISO;
                    this._Concepto = result.DESCRIPCION;
                    this._Importe = result.VALOR_CALCULO.ToString();
                    this._TotalPagar = result.VALOR_CALCULO.ToString();
                    clave = result.CLAVE;
                }
                else
                {
                    this._Inciso = "";
                    this._Concepto = "";
                    this._Importe = "";
                    this._TotalPagar = "";
                }
            }
            else
            {
                this._Inciso = "";
                this._Concepto = "";
                this._Importe = "";
                this._TotalPagar = "";
            }

            return this;
        }

        public string dameCpXCOlonia(string idCOl)
        {
            string CP = DataPUE.I_MUNICIPIO.dameCPporCOlonia(idCOl);
            return CP;
        }

        /// <summary>
        /// Método para obtener la colonia perteneciente a un municipio
        /// </summary>
        /// <param name="ClaveMunicipio"></param>
        /// <returns></returns>
        public List<DataPUE.Colonia> ObtenerColoniasPorMunicipio(string ClaveMunicipio, string strCP)
        {
            List<DataPUE.Colonia> CbColonia = new List<DataPUE.Colonia>();
            if (ClaveMunicipio != string.Empty)
            {
                CbColonia = DataPUE.I_MUNICIPIO.obtenerColoniasMunicipioPorClave(ClaveMunicipio, strCP);
                // { new DataPUE.Combos() { Identificador =1, Descripcion = "Colonia1" }, new DataPUE.Combos() { Identificador = 2, Descripcion = "Colonia2" } };
                return CbColonia;
            }

            return null;
        }

        /// <summary>
        /// Catalogo para cabello
        /// </summary>
        /// <returns></returns>
        public List<DataPUE.Combos> CatalogoCabello()
        {
            List<DataPUE.Combos> CbCabello = new List<DataPUE.Combos>();
            try
            {

                CbCabello = DataPUE.CATALOGOS.obtenerCatalogoCabello();
                // { new DataPUE.Combos() { Identificador = 1, Descripcion = "Castaño" }, new DataPUE.Combos() { Identificador = 2, Descripcion = "Negro" } };
                return CbCabello;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Catalogo para tipo de sangre
        /// </summary>
        /// <returns></returns>
        public List<DataPUE.Combos> CatalogoSangre()
        {
            List<DataPUE.Combos> CbSangre = new List<DataPUE.Combos>();
            try
            {
                CbSangre = DataPUE.CATALOGOS.obtenerCatalogoTipoSangre();
                // { new DataPUE.Combos() { Identificador = 1, Descripcion = "A-" }, new DataPUE.Combos() { Identificador = 2, Descripcion = "A+" }, new DataPUE.Combos() { Identificador = 3, Descripcion = "B+" }, new DataPUE.Combos() { Identificador = 4, Descripcion = "B-" }, new DataPUE.Combos() { Identificador = 5, Descripcion = "O+" }, new DataPUE.Combos() { Identificador = 6, Descripcion = "O-" } };
                return CbSangre;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Catalogo para municipio
        /// </summary>
        /// <returns></returns>
        public List<DataPUE.Muni> CatalogoMunicipio()
        {
            List<DataPUE.Muni> CbMunicipio = new List<DataPUE.Muni>();
            try
            {
                CbMunicipio = DataPUE.I_MUNICIPIO.obtenerMunicipios();
                //{ new DataPUE.Combos() { Identificador = 1, Descripcion = "Municipio1" }, new DataPUE.Combos() { Identificador = 2, Descripcion = "Municipio2" } };
                return CbMunicipio;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Catalogo para tipo de tramite
        /// </summary>
        /// <returns></returns>
        public List<DataPUE.Combos> CatalogoTipoTramite()
        {
            List<DataPUE.Combos> CbTramite = new List<DataPUE.Combos>();
            try
            {
                // { new DataPUE.Combos() { Identificador = 1, Descripcion = "Nueva" }, new DataPUE.Combos() { Identificador = 2, Descripcion = "Renovacion" } };
                CbTramite = DataPUE.CATALOGOS.obtenerCatalogoTipoTramite();
                return CbTramite;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private Resultado CbTramite;


        public List<DataPUE.Combos> catalogoDescuentos(int id)
        {
            List<DataPUE.Combos> listaDesc = new List<DataPUE.Combos>();

            listaDesc = DataPUE.CATALOGOS.getCatalogoDescuentos(id);
            return listaDesc;
        }

        public bool AplicaRelicencia(string curp, out string strRespuesta)
        {
            bool Aplica = false;
            strRespuesta = string.Empty;
            int Respuesta = DataPUE.CATALOGOS.getAplica(curp, out strRespuesta);
            if (Respuesta == 1 || Respuesta == 2)
                Aplica = true;
            return Aplica;
        }

        /// <summary>
        /// Catalogo para tipo de licencia
        /// </summary>
        /// <returns></returns>
        public List<DataPUE.Combos> CatalogoTipoLicencia()
        {


            List<DataPUE.Combos> listaLic = new List<DataPUE.Combos>();
            try
            {
                //--Valores reales
                //DataPUE.I_TIPOS_LICENCIA.obtenerTiposLicencias().Select(x => new ComboInfo() { Value = x.TIPO_LICENCIA_ID.ToString() + "_" + x.INCISO + "-" + x.SUBINCISO + "-" + x.NIVEL_SUBINCISO, Text = x.DESCRIPCION }).ToList();
                listaLic = DataPUE.I_TIPOS_LICENCIA.obtenerTiposLicencias();
                ////--Valores de prueba
                //var lstTiposLic = new List<ComboInfo>() { new ComboInfo() { Value = "1_1026-6-A)", Text = "AUTOMOVILISTA" },
                //new ComboInfo() { Value = "1_1026-6-B)", Text = "MOTOCICLISTA" }};

                //lstTiposLic.Insert(0, new ComboInfo(){ Value = "0_0", Text = "ELIJA EL TIPO DE LICENCIA"});

                //ReturnInfo.Tipo = TipoResultado.OK;
                //ReturnInfo.Informacion = lstTiposLic;
            }
            catch (Exception ex)
            {
                //ReturnInfo.Tipo = TipoResultado.ERROR;
                //ReturnInfo.Informacion = String.Format("Error al obtener tipo de tramite. Detalles: {0}. {1}", ex.Message, ex.InnerException != null ? ex.InnerException.Message : String.Empty);
                // log
            }
            listaLic.Add(new DataPUE.Combos { Descripcion = "---Seleccione---", Identificador = 0 });
            return listaLic.OrderBy(s => s.Identificador).ToList();

            //try
            //{
            //    List<DataPUE.Combos> CbLicencia = new List<DataPUE.Combos>() { new DataPUE.Combos() { Identificador = "1", Descripcion = "Automovilista" }, new DataPUE.Combos() { Identificador = "2", Descripcion = "Chofer" }, new DataPUE.Combos() { Identificador = "3", Descripcion = "Motociclista" } };
            //    return CbLicencia;
            //}
            //catch (Exception ex)
            //{
            //    return null;
            //}
        }
        #endregion

        public static string damedescCabello(int idcabe)
        {
            string cabello = string.Empty;
            switch (idcabe)
            {
                case (1):
                    cabello = "NEGRO";
                    break;
                case (2):
                    cabello = "CASTAÑO";
                    break;
                case (3):
                    cabello = "RUBIO";
                    break;
                case (4):
                    cabello = "PELIRROJO";
                    break;
            }
            return cabello;
        }

        public void FormatImage(List<ImageDocs> sd, int id)
        {
            DataPUE.I_DOCUMENTOS idoc = new DataPUE.I_DOCUMENTOS();
            DataPUE.I_SCANNER scan = new DataPUE.I_SCANNER();
            byte[] image = null;
            foreach (var item in sd)
            {
                //item.imagen.Save(@"C:\Licencias\Imagenes\img_\rolins.bmp");
                if (item.imagen != null)
                {

                    if (!scan.CheckImage(item.id, id))
                    {
                        image = ConevrtImageBytes(item.imagen);
                        idoc.InsertDocumentos(item.id, image, id);
                    }
                    else
                    {
                        image = ConevrtImageBytes(item.imagen);
                        idoc.UpdateDocuments(item.id, image, id);
                    }
                }
            }

        }

        //public void FormatImage(List<ImageDocs> sd, int id)
        //{
        //    DataPUE.I_DOCUMENTOS idoc = new DataPUE.I_DOCUMENTOS();
        //    byte[] image = null;
        //    foreach (var item in sd)
        //    {
        //        //item.imagen.Save(@"C:\Licencias\Imagenes\img_\rolins.bmp");
        //        image = ConevrtImageBytes(item.imagen);
        //        idoc.InsertDocumentos(item.id, image, id);
        //    }

        //}
        public byte[] ConevrtImageBytes(System.Drawing.Image image)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
            //ImageConverter imgCon = new ImageConverter();
            //byte[] xByte = (byte[])imgCon.ConvertTo(image, typeof(byte[]));
            //return xByte;
        }
    }
    public class ConsultaOrden
    {
        public string Nombre { get; set; }
        public string Tipo_Tramite { get; set; }
        public string Tipo_Licencia { get; set; }
        public string Fecha_Expedicion { get; set; }
        public string Fecha_Vencimiento { get; set; }
        public string Solicitud { get; set; }


    }

    public class Resultado
    {
        public enum Estatus { OK, ERROR }
        public Estatus _Estatus { get; set; }
        public dynamic _Detalles { get; set; }
        public TipoResultado Tipo { get; set; }
        public dynamic Informacion { get; set; }
    }


}
