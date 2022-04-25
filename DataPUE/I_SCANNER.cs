using DataPUE.Models;
using Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace DataPUE
{
    public class I_SCANNER
    {

        public List<Scanner> Resolucion()
        {
            try
            {
                return GetResolucion();
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

        private List<Scanner> GetResolucion()
        {
            EntitieLocal loc = new EntitieLocal();
            List<Scanner> res = loc.Database.SqlQuery<Scanner>("SELECT ID_SCAN_PROPERTIES,DPI, COLOR , SIDES FROM SCANNER_PROPERTIES").ToList();

            //List<Scanner> res = (from s in loc.SCANNER_PROPERTIES select new Scanner {
            // color = s.COLOR,
            //        dpi = s.DPI,
            //        sides = s.SIDES
            //    }).ToList();
            //loc.SCANNER_PROPERTIES.Select(s =>
            //    new Scanner
            //    {
            //        color = s.COLOR,
            //        dpi = s.DPI,
            //        sides = s.SIDES
            //    }).ToList();
            return res;
        }


        public List<EntiDocumentos> GetScanner()
        {
            EntitieLocal local = new EntitieLocal();
            //var itemScanner = local.DOCUMENTOS.Select(s => new EntiDocumentos { id = (int)s.ID_DOCUMENTO, nombre = s.NOMBRE }).ToList();
            //return itemScanner;
            var docc = local.Database.SqlQuery<EntiDocumentos>("SELECT ID_DOCUMENTO,NOMBRE FROM DOCUMENTOS").ToList();
            return docc;
        }

        public bool CheckImage(int id, int tramite)
        {
            EntitieLocal local = new EntitieLocal();
            var vivo = local.SCANNER.Any(s => s.ID_TRAMITES == tramite && s.DOCUMENT == id);
            return vivo;
        }
    }

}
