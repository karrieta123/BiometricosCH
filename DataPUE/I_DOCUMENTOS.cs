using DataPUE.Models;
using Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace DataPUE
{
    public class I_DOCUMENTOS
    {

        #region Metodo para insertar los documentos en bytes
        public void InsertDocumentos(int id, byte[] imagen, int ids)
        {
            SetInsetDocumentos(id, imagen, ids);
        }

        private void SetInsetDocumentos(int ids, byte[] image, int id)
        {
            EntitieLocal local = new EntitieLocal();
            SCANNER scan = new SCANNER()
            {
                //ID_SCANNER=0,
                IMG = image,
                DOCUMENT = ids,
                ID_TRAMITES = id //Esta linea de debe de cambiar cuando se tenga el ID_TRAMITES  
            };
            local.SCANNER.Add(scan);
            local.SaveChanges();
        }
        #endregion



        public void UpdateDocuments(int id_documento, byte[] image, int id)
        {
            EntitieLocal local = new EntitieLocal();
            var res = (from r in local.SCANNER where r.ID_TRAMITES == id && r.DOCUMENT == id_documento select r).First();
            res.IMG = image;
            local.SCANNER.Add(res);
            local.SaveChanges();
        }

        public List<EntiDocumentos> Documentos(List<string> DocumentosRemove = null)
        {
            try
            {
                return GetDocumentos(DocumentosRemove);
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
        private List<EntiDocumentos> GetDocumentos(List<string> DocumentosRemove = null)
        {
            //List<Documentos> doc = new List<Entidades.Documentos>();

            EntitieLocal local = new EntitieLocal();
            List<EntiDocumentos> docs = new List<EntiDocumentos>();
            if (DocumentosRemove == null)
            {
                docs = local.DOCUMENTOS.OrderBy(t => t.NOMBRE).Select(d => new EntiDocumentos()
                {
                    ID_DOCUMENTO = (int)d.ID_DOCUMENTO,
                    NOMBRE = d.NOMBRE
                }).ToList();
            }
            else
            {
                docs = local.DOCUMENTOS.Where(t => DocumentosRemove.Contains(t.NOMBRE)).OrderBy(t => t.NOMBRE).Select(d => new EntiDocumentos()
                {
                    ID_DOCUMENTO = (int)d.ID_DOCUMENTO,
                    NOMBRE = d.NOMBRE
                }).ToList();
            }

            return docs;
        }
        public string ScannerSettings()
        {
            return GetScannerSettings();
        }
        private string GetScannerSettings()
        {
            string sep = null;
            EntitieLocal loc = new EntitieLocal();
            List<EntiScannerProperties> lis = loc.SCANNER_PROPERTIES.Select(s => new EntiScannerProperties
            {
                color = s.COLOR,
                sided = (int)s.SIDES,
                resolucion = (int)s.DPI
            }).ToList();
            foreach (var item in lis)
            {
                sep += item.color + "|" + item.sided + "|" + item.resolucion + "|";
            }
            return sep;
        }
    }
    //public class EntiDocumentos
    //{
    //    public int id { get; set; }        
    //    public string nombre { get; set; }
    //}
    public class EntiScannerProperties
    {
        public string color { get; set; }
        public int sided { get; set; }
        public int resolucion { get; set; }
    }
}
