using DataPUE.Models;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataPUE
{
    public class I_ADMIN
    {
        #region LLena el comboBox de las Recaudadoras
        public List<Recaudaroda> GetFillRecuadadora()
        {
            return FillRecuadadora();
        }

        private List<Recaudaroda> FillRecuadadora()
        {
            Entities1 gobierno = new Entities1();
            List<Recaudaroda> rec = new List<Recaudaroda>();
            rec = gobierno.Database.SqlQuery<Recaudaroda>("SELECT CVE_REC,DESCRIPCION FROM ING.V_ING_RECAUDADORAS_LICENCIAS").ToList();
            rec.Add(new Recaudaroda { CVE_REC = "0000", DESCRIPCION = "Seleccione una opción por favor." });
            rec = rec.OrderBy(o => o.CVE_REC).ToList();
            return rec;
        }
        #endregion

        #region inserta los folios de las recaudadoras
        public void InsertFolios(int ini, int fin, string rec)
        {
            SetInsertFolios(ini, fin, rec);
        }
        private void SetInsertFolios(int folio_ini, int folio_fin, string recaudadora)
        {
            EntitieLocal local = new EntitieLocal();
            INVENTARIO inv = new INVENTARIO
            {
                FOLIO_INI = folio_ini,
                FOLIO_FIN = folio_fin,
                CANTIDAD = (folio_fin - folio_ini) + 1,
                FOLIO_ACT = folio_ini,
                ACTIVO = (local.INVENTARIO.Where(W => W.ACTIVO == 1 && W.CVE_REC == recaudadora).Count() >= 1 ? 0 : 1),
                CVE_REC = recaudadora     //Verificar si es string o int
            };
            local.INVENTARIO.Add(inv);
            local.SaveChanges();
        }
        #endregion

        #region Valida que los folios no se repitan
        public bool CheckUseFolios(int fini, int ffin)
        {
            bool res = false;
            EntitieLocal loc = new EntitieLocal();
            int con = loc.Database.SqlQuery<int>("SELECT ID_INVENTARIO FROM INVENTARIO WHERE ((" + fini + " BETWEEN FOLIO_INI AND FOLIO_FIN) OR (" + ffin + " BETWEEN FOLIO_INI AND FOLIO_FIN)) and CVE_REC != 'Matriz'").Count();
            res = (con == 0 ? true : false);
            return res;
        }
        #endregion

        #region VALIDACION DE FOLIOS DE MATRIZ
        public bool CheckUseFoliosMatriz(int fini, int ffin)
        {
            bool res = false;
            EntitieLocal loc = new EntitieLocal();
            int con = loc.Database.SqlQuery<int>("SELECT ID_INVENTARIO FROM INVENTARIO WHERE ((" + fini + " BETWEEN FOLIO_INI AND FOLIO_FIN) OR (" + ffin + " BETWEEN FOLIO_INI AND FOLIO_FIN)) and CVE_REC = 'Matriz'").Count();
            res = (con == 0 ? true : false);
            return res;
        }
        #endregion

        #region LLenamos el DG con las recaudadoras que ya tienen asignados los folios
        public List<DGRecaudadora> GetDataGrisReca()
        {
            return FillDGRecaudadora();
        }
        private List<DGRecaudadora> FillDGRecaudadora()
        {
            EntitieLocal local = new EntitieLocal();
            var dg = local.INVENTARIO.Where(w => w.CVE_REC != "Matriz").Select(s => new DGRecaudadora
            {
                Activo = (s.ACTIVO == 1 ? "Activo" : "Desactivado"),
                Cantidad = (int)s.CANTIDAD,
                CVE_REC = s.CVE_REC,
                Folio_Actual = (int)s.FOLIO_ACT,
                Folio_Fin = (int)s.FOLIO_FIN,
                Folio_Inicio = (int)s.FOLIO_INI,
                Folio_Libres = (int)(s.FOLIO_FIN - s.FOLIO_ACT) + 1
            }).OrderByDescending(O => O.Folio_Fin).ToList();
            return dg;
        }
        #endregion

        #region Llenamos el DataGrid con los datos de la matriz
        public List<DGRecaudadora> GetDataGrisRecaMatriz()
        {
            return FillDGRecaudadoraMatriz();
        }
        private List<DGRecaudadora> FillDGRecaudadoraMatriz()
        {
            EntitieLocal local = new EntitieLocal();
            var dg = local.INVENTARIO.Where(w => w.CVE_REC == "Matriz").Select(s => new DGRecaudadora
            {
                Activo = (s.ACTIVO == 1 ? "Activo" : "Desactivado"),
                Cantidad = (int)s.CANTIDAD,
                CVE_REC = s.CVE_REC,
                Folio_Actual = (int)s.FOLIO_ACT,
                Folio_Fin = (int)s.FOLIO_FIN,
                Folio_Inicio = (int)s.FOLIO_INI,
                Folio_Libres = (int)(s.FOLIO_FIN - s.FOLIO_ACT) + 1
            }).OrderByDescending(O => O.Folio_Fin).ToList();
            return dg;
        }
        #endregion

        #region Eliminamos Recaudadora
        public List<DGRecaudadora> SetDeleteRecaudadora(DGRecaudadora DgReca)
        {
            return DeleteRecaudadora(DgReca);
        }

        private List<DGRecaudadora> DeleteRecaudadora(DGRecaudadora DgReca)
        {
            EntitieLocal local = new EntitieLocal();
            List<DGRecaudadora> reca = new List<DGRecaudadora>();
            var res = local.INVENTARIO.Where(w => w.CVE_REC == DgReca.CVE_REC && w.ACTIVO == (DgReca.Activo == "Activo" ? 1 : 0) && w.FOLIO_INI == DgReca.Folio_Inicio).Select(s => s).FirstOrDefault();
            if (res.FOLIO_ACT == res.FOLIO_INI)
            {
                local.INVENTARIO.Attach(res);
                local.INVENTARIO.Remove(res);
                local.SaveChanges();
                reca = (DgReca.CVE_REC != "Matriz" ? FillDGRecaudadora() : FillDGRecaudadoraMatriz());
            }
            else
            {
                reca = null;
            }
            return reca;
        }
        #endregion

        #region Obtenemos el ultimo folio de las recaudadoras menos Matriz
        public int GetUltimoFolio()
        {
            return UltimoFolio();
        }
        private int UltimoFolio()
        {
            //decimal max = 0;
            EntitieLocal loc = new EntitieLocal();
            decimal? dec = loc.Database.SqlQuery<decimal?>("select max(folio_fin) from inventario where CVE_REC != 'Matriz'").Last();
            int res = (int)(dec == null ? 0 : dec);
            return (int)res;
        }
        #endregion

        #region Obtenemos el ultimo folio de la recaudadora Matriz
        public int GetUltimoFolioMatriz()
        {
            return UltimoFolioMatriz();
        }
        private int UltimoFolioMatriz()
        {
            //decimal max = 0;
            EntitieLocal loc = new EntitieLocal();
            decimal? dec = loc.Database.SqlQuery<decimal?>("select max(folio_fin) from inventario where CVE_REC = 'Matriz'").Last();
            return (int)(dec == null ? 0 : dec);

        }
        #endregion

        #region Folio Matriz
        public int getFolioMatriz()
        {
            return FolioMatriz();
        }

        private int FolioMatriz()
        {
            EntitieLocal loc = new EntitieLocal();
            decimal? Folio = loc.Database.SqlQuery<decimal?>("Select max(cantidad) from inventario where CVE_REC = 'Matriz'").Last();
            return (int)(Folio == null ? 0 : Folio);
        }
        #endregion

        public static List<ResumenFolio> getDataResumen(string recaudadora, DateTime fechaInit, DateTime fechaFin)
        {
            Models.EntitieLocal db = new EntitieLocal();
            TRAMITES resumen = new TRAMITES();
            List<TRAMITES> LSTRAMITES = new List<TRAMITES>();
            List<NUMERO_FOLIO> NumFolio = new List<NUMERO_FOLIO>();
            List<ResumenFolio> myResumen = new List<ResumenFolio>();

            if (string.IsNullOrEmpty(recaudadora))
                NumFolio = db.NUMERO_FOLIO.Where(r => r.FECHACREADO >= fechaInit && r.FECHACREADO <= fechaFin).OrderBy(f => f.IDFOLIO).ToList();
            else
                NumFolio = db.NUMERO_FOLIO.Where(r => r.FECHACREADO >= fechaInit && r.FECHACREADO <= fechaFin && r.CLAVE_REC == recaudadora).OrderBy(f => f.IDFOLIO).ToList();
            for (int i = 0; i < NumFolio.Count(); i++)
            {
                if (NumFolio != null)
                {
                    ResumenFolio EsteResumen = new ResumenFolio();
                    DateTime fech_Crea = NumFolio[i].FECHACREADO.Value;
                    int tram = int.Parse(NumFolio[i].IDTRAMITE.ToString());
                    resumen = db.TRAMITES.Where(x => x.ID == tram).FirstOrDefault();
                    string nombre = string.Empty;
                    nombre = CATALOGOS.getNombre(NumFolio[i].USUARIOCREO);
                    EsteResumen.FECHA_CREACION = fech_Crea.ToString("dd/MM/yy");
                    EsteResumen.NO_AVISO = NumFolio[i].IDTRAMITE.ToString();
                    EsteResumen.NO_LASER = NumFolio[i].IDFOLIO.ToString();
                    EsteResumen.RECAUDADORA = NumFolio[i].CLAVE_REC;
                    EsteResumen.USUARIO = nombre;
                    EsteResumen.NO_LICENCIA = resumen.NUM_LICENCIA;
                    myResumen.Add(EsteResumen);
                }
            }



            return myResumen;
        }
    }


}
