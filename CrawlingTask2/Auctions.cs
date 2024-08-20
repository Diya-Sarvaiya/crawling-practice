using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlingTask2
{
    class Auctions
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Link { get; set; }
        public int LotCount { get; set; }
        public int StartDate { get; set; }
        public string StartMonth { get; set; }
        public int StartYear { get; set; }
        public string StartTime { get; set; }
        public int EndDate { get; set; }
        public string EndMonth { get; set; }
        public int EndYear { get; set; }
        public string EndTime { get; set; }
        public string Location { get; set; }

        public string titleXPath { get; set; } = ".//h2[@class='auction-item__name']/a";
        public string descriptionXPath { get; set; } = ".//*[@class='auction-date-location']";
        public string ImageUrlXPath { get; set; } = ".//a[contains(@class,'auction-item__image')]/img";
        public string LinkXPath { get; set; } = ".//div[contains(@class,'auction-item__btns')]/a";
        public string LotCountXPath { get; set; } = ".//div[contains(@class,'auction-item__btns')]/a";
        public string DateXPath { get; set; } = ".//div[@class='auction-date-location']//div[i[contains(@class,'mdi-clock-outline')]]";
        public string LocationXPath { get; set; } = ".//div[@class='auction-date-location']//div[i[contains(@class,'mdi-map-marker-outline')]|i[contains(@class,'mdi-web')]]";

    }
}
