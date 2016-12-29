using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Patagames.Ocr;
using Patagames.Ocr.Enums;

namespace Ruc
{
    public class DniConsult : CatpchaResolver
    {
        #region Fields
        private const string UrlConsult = "https://cel.reniec.gob.pe/valreg/valreg.do";
        private const string UrlImage = "https://cel.reniec.gob.pe/valreg/codigo.do";
        #endregion

        #region Export

        public string[] Get(string dni)
        {
            return GetInternal(dni);
        }

        #endregion

        #region Private Methods

        private string[] GetInternal(string dni)
        {
            var catpcha = GetCaptcha(UrlImage);
            var url = UrlConsult + $"?accion=buscar&nuDni={dni}&imagen={catpcha}";
            var http = CreateRequest(url);
            var html = GetContent((HttpWebResponse)http.GetResponse());
            var doc = new HtmlDocument();
            doc.LoadHtml(HtmlEntity.DeEntitize(html));
            var nodName = doc.DocumentNode.SelectSingleNode("/html/body/table/tr[4]/td[1]/table[2]/tr[2]/td");
            var fullName = nodName.FirstChild.InnerText;
            if (fullName.StartsWith("Ingrese el código")) throw new ArgumentException("Captcha Incorrecto", nameof(catpcha));
            var values = new Queue<string>();
            using (var reader = new StringReader(fullName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if(line == string.Empty) continue;
                    values.Enqueue(line.Trim());
                }
            }
            return values.ToArray();        
        }

        private string GetContent(HttpWebResponse response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
                throw new ArgumentException("No se obtuvo una respuesta correcta: " + response.StatusCode);
            var stream = response.GetResponseStream();
            if (stream == null)
                throw new ArgumentException("No se encontro una respuesta válida.");
            using (var rd = new StreamReader(stream))
            {
                var html = rd.ReadToEnd();
                return html;
            }
        }
        private static void ToOptimize(Bitmap bm)
        {
            for (var x = 0; x < bm.Width; x++)
                for (var y = 0; y < bm.Height; y++)
                {
                    var pix = bm.GetPixel(x, y);
                    var isBlue = pix.B > 130 && pix.R < 20 && pix.G < 130;
                    bm.SetPixel(x, y, isBlue ? Color.Black : Color.White);
                }
            //bm.Save("image.jpg");
        }
        #endregion
        #region CapchaResolver
        public override string GetCatpcha(Bitmap imagen)
        {
            ToOptimize(imagen);
            using (var api = OcrApi.Create())
            {
                //Remote Server
#if DEBUG
                api.Init(@"C:\Users\Administrador\Documents\History\SunatRuc\Web.Graph\bin", "eng", OcrEngineMode.OEM_TESSERACT_ONLY);
                //api.Init(@"C:\Users\Giansalex\Source\Repos\github\SunatRuc\Ruc", "eng", OcrEngineMode.OEM_TESSERACT_ONLY);
#else
                api.Init(@"h:\root\home\giancarlos-001\www\pymestudio\bin", "eng", OcrEngineMode.OEM_TESSERACT_ONLY);
#endif
                //api.SetVariable("tessedit_char_whitelist", "ABCDEFGHIJKLMNPQRSTUVWXYZ0123456789");
                var text = Regex.Replace(api.GetTextFromImage(imagen), "[^A-Za-z0-9]", string.Empty);
                return text.ToUpper();
            }
        }
        #endregion

    }
}
