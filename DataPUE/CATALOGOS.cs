using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
//using Oracle.ManagedDataAccess.Client;
using System.Runtime.Serialization;

namespace DataPUE
{
    public class CATALOGOS
    {
        public static string cv_mpio = string.Empty;
        public static string cv_recaudadora = string.Empty;
        public static string id_Usuario = string.Empty;
        public static string idroli = string.Empty;

        public bool getLogin(string userName, string Pwd)
        {

            bool isOk = false;
            Models.Entities1 db = new Models.Entities1();
            try
            {
                var vista = db.Database.SqlQuery<string>("select ING_PKG_LICENCIAS_VH.ING_AUTENTIFICA_USUARIO ('" + userName + "','" + Pwd + "') FROM DUAL").ToList();
                if (vista[0] != string.Empty)
                {
                    if (vista[0].Substring(0, 1) == "0")
                        isOk = true;

                    string[] strsplit = vista[0].Split(new[] { "||" }, StringSplitOptions.None);
                    if (strsplit.Length > 3)
                    {
                        idroli = strsplit[4];
                        cv_mpio = strsplit[3];
                        cv_recaudadora = strsplit[2];
                        id_Usuario = strsplit[1];
                    }
                }
            }
            catch (Exception ex)
            {
                cv_mpio = "*";
                // QITAME ESTE #*
                //isOk = true;
                //idroli = "3";
                //cv_mpio = "003";
                //cv_recaudadora = "0102";
                //id_Usuario = "26436";
            }

            // List<resultadoListadoBolsasReporteRemision> resultado = db.Database.SqlQuery<resultadoListadoBolsasReporteRemision>("exec obtenerListadoBolsasAcuseRemision @empaqueId", new SqlParameter("@empaqueId", empaqueId.ToString())).ToList();

            return isOk;
        }

        /// <summary>
        /// este metodo ya no va a ir, x q se convierte en txt Método público para obtener el Catálogo de Tipos de Cabello
        /// </summary>
        /// <returns>Listado de Tipo Catalogo</returns>
        public static List<Combos> obtenerCatalogoCabello()
        {
            //Datos de prueba
            List<Combos> listado = new List<Combos>();
            Models.EntitieLocal db = new Models.EntitieLocal();

            //IQueryable<Models.TIPO_CABELLO> este = (IQueryable<Models.TIPO_CABELLO>)db.TIPO_CABELLO.Where(x=> x.ID==1);
            List<Models.TIPO_CABELLO> lst = new List<Models.TIPO_CABELLO>();

            lst = db.TIPO_CABELLO.ToList();
            listado = (from j in lst
                       select new Combos
                       {
                           Identificador = j.ID,
                           Descripcion = j.DESCRIPCION
                       }).ToList();
            //listado.Add(new Catalogo
            //{
            //    Id = "1",
            //    Descripcion = "Castaño"
            //});
            //listado.Add(new Catalogo
            //{
            //    Id = "2",
            //    Descripcion = "Negro"
            //});
            //listado.Add(new Catalogo
            //{
            //    Id = "3",
            //    Descripcion = "Pelirrojo"
            //});

            listado.Add(new Combos { Descripcion = "---Seleccione---", Identificador = 0 });
            return listado.OrderBy(s => s.Identificador).ToList();


        }

        /// <summary>
        /// Método público para obtener el Catálogo de Tipos de Sangre
        /// </summary>
        /// <returns>Listado de Tipo Catalogo</returns>
        public static List<Combos> obtenerCatalogoTipoSangre()
        {
            //Datos de prueba
            List<Combos> listadoReturn = new List<Combos>();
            List<Models.TIPOS_SANGRE> listado = new List<Models.TIPOS_SANGRE>();
            Models.EntitieLocal db = new Models.EntitieLocal();
            listado = db.TIPOS_SANGRE.ToList();
            listadoReturn = (from l in listado
                             select new Combos
                             {
                                 Identificador = l.ID,
                                 Descripcion = l.DESCRIPCION
                             }).ToList();

            listadoReturn.Add(new Combos { Descripcion = "---Seleccione---", Identificador = 0 });
            return listadoReturn.OrderBy(s => s.Identificador).ToList();


        }

        /// <summary>
        /// este metodo ya no va a ir se quita el combo
        /// Método público para obtener el Catálogo de Tipos de Pago
        /// </summary>
        /// <returns>Listado de Tipo Catalogo</returns>
        public static List<Catalogo> obtenerCatalogoTipoPago()
        {
            //Datos de prueba
            List<Catalogo> listado = new List<Catalogo>();
            //listado.Add(new Catalogo
            //{
            //    Id = "1",
            //    Descripcion = "Efectivo"
            //});
            //listado.Add(new Catalogo
            //{
            //    Id = "2",
            //    Descripcion = "Debito"
            //});
            //listado.Add(new Catalogo
            //{
            //    Id = "3",
            //    Descripcion = "INAPAM"
            //});

            return listado;
        }
        public static string getClaveDescuento(int idDesc)
        {
            Models.EntitieLocal db = new Models.EntitieLocal();
            Combos str = (from n in db.DESCRIPCION_DESCUENTO where n.ID == idDesc select new Combos { Descripcion = n.CLAVE, Identificador = n.ID }).FirstOrDefault();

            return str.Descripcion;
        }

        public static string getclavelic(int typelic)
        {
            Models.EntitieLocal db = new Models.EntitieLocal();
            Combos str = (from n in db.TIPO_LICENCIAS where n.ID == typelic select new Combos { Descripcion = n.INCISO_LICENCIA, Identificador = n.ID }).FirstOrDefault();

            return str.Descripcion;
        }

        public static string getEstado(string abreviatura)
        {
            string StrEstado = string.Empty;
            if (abreviatura != "NE")
            {
                Models.Entities1 db = new Models.Entities1();
                StrEstado = db.Database.SqlQuery<string>("select DESCRIPCION FROM ING_ESTADOS WHERE ABREVIATURA_CURP='" + abreviatura + "'").FirstOrDefault();

            }
            else
                StrEstado = "EXTRANJERO";

            if (string.IsNullOrEmpty(StrEstado))
                StrEstado = string.Empty;


            return StrEstado.ToUpper();
        }

        public static string getNombre(string idUser)
        {
            string nombre = string.Empty;
            Models.Entities1 db = new Models.Entities1();

            nombre = db.Database.SqlQuery<string>("select ING_USUARIO_NOMBRE('" + idUser + "') FROM DUAL ").FirstOrDefault();
            return nombre;
        }

        public static int getAplica(string Curpi, out string strXsiAplica)
        {
            strXsiAplica = string.Empty;
            Models.Entities1 db = new Models.Entities1();
            int Apli = 0;
            try
            {
                var vista = db.Database.SqlQuery<string>("select valida_relicenciamiento('" + Curpi + "') FROM DUAL").ToList();
                string[] strsplit = vista[0].Replace("||", "|").Split('|');
                Apli = int.Parse(strsplit[0].ToString());
                strXsiAplica = vista[0].Replace("||", "|").ToString();
            }
            catch (Exception ex) { }
            return Apli;
        }

        public static List<Combos> getCatalogoDescuentos(int idselected)
        {
            Models.EntitieLocal db = new Models.EntitieLocal();
            List<Combos> listado = new List<Combos>();

            listado = (from n in db.DESCRIPCION_DESCUENTO
                       where n.TIPO_LICENCIAS_ID == idselected
                       select new Combos
                       {
                           Identificador = n.ID,
                           Descripcion = n.DESCRIPCION
                       }).ToList();


            if (listado.Count == 0)
            {
                listado.Add(new DataPUE.Combos { Descripcion = "---Seleccione---", Identificador = 0 });
                listado.Add(new DataPUE.Combos { Descripcion = "SIN DESCUENTO", Identificador = 0 });
            }
            else
                listado.Add(new DataPUE.Combos { Descripcion = "SIN DESCUENTO", Identificador = 0 });

            return listado;
        }

        /// <summary>
        ///este metodo se va a consumir desde la bd de Gob
        ///Método público para obtener el Catálogo de Tipos de Trámite
        /// </summary>
        /// <returns>Listado de Tipo Catalogo</returns>
        public static List<Combos> obtenerCatalogoTipoTramite()
        {
            //Models.EntitieLocal dblo = new Models.EntitieLocal();
            //var todo = dblo.AVISO_ENTERO.FirstOrDefault();



            //Datos de prueba
            Models.EntitieLocal db = new Models.EntitieLocal();
            List<Combos> listado = new List<Combos>();
            listado = (from n in db.TIPO_TRAMITE
                       select new Combos
                       {

                           Identificador = n.ID,
                           Descripcion = n.DESCRIPCION
                       }).ToList();
            //listado.Add(new Catalogo
            //{
            //    Id = "1",
            //    Descripcion = "Renovación"
            //});
            //listado.Add(new Catalogo
            //{
            //    Id = "2",
            //    Descripcion = "Nueva"
            //});
            listado.Add(new Combos { Descripcion = "---Seleccione---", Identificador = 0 });
            return listado.OrderBy(s => s.Identificador).ToList();

        }
        public static List<Combos> obtieneEntidades()
        {
            Models.EntitieLocal db = new Models.EntitieLocal();
            List<Combos> listado = new List<Combos>();
            listado = (from v in db.ENTIDADES
                       select new Combos
                       {
                           Identificador = v.ENTIDAD_ID,
                           Descripcion = v.ENTIDAD
                       }).ToList();

            return listado;
        }

        public static List<Catalogo> obtentipoLicencias()
        {
            Models.EntitieLocal db = new Models.EntitieLocal();
            List<Catalogo> listado = new List<Catalogo>();
            listado = (from t in db.TIPO_LICENCIAS
                       select new Catalogo
                       {
                           // Id = t.ID,
                           Descripcion = t.DESCRIPCION
                       }).ToList();
            listado.Add(new Catalogo { Descripcion = "---Seleccione---", Id = "0" });
            return listado.OrderBy(s => s.Id).ToList();

        }


        public static List<Combos> obtenerPais()
        {
            Models.EntitieLocal db = new Models.EntitieLocal();
            List<DataPUE.Combos> CatPais = new List<DataPUE.Combos>();//{ new DataPUE.Combos() { Identificador = 1, Descripcion = "Pais 1" }, new DataPUE.Combos() { Identificador = 2, Descripcion = "Pais 2" } };
            CatPais = (from j in db.NACIONALIDAD
                       select new Combos
                       {
                           Identificador = j.NACIONALIDAD_ID,
                           Descripcion = j.PAIS
                       }).ToList();//.OrderBy(t=> t.Descripcion).ToList();

            CatPais.Add(new Combos { Descripcion = "---Seleccione---", Identificador = 0 });
            return CatPais.OrderBy(s => s.Identificador).ToList();

        }
    }

    [DataContract]
    public class RECORD
    {
        [DataMember]
        public int ID_RFC { get; set; }
        [DataMember]
        public string RFC { get; set; }
        [DataMember]
        public string RFC_LETRA { get; set; }
        [DataMember]
        public string RFC_NUM { get; set; }
        [DataMember]
        public string RFC_HOMO { get; set; }
        [DataMember]
        public string CURP { get; set; }
        [DataMember]
        public string NOMBRE { get; set; }
        [DataMember]
        public string NOMBRE_2 { get; set; }
        [DataMember]
        public string APELLIDO_P { get; set; }
        [DataMember]
        public string APELLIDO_M { get; set; }
        [DataMember]
        public string CALLE { get; set; }
        [DataMember]
        public string CALLE_PPAL { get; set; }
        [DataMember]
        public string REFERENCIA1 { get; set; }
        [DataMember]
        public string REFERENCIA2 { get; set; }
        [DataMember]
        public string NO_EXTERIOR { get; set; }
        [DataMember]
        public string NO_INTERIOR { get; set; }
        [DataMember]
        public int? CVE_COLONIA { get; set; }
        [DataMember]
        public string CODIGO_POSTAL { get; set; }
        [DataMember]
        public string TELEFONO { get; set; }
        [DataMember]
        public string TIPO_CONTRIBUYENTE { get; set; }
        [DataMember]
        public string E_MAIL { get; set; }
        [DataMember]
        public string TELEFONO_MOVIL { get; set; }
        [DataMember]
        public string PAIS_ORIGEN { get; set; }
        [DataMember]
        public string PAIS_RECIDENCIA { get; set; }
        [DataMember]
        public string PENSIONADO { get; set; }
        [DataMember]
        public string CVE_EDO { get; set; }
        [DataMember]
        public string CVE_MPIO { get; set; }
        [DataMember]
        public string CVE_LOC { get; set; }
        [DataMember]
        public string CVE_REC { get; set; }
        [DataMember]
        public int? CREADO_POR { get; set; }
        [DataMember]
        public string FECHA_CREACION { get; set; }
        [DataMember]
        public int? MODIFICADO_POR { get; set; }
        [DataMember]
        public string FECHA_MODIFICACION { get; set; }
        [DataMember]
        public string FECHA_NACIM { get; set; }


    }
}

