using System;
using System.Drawing;
using System.Net;

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

        #region Fields
        /// <summary>
        /// Optimiza la imagen para que pueda ser facilmente reconocida.
        /// </summary>
        public bool Optimize { get; set; }
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

        protected static void ImageToBlackAndWhite(Bitmap bm)
        {
            for (var x = 0; x < bm.Width; x++)
                for (var y = 0; y < bm.Height; y++)
                {
                    var pix = bm.GetPixel(x, y);
                    bm.SetPixel(x, y, pix.GetBrightness() > 0.90f ? Color.White : Color.Black);
                }
        }
        #endregion
    }
}
