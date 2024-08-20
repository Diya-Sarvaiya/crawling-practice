using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexPractice
{
    class Program
    {
        static void Main(string[] args)
        {
            var web = new HtmlWeb();
            var doc = web.Load("https://ineichen.com/auctions/timed-62/");
            HtmlNodeCollection parentNode = doc.DocumentNode.SelectNodes("//div[@class='lots-grid']");
            var card = ".//div[contains(@class,'aos-init')]";
            foreach (var node in parentNode)
            {
                var Est = node.SelectSingleNode(".//div[@class='lot-item__price-range'][span[contains(text(),'Est')]]");
                Console.WriteLine(Est.InnerText);
            }

            Console.ReadKey();
        }
    }
}
