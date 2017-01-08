using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using Ruc;

namespace Web.Graph.Controllers
{
    /// <summary>
    /// Consulta de DNI por nombre.
    /// </summary>
    [RoutePrefix("api/nombre")]
    public class NombreController : ApiController
    {
        /// <summary>
        /// Obtiene los DNI relacionados al nombre de la persona.
        /// </summary>
        /// <param name="name">Nombre de la Persona</param>
        /// <returns>Lista de Resultados</returns>
        [Route("{name}")]
        [ResponseType(typeof(string))]
        public HttpResponseMessage Get(string name)
        {
            return GetJsonResult(name);
        }

        /// <summary>
        /// Obtiene los DNI relacionados al nombre de la persona.
        /// </summary>
        /// <param name="name">Nombre de la Persona</param>
        /// <returns>Lista de Resultados</returns>
        [Route("")]
        [ResponseType(typeof(string))]
        public HttpResponseMessage Post(string name)
        {
            return GetJsonResult(name);
        }

        private HttpResponseMessage GetJsonResult(string name)
        {
            try
            {
                var resp = Request.CreateResponse(HttpStatusCode.OK);
                var json = new NombreConsult().Get(name);
                resp.Content = new StringContent(json, Encoding.UTF8, "application/json");
                return resp;
            }
            catch (Exception e)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(e);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }

        }
    }
}
