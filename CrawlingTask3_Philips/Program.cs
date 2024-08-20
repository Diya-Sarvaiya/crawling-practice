using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data;

namespace CrawlingTask3_Philips
{
    class Program
    {
        static void Main(string[] args)
        {

            IWebDriver driver = new ChromeDriver();
            HtmlDocument doc = new HtmlDocument();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
            driver.Navigate().GoToUrl("https://www.phillips.com/auctions/past");
            /*string pageSource = wait.Until(x => driver.PageSource);
            doc.LoadHtml(pageSource);*/

           
            var nodes = wait.Until(x=>x.FindElements(By.XPath("//ul[contains(@class,'standard-list')]/li[@class]")));
            int no = 1;

            string connStr = "Server=DESKTOP-66UP5QF;Database=Philips;Integrated Security=True;";

            Auctions getAuc = new Auctions();
            Watches watch = new Watches();

            using (SqlConnection sqlconn = new SqlConnection(connStr))
            {
                sqlconn.Open();
                foreach (var node in nodes)
                {
                    AuctionsModel auc = new AuctionsModel();

                    Console.WriteLine(no + ")");
                    getAuc.GetAuctionData(node, auc);
                    getAuc.InsertUpdateIntoDB(auc, sqlconn);
                    getAuc.printAuctionData(auc);
                    watch.GetUrlFromList(auc.URL,sqlconn);

                    no++;
                    Console.WriteLine();

                }
                sqlconn.Close();
            }
            
            Console.WriteLine("ended successfully...");
            
            driver.Quit();
            Console.ReadKey();
        }

       

    }
}
