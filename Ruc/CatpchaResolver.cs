using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;

namespace Ruc
{
    /// <summary>
    /// Class for Resolve Captcha (SUNAT).
    /// </summary>
    public abstract class CatpchaResolver
    {
        #region Fields
        private readonly CookieContainer _cookies;
        #endregion

        #region Construct
        static CatpchaResolver()
        {
            ServicePointManager.CheckCertificateRevocationList = false;
            if (ServicePointManager.ServerCertificateValidationCallback == null)
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }

        /// <summary>
        /// New Instance of <see cref="CatpchaResolver"/>
        /// </summary>
        protected CatpchaResolver()
        {
            _cookies = new CookieContainer();
        }
        #endregion

        #region Export

        /// <summary>
        /// Get string in Captcha Image.
        /// </summary>
        /// <param name="imagen">Resource image as Bitmap</param>
        /// <returns>string that contains captcha</returns>
        public abstract string GetCatpcha(Bitmap imagen);

        /// <summary>
        /// Get string in Captcha in url.
        /// </summary>
        /// <param name="url">Url of store image (Catpcha)</param>
        /// <returns>string that contains captcha</returns>
        public string GetCaptcha(string url)
        {
            var ima = GetImageInternal(url);
            return GetCatpcha(ima);
        }

        /// <summary>
        /// Crea un HttpRequest and set CookieContainer.
        /// </summary>
        /// <param name="url">Url for Request</param>
        /// <returns>Resource HttpWebRequest</returns>
        protected HttpWebRequest CreateRequest(string url)
        {
            var http = (HttpWebRequest)WebRequest.Create(url);
            http.CookieContainer = _cookies;
            return http;
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Obtiene la Imagen de la url.
        /// </summary>
        /// <param name="url">url of resource</param>
        /// <exception cref="ArgumentException">Not found resource</exception>
        /// <returns>Imagen as Bitmap</returns>
        private Bitmap GetImageInternal(string url)
        {
            var http = CreateRequest(url);
            var res = http.GetResponse();
            using (var wr = res.GetResponseStream())
            {
                if (wr == null) throw new ArgumentException("Not found resource");
                return (Bitmap)Image.FromStream(wr);
            }
        }
        #endregion

        #region Post
        // Url source : http://www.briangrinstead.com/blog/multipart-form-post-in-c

        public HttpWebResponse MultipartFormDataPost(string postUrl, Dictionary<string, object> postParameters)
        {
            string formDataBoundary = "----------bdf03862e22e455f8b9922decc84b446";
            string contentType = "multipart/form-data; boundary=" + formDataBoundary;

            byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);

            return PostForm(postUrl, contentType, formData);
        }
        private HttpWebResponse PostForm(string postUrl, string contentType, byte[] formData)
        {
            var request = CreateRequest(postUrl);
            request.Host = "www.sunat.gob.pe";
            // Set up the request properties.
            request.Method = "POST";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
            request.ContentType = contentType;
            request.ContentLength = formData.Length;
            // Send the form data to the request.
            using (var req = request.GetRequestStream())
            {
                req.Write(formData, 0, formData.Length);
            }

            return (HttpWebResponse)request.GetResponse();
        }

        private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
        {
            Stream formDataStream = new MemoryStream();
            var needsClrf = false;
            var encoding = Encoding.GetEncoding("ISO-8859-1");
            foreach (var param in postParameters)
            {
                // Thanks to feedback from commenters, add a CRLF to allow multiple parameters to be added.
                // Skip it on the first parameter, add it to subsequent parameters.
                if (needsClrf)
                    formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

                needsClrf = true;

                if (param.Value is FileParameter)
                {
                    FileParameter fileToUpload = (FileParameter)param.Value;

                    // Add just the first part of this param, since we will write the file data directly to the Stream
                    string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\";\r\nContent-Type: {3}\r\n\r\n",
                        boundary,
                        param.Key,
                        fileToUpload.FileName ?? param.Key,
                        fileToUpload.ContentType ?? "application/octet-stream");

                    formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));

                    // Write the file data directly to the Stream, rather than serializing it to a string.
                    formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
                }
                else
                {
                    string postData = $"--{boundary}\r\nContent-Disposition: form-data; name=\"{param.Key}\"\r\n\r\n{param.Value}";
                    formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
                }
            }

            // Add the end of the request.  Start with a newline
            string footer = "\r\n--" + boundary + "--\r\n";
            formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

            // Dump the Stream into a byte[]
            formDataStream.Position = 0;
            byte[] formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);
            formDataStream.Close();

            return formData;
        }

        #endregion
    }

    public class FileParameter
    {
        public byte[] File { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
    }
}
