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
    class Program
    {
        static void Main(string[] args)
        {
            /* Sample1 s1 = new Sample1();
             s1.Initialize();
             s1.Exceute_Test();
             s1.End_Test();*/

            /* Sample2 s2 = new Sample2();
             s2.method2();

             Sample2 s3 = new Sample2();
             s3.method2();*/

            Sample4 s4 = new Sample4();
            s4.method4();

        }
        
    }
}
