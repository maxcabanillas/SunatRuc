using GraphQL.Types;
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


    public class CompanyType : ObjectGraphType<Company>
    {
        public CompanyType()
        {
            Field(x => x.Ruc).Description("Numero Ruc");
            Field(x => x.Nombre, true).Description("Nombre / Razón Social");
            Field(x => x.TipoContribuyente, true).Name("tipo_contribuyente").Description("Tipo de Contribuyente");            
            Field(x => x.Profesion, true).Description("Profesión u Oficio");
            Field(x => x.NombreComercial, true).Name("nombre_comercial").Description("Nombre Comercial");
            Field(x => x.CondicionContribuyente, true).Name("condicion_contribuyente").Description("Condición del Contribuyente");
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
}