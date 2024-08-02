using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;

namespace SeleniumTest
{
    [TestFixture]
    class Sample1
    {

        IWebDriver driver = new ChromeDriver();
        [SetUp]
        public void Initialize()
        {
            // open browser

            // navigate to url
            driver.Navigate().GoToUrl("https://github.com/login");

            //Maximize the browser window
            driver.Manage().Window.Maximize();
            Thread.Sleep(2000);
        }
        [Test]
        public void Exceute_Test()
        {
            //perform browser operations  

            IWebElement usernameEle = driver.FindElement(By.Name("login"));
            usernameEle.SendKeys("diya-sarvaiya");
            Thread.Sleep(2000);
            Console.WriteLine("Username Entered...");

            IWebElement passwordEle = driver.FindElement(By.Name("password"));
            passwordEle.SendKeys("Diya@git5604");
            Thread.Sleep(2000);
            Console.WriteLine("Password Entered...");

            IWebElement btnEle = driver.FindElement(By.Name("commit"));
            btnEle.Click();
            Thread.Sleep(2000);
            Console.WriteLine("sign in...");
         

        }
        [TearDown]
        public void End_Test()
        {
            //close the browser  

            driver.Close();
        }
    }
}
