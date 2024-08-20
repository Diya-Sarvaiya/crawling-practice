using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlingTask4
{
    class AuctionsModel
    {
        public string Title { get; set; }
        public string Location { get; set; }
        public string DateRaw { get; set; }
        public string URL { get; set; }

        public string UrlID { get; set; }

        public string TitleXpath = ".//h2/a";
        public string DateRawXpath = ".//p";

        public string locationRegex = @"(.*)\sAuction";
        public string dateRawRegex = @"^.*?(?:Auction\s)?(\d+.*)";
    }
}
