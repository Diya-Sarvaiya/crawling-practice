using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;
using System.Text.RegularExpressions;

namespace CrawlingTask4
{
    class Program
    {
        static string PageURL = @"https://www.phillips.com/auctions/past";
        static void Main(string[] args)
        {
            /* HtmlWeb web = new HtmlWeb();
             HtmlDocument doc = web.Load("https://www.phillips.com/auctions/past");
 */
            AuctionData();
            Console.ReadKey();
        }
        private static void AuctionAllURL(string AucURL,HtmlDocument doc)
        {
            IWebDriver driver = new ChromeDriver();
            driver.Url = $"https://www.phillips.com{AucURL}";
            /*driver.Navigate().GoToUrl($"https://www.phillips.com{AucURL}");*/
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
            string pageSource = wait.Until(x => driver.PageSource);

            // Load HTML into HtmlDocument
           /* HtmlDocument doc = new HtmlDocument();*/
            doc.LoadHtml(pageSource);
            WatchesModel AucModel = new WatchesModel();

            var AuctionURLNodes = wait.Until(x=>doc.DocumentNode.SelectNodes("//ul[contains(@class,'standard-grid')]/li"));
            foreach (var URLNode in AuctionURLNodes)
            {
                WatchesModel watchModel = new WatchesModel();
                try
                {
                    watchModel.URL = wait.Until(x=>URLNode.SelectSingleNode(".//div[contains(@class,'phillips-lot__image')]/a")).GetAttributeValue("href","");
                    
                    /*driver.Navigate().GoToUrl(watchModel.URL);*/
                }
                catch (Exception ex)
                {
                    watchModel.URL = "";
                }
                Console.WriteLine($"{watchModel.URL}");
                if(watchModel.URL== "https://www.phillips.com/detail/shepard-fairey/NY011024/151")
                {

                }

            }
            driver.Quit();
        }
        private static void AuctionData()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://www.phillips.com/auctions/past");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            string pageSource = wait.Until(x => driver.PageSource);

            // Load HTML into HtmlDocument
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(pageSource);
            AuctionsModel AucModel = new AuctionsModel();

            int n = 1;

            HtmlNodeCollection AuctionList = doc.DocumentNode.SelectNodes("//li[contains(@class,'has-image')]");
            if (AuctionList != null)
            {
                
                foreach (var node in AuctionList)
                {
                    AucModel.URL = node.SelectSingleNode(AucModel.TitleXpath).GetAttributeValue("href","");
                    AucModel.Title = node.SelectSingleNode(AucModel.TitleXpath).InnerText;
                    try
                    {
                        string RawDate = wait.Until(x=>node.SelectSingleNode(AucModel.DateRawXpath)).InnerText;
                        Regex locMatch = new Regex(AucModel.locationRegex);
                        Regex dateRawMatch = new Regex(AucModel.dateRawRegex);
                        AucModel.Location = locMatch.Match(RawDate).Groups[1].Value == null ? " " : locMatch.Match(RawDate).Groups[1].Value;
                        AucModel.DateRaw = dateRawMatch.Match(RawDate).Groups[1].Value == null ? " " : dateRawMatch.Match(RawDate).Groups[1].Value;
                    }
                    catch (Exception ex)
                    {
                        AucModel.Location = "";
                        AucModel.DateRaw = "";
                    }

                    PrintAUctionData(AucModel,n);
                    AuctionAllURL(AucModel.URL,doc);
                    n++;
                }
            }
            driver.Quit();
        }
        private static void PrintAUctionData(AuctionsModel auc,int n)
        {
            Console.WriteLine(n+") ");
            Console.WriteLine($"Title : {auc.Title} ");
            Console.WriteLine($"URL : {auc.URL}");
            Console.WriteLine($"Location : {auc.Location}");
            Console.WriteLine($"DateRaw : {auc.DateRaw}");
            Console.WriteLine();
        }
    }
}
