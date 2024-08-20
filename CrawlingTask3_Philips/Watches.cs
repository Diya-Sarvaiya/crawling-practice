using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace CrawlingTask3_Philips
{
    class Watches
    {
        string connStr = "Server=DESKTOP-66UP5QF;Database=Philips;Integrated Security=True;";
        public void GetUrlFromList(string auctionUrl, SqlConnection sqlConn)
        {

            IWebDriver driver = new ChromeDriver();
            HtmlDocument doc = new HtmlDocument();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Navigate().GoToUrl(auctionUrl);
            /* string pageSource = wait.Until(x => driver.PageSource);
             doc.LoadHtml(pageSource);*/



            var watchNodes = wait.Until(x => driver.FindElements(By.XPath("//ul[contains(@class,'standard-grid')]/li")));
            /*var watchNodes = wait.Until(x =>doc.DocumentNode.SelectNodes("//ul[contains(@name,'auctions')]/li"));*/


            foreach (var nd in watchNodes)
            {
                WatchModel watchModel = new WatchModel();
                try
                {
                    var watchUrl = nd.FindElement(By.XPath(".//div[contains(@class,'phillips-lot__image')]/a"));
                    watchModel.URL = watchUrl.GetAttribute("href");
                    /* driver.Navigate().GoToUrl(watchModel.URL);*/
                    WatchesData(watchModel, driver, nd, wait);
                    InsertUpdateWatchesIntoDB(watchModel, sqlConn);
                }
                catch (Exception ex)
                {
                    watchModel.URL = "";
                }
                Console.WriteLine($"{watchModel.URL}");

                /* using (SqlConnection sqlConn = new SqlConnection(connStr))
                 {
                     sqlConn.Open();
                     GetDataFromWatches(watchModel, sqlConn);
                     sqlConn.Close();
                 }*/

                /*GetDataFromWatches(watchModel);*/
                /*IWebDriver driver1 = new ChromeDriver();
                driver1.Navigate().GoToUrl(watchModel.URL);

                var ID = nd.FindElement(By.XPath("//h3[contains(@class,'lot-page__lot__number')]"));
                watchModel.ID = Convert.ToInt32(ID.Text);
                Console.WriteLine(watchModel.ID);
                driver1.Close();*/

            }
            driver.Close();
            /* driver.Navigate().Back();*/
        }
        public void WatchesData(WatchModel watchModel, IWebDriver driver, IWebElement nd, WebDriverWait wait)
        {

            IWebDriver driver1 = new ChromeDriver();
            HtmlDocument doc = new HtmlDocument();
            WebDriverWait wait1 = new WebDriverWait(driver1, TimeSpan.FromSeconds(10));
            driver1.Navigate().GoToUrl(watchModel.URL);

            watchModel.ID = GetID(watchModel, wait1);
            watchModel.Title = GetTitle(watchModel, wait1);
            watchModel.Description = GetDescription(watchModel, wait1);
            if (watchModel.ID == 5)
            {

            }
            watchModel.PriceRaw = GetPriceRaw(watchModel, wait1);
            watchModel.EstimatePriceRaw = GetEstimatePriceRaw(watchModel, wait1);
            watchModel.IsSold = GetIsSold(watchModel, wait1,driver1);
            watchModel.DimensionsRaw = GetDimensionsRaw(watchModel, wait1);
            watchModel.Manufacturer = GetManufacturer(watchModel, wait1);

            //urlID 
            string urlIDPattern = @"(\w+)/\w+$";
            Regex urlIDRegex = new Regex(urlIDPattern);
            watchModel.UrlID = urlIDRegex.Match(watchModel.URL).Groups[1].Value;


            printWatchData(watchModel);
            driver1.Quit();

        }

        private void InsertUpdateWatchesIntoDB(WatchModel watchModel, SqlConnection sqlConn)
        {
            using (SqlCommand sqlCmd = new SqlCommand("PR_CheckForDuplicate_Watches", sqlConn))
            {
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@urlID", watchModel.UrlID);
                sqlCmd.Parameters.AddWithValue("@id", watchModel.ID);
                int count = (int)sqlCmd.ExecuteScalar();
                Console.WriteLine("count : " + count);

                if (count > 0)
                {
                    using (SqlCommand sqlCmd2 = new SqlCommand("PR_Update_Watches", sqlConn))
                    {
                        sqlCmd2.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlCmd2.Parameters.AddWithValue("@urlID", watchModel.UrlID);
                        sqlCmd2.Parameters.AddWithValue("@id", watchModel.ID);
                        sqlCmd2.Parameters.AddWithValue("@title", watchModel.Title);
                        sqlCmd2.Parameters.AddWithValue("@priceRaw", watchModel.PriceRaw);
                        sqlCmd2.Parameters.AddWithValue("@isSold", watchModel.IsSold);
                        sqlCmd2.Parameters.AddWithValue("@estimatePriceRaw", watchModel.EstimatePriceRaw);
                        sqlCmd2.Parameters.AddWithValue("@dimensionsRaw", watchModel.DimensionsRaw);
                        sqlCmd2.Parameters.AddWithValue("@description", watchModel.Description);
                        sqlCmd2.Parameters.AddWithValue("@manufacturer", watchModel.Manufacturer);

                        sqlCmd2.ExecuteNonQuery();
                    }
                }
                else
                {
                    using (SqlCommand sqlCmd1 = new SqlCommand("PR_Insert_Watches", sqlConn))
                    {
                        sqlCmd1.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlCmd1.Parameters.AddWithValue("@urlID", watchModel.UrlID);
                        sqlCmd1.Parameters.AddWithValue("@id", watchModel.ID);
                        sqlCmd1.Parameters.AddWithValue("@title", watchModel.Title);
                        sqlCmd1.Parameters.AddWithValue("@priceRaw", watchModel.PriceRaw);
                        sqlCmd1.Parameters.AddWithValue("@isSold", watchModel.IsSold);
                        sqlCmd1.Parameters.AddWithValue("@estimatePriceRaw", watchModel.EstimatePriceRaw);
                        sqlCmd1.Parameters.AddWithValue("@dimensionsRaw", watchModel.DimensionsRaw);
                        sqlCmd1.Parameters.AddWithValue("@description", watchModel.Description);
                        sqlCmd1.Parameters.AddWithValue("@manufacturer", watchModel.Manufacturer);

                        sqlCmd1.ExecuteNonQuery();
                    }

                }

            }
        }

        private void printWatchData(WatchModel watchModel)
        {
            Console.WriteLine("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - ");
            Console.WriteLine("ID : " + watchModel.ID);
            Console.WriteLine("Title : " + watchModel.Title);
            Console.WriteLine("PriceRaw : " + watchModel.PriceRaw);
            Console.WriteLine("IsSold : " + watchModel.IsSold);
            Console.WriteLine("EstimatePriceRaw : " + watchModel.EstimatePriceRaw);
            Console.WriteLine("DimensionsRaw : " + watchModel.DimensionsRaw);
            Console.WriteLine("Description : " + watchModel.Description);
            Console.WriteLine("Manufacturer : " + watchModel.Manufacturer);
            Console.WriteLine("URLID : " + watchModel.UrlID);
            Console.WriteLine("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - ");
        }

        private string GetDimensionsRaw(WatchModel watchModel, WebDriverWait wait1)
        {
            watchModel.DimensionsRaw = wait1.Until(x =>
            {
                try
                {
                    // Attempt to find the element
                    return x.FindElement(By.XPath("//span[contains(text(),'cm')]")).Text;
                }
                catch (NoSuchElementException)
                {
                    // Return null if the element is not found
                    return x.FindElement(By.XPath("//p[contains(@class,'lot-page__lot__sold')]")).Text;
                }
            });
            return watchModel.DimensionsRaw;
        }
        private bool GetIsSold(WatchModel watchModel, WebDriverWait wait1,IWebDriver driver1)
        {
            try
            {
                var isSold = driver1.FindElement(By.XPath("//p[contains(@class,'lot-page__lot__sold')]"));
                return watchModel.IsSold = true;
            }
            catch (Exception ex)
            {
                return watchModel.IsSold = false;
            }
                
           

            /*try
            {
                watchModel.IsSold = wait1.Until(x =>
                {
                    try
                    {
                        var element = x.FindElement(By.XPath("//p[contains(@class,'lot-page__lot__sold')]"));
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                });
            }
            catch (TimeoutException)
            {

                return watchModel.IsSold = false;
            }*/
            /*if (watchPriceRaw is null)
            {
                watchModel.IsSold = false;
            }
            else
            {
                watchModel.IsSold = true;
            }*/

            return watchModel.IsSold;

        }
        private string GetEstimatePriceRaw(WatchModel watchModel, WebDriverWait wait1)
        {
            watchModel.EstimatePriceRaw = wait1.Until(x =>
            {
                try
                {
                    // Attempt to find the element
                    return x.FindElement(By.XPath("//p[contains(@class,'lot-page__lot__estimate')]")).Text;
                }
                catch (NoSuchElementException)
                {
                    // Return null if the element is not found
                    return "";
                }
            });
            return watchModel.EstimatePriceRaw;
        }
        private string GetPriceRaw(WatchModel watchModel, WebDriverWait wait1)
        {

            watchModel.PriceRaw = wait1.Until(x =>
               {
                   try
                   {
                       // Attempt to find the element
                       return x.FindElement(By.XPath("//p[contains(@class,'lot-page__lot__sold')]")).Text;
                   }
                   catch (NoSuchElementException)
                   {
                       // Return null if the element is not found
                       return "";
                   }
               });

            return watchModel.PriceRaw;
        }
        private string GetTitle(WatchModel watchModel, WebDriverWait wait1)
        {
            watchModel.Title = wait1.Until(x =>
            {
                try
                {
                    return x.FindElement(By.XPath("//h1[contains(@class,'lot-page__lot__maker__name')]")).Text;
                }
                catch (NoSuchElementException)
                {
                    return "";
                }
            });
            return watchModel.Title;
        }
        private int GetID(WatchModel watchModel, WebDriverWait wait1)
        {
            var watchID = wait1.Until(x => x.FindElement(By.XPath("//h3[contains(@class,'lot-page__lot__number')]")));
            watchModel.ID = Convert.ToInt32(watchID.Text);
            return Convert.ToInt32(watchModel.ID);
        }
        private string GetDescription(WatchModel watchModel, WebDriverWait wait1)
        {

            watchModel.Description = wait1.Until(x =>
            {
                try
                {
                    // Attempt to find the element
                    return x.FindElement(By.XPath("//p[contains(@class,'lot-page__lot__additional-info')]")).Text;
                }
                catch (NoSuchElementException)
                {
                    // Return null if the element is not found
                    return "";
                }
            });

            return watchModel.Description;

        }

        private string GetManufacturer(WatchModel watchModel, WebDriverWait wait1)
        {
            watchModel.Manufacturer = wait1.Until(x =>
            {
                try
                {
                    return x.FindElement(By.XPath("//h1[contains(@class,'lot-page__lot__maker__name')]")).Text;
                }
                catch (NoSuchElementException)
                {
                    return "";
                }
            });
            return watchModel.Manufacturer;
        }

        public void GetDataFromWatches(WatchModel watchModel, SqlConnection sqlConn)
        {
            using (SqlCommand sqlCmd = new SqlCommand("PR_GetData_Auction", sqlConn))
            {
                sqlCmd.CommandType = CommandType.StoredProcedure;
                IDataReader reader = sqlCmd.ExecuteReader();

                while (reader.Read())
                {
                    watchModel.ID = Convert.ToInt32(reader["AuctionID"]);
                    watchModel.URL = $"{reader["URL"]}";
                }
                Console.WriteLine(watchModel.ID);
                Console.WriteLine(watchModel.URL);

            }
        }
        /*public void GetDataFromWatches(WatchModel watchModel)
        {
            using (SqlConnection sqlConn = new SqlConnection(connStr))
            {
                sqlConn.Open();
                List<WatchModel> wl = GetDataFromAuction(sqlConn, watchModel);
                IWebDriver driver1 = new ChromeDriver();
                foreach (WatchModel model1 in wl)
                {
                    driver1.Navigate().GoToUrl(model1.URL);
                    watchModel.ID = model1.ID;
                    watchModel.URL = model1.URL;
                    Console.WriteLine(watchModel.ID);
                    Console.WriteLine(watchModel.URL);
                }
                driver1.Close();
                sqlConn.Close();
            }

          */
        /*  IWebDriver driver2 = new ChromeDriver();
            driver2.Navigate().GoToUrl(watchModel.URL);
            string priceXpath = "//p[contains(@class,'lot-page__lot__sold')]";
            IWebElement priceEle = driver2.FindElement(By.XPath(priceXpath));
            watchModel.PriceRaw = priceEle.Text;
            driver2.Close();*//*

        }*/

        public List<WatchModel> GetDataFromAuction(SqlConnection sqlConn, WatchModel watchModel)
        {
            List<WatchModel> watchList = new List<WatchModel>();

            using (SqlCommand sqlCmd = new SqlCommand("PR_GetData_Auction", sqlConn))
            {
                sqlCmd.CommandType = CommandType.StoredProcedure;
                IDataReader reader = sqlCmd.ExecuteReader();
                while (reader.Read())
                {
                    WatchModel wm = new WatchModel();
                    wm.ID = Convert.ToInt32(reader["AuctionID"]);
                    wm.URL = $"{reader["URL"]}";
                    watchList.Add(wm);
                }

            }
            return watchList;
        }
    }
}
