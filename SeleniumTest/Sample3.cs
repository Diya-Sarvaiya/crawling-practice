using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SeleniumTest
{
    class Sample3
    {
        //create the reference for the browser  
        IWebDriver driver = new ChromeDriver();

        // navigate to URL  
        public void method3()
        {
            driver.Navigate().GoToUrl("https://www.google.co.in/");
            Thread.Sleep(2000);

            // identify the Google search text box  
            IWebElement searchEle = driver.FindElement(By.Name("q"));
            //enter the value in the google search text box  
            searchEle.SendKeys("hello");
            Thread.Sleep(2000);

            //identify the google search button  
            IWebElement btnEle = driver.FindElement(By.Name("btnK"));
            // click on the Google search button  
            btnEle.Click();
            Thread.Sleep(2000);

            //close the browser  
            driver.Close();

            Console.WriteLine("test ended...");
        }
    }
}
