using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using HtmlAgilityPack;
using static CrawlingTask1.Auction;
using System.Text.RegularExpressions;
using System.Data;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace CrawlingTask1
{
    class Program
    {
        public static string link = "https://ineichen.com";

        public static void GetName()
        {
            var web = new HtmlWeb();
            var doc = web.Load("https://watchcollecting.com/for-sale/2019-breitling-superocean-46-black-steel");

            var parentDivNodes = doc.DocumentNode.SelectSingleNode("//div[@class='charge-info']");

            Console.WriteLine("");
        }

        public static void getBigImg()
        {
            var web = new HtmlWeb();
            HtmlDocument doc = web.Load("https://www.sothebys.com/en/buy/pdp/luxury/watches/watch/_audemars-piguet-royal-oak-reference-15451stzz1256st03-a-diamond-set-stainless-steel-automatic-wristwatch-with-date-2018-83af?locale=en");

            var ImgXpath = "//img[contains(@class,'carousel_mainImage__KNh2W')]";
            HtmlNodeCollection ImgNodes = doc.DocumentNode.SelectNodes(ImgXpath);
            int n = 1;

            List<string> ImgRaw = new List<string>();

            foreach (HtmlNode Node in ImgNodes)
            {
                Console.WriteLine($"{n++}) {Node.GetAttributeValue("src","")}");
                ImgRaw.Add(Node.GetAttributeValue("src", ""));
            }
            string pattern = @"com(/.*?)%2F..%2F";
            Regex regex = new Regex(pattern);
            for(int i=0; i<ImgRaw.Count;i++)
            {
                ImgRaw[i] = Regex.Replace(ImgRaw[1], regex.Match(pattern).Groups[1].Value, "");
                Console.WriteLine(ImgRaw[i]);
            }

        }

        public static void waitHelperDemo()
        {
            IWebDriver driver = new ChromeDriver();

            // Navigate to a page
            driver.Navigate().GoToUrl("https://watchcollecting.com/for-sale/2023-omega-speedmaster-silver-snoopy-award-8");

            Console.WriteLine("Opened page...");

            // Create WebDriverWait instance
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));

            // Wait for the element to be visible
            var elements = wait.Until(dr =>
            {
                return dr.FindElements(By.XPath("//div[contains(@class,'product-overview__columns')][1]//ul[contains(.,'Box:')]"));
            });

            foreach (var i in elements)
            {
                Console.WriteLine();
            }
           
           /* foreach(HtmlNode node in elements)
            {
                Console.WriteLine(node);
            }*/

            // Close the browser
            driver.Quit();
        }

        static void Main(string[] args)
        {
            waitHelperDemo();
           /* getBigImg();*/
            /*GetName();

            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://ineichen.com/auctions/past/");

            //div[@class='product-reserve-info']

            var web = new HtmlWeb();
            var doc = web.Load("https://ineichen.com/auctions/timed-64/2/");

            var allNodes = doc.DocumentNode.SelectNodes("//div[@class='product-reserve-info']");

            string NameRegex = @"(.*?)";
*/


            /*var web = new HtmlWeb();
            var doc = web.Load("https://ineichen.com/auctions/past/");*/

            /*List<string> title = TitleList(doc);
            List<string> description = DesciptionList(doc);
            List<string> imageUrl = ImageUrlList(doc);
            List<string> link = LinkList(doc);
            List<int> lotCount = LotCountList(doc);
            List<int> startDate = StartDateList(doc);
            List<string> startMonth = StartMonthList(doc);
            List<int> startYear = StartYearList(doc);
            List<string> startTime = StartTimeList(doc);
            List<int> endDate = EndDateList(doc);
            List<string> endMonth = EndMonthList(doc);
            List<int> endYear = EndYearList(doc);
            List<string> endTime = EndTimeList(doc);
            List<string> location = LocationList(doc);*/

            Auction allData = new Auction();

           /* for (int i = 1; i <= 50; i++)
            {
                allData.Title = title[i];
                allData.Description = description[i];
                allData.ImageUrl = imageUrl[i];
                allData.Link = link[i];
                allData.LotCount = lotCount[i];
                allData.StartDate = startDate[i];
                allData.StartMonth = startMonth[i];
                allData.StartYear = startYear[i];
                allData.StartTime = startTime[i];
                allData.EndDate = endDate[i];
                allData.EndMonth = endMonth[i];
                allData.EndYear = endYear[i];
                allData.EndTime = endTime[i];
                allData.Location = location[i];
            }*/

            //InsertIntoDB();


           /* List<string> title = TitleList(doc);
            printList<string>(title);
            List<string> description = DesciptionList(doc);
            printList<string>(description);
            List<string> imageUrl = ImageUrlList(doc);
            printList<string>(imageUrl);
            List<string> link = LinkList(doc);
            printList<string>(link);
            List<string> lotCount = LotCountList(doc);
            printList<string>(lotCount);*/


           /* List<string> startDate = StartDateList(doc);
            printList<string>(startDate);
            List<string> startMonth = StartMonthList(doc);
            printList<string>(startMonth);
            List<string> startYear = StartYearList(doc);
            printList<string>(startYear);
            List<string> endDate = EndDateList(doc);
            printList<string>(endDate);
            List<string> endMonth = EndMonthList(doc);
            printList<string>(endMonth);
            List<string> endYear = EndYearList(doc);
            printList<string>(endYear);
            List<string> location = LocationList(doc);
            printList<string>(location);
            List<string> startDate = StartDateList(doc);
            printList<string>(startDate);
            List<string> startTime = StartTimeList(doc);
            printList<string>(startTime);*/


            Console.ReadKey();
        }

       /* private static void InsertIntoDB(Auction data)
        {
            string connStr = "Server=DESKTOP-B32RQ3U;Database=Learning;Integrated Security=True;";
            string connectionString = "Your_Connection_String_Here";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                foreach (var data in auctionDataList)
                {
                    using (SqlCommand command = new SqlCommand("InsertAuctionData", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters to the stored procedure
                        command.Parameters.AddWithValue("@Title", data.Title);
                        command.Parameters.AddWithValue("@Description", data.Description);
                        command.Parameters.AddWithValue("@ImageUrl", data.ImageUrl);
                        command.Parameters.AddWithValue("@LotCount", data.LotCount);
                        command.Parameters.AddWithValue("@StartDate", data.StartDate);
                        command.Parameters.AddWithValue("@StartMonth", data.StartMonth);
                        command.Parameters.AddWithValue("@StartYear", data.StartYear);
                        command.Parameters.AddWithValue("@StartTime", data.StartTime);
                        command.Parameters.AddWithValue("@EndDate", data.EndDate);
                        command.Parameters.AddWithValue("@EndMonth", data.EndMonth);
                        command.Parameters.AddWithValue("@EndYear", data.EndYear);
                        command.Parameters.AddWithValue("@EndTime", data.EndTime);
                        command.Parameters.AddWithValue("@Location", data.Location);

                        // Execute the stored procedure
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
*/
        public static void printList<T>(List<T> list)
        {
            var no = 1;
            foreach (var data in list)
            {
                Console.WriteLine(no+") "+data);
                /*   Console.WriteLine(data);*/
                no++;
            }
        }

        public static List<Auction> GetAll(HtmlDocument doc)
        {
            var titleNodes = doc.DocumentNode.SelectNodes("//*[@class='auction-item__name']/a");
            var descriptionNodes = doc.DocumentNode.SelectNodes("//*[@class='auction-date-location']");
            var ImageUrlNodes = doc.DocumentNode.SelectNodes("//a[contains(@class,'auction-item__image')]/img");
            var LinkNodes = doc.DocumentNode.SelectNodes("//div[contains(@class,'auction-item__btns')]/a");
            var LotCountNodes = doc.DocumentNode.SelectNodes("//div[contains(@class,'auction-item__btns')]/a");
            var StartDateNodes = doc.DocumentNode.SelectNodes("//div[@class='auction-date-location']//div[i[contains(@class,'mdi-clock-outline')]]");
            var StartMonthNodes = doc.DocumentNode.SelectNodes("//div[@class='auction-date-location']//div[i[contains(@class,'mdi-clock-outline')]]");
            var StartYearNodes = doc.DocumentNode.SelectNodes("//div[@class='auction-date-location']//div[i[contains(@class,'mdi-clock-outline')]]");
            var StartTimeNodes = doc.DocumentNode.SelectNodes("//div[@class='auction-date-location']//div[i[contains(@class,'mdi-clock-outline')]]");
            var EndDateNodes = doc.DocumentNode.SelectNodes("//div[@class='auction-date-location']//div[i[contains(@class,'mdi-clock-outline')]]");
            var EndMonthNodes = doc.DocumentNode.SelectNodes("//div[@class='auction-date-location']//div[i[contains(@class,'mdi-clock-outline')]]");
            var EndYearNodes = doc.DocumentNode.SelectNodes("//div[@class='auction-date-location']//div[i[contains(@class,'mdi-clock-outline')]]");
            var EndTimeNodes = doc.DocumentNode.SelectNodes("//div[@class='auction-date-location']//div[i[contains(@class,'mdi-clock-outline')]]");
            var LocationNodes = doc.DocumentNode.SelectNodes("//div[@class='auction-date-location']//div[i[contains(@class,'mdi-map-marker-outline')]|i[contains(@class,'mdi-web')]]");

            List<Auction> allData = new List<Auction>();


            return allData;
        }


        public static List<string> TitleList(HtmlDocument doc)
        {
            var nodes = doc.DocumentNode.SelectNodes("//*[@class='auction-item__name']/a");
            var titledata = new List<string>();

            if (nodes != null);
            {
                foreach (var node in nodes)
                {
                    titledata.Add(node.InnerText.Trim());
                }
            }
           
            return titledata;
        }

        public static List<string> DesciptionList(HtmlDocument doc)
        {
            var descriptionNodes = doc.DocumentNode.SelectNodes("//*[@class='auction-date-location']");
            var description = new List<string>();
            if (descriptionNodes != null)
            {
                string pattern = @"\s+";

                foreach (var node in descriptionNodes)
                {
                    string des = Regex.Replace(node.InnerText.Trim(), pattern," ");
                    description.Add(des);
                }
            }

            return description;
        }

        public static List<string> ImageUrlList(HtmlDocument doc)
        {

            var ImageUrlNodes = doc.DocumentNode.SelectNodes("//a[contains(@class,'auction-item__image')]/img");
            var ImageUrl = new List<string>();
            if (ImageUrlNodes != null)
            {
                foreach (var node in ImageUrlNodes)
                {
                    string src = node.GetAttributeValue("src", string.Empty);
                    string url = string.Concat(link, src);
                    ImageUrl.Add(url);
                }
            }

            return ImageUrl;
        }

        public static List<string> LinkList(HtmlDocument doc)
        {
            var LinkNodes = doc.DocumentNode.SelectNodes("//div[contains(@class,'auction-item__btns')]/a");
            var Link = new List<string>();
            if (LinkNodes != null)
            {
                foreach (var node in LinkNodes)
                {
                    var href = node.GetAttributeValue("href", string.Empty); 
                    string url = string.Concat(link, href);
                    Link.Add(url);
                }
            }

            return Link;
        }
        public static List<string> LotCountList(HtmlDocument doc)
        {
            var LotCountNodes = doc.DocumentNode.SelectNodes("//div[contains(@class,'auction-item__btns')]/a");
            var LotCount = new List<string>();
            if (LotCountNodes != null)
            {
                string pattern = @"(?<=View\s)\d+(?=\slots)";
                
                foreach (var node in LotCountNodes)
                {
                    Match match = Regex.Match(node.InnerText,pattern);
                   if(match.Success)
                        LotCount.Add(match.Value);
                }
            }

            return LotCount;
        }

        public static List<int> StartDateList(HtmlDocument doc)
        {
            var StartDateNodes = doc.DocumentNode.SelectNodes("//div[@class='auction-date-location']//div[i[contains(@class,'mdi-clock-outline')]]");
            var StartDate = new List<int>();
            if (StartDate != null)
            {
                string pattern = @"^\d+(?=-|\s)";
                foreach (var node in StartDateNodes)
                {
                    Match match = Regex.Match(node.InnerText.Trim(), pattern);
                    StartDate.Add(Convert.ToInt32(match));
                }
            }
            return StartDate;
        }

        public static List<string> StartMonthList(HtmlDocument doc)
        {
            var StartMonthNodes = doc.DocumentNode.SelectNodes("//div[@class='auction-date-location']//div[i[contains(@class,'mdi-clock-outline')]]");
            var StartMonth = new List<string>();
            if (StartMonth != null)
            {
                string pattern = @"(?<=\d*\w*)[A-Z]+";
                foreach (var node in StartMonthNodes)
                {
                    Match match = Regex.Match(node.InnerText.Trim(), pattern);
                    if(match.Success)
                        StartMonth.Add(match.ToString());
                }
            }
            return StartMonth;
        }
        public static List<int> StartYearList(HtmlDocument doc)
        {
            var StartYearNodes = doc.DocumentNode.SelectNodes("//div[@class='auction-date-location']//div[i[contains(@class,'mdi-clock-outline')]]");
            var StartYear = new List<int>();
            if (StartYear != null)
            {
                string pattern = @"(?<=[A-Z]+\s)\d+$";
                foreach (var node in StartYearNodes)
                {
                    Match match = Regex.Match(node.InnerHtml.Trim(), pattern);
                    StartYear.Add(Convert.ToInt32(match));
                }
            }
            return StartYear;
        }
        public static List<string> StartTimeList(HtmlDocument doc)
        {
            var StartTimeNodes = doc.DocumentNode.SelectNodes("//div[@class='auction-date-location']//div[i[contains(@class,'mdi-clock-outline')]]");
            var StartTime = new List<string>();
            if (StartTime != null)
            {
                string pattern = @"(?<=\,\s)\d{2}\:\d{2}\s\(?CET\)?(?=\s\d{1,2})";
                foreach (var node in StartTimeNodes)
                {
                    Match match = Regex.Match(node.InnerHtml.Trim(), pattern);
                    StartTime.Add(match.ToString());
                }
            }
            return StartTime;
        }
        public static List<int> EndDateList(HtmlDocument doc)
        {
            var EndDateNodes = doc.DocumentNode.SelectNodes("//div[@class='auction-date-location']//div[i[contains(@class,'mdi-clock-outline')]]");
            var EndDate = new List<int>();
            if (EndDate != null)
            {
                string pattern = @"(?<=\s|-)\d{1,2}(?=\s)";
                foreach (var node in EndDateNodes)
                {
                    Match match = Regex.Match(node.InnerText.Trim(), pattern);
                    EndDate.Add(Convert.ToInt32(match));
                }
            }
            return EndDate;
        }

        public static List<string> EndMonthList(HtmlDocument doc)
        {
            var EndMonthNodes = doc.DocumentNode.SelectNodes("//div[@class='auction-date-location']//div[i[contains(@class,'mdi-clock-outline')]]");
            var EndMonth = new List<string>();
            if (EndMonth != null)
            {
                string pattern = @"(?<=\s\d{1,2}\s|-\d{1,2}\s)[A-Z]+";
                foreach (var node in EndMonthNodes)
                {
                    Match match = Regex.Match(node.InnerText.Trim(), pattern);
                    EndMonth.Add(match.ToString());
                }
            }
            return EndMonth;
        }
        public static List<int> EndYearList(HtmlDocument doc)
        {
            var EndYearNodes = doc.DocumentNode.SelectNodes("//div[@class='auction-date-location']//div[i[contains(@class,'mdi-clock-outline')]]");
            var EndYear = new List<int>();
            if (EndYear != null)
            {
                string pattern = @"(?<=\s\d+\s[A-Z]+\s)\d{3,4}$";
                foreach (var node in EndYearNodes)
                {
                    Match match = Regex.Match(node.InnerText.Trim(), pattern);
                    EndYear.Add(Convert.ToInt32(match));
                }
            }
            return EndYear;
        }
        public static List<string> EndTimeList(HtmlDocument doc)
        {
            var EndTimeNodes = doc.DocumentNode.SelectNodes("//div[@class='auction-date-location']//div[i[contains(@class,'mdi-clock-outline')]]");
            var EndTime = new List<string>();
            if (EndTime != null)
            {
                string pattern = @"\d{2}\:\d{2}\s\(?CET\)?(?=\s\d{1,2})";
                foreach (var node in EndTimeNodes)
                {
                    Match match = Regex.Match(node.InnerHtml.Trim(), pattern);
                    EndTime.Add(match.ToString());
                }
            }
            return EndTime;
        }
        public static List<string> LocationList(HtmlDocument doc)
        {
            var LocationNodes = doc.DocumentNode.SelectNodes("//div[@class='auction-date-location']//div[i[contains(@class,'mdi-map-marker-outline')]|i[contains(@class,'mdi-web')]]");
            var Location = new List<string>();
            if (Location != null)
            {
                foreach (var node in LocationNodes)
                {
                    Location.Add(node.InnerText.Trim());
                }
            }
            return Location;
        }
    }
}
