using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace CrawlingTask2
{
    class Program
    {

        static string desRegex = @"\s+";
        static string lotCountRegex = @"\d+";

        static string connectionString = "Server=DESKTOP-66UP5QF;Database=Ineichen;Integrated Security=True;";

        static string Update_Proc_Cmd = "UpdateAuctionData";
        static string Insert_Proc_Cmd = "InsertAuctionData";
        static string Count_From_Link = "select count(*) from Auctions where Link=@link";

        static void Main(string[] args)
        {
            HtmlDocument doc = InitializeDoc();
            HtmlNodeCollection Nodes = doc.DocumentNode.SelectNodes("//div[contains(@class,'auctions-list')]//div[@id]");

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
               
                try
                {
                    if (Nodes != null)
                    {
                        var n = 1;
                        foreach (var card in Nodes)
                        {
                            Auctions auc = new Auctions();

                            var titleNode = card.SelectSingleNode(auc.titleXPath);
                            var descriptionNode = card.SelectSingleNode(auc.descriptionXPath);
                            var imageUrlNode = card.SelectSingleNode(auc.ImageUrlXPath);
                            var linkNode = card.SelectSingleNode(auc.LinkXPath);
                            var lotCountNode = card.SelectSingleNode(auc.LotCountXPath);
                            var dateNode = card.SelectSingleNode(auc.DateXPath);
                            var locationNode = card.SelectSingleNode(auc.LocationXPath);

                            auc.Title = titleNode.InnerText.Trim().ToString();
                            auc.Description = Regex.Replace(descriptionNode.InnerText.Trim(), desRegex, " ").ToString();
                            auc.ImageUrl = string.Concat("https://ineichen.com", imageUrlNode.GetAttributeValue("src", string.Empty));
                            auc.Link = string.Concat("https://ineichen.com", linkNode.GetAttributeValue("href", string.Empty));
                            auc.LotCount = Convert.ToInt32(Regex.Match(lotCountNode.InnerText.Trim(), lotCountRegex).Value);
                            auc.Location = locationNode.InnerText.Trim().ToString();

                            var patterns = new Dictionary<string, string>
                                 {
                                        { "DateFormat1", @"^(?<sd>\d+)\s(?<sm>\w+)\s-\s(?<ed>\d+)\s(?<em>\w+)$" },
                                        { "DateFormat2", @"^(?<sd>\d+)\s-\s(?<ed>\d+)\s(?<em>\w+)$" },
                                        { "DateFormat3", @"^(?<sd>\d+)\s?-\s?(?<ed>\d+)\s(?<em>\w+)\s(?<et>\d+:\d+\sCET)$" },
                                        { "DateFormat4", @"^(?<sd>\d+)\s(?<sm>\w+)\s-\s(?<ed>\d+)\s(?<em>\w+)\s(?<time>\d+:\d+\sCET)$" },
                                        { "DateFormat5", @"^(?<sd>\d+)\s(?<sm>\w+),\s(?<st>\d+:\d+\s\(CET\))$" },
                                        { "DateFormat6", @"^(?<sd>\d+)\s(?<sm>\w+),\s(?<st>\d+:\d+\sCET)\s(?<ed>\d+)\s(?<em>\w+),\s(?<et>\d+:\d+\sCET)$" },
                                        { "DateFormat7", @"^(?<sd>\d+)\s(?<sm>\w+)\s-\s(?<ed>\d+)\s(?<em>\w+)\s(?<ey>\d{4})$" },
                                        { "DateFormat8", @"^(?<sd>\d+)\s(?<sm>\w+)\s(?<sy>\d+)\s-\s(?<ed>\d+)\s(?<em>\w+)\s(?<ey>\d{4})$" },
                                        { "DateFormat9", @"^(?<sd>\d+)\s(?<sm>\w{3,10})\s(?<sy>\d{4}),\s(?<st>\d+:\d+\s\(CET\))$" },
                                        { "DateFormat10", @"^(?<sd>\d+)\s-\s(?<ed>\d+)\s(?<em>\w+)\s(?<ey>\d+)$" }
                                };

                            // Process each pattern to find dates
                            var dateString = Regex.Replace(dateNode.InnerText.Trim(), desRegex, " ");
                            getAllDates(patterns, dateString, auc);

                            // Print Data
                            printData(auc, n);

                            // Check for updated data by Link 
                            // Only update if there is same link found and Insert if not
                            Insert_Update_Into_DB(Count_From_Link, Update_Proc_Cmd, Insert_Proc_Cmd, auc, connection);
                           
                            n++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                connection.Close();
            }
            Console.ReadKey();

        }
      
        private static HtmlDocument InitializeDoc()
        {
            var web = new HtmlWeb();
            var doc = web.Load("https://ineichen.com/auctions/past/");
            return doc;
        }

        private static void Insert_Update_Into_DB(string Count_From_Link, string Update_Proc_Cmd, string Insert_Proc_Cmd, Auctions auc, SqlConnection connection)
        {
            using (SqlCommand sqlCmd = new SqlCommand(Count_From_Link, connection))
            {
                sqlCmd.CommandType = System.Data.CommandType.Text;
                sqlCmd.Parameters.AddWithValue("@link", auc.Link);
                int count = (int)sqlCmd.ExecuteScalar();
                Console.WriteLine("count : " + count);

                if (count > 0)
                {
                    using (SqlCommand sqlCmd1 = new SqlCommand(Update_Proc_Cmd, connection))
                    {
                        sqlCmd1.CommandType = System.Data.CommandType.StoredProcedure;

                        sqlCmd1.Parameters.AddWithValue("@Title", auc.Title);
                        sqlCmd1.Parameters.AddWithValue("@Description", auc.Description);
                        sqlCmd1.Parameters.AddWithValue("@ImageUrl", auc.ImageUrl);
                        sqlCmd1.Parameters.AddWithValue("@Link", auc.Link);
                        sqlCmd1.Parameters.AddWithValue("@LotCount", auc.LotCount);
                        sqlCmd1.Parameters.AddWithValue("@StartDate", auc.StartDate);
                        sqlCmd1.Parameters.AddWithValue("@StartMonth", auc.StartMonth == null ? string.Empty : auc.StartMonth);
                        sqlCmd1.Parameters.AddWithValue("@StartYear", auc.StartYear);
                        sqlCmd1.Parameters.AddWithValue("@StartTime", auc.StartTime == null ? string.Empty : auc.StartTime);
                        sqlCmd1.Parameters.AddWithValue("@EndDate", auc.EndDate);
                        sqlCmd1.Parameters.AddWithValue("@EndMonth", auc.EndMonth == null ? string.Empty : auc.EndMonth);
                        sqlCmd1.Parameters.AddWithValue("@EndYear", auc.EndYear);
                        sqlCmd1.Parameters.AddWithValue("@EndTime", auc.EndTime == null ? string.Empty : auc.EndTime);
                        sqlCmd1.Parameters.AddWithValue("@Location", auc.Location);

                        sqlCmd1.ExecuteNonQuery();
                    }
                }
                else
                {
                    using (SqlCommand sqlCmd2 = new SqlCommand(Insert_Proc_Cmd, connection))
                    {
                        sqlCmd2.CommandType = System.Data.CommandType.StoredProcedure;

                        sqlCmd2.Parameters.AddWithValue("@Title", auc.Title);
                        sqlCmd2.Parameters.AddWithValue("@Description", auc.Description);
                        sqlCmd2.Parameters.AddWithValue("@ImageUrl", auc.ImageUrl);
                        sqlCmd2.Parameters.AddWithValue("@Link", auc.Link);
                        sqlCmd2.Parameters.AddWithValue("@LotCount", auc.LotCount);
                        sqlCmd2.Parameters.AddWithValue("@StartDate", auc.StartDate);
                        sqlCmd2.Parameters.AddWithValue("@StartMonth", auc.StartMonth == null ?string.Empty: auc.StartMonth);
                        sqlCmd2.Parameters.AddWithValue("@StartYear", auc.StartYear);
                        sqlCmd2.Parameters.AddWithValue("@StartTime", auc.StartTime == null ? string.Empty : auc.StartTime);
                        sqlCmd2.Parameters.AddWithValue("@EndDate", auc.EndDate);
                        sqlCmd2.Parameters.AddWithValue("@EndMonth", auc.EndMonth == null ? string.Empty : auc.EndMonth);
                        sqlCmd2.Parameters.AddWithValue("@EndYear", auc.EndYear);
                        sqlCmd2.Parameters.AddWithValue("@EndTime", auc.EndTime == null ? string.Empty : auc.EndTime);
                        sqlCmd2.Parameters.AddWithValue("@Location", auc.Location);

                        sqlCmd2.ExecuteNonQuery();
                    }
                }

            }
        }

        private static void printData(Auctions auc, int n)
        {
            Console.WriteLine(n + ") ");
            Console.WriteLine("Title : " + auc.Title);
            Console.WriteLine("Description : " + auc.Description);
            Console.WriteLine("ImageUrl : " + auc.ImageUrl);
            Console.WriteLine("Link : " + auc.Link);
            Console.WriteLine("LotCount : " + auc.LotCount);
            Console.WriteLine("StartDate : " + auc.StartDate);
            Console.WriteLine("StartMonth : " + auc.StartMonth);
            Console.WriteLine("StartYear : " + auc.StartYear);
            Console.WriteLine("StartTime : " + auc.StartTime);
            Console.WriteLine("EndDate : " + auc.EndDate);
            Console.WriteLine("EndMonth : " + auc.EndMonth);
            Console.WriteLine("EndYear : " + auc.EndYear);
            Console.WriteLine("EndTime : " + auc.EndTime);
            Console.WriteLine("Location : " + auc.Location);
            Console.WriteLine();
        }

        /*static bool HasNamedGroup(Regex regex, string groupName)
        {
            // Get all group names from the regex pattern
            var groupNames = regex.GetGroupNames();

            // Check if the specified group name exists
            return Array.Exists(groupNames, name => name.Equals(groupName, StringComparison.OrdinalIgnoreCase));
        }*/

        private static void getAllDates(Dictionary<string, string> patterns, string dateString, Auctions auc)
        {
            foreach (var kvp in patterns)
            {
                var regex = new Regex(kvp.Value);
                Match match = regex.Match(dateString);
                /*bool hasMonth = HasNamedGroup(regex, "month");
                bool hasYear = HasNamedGroup(regex, "year");
                bool hasTime = HasNamedGroup(regex, "time");*/
                if (match.Success)
                {
                    auc.StartDate = match.Groups["sd"].Value == "" ? 0 : Convert.ToInt32(match.Groups["sd"].Value);
                    auc.EndDate = match.Groups["ed"].Value == "" ? 0 : Convert.ToInt32(match.Groups["ed"].Value);
                    auc.StartMonth = match.Groups["sm"].Value == "" ? string.Empty : match.Groups["sm"].Value;
                    auc.EndMonth = match.Groups["em"].Value == "" ? string.Empty : match.Groups["em"].Value.ToString();
                    auc.StartYear = match.Groups["sy"].Value == "" ? 0 : Convert.ToInt32(match.Groups["sy"].Value);
                    auc.EndYear = match.Groups["ey"].Value == "" ? 0 : Convert.ToInt32(match.Groups["ey"].Value);
                    auc.StartTime = match.Groups["st"].Value == "" ? string.Empty : match.Groups["st"].Value.ToString();
                    auc.EndTime = match.Groups["et"].Value == "" ? string.Empty : match.Groups["et"].Value.ToString();

                   /* auc.StartDate = match.Groups["sd"].Value == "" ? 0 : Convert.ToInt32(match.Groups["sd"].Value);
                    auc.EndDate = match.Groups["ed"].Value == "" ? 0 : Convert.ToInt32(match.Groups["ed"].Value);*/
/*
                    if (hasMonth)
                    {
                        auc.StartMonth = match.Groups["month"].Value == "" ? string.Empty : $"{match.Groups["month"].Value}";
                        auc.EndMonth = match.Groups["month"].Value == "" ? string.Empty : $"{match.Groups["month"].Value}";
                    }
                    else
                    {
                        auc.StartMonth = match.Groups["sm"].Value == "" ? string.Empty : match.Groups["sm"].Value;
                        auc.EndMonth = match.Groups["em"].Value == "" ? string.Empty : match.Groups["em"].Value.ToString();
                    }

                    if (hasYear)
                    {
                        auc.StartYear = match.Groups["year"].Value == "" ? 0 : Convert.ToInt32(match.Groups["year"].Value);
                        auc.EndYear = match.Groups["year"].Value == "" ? 0 : Convert.ToInt32(match.Groups["year"].Value);
                    }
                    else
                    {
                        auc.StartYear = match.Groups["sy"].Value == "" ? 0 : Convert.ToInt32(match.Groups["sy"].Value);
                        auc.EndYear = match.Groups["ey"].Value == "" ? 0 : Convert.ToInt32(match.Groups["ey"].Value);
                    }

                    if (hasTime)
                    {
                        auc.StartTime = match.Groups["time"].Value == "" ? string.Empty : $"{match.Groups["time"].Value}";
                        auc.EndTime = match.Groups["time"].Value == "" ? string.Empty : $"{match.Groups["time"].Value}";
                    }
                    else
                    {
                        auc.StartTime = match.Groups["st"].Value == "" ? string.Empty : match.Groups["st"].Value.ToString();
                        auc.EndTime = match.Groups["et"].Value == "" ? string.Empty : match.Groups["et"].Value.ToString();
                    }*/
                }

            }
        }
    }
}
