using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Ruc
{
    /// <summary>
    /// Class for consult by name.
    /// </summary>
    public class NombreConsult
    {
        #region Fields
        private const string UrlConsult = "http://peru.rutificador.com/get_generic_ajax/";
        private const string Csrftoken = "Ivild5Epk";
        #endregion

        #region Export
        /// <summary>
        /// Consult for Name or DNI.
        /// </summary>
        /// <param name="input">Name or DNI</param>
        /// <example>
        /// Exito: {"status": "success", "value": [{"dv": 6, "dni": "22294023", "name": "MARIA GLADYS TORRES MAGALLANES"}, {"dv": 5, "dni": "21831936", "name": "GLADYS MAGALLANES TORRES"}]}
        /// NotFound : {"status": "not_found", "status_text": "No se encontraron coincidencias"}
        /// </example>
        /// <returns>Json string Result.</returns>
        public string Get(string input)
        {
            return GetJson(input);
        }

        #endregion

        #region Private Methods

        private string GetJson(string value)
        {
            var http = (HttpWebRequest)WebRequest.Create(UrlConsult);
            http.CookieContainer = new CookieContainer(1);
            http.CookieContainer.Add(new Cookie("csrftoken", Csrftoken, "/", "peru.rutificador.com"));
            http.Method = "POST";
            http.ContentType = "application/x-www-form-urlencoded";
            var bytes = Encoding.UTF8.GetBytes($"csrfmiddlewaretoken={Csrftoken}&entrada={value}");
            using (var rw = http.GetRequestStream())
            {
                rw.Write(bytes, 0, bytes.Length);
            }
            var res = (HttpWebResponse)http.GetResponse();
            if(res.StatusCode != HttpStatusCode.OK)
                throw new Exception("Not valid request");
            using (var rd = new StreamReader(res.GetResponseStream()))
            {
                return rd.ReadToEnd();
            }
        }

        #endregion
    }
}
