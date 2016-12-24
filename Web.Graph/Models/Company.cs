using System;
using System.Collections.Generic;
using System.Linq;
using GraphQL;
using GraphQL.Types;
using Ruc;
using Web.Graph.Controllers;

// ReSharper disable All

namespace Web.Graph.Models
{
    public class Company
    {
        public string Ruc { get; set; }
        public string Nombre { get; set; }
        public string TipoContribuyente { get; set; }
        public string Profesion { get; set; }
        public string NombreComercial { get; set; }
        public string CondicionContribuyente { get; set; }
        public string EstadoContribuyente { get; set; }
        public string FechaInscripcion { get; set; }
        public string FechaInicio { get; set; }
        public string Departamento { get; set; }
        public string Provincia { get; set; }
        public string Distrito { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Fax { get; set; }
        public string ComercioExterior { get; set; }
        public string Principal { get; set; }
        public string Secundario1 { get; set; }
        public string Secundario2 { get; set; }
        public string Rus { get; set; }
        public string BuenContribuyente { get; set; }
        public string Retencion { get; set; }
        public string PercepcionVinterna { get; set; }
        public string PercepcionCliquido { get; set; }
    }

    /// <summary>
    /// Company Type GraphQL
    /// </summary>
    public class CompanyType : ObjectGraphType<Company>
    {
        /// <summary>
        /// New Instance of <see cref="CompanyType"/>
        /// </summary>
        public CompanyType()
        {
            
            Name = "empresa";
            Field(x => x.Ruc).Description("Numero Ruc");
            Field(x => x.Nombre, true).Description("Nombre / Razón Social");
            Field(x => x.TipoContribuyente, true).Name("tipo_contribuyente").Description("Tipo de Contribuyente");
            Field(x => x.Profesion, true).Description("Profesión u Oficio");
            Field(x => x.NombreComercial, true).Name("nombre_comercial").Description("Nombre Comercial");
            Field(x => x.CondicionContribuyente, true)
                .Name("condicion_contribuyente")
                .Description("Condición del Contribuyente");
            Field(x => x.EstadoContribuyente, true).Name("estado_contribuyente").Description("Estado Contribuyente");
            Field(x => x.FechaInscripcion, true).Name("fecha_inscripcion").Description("Fecha de Inscripción");
            Field(x => x.FechaInicio, true).Name("fecha_inicio").Description("Fecha de Inicio de Actividades");
            Field(x => x.Departamento, true).Description("Departamento");
            Field(x => x.Provincia, true).Description("Provincia");
            Field(x => x.Distrito, true).Description("Distrito");
            Field(x => x.Direccion, true).Description("Dirección");
            Field(x => x.Telefono, true).Description("Telefono (s)");
            Field(x => x.Fax, true).Description("Fax");
            Field(x => x.ComercioExterior, true).Name("comercio_exterior").Description("Actividad de Comercio Exterior");
            Field(x => x.Principal, true).Description("Actividad Principal- CIIU");
            Field(x => x.Secundario1, true).Description("Actividad Secundario 1- CIIU");
            Field(x => x.Secundario2, true).Description("Actividad Secundario 2- CIIU");
            Field(x => x.Rus, true).Description("Nuevo RUS");
            Field(x => x.BuenContribuyente, true).Name("buen_contribuyente").Description("Buen Contribuyente");
            Field(x => x.Retencion, true).Description("Agentes de Retención del IGV");
            Field(x => x.PercepcionVinterna, true).Name("percepcion_vinterna")
                .Description("Agentes de Percepción IGV-Venta Interna");
            Field(x => x.PercepcionCliquido, true).Name("percepcion_cliquido")
                .Description("Agentes de Percepción IGV-Combustible Líquido");
        }
    }

    /// <summary>
    /// Query Ruc
    /// </summary>
    public class RucsQuery : ObjectGraphType
    {
        /// <summary>
        /// new Instance of <see cref="RucsQuery"/>
        /// </summary>
        public RucsQuery()
        {
            Field<CompanyType>(
                "empresa",
                arguments: new QueryArguments(new QueryArgument<StringGraphType> { Name = "ruc" }),
                resolve: context =>
                {
                    var ruc = context.GetArgument<string>("ruc");
                    try
                    {
                        //Validar ruc.
                        var empresa = new Company();
                        if (ruc == null || ruc.Length != 11) return empresa;
                        var cs = new RucMultipleConsult();
                        var result = cs.GetInfo(ruc);
                        var type = empresa.GetType();
                        var comp = result.First();
                        byte i = 0;
                        foreach (var prop in type.GetProperties())
                        {
                            prop.SetValue(empresa, comp[i++].TrimEnd());
                        }
                        return empresa;
                    }
                    catch (Exception e)
                    {
                        ExceptionUtility.LogException(e, "Consultando :" + ruc);
                    }
                    //return null;
                    return new Company {Ruc = ruc, Nombre = "Giansalex"};
                }
            );
        }
    }

    /// <summary>
    /// Class for Query receive multiple ruc, and multiple responses.
    /// </summary>
    public class ListQuery : ObjectGraphType
    {
        /// <summary>
        /// New Instance of <see cref="ListQuery"/>
        /// </summary>
        public ListQuery()
        {
            Field<ListGraphType<CompanyType>>(
                "empresa",
                arguments: new QueryArguments(new QueryArgument<ListGraphType<StringGraphType>> {Name = "ruc"}),
                resolve: context =>
                {
                    var rucs = context.GetArgument<List<string>>("ruc");
                    var response = new List<Company>();
                    try
                    {
                        var cs = new RucMultipleConsult();
                        var result = cs.GetInfo(rucs.ToArray());
                        var props = typeof(Company).GetProperties();
                        foreach (var res in result)
                        {
                            var empresa = new Company();
                            byte i = 0;
                            foreach (var prop in props)
                            {
                                prop.SetValue(empresa, res[i++].TrimEnd());
                            }
                            response.Add(empresa);
                        }
                    }
                    catch (Exception e)
                    {
                        ExceptionUtility.LogException(e, "Query GraphQL");
                    }
                    return response;
               }
            );
        }
    }
}