using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SeleniumTest
{
    class Sample2
    {
        IWebDriver driver = new ChromeDriver();
        public void method2()
        {
            try { 
            driver.Navigate().GoToUrl(" https://chatgpt.com/");
            driver.Manage().Window.Maximize();

            Thread.Sleep(2000);
            /*IWebElement stayLogOut = driver.FindElement(By.XPath("/html/body/div[1]/div/div/main/div[1]/div[1]/div/div[1]/div/div[2]/div[1]/span/button/svg"));
            stayLogOut.Click();
            Thread.Sleep(2000);*/
        
            IWebElement searchEle = driver.FindElement(By.Id("prompt-textarea"));
            searchEle.SendKeys("tell me a joke");
            Thread.Sleep(2000);

            IWebElement btnEle = driver.FindElement(By.XPath("/html/body/div[1]/div/div/main/div[1]/div[2]/div[1]/div/form/div/div[2]/div/div/button"));
            btnEle.Click();
            Thread.Sleep(10000);
            }
            catch(Exception e)
            {
                Console.WriteLine("error ...");
            }
            finally
            {
                driver.Close();
            }
        }
    }
}
