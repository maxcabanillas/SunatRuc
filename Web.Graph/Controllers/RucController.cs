using System;
using System.IO;
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
        private static readonly Schema Esquema = new Schema { Query = new RucsQuery() };

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
                _.Schema = Esquema;
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
                  return null;
              }
            );
        }
    }

    /// <summary>
    /// Class for save exceptions.
    /// </summary>
    public static class ExceptionUtility
    {
        // All methods are static, so this can be private 

        // Log an Exception 
        /// <summary>
        /// Añade el detalle de una Excepcion.
        /// </summary>
        /// <param name="exc"></param>
        /// <param name="source"></param>
        public static void LogException(Exception exc, string source)
        {
            // Include enterprise logic for logging exceptions 
            // Get the absolute path to the log file 

            // Open the log file for append and write the log
            using (StreamWriter sw = new StreamWriter(System.Web.HttpContext.Current.Server.MapPath("~/log.txt"), true))
            {
                sw.WriteLine("********** {0} **********", DateTime.Now);
                if (exc.InnerException != null)
                {
                    sw.Write("Inner Exception Type: ");
                    sw.WriteLine(exc.InnerException.GetType().ToString());
                    sw.Write("Inner Exception: ");
                    sw.WriteLine(exc.InnerException.Message);
                    sw.Write("Inner Source: ");
                    sw.WriteLine(exc.InnerException.Source);
                    if (exc.InnerException.StackTrace != null)
                    {
                        sw.WriteLine("Inner Stack Trace: ");
                        sw.WriteLine(exc.InnerException.StackTrace);
                    }
                }
                sw.Write("Exception Type: ");
                sw.WriteLine(exc.GetType().ToString());
                sw.WriteLine("Exception: " + exc.Message);
                sw.WriteLine("Source: " + source);
                sw.WriteLine("Stack Trace: ");
                if (exc.StackTrace != null)
                {
                    sw.WriteLine(exc.StackTrace);
                    sw.WriteLine();
                }
                sw.Close();
            }
        }
    }
}
