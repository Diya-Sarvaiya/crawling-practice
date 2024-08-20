using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlingTask3_Philips
{
    class WatchModel
    {
        public string URL { get; set; }

        public int ID { get; set; }

        public string PriceRaw { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string EstimatePriceRaw { get; set; }

        public bool IsSold { get; set; }

        public string DimensionsRaw { get; set; }

        public string Manufacturer { get; set; }
        public string UrlID { get; set; }
    }
}
