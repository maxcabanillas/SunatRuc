using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Patagames.Ocr;
using Patagames.Ocr.Enums;
using Ruc.Helper;

namespace Ruc
{
    /// <summary>
    /// Consulta a SUNAT, con multiples RUC.
    /// </summary>
    public class RucMultipleConsult : CaptchaResolver
    {

        #region Export

        /// <summary>
        /// Get info from rucs.
        /// </summary>
        /// <param name="rucs">Multiples rucs</param>
        /// <returns>Infor de todos los ruc.</returns>
        /// <exception cref="ArgumentException"></exception>
        public IEnumerable<string[]> GetInfo(params string[] rucs)
        {
            if (rucs.Length == 0)
                throw new ArgumentException("Se requiere almenos un Ruc", nameof(rucs));

            //if (rucs.Length > 10)
            //{
            //    return GetOpcion2(rucs);
            //}
            var captcha = GetCaptchaString();
            var url = Properties.Resources.RucMCons + "?accion=consManual&" + string.Join("&", rucs.Select(ruc => "selRuc=" + ruc)) + "&codigoM=" + captcha;
            var http = CreateRequest(url);
            return GetProcessResponse((HttpWebResponse)http.GetResponse());
        }

        #endregion

        #region Private Methods

        private IEnumerable<string[]> GetOpcion2(IEnumerable<string> rucs)
        {
            var builder = new StringBuilder();
            foreach (var ruc in rucs)
            {
                builder.AppendLine(ruc + "|");
            }
            var captcha = GetCaptchaString();
            var zip = ZipHelper.Compress(new KeyValuePair<string, byte[]>("ruc.txt", Encoding.ASCII.GetBytes(builder.ToString())));
            var postParameters = new Dictionary<string, object>
            {
                {"accion", "consArchivo"},
                {
                    "archivo", new FileParameter
                    {
                        File = zip,
                        FileName = "rucs.zip"
                    }
                },
                {"codigoA", captcha}
            };

            HttpWebResponse webResponse = MultipartFormDataPost(Properties.Resources.RucMCons, postParameters);

            return GetProcessResponse(webResponse);
        }

        private IEnumerable<string[]> GetProcessResponse(HttpWebResponse response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
                throw new ArgumentException("No se obtuvo una respuesta correcta: " + response.StatusCode);

            var stream = response.GetResponseStream();
            if (stream == null)
                throw new ArgumentException("No se encontro una respuesta válida.");
            using (var rd = new StreamReader(stream))
            {
                var html = rd.ReadToEnd();
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                var nods = doc.DocumentNode.SelectNodes("//a[@target='_blank']");
                if(nods == null) 
                    throw new CaptchaException("Catpcha Incorrecto");
                var link = nods[0].Attributes["href"].Value;
                return DownloadZipAndProcess(link);
            } 
            
        }
        private static byte[] ToBytes(Stream input)
        {
            using (var ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        private IEnumerable<string[]> DownloadZipAndProcess(string link)
        {
            var req = (HttpWebRequest)WebRequest.Create(link);

            using (var response = req.GetResponse().GetResponseStream())
            {
                var streamTxt = ZipHelper.ExtractOnlyFile(ToBytes(response));              
                var contents = new Queue<string[]>();
                using (var r = new StreamReader(streamTxt, Encoding.GetEncoding("ISO-8859-1")))
                {
                    //Remove Header
                    r.ReadLine();
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        contents.Enqueue(line.Split('|'));
                    }
                }
                return contents;
            }

        }
        private string GetCaptchaString()
        {
            for (byte i = 0; i < 4; i++)
            {
                var captcha = GetCaptcha(Properties.Resources.RucMImage);
                if (captcha != null && captcha.Length == 4)
                    return captcha;
            }
            throw new ArgumentException("No se pudo obtener el capcha para el Ruc despues de 4 intentos.");
        }
        #endregion

        #region CatpcharResolver
        /// <inheritdoc />
        public override string GetCatpcha(Bitmap imagen)
        {
            ImageFilters.ImageToBlackAndWhite(imagen);
            using (var api = OcrApi.Create())
            {
                //Remote Server
#if DEBUG
                //api.Init(@"C:\Users\Administrador\Documents\History\SunatRuc\Web.Graph\bin", "eng", OcrEngineMode.OEM_TESSERACT_ONLY);
                api.Init(@"C:\Users\Giansalex\Source\Repos\github\SunatRuc\Ruc", "eng", OcrEngineMode.OEM_TESSERACT_ONLY);
#else
                api.Init(@"h:\root\home\giancarlos-001\www\pymestudio\bin", "eng", OcrEngineMode.OEM_TESSERACT_ONLY);
#endif
                api.SetVariable("tessedit_char_whitelist", "ABCDEFGHIJKLMNPQRSTUVWXYZ"); // Not Work
                var text = Regex.Replace(api.GetTextFromImage(imagen), "[^A-Za-z]", string.Empty);
                return text.ToUpper();
            }
        }

#endregion

    }
}
