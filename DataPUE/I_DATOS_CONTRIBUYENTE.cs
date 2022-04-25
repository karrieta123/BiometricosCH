using DataPUE.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Validation;
using System.Globalization;
using System.IO;
using System.Linq;
//using System.Data.OracleClient;
using System.Runtime.Serialization.Json;
using System.Text;

namespace DataPUE
{
    public class I_DATOS_CONTRIBUYENTE
    {

        public static void cierraClico(string idSol, string folioLaser)
        {
            int estatus = 0;
            Oracle.DataAccess.Client.OracleConnection conn = new Oracle.DataAccess.Client.OracleConnection(GetCadena());
            conn.Open();
            Oracle.DataAccess.Client.OracleCommand cmd = new Oracle.DataAccess.Client.OracleCommand("ING_PKG_LICENCIAS_VH.ING_VALIDA_TRAMITES", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            Oracle.DataAccess.Client.OracleParameter[] param = new Oracle.DataAccess.Client.OracleParameter[4];// new Oracle.DataAccess.Client.OracleParameter[2]();


            param[0] = new Oracle.DataAccess.Client.OracleParameter();
            param[0].ParameterName = ":P_USUARIO_ID";
            param[0].OracleDbType = Oracle.DataAccess.Client.OracleDbType.Decimal;
            param[0].Value = CATALOGOS.id_Usuario;
            param[0].Direction = ParameterDirection.Input;
            param[1] = new Oracle.DataAccess.Client.OracleParameter();
            param[1].ParameterName = ":P_CVE_REC";
            param[1].OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
            param[1].Value = CATALOGOS.cv_recaudadora;
            param[1].Direction = ParameterDirection.Input;
            param[2] = new Oracle.DataAccess.Client.OracleParameter();
            param[2].ParameterName = ":P_NO_AVISO";
            param[2].OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
            param[2].Value = idSol;
            param[2].Direction = ParameterDirection.Input;
            param[3] = new Oracle.DataAccess.Client.OracleParameter();
            param[3].ParameterName = ":P_FOLIO_LICENCIA";
            param[3].OracleDbType = Oracle.DataAccess.Client.OracleDbType.Decimal;
            param[3].Value = folioLaser;
            param[3].Direction = ParameterDirection.Input;
            cmd.Parameters.Add(param[0]);
            cmd.Parameters.Add(param[1]);
            cmd.Parameters.Add(param[2]);
            cmd.Parameters.Add(param[3]);

            try
            {
                Oracle.DataAccess.Client.OracleDataReader rdr = cmd.ExecuteReader();
                estatus = 1;

            }
            catch (Oracle.DataAccess.Client.OracleException ex)
            {

            }
            cmd.Dispose();

        }

        public static void cierraReimpresion(string curp, int tramite, string userOperador)
        {
            Models.Entities1 db = new Entities1();
            Oracle.DataAccess.Client.OracleConnection conn = new Oracle.DataAccess.Client.OracleConnection(GetCadena());
            conn.Open();
            Oracle.DataAccess.Client.OracleCommand cmd = new Oracle.DataAccess.Client.OracleCommand("CIERRA_RELICENCIAMIENTO", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            Oracle.DataAccess.Client.OracleParameter[] param = new Oracle.DataAccess.Client.OracleParameter[3];// new Oracle.DataAccess.Client.OracleParameter[2]();


            param[0] = new Oracle.DataAccess.Client.OracleParameter();
            param[0].ParameterName = ":P_CURP";
            param[0].OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
            param[0].Value = curp;
            param[0].Direction = ParameterDirection.Input;
            param[1] = new Oracle.DataAccess.Client.OracleParameter();
            param[1].ParameterName = ":ID_TRAMITE";
            param[1].OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
            param[1].Value = tramite;
            param[1].Direction = ParameterDirection.Input;
            param[2] = new Oracle.DataAccess.Client.OracleParameter();
            param[2].ParameterName = ":P_ID_USUARIO";
            param[2].OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
            param[2].Value = userOperador;
            param[2].Direction = ParameterDirection.Input;
            cmd.Parameters.Add(param[0]);
            cmd.Parameters.Add(param[1]);
            cmd.Parameters.Add(param[2]);
            try
            {
                Oracle.DataAccess.Client.OracleDataReader rdr = cmd.ExecuteReader();
            }
            catch (Exception ex)
            { }
            cmd.Dispose();
        }


        public static int dameFolioxRecaudadora()
        {
            int folio = 0;
            Models.EntitieLocal db = new EntitieLocal();
            try
            {
                var foli = (from i in db.INVENTARIO where i.CVE_REC == CATALOGOS.cv_recaudadora select new Combos { Identificador = i.FOLIO_ACT, Descripcion = i.CVE_REC }).FirstOrDefault();
                folio = int.Parse(foli.Identificador.ToString());
            }
            catch (Exception ex)
            {

            }

            return folio;
        }

        public static int ingresaFolio(int idtramite, string idUser, string rec, int folio)
        {
            int idLaser = 0;
            EntitieLocal db = new EntitieLocal();
            //List<NUMERO_FOLIO> lst=  db.NUMERO_FOLIO.Where(j=> j.IDTRAMITE==idtramite && j.ESACTIVO==1).ToList();
            //if (lst.Count == 0)
            //{
            var numfolio = new DataPUE.Models.NUMERO_FOLIO { IDFOLIO = folio, DESCRIPCION = string.Empty, ESACTIVO = 1, IDTRAMITE = idtramite, USUARIOCREO = idUser, FECHACREADO = DateTime.Now, CLAVE_REC = rec };

            db.NUMERO_FOLIO.Add(numfolio);
            db.SaveChanges();

            var inventa = db.INVENTARIO.First(c => c.FOLIO_ACT == folio && c.CVE_REC == rec);
            inventa.FOLIO_ACT = folio + 1;
            db.SaveChanges();

            //var laser = (from l in db.NUMERO_FOLIO
            //             where (l.IDTRAMITE == idtramite && l.ESACTIVO == 1)
            //             select new Combos
            //             {
            //                 Identificador = l.IDFOLIO,
            //                 Descripcion = l.DESCRIPCION
            //             }).ToList();
            ////db.NUMERO_FOLIO.Where(l => l.IDTRAMITE == idtramite && l.ESACTIVO == 1).FirstOrDefault();
            //idLaser = int.Parse(laser[0].Identificador.ToString());
            //}
            //else
            //    idLaser = int.Parse(lst[0].IDFOLIO.ToString());

            return idLaser;

        }

        public static int UpgradeFolioReturnNew(int idTramite, string desc, string idUser, int folio)
        {
            int idLaser = 0;
            Models.EntitieLocal db = new EntitieLocal();
            try
            {
                //var laserAnterior = db.NUMERO_FOLIO.First(f => f.IDTRAMITE == idTramite && f.ESACTIVO == 1);
                //laserAnterior.ESACTIVO = 0;
                //laserAnterior.DESCRIPCION = desc;
                //db.SaveChanges();


                var numfolio = new DataPUE.Models.NUMERO_FOLIO { IDFOLIO = folio, DESCRIPCION = desc, ESACTIVO = 0, IDTRAMITE = idTramite, USUARIOCREO = idUser, FECHACREADO = DateTime.Now };

                db.NUMERO_FOLIO.Add(numfolio);
                db.SaveChanges();

                var inventa = db.INVENTARIO.First(c => c.FOLIO_ACT == folio && c.CVE_REC == CATALOGOS.cv_recaudadora);
                inventa.FOLIO_ACT = folio + 1;
                db.SaveChanges();

                //var idnuevo = (from l in db.NUMERO_FOLIO
                //               where (l.ESACTIVO == 1 && l.DESCRIPCION == null && l.IDTRAMITE == idTramite && l.USUARIOCREO == idUser)
                //               select new Combos { 
                //                   Identificador=l.IDFOLIO,
                //                   Descripcion=l.DESCRIPCION
                //               }).FirstOrDefault();

                //idLaser =int.Parse(idnuevo.Identificador.ToString());

            }
            catch (Exception ex)
            { }

            return idLaser;
        }

        public static string descripcionCOl(string idcol, string muni)
        {
            string strdesc = string.Empty;
            Models.Entities1 db = new Entities1();

            strdesc = db.Database.SqlQuery<string>("SELECT DESCRIPCION FROM ING_COLONIAS WHERE CVE_COL=" + idcol + " AND CVE_MPIO=" + muni).FirstOrDefault();

            return strdesc;
        }

        public static List<Muni> lstBuscaCP(string cp, out List<Colonia> cols, string ColoniaId, int XdondeEs, string municipio)
        {
            List<Muni> lst = new List<Muni>();
            cols = new List<Colonia>();
            try
            {

                Models.Entities1 db = new Entities1();
                if (XdondeEs == 0)
                {
                    cols = db.Database.SqlQuery<Colonia>("select CVE_COL, DESCRIPCION, CVE_MPIO FROM ING_COLONIAS WHERE CODIGO_POSTAL='" + cp + "'").ToList();
                    //var query = cols.GroupBy(o => new {o.CVE_MPIO })
                    //                  .Select(o => o.FirstOrDefault());
                    //List<Colonia> LSTcOL = (Colonia)query.c.ToList();
                    //string strIN = string.Empty;
                    //for (int i = 0; i < query.Count(); i++)
                    //{
                    //    if (i + 1 == query.Count())
                    //        strIN = "'" + query[i].CVE_MPIO + "'";
                    //}
                    if (cols.Count > 0)
                        lst = db.Database.SqlQuery<Muni>("select CVE_MPIO,DESCRIPCION FROM ING_MUNICIPIOS WHERE CVE_MPIO='" + cols[0].CVE_MPIO + "' AND CVE_EDO='03'").ToList();
                }
                else
                {
                    cols = db.Database.SqlQuery<Colonia>("select CVE_COL, DESCRIPCION, CVE_MPIO FROM ING_COLONIAS WHERE CVE_COL='" + ColoniaId + "' AND CVE_MPIO='" + municipio + "' AND CVE_EDO='03'").ToList();
                    if (cols.Count > 0)
                        lst = db.Database.SqlQuery<Muni>("select CVE_MPIO,DESCRIPCION FROM ING_MUNICIPIOS WHERE CVE_MPIO='" + cols[0].CVE_MPIO + "' AND CVE_EDO='03'").ToList();
                }
            }
            catch (Exception ex) { }

            return lst;

        }

        public static int insertaContribuyenteGobierno(RECORD rfc)
        {
            int estatus = 0;
            object OBJECT = rfc;
            string xml = string.Empty;

            //XmlSerializer xsSubmit = new XmlSerializer(typeof(RFCLIC));
            //var subReq = rfc;//new RFCLIC();
            //using (StringWriter sww = new StringWriter())
            //using (XmlWriter writer = XmlWriter.Create(sww))
            //{
            //    xsSubmit.Serialize(writer, subReq);
            //     xml = sww.ToString(); // Your XML
            //}
            CultureInfo ci = new CultureInfo("en-US");

            // rfc.FECHA_MODIFICACION = DateTime.Now.ToString("dd-MMM-yyyy", ci);
            rfc.FECHA_NACIM = DateTime.Now.ToString("dd-MMM-yyyy", ci);
            rfc.TIPO_CONTRIBUYENTE = "f";
            rfc.PENSIONADO = "N";
            // string P_ERROR = "_";
            string json = Json.Serialize<RECORD>(rfc);
            Oracle.DataAccess.Client.OracleConnection conn = new Oracle.DataAccess.Client.OracleConnection(GetCadena());
            //  //System.Data.OracleClient.OracleConnection conn = new OracleConnection("DATA SOURCE=3.5.1.4:1521/DESA;PERSIST SECURITY INFO=True;USER ID=LICENCIASM; PASSWORD=MUNICIPALES");
            //    // conn.ConnectionString = "context connection=true";
            conn.Open();

            //    // Create and execute a command
            Oracle.DataAccess.Client.OracleCommand cmd = new Oracle.DataAccess.Client.OracleCommand("ING_PKG_LICENCIAS_VH.ING_INSERTA_RFC_LICENCIA", conn);
            //   // OracleCommand cmd = conn.CreateCommand();
            //    //cmd.CommandText = "execute ING_PKG_LICENCIAS_VH.ING_INSERTA_RFC_LICENCIA";
            cmd.CommandType = CommandType.StoredProcedure;
            Oracle.DataAccess.Client.OracleParameter[] param = new Oracle.DataAccess.Client.OracleParameter[1];// new Oracle.DataAccess.Client.OracleParameter[2]();


            param[0] = new Oracle.DataAccess.Client.OracleParameter();

            param[0].ParameterName = ":P_CADENA_JSON";
            param[0].OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
            param[0].Size = 2800;
            param[0].Value = json;
            param[0].Direction = ParameterDirection.Input;
            //param[1] = new Oracle.DataAccess.Client.OracleParameter();
            //param[1].ParameterName = ":P_ERROR";
            //param[1].OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
            //param[1].Value = P_ERROR;
            //param[1].Direction = ParameterDirection.Output;
            cmd.Parameters.Add(param[0]);
            // cmd.Parameters.Add(param[1]);
            //    //param.UdtTypeName = "RECORD";

            //    //OracleParameter param = new OracleParameter(":P_REG", OBJECT);
            //    //param.Direction = System.Data.ParameterDirection.Input;
            //    //param.ResetOracleType();
            //    //param.ResetDbType();         

            //    //var output = new System.Data.OracleClient.OracleParameter
            //    //{
            //    //    UdtTypeName = "PREFS.PREFERENCE_LIST",
            //    //    ParameterName = "P_REG",
            //    //    OracleDbType = OracleDbType.Object,
            //    //    Direction = ParameterDirection.Input
            //    //};
            //    //cmd.Parameters.Add(output);
            ////   cmd.Parameters.Add(":P_REG",System.Data.OracleClient.OracleType.Blob ).Value=OBJECT.GetType();
            //    //cmd.Parameters.Add(param);

            //    //cmd.ExecuteNonQuery();


            //    //////cmd.Parameters.AddWithValue(":P_REG", OBJECT);
            try
            {
                Oracle.DataAccess.Client.OracleDataReader rdr = cmd.ExecuteReader();
                estatus = 1;
                //cmd.ExecuteNonQuery();
            }
            catch (Oracle.DataAccess.Client.OracleException ex)
            {

            }

            //while (rdr.Read())
            //    // department_id = rdr.GetInt32(0);

            //    rdr.Close();
            cmd.Dispose();

            return estatus;
            // Models.Entities1 db = new Entities1();
            //var paramer = new Oracle.ManagedDataAccess.Client.OracleParameter(":P_REG",Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2);
            //paramer.Direction = System.Data.ParameterDirection.Input;
            //paramer.Value = rfc;

            //var  P_ERRORvar= db.Database.SqlQuery<string>("exec ING_PKG_LICENCIAS_VH.ING_INSERTA_RFC_LICENCIA(" + json + "," + P_ERROR + ") ");
        }

        public static int insertacontribuyente(DATOS_CONTRIBUYENTE data)
        {
            EntitieLocal db = new EntitieLocal();
            try
            {
                db.DATOS_CONTRIBUYENTE.Add(data);
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
                return 0;
            }




            var idref = db.DATOS_CONTRIBUYENTE.Where(d => d.RFC == data.RFC).FirstOrDefault();
            return (int)idref.DATOS_CONTRIBUYENTE_ID;
            // return 0;
        }

        public static string dameCurp(int idSol)
        {
            Models.EntitieLocal db = new EntitieLocal();
            var Ciudadano = db.TRAMITES.Where(t => t.ID == idSol).FirstOrDefault();
            return Ciudadano.CIUDADANO.CURP;
        }

        public static bool actualizaCiudadano(Models.CIUDADANO ActualizaCiud, int idsolicitud, int isciudadano)
        {
            bool actualizo = false;
            Models.EntitieLocal db = new EntitieLocal();
            try
            {
                int idCiudadano = 0;

                var tramites = db.TRAMITES.Where(t => t.ID == idsolicitud).FirstOrDefault();
                if (tramites != null)
                    idCiudadano = int.Parse(tramites.CIUDADANO.ID.ToString());
                if (isciudadano != 0)
                    idCiudadano = isciudadano;


                //var ciudadanoUpgrade = db.CIUDADANO.First(f => f.ID == tramites.CIUDADANO.ID);
                var ciudadanoUpgrade = db.CIUDADANO.First(f => f.ID == idCiudadano);
                ciudadanoUpgrade.MUNICIPIO = ActualizaCiud.MUNICIPIO;
                ciudadanoUpgrade.NOMBRE = ActualizaCiud.NOMBRE;
                ciudadanoUpgrade.NUM_EXT = ActualizaCiud.NUM_EXT;
                ciudadanoUpgrade.NUM_INT = ActualizaCiud.NUM_INT;
                ciudadanoUpgrade.PROFESION = ActualizaCiud.PROFESION;
                ciudadanoUpgrade.SENAS_PARTICULARES = ActualizaCiud.SENAS_PARTICULARES;
                ciudadanoUpgrade.SEXO = ActualizaCiud.SEXO;
                ciudadanoUpgrade.TELEFONO = ActualizaCiud.TELEFONO;
                ciudadanoUpgrade.TIPO_SANGRE = ActualizaCiud.TIPO_SANGRE;
                ciudadanoUpgrade.USA_LENTES = ActualizaCiud.USA_LENTES;
                ciudadanoUpgrade.ALERGIAS = ActualizaCiud.ALERGIAS;
                ciudadanoUpgrade.ALTURA = ActualizaCiud.ALTURA;
                if (!string.IsNullOrEmpty(ActualizaCiud.APELLIDO_MATERNO))
                    ciudadanoUpgrade.APELLIDO_MATERNO = ActualizaCiud.APELLIDO_MATERNO;
                else
                    ciudadanoUpgrade.APELLIDO_MATERNO = ".";
                ciudadanoUpgrade.APELLIDO_PATERNO = ActualizaCiud.APELLIDO_PATERNO;
                ciudadanoUpgrade.CABELLO = ActualizaCiud.CABELLO;
                ciudadanoUpgrade.CODIGO_POSTAL = ActualizaCiud.CODIGO_POSTAL;
                ciudadanoUpgrade.COLONIA = ActualizaCiud.COLONIA;
                ciudadanoUpgrade.CURP = ActualizaCiud.CURP;
                ciudadanoUpgrade.DIRECCION = ActualizaCiud.DIRECCION;
                ciudadanoUpgrade.DONADOR_ORGANOS = ActualizaCiud.DONADOR_ORGANOS;
                ciudadanoUpgrade.EMAIL = ActualizaCiud.EMAIL;
                ciudadanoUpgrade.ENTRE_CALLE1 = ActualizaCiud.ENTRE_CALLE1;
                ciudadanoUpgrade.ENTRE_CALLE2 = ActualizaCiud.ENTRE_CALLE2;
                ciudadanoUpgrade.FECHA_NACIMIENTO = ActualizaCiud.FECHA_NACIMIENTO;
                ciudadanoUpgrade.LOCALIDAD = ActualizaCiud.LOCALIDAD;
                db.SaveChanges();

                actualizo = true;
            }
            catch (Exception ex)
            { }

            return actualizo;
        }

        public static int guardaCiudadano(Models.CIUDADANO ingresaCiudadano)
        {
            //ingresaCiudadano.ID = 1;
            DataPUE.Models.EntitieLocal db = new DataPUE.Models.EntitieLocal();
            try
            {
                db.CIUDADANO.Add(ingresaCiudadano);
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

            int id = 0;
            try
            {
                var ciudadano = db.CIUDADANO.Where(y => y.CURP == ingresaCiudadano.CURP).Select(s => s.ID).FirstOrDefault();
                id = Convert.ToInt32(ciudadano);
            }
            catch (Exception ex)
            {
            }
            return id;
        }

        /// <summary>
        /// Método público para completar los datos de contribuyente
        /// </summary>
        /// <param name="numeroAvisoEntero">Numero de Aviso de Entero perteneciente al registro</param>
        /// <param name="rfc">RFC</param>
        /// <param name="numeroInterior">Número interior de la dirección</param>
        /// <param name="coloniaId">Identificador de Colonia</param>
        /// <param name="codigoPostal">Código Postal</param>
        /// <param name="localidad">Identificador de la localidad</param>
        /// <param name="observaciones">Observaciones</param>
        /// <param name="telefono">Número Telefónico</param>
        /// <param name="identificadorNacionalidad">Identificador de la Nacionalidad</param>
        /// <param name="estadoId">Identificador del Estado</param>
        /// <param name="municipio">Identificador del municipio</param>
        /// <param name="oficio">Oficio</param>
        /// <param name="usaLentes">Bandera para indicar si la persona utiliza Lentes</param>
        /// <param name="donadorOrganos">Bandera para indicar si la persona es donadora de organos</param>
        /// <param name="alergias">Alergias de la persona</param>
        /// <param name="cabello">Descripción Cabello de la persona</param>
        /// <param name="senas">Señas particulares de la persona</param>
        /// <param name="sangre">Tipo de sangre de la persona</param>
        /// <param name="altura">Altura de la persona</param>
        /// <param name="email">Dirección de correo electrónico de la persona</param>
        /// <returns>Identificador del registro</returns>
		public static int agregarDatosContribuyente(int numeroAvisoEntero, string rfc, string numeroInterior, int coloniaId, string codigoPostal, int localidad, string observaciones, string telefono, int identificadorNacionalidad, int estadoId, int municipio, string oficio, bool usaLentes, bool donadorOrganos, string alergias, string cabello, string senas, string sangre, string altura, string email)
        {
            try
            {
                bool existe = true;
                Models.EntitieLocal db = new Models.EntitieLocal();
                //Obteniendo aviso de entero
                Models.CIUDADANO aviso = db.CIUDADANO.Where(x => x.ID == numeroAvisoEntero).SingleOrDefault();
                //Si no existe el aviso de entero se genera un error
                if (aviso == null)
                    throw new Exception("El aviso de entero especificado es incorrecto");
                //Validando existencia de Datos Contribuyente
                Models.DATOS_CONTRIBUYENTE datos = new Models.DATOS_CONTRIBUYENTE(); //db.DATOS_CONTRIBUYENTE.Where(x => x.AVISO_ENTERO_ID == aviso.ID).FirstOrDefault();
                if (datos == null)
                {
                    datos = new Models.DATOS_CONTRIBUYENTE();
                    //Relacionando aviso de entero con datos de contribuyente
                    //  datos.AVISO_ENTERO_ID = aviso.AVISOENTERO_ID;
                    existe = false;
                }
                //Llenando datos
                datos.RFC = rfc;
                datos.NUMERO_INTERIOR = numeroInterior;
                datos.COLONIA_ID = coloniaId;
                datos.CODIGO_POSTAL = codigoPostal;
                datos.LOCALIDAD = localidad;
                // datos.OBSERVACIONES = observaciones;
                datos.TELEFONO = telefono;
                datos.NACIONALIDAD_ID = identificadorNacionalidad;
                datos.ESTADO_ID = estadoId;
                datos.MUNICIPIO = municipio;
                // datos.OFICIO = oficio;
                //datos.USA_LENTES = usaLentes ? 1 : 0;
                //datos.DONADOR = donadorOrganos ? 1 : 0;
                //datos.ALERGIAS = alergias;
                //datos.CABELLO = cabello;
                //datos.SENAS = senas;
                //datos.SANGRE = sangre;
                //datos.ALTURA = altura;
                datos.EMAIL = email;
                //datos.AVISO_ENTERO_ID = numeroAvisoEntero;
                //Si aun no existe un registro, se añade
                if (!existe)
                    db.DATOS_CONTRIBUYENTE.Add(datos);
                db.SaveChanges();
                //  db.Database.SqlQuery<RFCLIC>("exec ING_PKG_LICENCIAS_VH.ING_INSERTA_RFC_LICENCIA @empaqueId", new SqlParameter("@empaqueId", empaqueId.ToString())).ToList();
                //Obteniendo Id generado
                decimal? id = 0;// buscarDatosContribuyentePorAvisoEntero(numeroAvisoEntero).DATOS_CONTRIBUYENTE_ID;

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
        /// Método público para modificar los datos de contribuyente
        /// </summary>
        /// <param name="identificadorDatosContribuyente">Identificador del registro a modificar</param>
        /// <param name="rfc">RFC</param>
        /// <param name="numeroInterior">Número interior de la dirección</param>
        /// <param name="coloniaId">Identificador de Colonia</param>
        /// <param name="codigoPostal">Código Postal</param>
        /// <param name="localidad">Identificador de la localidad</param>
        /// <param name="observaciones">Observaciones</param>
        /// <param name="telefono">Número Telefónico</param>
        /// <param name="identificadorNacionalidad">Identificador de la Nacionalidad</param>
        /// <param name="estadoId">Identificador del Estado</param>
        /// <param name="municipio">Identificador del municipio</param>
        /// <param name="oficio">Oficio</param>
        /// <param name="usaLentes">Bandera para indicar si la persona utiliza Lentes</param>
        /// <param name="donadorOrganos">Bandera para indicar si la persona es donadora de organos</param>
        /// <param name="alergias">Alergias de la persona</param>
        /// <param name="cabello">Descripción Cabello de la persona</param>
        /// <param name="senas">Señas particulares de la persona</param>
        /// <param name="sangre">Tipo de sangre de la persona</param>
        /// <param name="altura">Altura de la persona</param>
        /// <param name="email">Dirección de correo electrónico de la persona</param>
        /// <returns>Identificador del registro</returns>
        public static int modificarDatosContribuyente(int identificadorDatosContribuyente, string rfc, string numeroInterior, int coloniaId, string codigoPostal, int localidad, string observaciones, string telefono, int identificadorNacionalidad, int estadoId, int municipio, string oficio, bool usaLentes, bool donadorOrganos, string alergias, string cabello, string senas, string sangre, string altura, string email)
        {
            try
            {
                Models.EntitieLocal db = new Models.EntitieLocal();
                //Obteniendo registro de datos de contribuyente
                Models.DATOS_CONTRIBUYENTE datos = db.DATOS_CONTRIBUYENTE.Where(x => x.DATOS_CONTRIBUYENTE_ID == identificadorDatosContribuyente).SingleOrDefault();
                //Si no existe el aviso de entero se genera un error
                if (datos == null)
                    throw new Exception("El registro especificado es incorrecto");

                //Modificando datos
                datos.RFC = rfc;
                datos.NUMERO_INTERIOR = numeroInterior;
                datos.COLONIA_ID = coloniaId;
                datos.CODIGO_POSTAL = codigoPostal;
                datos.LOCALIDAD = localidad;
                // datos.OBSERVACIONES = observaciones;
                datos.TELEFONO = telefono;
                datos.NACIONALIDAD_ID = identificadorNacionalidad;
                datos.ESTADO_ID = estadoId;
                datos.MUNICIPIO = municipio;
                //datos.OFICIO = oficio;
                //datos.USA_LENTES = usaLentes ? 1 : 0;
                //datos.DONADOR = donadorOrganos ? 1 : 0;
                //datos.ALERGIAS = alergias;
                //datos.CABELLO = cabello;
                //datos.SENAS = senas;
                //datos.SANGRE = sangre;
                //datos.ALTURA = altura;
                datos.EMAIL = email;
                //Guardando cambios
                db.SaveChanges();
                //Regresando Id
                return Convert.ToInt32(datos.DATOS_CONTRIBUYENTE_ID);
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


        public static Models.TRAMITES buscaTramitePorCiudadano(int IdCiud)
        {
            Models.EntitieLocal db = new Models.EntitieLocal();
            Models.TRAMITES tramites = db.TRAMITES.Where(x => x.ID == IdCiud).FirstOrDefault();

            return tramites;
        }

        public static List<Models.IMGBIOMETRICOS> buscaBiometrocsporTramite(int idTramite)
        {
            Models.EntitieLocal db = new EntitieLocal();
            List<Models.IMGBIOMETRICOS> bios = db.IMGBIOMETRICOS.Where(r => r.ID_TRAMITES == idTramite).ToList();

            return bios;
        }

        /// <summary>
        /// Método público para realizar la búsqueda de datos contribuyente en base al Identificador del registro
        /// </summary>
        /// <param name="datosContribuyenteId">Identificador del registro a buscar</param>
        /// <returns>Datos de contribuyente</returns>
        public static Models.DATOS_CONTRIBUYENTE buscarDatosContribuyentePorId(int datosContribuyenteId)
        {
            Models.EntitieLocal db = new Models.EntitieLocal();
            //REalizando búsqueda
            Models.DATOS_CONTRIBUYENTE datos = db.DATOS_CONTRIBUYENTE.Find(datosContribuyenteId);
            return datos;
        }

        /// <summary>
        /// Método público para realizar la búsqueda de datos contribuyente en base al Numero de Aviso Entero
        /// </summary>
        /// <param name="numeroAvisoEntero">Número de aviso para realizar la búsqueda</param>
        /// <returns>Datos de contribuyente</returns>
        //public static Models.DATOS_CONTRIBUYENTE buscarDatosContribuyentePorAvisoEntero(int numeroAvisoEntero)
        //{
        //    Models.EntitiesLocal db = new Models.EntitieLocal();
        //    //REalizando búsqueda
        //    Models.DATOS_CONTRIBUYENTE datos = db.DATOS_CONTRIBUYENTE.Where(x => x.AVISO_ENTERO.NUMERO_AVISO == numeroAvisoEntero).FirstOrDefault();
        //    return datos;
        //}

        /// <summary>
        /// Método público para realizar la búsqueda de datos contribuyente en base al RFC
        /// </summary>
        /// <param name="rfc">RFC a buscar</param>
        /// <returns>Datos de contribuyente</returns>
        public static RECORD buscarDatosContribuyentePorRFC(string rfc)
        {
            Models.EntitieLocal db = new Models.EntitieLocal();
            //REalizando búsqueda
            Models.DATOS_CONTRIBUYENTE datos = db.DATOS_CONTRIBUYENTE.Where(x => x.RFC == rfc).OrderByDescending(x => x.DATOS_CONTRIBUYENTE_ID).FirstOrDefault();
            Models.Entities1 dab = new Models.Entities1();
            var vista = dab.Database.SqlQuery<RECORD>("select ID_RFC,RFC, CURP, NOMBRE,APELLIDO_P,APELLIDO_M, CALLE, CALLE_PPAL, REFERENCIA1, REFERENCIA2,NO_EXTERIOR, NO_INTERIOR, CVE_COLONIA, CODIGO_POSTAL,TELEFONO,TIPO_CONTRIBUYENTE,E_MAIL,PAIS_ORIGEN, PAIS_RESIDENCIA, PENSIONADO,CVE_EDO, CVE_MPIO, CVE_LOC, CVE_REC, CVE_COLONIA from V_ING_RFC_LICENCIAS where RFC='" + rfc + "'").FirstOrDefault();
            // var vista = dab.Database.SqlQuery<RFCLIC>("select * from V_ING_TRAMITES_LICENCIAS WHERE rfc='MECF660115MN9'").ToList();


            return vista;
        }

        public static List<Models.PARAMETROS> GetParams()
        {
            Models.EntitieLocal db = new EntitieLocal();
            List<Models.PARAMETROS> param = db.PARAMETROS.Where(p => p.ACTIVO == 1).OrderBy(z => z.ID).ToList();
            return param;
        }


        public static Models.CIUDADANO BuscaPorIDSolicitud(int IdSol, bool isExpedicion, out string rfcTram)
        {
            Models.EntitieLocal db = new EntitieLocal();
            Models.CIUDADANO ciudadano = null;
            rfcTram = string.Empty;
            // var tramite = db.TRAMITES.Where(f => f.ID == IdSol).FirstOrDefault();



            ciudadano = new CIUDADANO();
            ciudadano.NOMBRE = "0";

            return ciudadano;
        }

        public static int DameNumLic(int idTramite)
        {
            int lic = 0;
            EntitieLocal DB = new EntitieLocal();
            var licNew = new NUMERO_LICENCIA { ID_TRAMITE = idTramite };
            DB.NUMERO_LICENCIA.Add(licNew);
            DB.SaveChanges();

            var licencia = (from n in DB.NUMERO_LICENCIA where (n.ID_TRAMITE == idTramite) select new Combos { Identificador = n.NUMERO_LIC }).FirstOrDefault();
            lic = int.Parse(licencia.Identificador.ToString());

            return lic;
        }


        public static void actualizaTramite_Licencia(int idtramite, string Numero_Licencia)
        {
            EntitieLocal db = new EntitieLocal();
            var tramiteActualizar = db.TRAMITES.First(g => g.ID == idtramite);
            tramiteActualizar.NUM_LICENCIA = Numero_Licencia.ToString();

            tramiteActualizar.STATUS = "PAGADO";
            tramiteActualizar.FECHA_MODIFICACION = DateTime.Now;
            db.SaveChanges();
        }

        public static string doyDescTram(int idtramite)
        {
            string tram = string.Empty;
            Models.EntitieLocal db = new Models.EntitieLocal();
            Models.TIPO_TRAMITE trami = db.TIPO_TRAMITE.Where(t => t.ID == idtramite).FirstOrDefault();



            tram = trami.DESCRIPCION;
            return tram;
        }

        public static List<Consultas> ConsultaCurp(string curpi)
        {
            Models.EntitieLocal db = new Models.EntitieLocal();
            List<Consultas> Lstdatos = new List<Consultas>();


            //datos = (from y in db.CIUDADANO where (y.CURP == curpi) select new  Consultas
            //                           {
            //                               Nombre = y.NOMBRE + " " + y.APELLIDO_PATERNO + " " + y.APELLIDO_MATERNO,
            //                               FechaExpedicion=y.TRAMITES.FirstOrDefault().FECHA_EXPEDICION,
            //                               FechaVencimiento=y.TRAMITES.FirstOrDefault().FECHA_MOVIMIENTO,
            //                               tipoLic=y.TRAMITES.FirstOrDefault().TIPO_LICENCIA,
            //                               TipoTram=y.TRAMITES.FirstOrDefault().TIPO_TRAMITE

            //                           }).ToList();

            //for (int i = 0; i < datos.Count; i++)
            //{
            // Models.CIUDADANO tram = datos[i];

            Lstdatos.Add(new Consultas() { FechaVencimiento = new DateTime().AddYears(2), FechaExpedicion = new DateTime(), tipoLic = 1, TipoTram = 2, Nombre = "Juan Lopez Pérez" });


            //SE COMENTARÁ PORQUE NO HAY CONSULTAS A LA BASE DE DOS AÚN    
            //var consul = (from y in tram 
            //              select new Consultas
            //                  {
            //                      Nombre = y.CIUDADANO.NOMBRE + " " + y.CIUDADANO.APELLIDO_PATERNO + " " + y.CIUDADANO.APELLIDO_MATERNO,
            //                      FechaExpedicion = y.FECHA_EXPEDICION,
            //                      FechaVencimiento = y.FECHA_MOVIMIENTO,
            //                      tipoLic = y.TIPO_LICENCIA,
            //                      TipoTram = y.TIPO_TRAMITE,
            //                      id=y.ID

            //                  }).ToList();
            //}
            return Lstdatos;
        }

        /// <summary>
        /// Método público para realizar la búsqueda de datos contribuyente en base al CURP
        /// </summary>
        /// <param name="curp">CURP a buscar</param>
        /// <returns>Datos de contribuyente</returns>
        public static Models.CIUDADANO buscarDatosContribuyentePorCURP(string curp)
        {
            Models.EntitieLocal db = new Models.EntitieLocal();
            Models.CIUDADANO datos = new CIUDADANO();
            //REalizando búsqueda
            try
            {
                datos = db.CIUDADANO.Where(x => x.CURP == curp).OrderByDescending(x => x.ID).FirstOrDefault();
            }
            catch (Exception ex)
            { }
            return datos;
        }

        /// <summary>
        /// Método público para el aviso de entero en base al registro Datos Contribuyente
        /// </summary>
        /// <param name="datosContribuyenteId">Identificador del registro a buscar</param>
        /// <returns>Registro de tipo Aviso Entero</returns>
        public static Models.DATOS_CONTRIBUYENTE obtenerAvisoEntero(int datosContribuyenteId)
        {
            Models.EntitieLocal db = new Models.EntitieLocal();
            //REalizando búsqueda
            Models.DATOS_CONTRIBUYENTE datos = db.DATOS_CONTRIBUYENTE.Find(datosContribuyenteId);
            return datos;
        }

        public static calificaCobro obtenValoresFaltantesCobro(string DESCRIPCION, string VALOR_CALCULO)
        {
            Entities1 db = new Entities1();

            var combos = db.Database.SqlQuery<calificaCobro>("select INCISO,SUBINCISO,DET_SUBINCISO from V_ING_TIPOS_LICENCIAS where DESCRIPCION='" + DESCRIPCION + "' and VALOR_CALCULO='" + VALOR_CALCULO + "'").FirstOrDefault();
            return combos;
        }

        public static calificaCobro CalificaCobro(string municipio, string clave)
        {
            //clave = clave.PadLeft(3, '0');
            Models.Entities1 dab = new Models.Entities1();
            var vista = dab.Database.SqlQuery<calificaCobro>("select INCISO,DESCRIPCION, VALOR_CALCULO,SUBINCISO,CLAVE from V_ING_TIPOS_LICENCIAS where CLAVE='" + clave + "' and CVE_MPIO='" + CATALOGOS.cv_mpio + "'").FirstOrDefault();
            return vista;
        }

        public static bool sicayo(string numAviso, string rfc)
        {
            bool cayo = false;
            Models.Entities1 dab = new Models.Entities1();
            var existe = dab.Database.SqlQuery<string>("select RFC from V_ING_TRAMITES_LICENCIAS WHERE NO_AVISO='" + numAviso + "' and RFC='" + rfc + "' and ESTATUS_PAGO='PENDIENTE'").FirstOrDefault();
            if (existe != null)
                cayo = true;
            return cayo;
        }



        public static void RegistraCobro(int P_USUARIO, string P_CVE_REC, string P_ID_RFC, string P_INCISO, string P_SUBINCISO, string P_NIVEL_SUBINCISO, string P_NO_AVISO, double P_IMPORTE)
        {
            string P_MSG_ERROR = string.Empty;
            Models.Entities1 db = new Models.Entities1();

            Oracle.DataAccess.Client.OracleConnection conn = new Oracle.DataAccess.Client.OracleConnection(GetCadena());
            //  //System.Data.OracleClient.OracleConnection conn = new OracleConnection("DATA SOURCE=3.5.1.4:1521/DESA;PERSIST SECURITY INFO=True;USER ID=LICENCIASM; PASSWORD=MUNICIPALES");
            //    // conn.ConnectionString = "context connection=true";
            conn.Open();

            //    // Create and execute a command
            Oracle.DataAccess.Client.OracleCommand cmd = new Oracle.DataAccess.Client.OracleCommand("ING_PKG_LICENCIAS_VH.ING_REGISTRA_COBRO", conn);
            //   // OracleCommand cmd = conn.CreateCommand();
            //    //cmd.CommandText = "execute ING_PKG_LICENCIAS_VH.ING_INSERTA_RFC_LICENCIA";
            cmd.CommandType = CommandType.StoredProcedure;
            Oracle.DataAccess.Client.OracleParameter[] param = new Oracle.DataAccess.Client.OracleParameter[9];// new Oracle.DataAccess.Client.OracleParameter[2]();


            param[0] = new Oracle.DataAccess.Client.OracleParameter();

            param[0].ParameterName = ":P_USUARIO";
            param[0].OracleDbType = Oracle.DataAccess.Client.OracleDbType.Decimal;
            param[0].Value = P_USUARIO;
            param[0].Direction = ParameterDirection.Input;
            param[1] = new Oracle.DataAccess.Client.OracleParameter();
            param[1].ParameterName = ":P_CVE_REC";
            param[1].OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
            param[1].Value = P_CVE_REC;
            param[1].Direction = ParameterDirection.Input;
            param[2] = new Oracle.DataAccess.Client.OracleParameter();
            param[2].ParameterName = ":P_ID_RFC";
            param[2].OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
            param[2].Value = P_ID_RFC;
            param[2].Direction = ParameterDirection.Input;
            param[3] = new Oracle.DataAccess.Client.OracleParameter();
            param[3].ParameterName = ":P_INCISO";
            param[3].OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
            param[3].Value = P_INCISO;
            param[3].Direction = ParameterDirection.Input;
            param[4] = new Oracle.DataAccess.Client.OracleParameter();
            param[4].ParameterName = ":P_SUBINCISO";
            param[4].OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
            param[4].Value = P_SUBINCISO;
            param[4].Direction = ParameterDirection.Input;
            param[5] = new Oracle.DataAccess.Client.OracleParameter();
            param[5].ParameterName = ":P_NIVEL_SUBINCISO";
            param[5].OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
            param[5].Value = P_NIVEL_SUBINCISO;
            param[5].Direction = ParameterDirection.Input;

            param[6] = new Oracle.DataAccess.Client.OracleParameter();
            param[6].ParameterName = ":P_NO_AVISO";
            param[6].OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
            param[6].Value = P_NO_AVISO;
            param[6].Direction = ParameterDirection.Input;
            param[7] = new Oracle.DataAccess.Client.OracleParameter();
            param[7].ParameterName = ":P_IMPORTE";
            param[7].OracleDbType = Oracle.DataAccess.Client.OracleDbType.Decimal;
            param[7].Value = P_IMPORTE;
            param[7].Direction = ParameterDirection.Input;
            param[8] = new Oracle.DataAccess.Client.OracleParameter();
            param[8].ParameterName = ":P_NOMBRE_GENERICO";
            param[8].OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
            param[8].Value = string.Empty;
            param[8].Direction = ParameterDirection.Input;
            //param[9] = new Oracle.DataAccess.Client.OracleParameter();
            //param[9].ParameterName = ":P_MSG_ERROR";
            //param[9].OracleDbType = Oracle.DataAccess.Client.OracleDbType.Varchar2;
            //param[9].Value = string.Empty;
            //param[9].Direction = ParameterDirection.Output;
            cmd.Parameters.Add(param[0]);
            cmd.Parameters.Add(param[1]);
            cmd.Parameters.Add(param[2]);
            cmd.Parameters.Add(param[3]);
            cmd.Parameters.Add(param[4]);
            cmd.Parameters.Add(param[5]);
            cmd.Parameters.Add(param[6]);
            cmd.Parameters.Add(param[7]);
            cmd.Parameters.Add(param[8]);
            // cmd.Parameters.Add(param[9]);
            try
            {
                Oracle.DataAccess.Client.OracleDataReader rdr = cmd.ExecuteReader();
                //cmd.ExecuteNonQuery();
            }
            catch (Oracle.DataAccess.Client.OracleException ex)
            {

            }

            //while (rdr.Read())
            //    // department_id = rdr.GetInt32(0);

            //    rdr.Close();
            cmd.Dispose();
            //    //param.UdtTypeName = "RECORD";

            // var ALGO= db.Database.SqlQuery<string>("exec ING_PKG_LICENCIAS_VH.ING_REGISTRA_COBRO(" + P_USUARIO + "," + P_CVE_REC + ",'" + P_ID_RFC + "'," + P_INCISO + "," + P_SUBINCISO + ",'" + P_NIVEL_SUBINCISO + "'," + P_NO_AVISO + "," + P_IMPORTE + "," + P_MSG_ERROR + ")");

        }

        public static string ConsultaTramites(string cve_rec)
        {
            Models.Entities1 db = new Models.Entities1();
            var vistaso = db.Database.SqlQuery<RECORD>("select * from V_ING_TRAMITES_LICENCIAS where CVE_REC='" + cve_rec + "'");
            return string.Empty;
        }

        public static string GetCadena()
        {
            string cadena = ConfigurationManager.ConnectionStrings["DataPUE.Properties.Settings.ConnectionString"].ConnectionString;
            return cadena;
        }

    }

    public class Consultas
    {
        public DateTime? FechaVencimiento { get; set; }
        public DateTime FechaExpedicion { get; set; }
        public decimal tipoLic { get; set; }
        public decimal TipoTram { get; set; }
        public string Nombre { get; set; }
        public decimal id { get; set; }
        // public string INCISO { get; set; }
    }

    public class calificaCobro
    {
        public string INCISO { get; set; }
        public string DESCRIPCION { get; set; }
        public double VALOR_CALCULO { get; set; }
        public string SUBINCISO { get; set; }
        public string P_NIVEL_SUBINCISO { get; set; }
        public string P_NO_AVISO { get; set; }
        public string CLAVE { get; set; }
        public string DET_SUBINCISO { get; set; }


    }

    public class identifica
    {
        public decimal CiuID { get; set; }
        public decimal contribuyenteID { get; set; }
        public string islocal { get; set; }
        public decimal idTram { get; set; }
        public string status { get; set; }
        public decimal tipoTram { get; set; }

    }


    public static class Json
    {
        public static T Deserialise<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            if (json != string.Empty)
            {

                using (MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                    obj = (T)serializer.ReadObject(ms);
                    return obj;
                }
            }
            else
                return obj;
        }
        public static string Serialize<T>(T obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

    }
}
