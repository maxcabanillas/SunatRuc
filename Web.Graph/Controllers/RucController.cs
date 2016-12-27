using GraphQL;
using GraphQL.Types;
using System.Threading.Tasks;
using System.Web.Http;
using Web.Graph.Models;

namespace Web.Graph.Controllers
{
    /// <summary>
    /// Consulta Ruc - SUNAT
    /// </summary>
    public class RucController : ApiController
    {
        private static readonly Schema Esquema;

        static RucController()
        {
            Esquema = new Schema {Query = new RucsQuery()};
        }

        // GET api/ruc?query={query}
        /// <summary>
        /// Retorna la informacion del Ruc.
        /// </summary>
        /// <param name="query">consulta GraphQL</param>
        /// <returns>resultado de la Consulta</returns>
        public object Get(string query)
        {
            return Run(query).Result;
        }

        // POST api/ruc
        /// <summary>
        /// Retorna la informacion del Ruc.
        /// </summary>
        /// <param name="query">consulta GraphQL</param>
        /// <returns>resultado de la Consulta</returns>
        public object Post([FromBody]string query)
        {
            return Run(query).Result;
        }

        private async Task<ExecutionResult> Run(string query)
        {

            //var schema = new Schema { Query = new RucsQuery() };

            var result = await new DocumentExecuter().ExecuteAsync(_ =>
            {
                _.Schema = Esquema;
                _.Query = query;
            }).ConfigureAwait(false);
            return result;
        }
    }
}
