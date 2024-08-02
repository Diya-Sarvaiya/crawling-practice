using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SeleniumTest
{
    class Sample4
    {
        IWebDriver driver = new ChromeDriver();
        public void method4()
        {
            driver.Navigate().GoToUrl("https://www.w3schools.com/");

            driver.Manage().Window.Maximize();

            IWebElement detailEle = driver.FindElement(By.ClassName("learntocodeh1"));
            string detailData = detailEle.Text;
            Thread.Sleep(2000);
            Console.WriteLine(detailData);
            driver.Close();
        }
    }
}
