using GraphQL;
using GraphQL.Types;
using System.Threading.Tasks;
using System.Web.Http;
using Web.Graph.Models;

namespace Web.Graph.Controllers
{
    /// <summary>
    /// Consulta DNI - Reniec
    /// </summary>
    public class DniController : ApiController
    {
        private static readonly Schema Esquema;

        static DniController()
        {
            Esquema = new Schema { Query = new DniQuery() };
        }

        // GET api/dni?query={query}
        /// <summary>
        /// Retorna la informacion del Dni.
        /// </summary>
        /// <param name="query">consulta GraphQL</param>
        /// <returns>resultado de la Consulta</returns>
        public object Get(string query)
        {
            return Run(query).Result;
        }

        // POST api/dni
        /// <summary>
        /// Retorna la informacion del Dni.
        /// </summary>
        /// <param name="query">consulta GraphQL</param>
        /// <returns>resultado de la Consulta</returns>
        public object Post([FromBody]string query)
        {
            return Run(query).Result;
        }

        private async Task<ExecutionResult> Run(string query)
        {
            var result = await new DocumentExecuter().ExecuteAsync(_ =>
            {
                _.Schema = Esquema;
                _.Query = query;
            }).ConfigureAwait(false);
            return result;
        }
    }
}
