using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using HtmlAgilityPack;
using Patagames.Ocr;
using Patagames.Ocr.Enums;

namespace Ruc
{
    public class RucConsult : CatpchaResolver
    {
        #region Fields
        private const string UrlConsult = "http://e-consultaruc.sunat.gob.pe/cl-ti-itmrconsruc/jcrS00Alias";
        private const string UrlImage = "http://e-consultaruc.sunat.gob.pe/cl-ti-itmrconsruc/captcha?accion=image";
        #endregion

        #region Methods

        public void GetInfo(string ruc)
        {
            var catpcha = GetCaptcha(UrlImage);
            var url = UrlConsult + $"?accion=consPorRuc&nroRuc={ruc}&codigo={catpcha}&tipdoc=";
            var http = CreateRequest(url);
            var r = (HttpWebResponse)http.GetResponse();
            if (r.StatusCode == HttpStatusCode.OK)
            {
                var st = r.GetResponseStream();
                if(st == null) throw new ArgumentException("No valid Request");
                var coding = r.CharacterSet == null ? System.Text.Encoding.UTF8 : System.Text.Encoding.GetEncoding(r.CharacterSet);
                using (var rd = new StreamReader(st, coding))
                {
                    var e = rd.ReadToEnd();
                    GetDetails(HtmlEntity.DeEntitize(e));
                }
            }
        }

        /*public void Query2(string ruc)
        {
            var catpcha = GetCaptcha("http://e-consultaruc.sunat.gob.pe/cl-ti-itmrconsruc/captcha?accion=image");
            var data = "accion=consPorRuc&razSoc=&nroRuc=" + ruc +"&nrodoc=&contexto=ti-it&tQuery=on&search1=" +ruc +"&codigo="+ catpcha +"&tipdoc=1&search2=&coddpto=&codprov=&coddist=&search3=";
            var bytes = System.Text.Encoding.UTF8.GetBytes(data);
            var http = (HttpWebRequest)WebRequest.Create("http://e-consultaruc.sunat.gob.pe/cl-ti-itmrconsruc/jcrS00Alias");
            http.Method = "POST";
            http.UseDefaultCredentials = true;
            http.KeepAlive = true;
            http.ContentLength = bytes.Length;
            http.Headers.Add("Cache-Control", "max-age=0");
            http.Headers.Add("Origin", "http://e-consultaruc.sunat.gob.pe");
            http.Headers.Add("Upgrade-Insecure-Requests", "1");
            http.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
            http.Referer = "http://e-consultaruc.sunat.gob.pe/cl-ti-itmrconsruc/frameCriterioBusqueda.jsp";
            http.Headers.Add("Accept-Encoding", "gzip, deflate");
            http.Headers.Add("Accept-Language", "es-ES,es;q=0.8");
            http.CookieContainer = _cookies;

            using (var wr = http.GetRequestStream())
            {
                wr.Write(bytes, 0, bytes.Length);
            }

            var r = (HttpWebResponse)http.GetResponse();
            
            if (r.StatusCode == HttpStatusCode.OK)
            {
                using (var rd = new StreamReader(r.GetResponseStream()))
                {
                    var e = rd.ReadToEnd();
                }
            }
        }*/
        #endregion

        #region Private Methods
        private Dictionary<string, string> GetDetails(string html)
        {
            
            var page = new HtmlDocument();
            page.LoadHtml(html);
            var table = page.DocumentNode.SelectSingleNode("/html/body/table[1]");
            var dic = new Dictionary<string, string>();
            string temp = string.Empty;
            foreach (var item in table.ChildNodes)
            {
                if (item.NodeType != HtmlNodeType.Element && item.Name != "tr") continue;
                var i = 0;
                foreach (var item2 in item.ChildNodes)
                {
                    if (item2.NodeType != HtmlNodeType.Element
                        && item2.Name != "td") continue;
                    i++;
                    if(i == 1)
                    {
                        temp = item2.InnerText;

                    }
                    else
                    {
                        ClearComment(item2);
                        dic.Add(temp, item2.InnerText.Trim());
                        i = 0;
                    }
                }
                
            }
            return dic;
            //var rzs = table.SelectSingleNode("tr[1]/td[2]").InnerText.Split('-');
            //dic.Add("ruc", rzs[0].Trim());
            //dic.Add("razonsocial", rzs[1].Trim());
            //dic.Add("tipoContribuyente", table.SelectSingleNode("tr[2]/td[2]").InnerText);
        }

        private void ClearComment(HtmlNode node)
        {
            if(!node.HasChildNodes) return;
            var childs = node.ChildNodes.ToArray();
            foreach (var child in childs)
            {
                if (child.NodeType == HtmlNodeType.Comment)
                    child.Remove();
                else if (child.NodeType == HtmlNodeType.Element)
                    ClearComment(child);
                    
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
