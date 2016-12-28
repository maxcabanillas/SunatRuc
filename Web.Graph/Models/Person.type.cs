using GraphQL.Types;

namespace Web.Graph.Models
{
    /// <summary>
    /// The Persontype Class
    /// </summary>
    public class PersonType : ObjectGraphType<Person>
    {
        /// <summary>
        /// New Instance of <see cref="PersonType"/>
        /// </summary>
        public PersonType()
        {
            Field(p => p.PrimerNombre).Name("primer_nombre");
            Field(p => p.SegundoNombre).Name("segundo_nombre");
            Field(p => p.ApellidoPaterno).Name("apellido_paterno");
            Field(p => p.ApellidoMaterno).Name("apellido_materno");
        }
    }
}