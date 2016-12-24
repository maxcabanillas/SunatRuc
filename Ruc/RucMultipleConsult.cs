using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using Patagames.Ocr;
using Patagames.Ocr.Enums;
using Ruc.Helper;

namespace Ruc
{
    public class RucMultipleConsult : CatpchaResolver
    {
        #region Fields
        private const string UrlConsult = "http://www.sunat.gob.pe/cl-ti-itmrconsmulruc/jrmS00Alias";
        private const string UrlImage = "http://www.sunat.gob.pe/cl-ti-itmrconsmulruc/captcha?accion=image";
        #endregion

        #region Export

        public IEnumerable<string[]> GetInfo(params string[] rucs)
        {
            if (rucs.Length == 0)
                throw new ArgumentException("Se requiere almenos un Ruc", nameof(rucs));
            // Considerar la carga que aumenta.
            Optimize = true;
            
            //if (rucs.Length > 10)
            //{
            //    return GetOpcion2(rucs);
            //}
            var catpcha = GetCaptcha(UrlImage);
            var url = UrlConsult + "?accion=consManual&" + string.Join("&", rucs.Select(ruc => "selRuc=" + ruc)) + "&codigoM=" + catpcha;
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
            var catpcha = GetCaptcha(UrlImage);
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
                {"codigoA", catpcha}
            };

            HttpWebResponse webResponse = MultipartFormDataPost(UrlConsult, postParameters);

            return GetProcessResponse(webResponse);
        }

        private IEnumerable<string[]> GetProcessResponse(HttpWebResponse response)
        {
            if (response.StatusCode != HttpStatusCode.OK)
                throw new ArgumentException("No se obtuvo una respuesta correcta: " + response.StatusCode);

            using (var stream = response.GetResponseStream())
            {
                if (stream == null)
                    throw new ArgumentException("No se encontro una respuesta válida.");
                using (var rd = new StreamReader(stream))
                {
                    var html = rd.ReadToEnd();
                    var doc = new HtmlDocument();
                    doc.LoadHtml(html);
                    var nods = doc.DocumentNode.SelectNodes("//a[@target='_blank']");
                    if(nods == null) 
                        throw new Exception("Catpcha Incorrecto");
                    var link = nods[0].Attributes["href"].Value;
                    return DownloadZipAndProcess(link);
                } 
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
                using (var streamTxt = ZipHelper.ExtractOnlyFile(ToBytes(response)))
                {
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

        }

        #endregion

        #region CatpcharResolver
        public override string GetCatpcha(Bitmap imagen)
        {
#if !DEBUG
            //OcrApi.PathToEngine = @"h:\root\home\giancarlos-001\www\pymestudio\bin\x86\tesseract.dll";
#endif
            if (Optimize)
                ImageToBlackAndWhite(imagen);
            using (var api = OcrApi.Create())
            {
                //Remote Server
#if DEBUG
                //api.Init(@"C:\Users\Administrador\Documents\History\SunatRuc\Web.Graph\bin", "eng", OcrEngineMode.OEM_TESSERACT_ONLY);
                api.Init(@"C:\Users\Giansalex\Source\Repos\github\SunatRuc\Ruc", "eng", OcrEngineMode.OEM_TESSERACT_ONLY);
#else
                api.Init(@"h:\root\home\giancarlos-001\www\pymestudio\bin", "eng", OcrEngineMode.OEM_TESSERACT_ONLY);
#endif
                api.SetVariable("tessedit_char_whitelist", "ABCDEFGHIJKLMNPQRSTUVWXYZ");
                var text = api.GetTextFromImage(imagen);
                return text.Trim();
            }
        }

#endregion

    }
}
