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

        public void GetInfo(IEnumerable<string> rucs)
        {
            var values = rucs.ToArray();
            if (values.Length > 10)
            {
                GetOpcion2(values);
            }
            else
            {
                GetOpcion1(values);
            }
        }

        #endregion

        #region Private Methods

        private string GetOpcion1(IEnumerable<string> rucs)
        {
            var catpcha = GetCaptcha(UrlImage);
            var url = UrlConsult + "?accion=consManual&" + string.Join("&", rucs.Select(ruc => "selRuc=" + ruc)) + "&codigoM=" + catpcha;
            var http = CreateRequest(url);
            return GetProcessResponse((HttpWebResponse)http.GetResponse());
        }

        private void GetOpcion2(IEnumerable<string> rucs)
        {
            var builder = new StringBuilder();
            foreach (var ruc in rucs)
            {
                builder.AppendLine(ruc + "|");
            }
            var zip = ZipHelper.Compress(new KeyValuePair<string, byte[]>("ruc.txt", Encoding.ASCII.GetBytes(builder.ToString())));

            var catpcha = GetCaptcha(UrlImage);
            var http = CreateRequest(UrlConsult);
            var boundary = "--IMM";
            http.ContentType = "multipart/form-data; boundary=" + boundary;

            GetProcessResponse((HttpWebResponse)http.GetResponse());
        }

        private string GetProcessResponse(HttpWebResponse response)
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
                    var link = nods[0].Attributes["href"].Value;
                    DownloadZipAndProcess(link);
                } 
            }
            return "";
        }
        private static byte[] ToBytes(Stream input)
        {
            using (var ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        private void DownloadZipAndProcess(string link)
        {
            var req = (HttpWebRequest)WebRequest.Create(link);

            using (var response = req.GetResponse().GetResponseStream())
            {
                using (var streamTxt = ZipHelper.ExtractOnlyFile(ToBytes(response)))
                {
                    using (var r = new StreamReader(streamTxt, Encoding.GetEncoding("ISO-8859-1")))
                    {
                        var line = r.ReadLine();
                        //var cols = line.Split('|');
                        //foreach (var item in cols)
                        //{
                        //    datagrid.Columns.Add(item, item);
                        //}

                        //while ((line = r.ReadLine()) != null)
                        //{
                        //    datagrid.Rows.Add(line.Split('|'));
                        //}
                    }
                }
             
            }

        }

        #endregion

        #region CatpcharResolver
        public override string GetCatpcha(Bitmap imagen)
        {
            if (Optimize)
                ImageToBlackAndWhite(imagen);
            using (var api = OcrApi.Create())
            {
                api.Init(string.Empty, "eng", OcrEngineMode.OEM_TESSERACT_ONLY);
                api.SetVariable("tessedit_char_whitelist", "ABCDEFGHIJKLMNPQRSTUVWXYZ");
                var text = api.GetTextFromImage(imagen);
                return text.Trim();
            }
        }

        #endregion
    }
}
