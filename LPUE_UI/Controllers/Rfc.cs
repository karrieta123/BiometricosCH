using DataPUE;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PUE.Controllers
{
    public class Rfc
    {
        public enum Regimen { Fiscal, Moral }

        public Regimen _RegimenFiscal = Regimen.Fiscal;
        public String _Rfc { get; set; }
        public String _Curp { get; set; }
        public String _Nombre1 { get; set; }
        public String _Nombre2 { get; set; }
        public String _ApellidoPaterno { get; set; }
        public String _ApellidoMaterno { get; set; }
        public bool _EsPensionado { get; set; }
        public String _PaisOrigen { get; set; }
        public String _PaisReferencia { get; set; }
        public String _Estado { get; set; }
        public String _Municipio { get; set; }
        public String _Localidad { get; set; }
        public String _Domicilio { get; set; }
        public String _Calle { get; set; }
        public String _CallePrincipal { get; set; }
        public String _Esquina1 { get; set; }
        public String _Esquina2 { get; set; }
        public String _Colonia { get; set; }
        public String _NumeroExterior { get; set; }
        public String _NumeroInterior { get; set; }
        public String _CP { get; set; }
        public String _CorreoElectronico { get; set; }
        public String _Telefono { get; set; }
        public String _Referencia1 { get; set; }
        public String _Referencia2 { get; set; }

        public RECORD BuscaPorRFC(string rfc)
        {
            RECORD obj = new RECORD();
            try
            {
                obj = DataPUE.I_DATOS_CONTRIBUYENTE.buscarDatosContribuyentePorRFC(rfc);
                if (obj == null)
                    new Resultado() { _Estatus = Resultado.Estatus.ERROR, _Detalles = "No se encontraron resultados con este RFC" };
            }
            catch (Exception ex)
            {
                new Resultado() { _Estatus = Resultado.Estatus.ERROR, _Detalles = "Existe un problema al buscar el RFC consulteal administrador" };
            }
            return obj;
        }

        public static int idDatosContribuyente = 0;
        public static string isLocal = "0";

        /// <summary>
        /// Método público para registrar contribuyente
        /// </summary>
        /// <returns></returns>
        public Resultado GuardarDatosContribuyente(Rfc rfc)
        {

            int pension = 0;
            if (rfc._EsPensionado)
                pension = 1;
            RECORD rfcExist = BuscaPorRFC(rfc._Rfc);
            if (rfcExist == null)
            {
                try
                {
                    //var rfctabla = new DataPUE.Models.DATOS_CONTRIBUYENTE
                    //{
                    //    RFC = rfc._Rfc,
                    //    CURP = rfc._Curp,
                    //    NOMBRE = rfc._Nombre1,
                    //    APELLIDO_PATERNO = rfc._ApellidoPaterno,
                    //    APELLIDO_MATERNO = rfc._ApellidoMaterno,
                    //    PENSIONADO = pension,
                    //    NACIONALIDAD_ID = int.Parse(string.IsNullOrEmpty(rfc._PaisOrigen) ? "0" : rfc._PaisOrigen),
                    //    ESTADO_ID = int.Parse(string.IsNullOrEmpty(rfc._Estado) ? "0" : rfc._Estado),
                    //    MUNICIPIO = int.Parse(rfc._Municipio),
                    //    LOCALIDAD = int.Parse(string.IsNullOrEmpty(rfc._Localidad) ? "0" : rfc._Localidad),
                    //    DIRECCION = rfc._Calle,
                    //    CALLE_PRINCIPAL = rfc._CallePrincipal,
                    //    ENTRE_CALLE1 = rfc._Esquina1,
                    //    ENTRE_CALLE2 = rfc._Esquina2,
                    //    COLONIA_ID = int.Parse(string.IsNullOrEmpty(rfc._Colonia) ? "0" : rfc._Colonia),
                    //    NUMERO_INTERIOR = rfc._NumeroInterior,
                    //    CODIGO_POSTAL = rfc._CP,
                    //    EMAIL = rfc._CorreoElectronico,
                    //    TELEFONO = rfc._Telefono,
                    //    FISICA_MORAL = rfc._RegimenFiscal.ToString()
                    //};


                    //idDatosContribuyente = DataPUE.I_DATOS_CONTRIBUYENTE.insertacontribuyente(rfctabla);

                    //if (idDatosContribuyente != 0)
                    //{
                    //debe de guardar tambien en la bd de gob

                    // CultureInfo ci  = new CultureInfo("en-US");
                    RECORD obj = new RECORD();
                    obj.ID_RFC = 0;
                    obj.RFC = rfc._Rfc;
                    obj.RFC_LETRA = "";
                    //obj.RFC_NUM = DateTime.Now.ToString("dd-MMM-yyyy",ci); 
                    obj.RFC_HOMO = "";
                    obj.CURP = rfc._Curp;
                    // obj.NOMBRE = rfc._Nombre1;
                    obj.NOMBRE_2 = rfc._Nombre2;
                    obj.APELLIDO_P = rfc._ApellidoPaterno;
                    obj.APELLIDO_M = rfc._ApellidoMaterno;
                    obj.CALLE = rfc._Calle;
                    obj.CALLE_PPAL = rfc._CallePrincipal;
                    obj.REFERENCIA1 = rfc._Esquina1;
                    obj.REFERENCIA2 = rfc._Esquina2;
                    obj.NO_EXTERIOR = rfc._NumeroExterior;
                    obj.NO_INTERIOR = rfc._NumeroInterior;
                    obj.CVE_COLONIA = int.Parse(string.IsNullOrEmpty(rfc._Colonia) ? "0" : rfc._Colonia);
                    obj.CODIGO_POSTAL = rfc._CP;
                    obj.TELEFONO = rfc._Telefono;
                    obj.TIPO_CONTRIBUYENTE = rfc._RegimenFiscal.ToString();
                    obj.E_MAIL = rfc._CorreoElectronico;
                    obj.TELEFONO_MOVIL = string.Empty;
                    obj.PAIS_ORIGEN = rfc._PaisOrigen;
                    obj.PAIS_RECIDENCIA = rfc._PaisReferencia;
                    obj.PENSIONADO = rfc._EsPensionado ? "Si" : "No";
                    obj.CVE_EDO = rfc._Estado;
                    obj.CVE_MPIO = rfc._Municipio;
                    obj.CVE_LOC = rfc._Localidad;
                    obj.CVE_REC = CATALOGOS.cv_recaudadora;
                    obj.CREADO_POR = int.Parse(CATALOGOS.id_Usuario);
                    // obj.FECHA_CREACION = DateTime.Now.ToString("dd-MMM-yyyy", ci); 
                    obj.MODIFICADO_POR = int.Parse(CATALOGOS.id_Usuario);

                    int idestatus = DataPUE.I_DATOS_CONTRIBUYENTE.insertaContribuyenteGobierno(obj);
                    rfcExist = BuscaPorRFC(rfc._Rfc);
                    if (rfcExist != null)
                    {
                        idDatosContribuyente = rfcExist.ID_RFC;
                        isLocal = rfc._Rfc;
                        if (idestatus == 1)
                            return new Resultado() { _Estatus = Resultado.Estatus.OK, _Detalles = "Se inserto correctamente los datos del contribuyente" };
                        else
                            return new Resultado() { _Estatus = Resultado.Estatus.ERROR, _Detalles = "existe un problema al guardar los datos del contribuyente en gobierno" };
                    }
                    else
                        return new Resultado() { _Estatus = Resultado.Estatus.ERROR, _Detalles = "existe un problema al guardar los datos del contribuyente en gobierno" };
                    //}
                    //else
                    //    return new Resultado() { _Estatus = Resultado.Estatus.ERROR, _Detalles = "Existe un error al insertar los datos del contribuyente consulte al administrador" };
                }
                catch (Exception ex)
                {
                    return new Resultado() { _Estatus = Resultado.Estatus.ERROR, _Detalles = "no se inserto correctamente los datos del contribuyente" };
                }
            }
            else
            {
                idDatosContribuyente = rfcExist.ID_RFC;
                isLocal = rfcExist.RFC.ToString();
                return new Resultado() { _Estatus = Resultado.Estatus.OK, _Detalles = "Este RFC ya existia, solo se asocio" };
            }



        }

        /// <summary>
        /// Catalogo de paises
        /// </summary>
        /// <returns></returns>
        public List<DataPUE.Combos> CatalogoPais()
        {
            List<DataPUE.Combos> CatPais = new List<Combos>();
            try
            {
                CatPais = DataPUE.CATALOGOS.obtenerPais();
            }
            catch (Exception ex)
            { }
            // CatPais.Add(new Combos { Descripcion = "---Seleccione---", Identificador = 0 });
            return CatPais;//.OrderBy(s=> s.Identificador).ToList();
        }

        /// <summary>
        /// Catalogo de estados
        /// </summary>
        /// <returns></returns>
        public List<DataPUE.Combos> CatalogoEstado()
        {
            List<DataPUE.Combos> CatEstado = new List<DataPUE.Combos>();// { new DataPUE.Combos() { Identificador = 1, Descripcion = "Estado 1" }, new DataPUE.Combos() { Identificador = 2, Descripcion = "Estado 2" } };
            CatEstado = DataPUE.CATALOGOS.obtieneEntidades();
            CatEstado.Add(new Combos { Descripcion = "---Seleccione---", Identificador = 0 });
            return CatEstado.OrderBy(s => s.Identificador).ToList();
        }

        /// <summary>
        /// Catalogo de municipios
        /// </summary>
        /// <returns></returns>
        public List<DataPUE.Combos> CatalogoMunicipio()
        {
            List<DataPUE.Combos> CatMunicipio = new List<DataPUE.Combos>();// { new DataPUE.Combos() { Identificador = 1, Descripcion = "Municipio 1" }, new DataPUE.Combos() { Identificador = 2, Descripcion = "Municipio 2" } };
            CatMunicipio.Add(new Combos { Descripcion = "---Seleccione---", Identificador = 0 });
            return CatMunicipio.OrderBy(s => s.Identificador).ToList();
        }

        /// <summary>
        /// Catalogo de localidades
        /// </summary>
        /// <returns></returns>
        public List<DataPUE.Combos> CatalogoLocalidad()
        {
            List<DataPUE.Combos> CatLocalidad = new List<DataPUE.Combos>() { new DataPUE.Combos() { Identificador = 1, Descripcion = "Localidad 1" }, new DataPUE.Combos() { Identificador = 2, Descripcion = "Localidad 2" } };
            CatLocalidad.Add(new Combos { Descripcion = "---Seleccione---", Identificador = 0 });
            return CatLocalidad.OrderBy(s => s.Identificador).ToList();
        }

        /// <summary>
        /// Catalogo de localidades
        /// </summary>
        /// <returns></returns>
        public List<DataPUE.Combos> CatalogoColonia()
        {
            List<DataPUE.Combos> CatColonia = new List<DataPUE.Combos>() { new DataPUE.Combos() { Identificador = 1, Descripcion = "Colonia 1" }, new DataPUE.Combos() { Identificador = 2, Descripcion = "Colonia 2" } };
            CatColonia.Add(new Combos { Descripcion = "---Seleccione---", Identificador = 0 });
            return CatColonia.OrderBy(s => s.Identificador).ToList();
        }
    }
}
