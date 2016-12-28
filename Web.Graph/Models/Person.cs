namespace Web.Graph.Models
{
    /// <summary>
    /// Representa la informacion de una person por DNI.
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Primer Nombre.
        /// </summary>
        public string PrimerNombre { get; set; }
        /// <summary>
        /// Segundo Nombre
        /// </summary>
        public string SegundoNombre { get; set; }
        /// <summary>
        /// Apellido Paterno.
        /// </summary>
        public string ApellidoPaterno { get; set; }
        /// <summary>
        /// Apellido Materno.
        /// </summary>
        public string ApellidoMaterno { get; set; }
    }
}