using HtmlAgilityPack;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CrawlingTask3_Philips
{
    class Auctions
    {
        public void GetAuctionData(IWebElement node, AuctionsModel auc)
        {
            string locationRegex = @"(.*)\sAuction";
            string dateRawRegex = @"^.*?(?:Auction\s)?(\d+.*)";
            string urlIDRegex = @"\w+$";

            var url = node.FindElement(By.XPath(".//h2/a"));
            auc.Title = url.Text;
            auc.URL = url.GetAttribute("href");

            Regex urlIDPattern = new Regex(urlIDRegex);
            auc.UrlID = urlIDPattern.Match(auc.URL).Value;
            try
            {
                var dateRawEle = node.FindElement(By.XPath(".//p"));
                Regex locMatch = new Regex(locationRegex);
                Regex dateRawMatch = new Regex(dateRawRegex);
                auc.Location = locMatch.Match(dateRawEle.Text).Groups[1].Value == null ? " " : locMatch.Match(dateRawEle.Text).Groups[1].Value;
                auc.DateRaw = dateRawMatch.Match(dateRawEle.Text).Groups[1].Value == null ? " " : dateRawMatch.Match(dateRawEle.Text).Groups[1].Value;
            }
            catch (Exception ex)
            {
                auc.Location = "";
                auc.DateRaw = "";
            }

        }

        public void printAuctionData(AuctionsModel auc)
        {
            Console.WriteLine($"Title : {auc.Title} ");
            Console.WriteLine($"URL : {auc.URL}");
            Console.WriteLine($"Location : {auc.Location}");
            Console.WriteLine($"DateRaw : {auc.DateRaw}");
            Console.WriteLine($"UrlID : {auc.UrlID}");
        }

        public void InsertUpdateIntoDB(AuctionsModel auc, SqlConnection sqlConn)
        {
            using (SqlCommand sqlCmd = new SqlCommand("select count(*) from Auctions where URL = @url", sqlConn))
            {
                sqlCmd.CommandType = System.Data.CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@url", auc.URL);
                int count = (int)sqlCmd.ExecuteScalar();
                Console.WriteLine("count : " + count);

                if (count > 0)
                {
                    using (SqlCommand sqlCmd1 = new SqlCommand("PR_Update_Auctions", sqlConn))
                    {
                        sqlCmd1.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlCmd1.Parameters.AddWithValue("@title", auc.Title);
                        sqlCmd1.Parameters.AddWithValue("@location", auc.Location);
                        sqlCmd1.Parameters.AddWithValue("@dateRaw", auc.DateRaw);
                        sqlCmd1.Parameters.AddWithValue("@url", auc.URL);
                        sqlCmd1.Parameters.AddWithValue("@urlID",auc.UrlID);

                        sqlCmd1.ExecuteNonQuery();
                    }
                }
                else
                {
                    using (SqlCommand sqlCmd2 = new SqlCommand("PR_Insert_Auctions", sqlConn))
                    {
                        sqlCmd2.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlCmd2.Parameters.AddWithValue("@title", auc.Title);
                        sqlCmd2.Parameters.AddWithValue("@location", auc.Location);
                        sqlCmd2.Parameters.AddWithValue("@dateRaw", auc.DateRaw);
                        sqlCmd2.Parameters.AddWithValue("@url", auc.URL);
                        sqlCmd2.Parameters.AddWithValue("@urlID", auc.UrlID);

                        sqlCmd2.ExecuteNonQuery();
                    }
                }

            }
        }
    }
}
