using GraphQL;
using GraphQL.Types;
using Ruc;
using System.Linq;
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
        private static Schema _schema;

        static RucController()
        {
            _schema = new Schema { Query = new RucsQuery() };
        }
        //// GET api/values
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/values/5
        /// <summary>
        /// Retorna la informacion del Ruc.
        /// </summary>
        /// <param name="query">consulta GraphQL</param>
        /// <returns>resultado de la Consulta</returns>
        public object Get(string query)
        {
            return Run(query).Result;
        }

        /// <summary>
        /// Retorna la informacion del Ruc.
        /// </summary>
        /// <param name="query">consulta GraphQL</param>
        /// <returns>resultado de la Consulta</returns>
        // POST api/values
        public object Post([FromBody]string query)
        {
            return Run(query).Result;
        }

        private async Task<ExecutionResult> Run(string query)
        {

            //var schema = new Schema { Query = new RucsQuery() };

            var result = await new DocumentExecuter().ExecuteAsync(_ =>
            {
                _.Schema = _schema;
                _.Query = query;
            }).ConfigureAwait(false);
            return result;
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
            );
        }
    }
}
