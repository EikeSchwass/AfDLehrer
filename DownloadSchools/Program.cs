using System;
using System.Collections.Generic;
using System.Linq;
using Flurl.Http;
using HtmlAgilityPack;

namespace DownloadSchools
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var grundschulen = GetSchulen("http://www.schulliste.eu/type/grundschulen/?bundesland=&start=").ToArray();
            var realschulen = GetSchulen("http://www.schulliste.eu/type/realschulen/?bundesland=&start=").ToArray();
            var gymnasien = GetSchulen("http://www.schulliste.eu/type/gymnasien/?bundesland=&start=").ToArray();

            string s = string.Join("\",\"", grundschulen.Concat(realschulen).Concat(gymnasien));
            Console.WriteLine(s);
        }

        private static IEnumerable<string> GetSchulen(string address)
        {
            for (var i = 0; i <= 500; i += 20)
            {
                string tempAddress = address + i;
                string result = tempAddress.WithHeaders(new Dictionary<string, string>
                {
                    {"Accept","text/html,application/xhtml+xm…plication/xml;q=0.9,*/*;q=0.8"},
                    {"Accept-Encoding","gzip, deflate"},
                    {"Accept-Language","de,en-US;q=0.7,en;q=0.3"},
                    {"Connection","keep-alive"},
                    {"DNT","1"},
                    {"Host","www.schulliste.eu"},
                    {"Upgrade-Insecure-Requests","1"},
                    {"User-Agent","Mozilla/6.0 (Windows NT 10.0; Win64; x64; rv: 64.0) Gecko/20100101 Firefox/61.0"}
                }).GetAsync().Result.Content.ReadAsStringAsync().Result;

                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(result);

                var htmlNodes = htmlDocument.DocumentNode.Descendants().Where(n => n.GetClasses().Contains("school_name")).ToList();
                if (!htmlNodes.Any())
                    break;
                foreach (var htmlNode in htmlNodes)
                {
                    yield return htmlNode.InnerText;
                }
            }
        }
    }
}
