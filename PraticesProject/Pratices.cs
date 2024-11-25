using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverManager.DriverConfigs.Impl;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using OpenQA.Selenium.Support.UI;

namespace PraticesProject
{
    public class Pratice
    {
        IWebDriver driver;

        

        [SetUp]
        public void StartBrowser()
        {
            new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig());
            driver = new ChromeDriver();
            //driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1000);
           
            driver.Manage().Window.Maximize();
            driver.Url = "https://www.tendable.com/";

        }

        [Test]
        public void FirstTest()
        {
            ArrayList expTopMenu = new ArrayList();
            ArrayList actualTopMenn = new ArrayList();
            expTopMenu.Add("About");
            expTopMenu.Add("Products");
            expTopMenu.Add("Sectors");
            expTopMenu.Add("Content Hub");
            expTopMenu.Add("Contact");

            foreach (string ele in expTopMenu)
            {
                TestContext.WriteLine("Expected Array: " + ele);
            }

            IList<IWebElement> elements = driver.FindElements(By.CssSelector(".navbar7_menu-left>a"));
            foreach (IWebElement element in elements)
            {
                actualTopMenn.Add(element.Text);
            }

            // there are 2 hidden elements are available on page that need to be removed
            int count = actualTopMenn.Count;
            actualTopMenn.RemoveAt(6);
            actualTopMenn.RemoveAt(5);


            foreach (string ele1 in  actualTopMenn)
            {
                TestContext.WriteLine("Actual Array: "   +ele1);    
            }

           bool result = expTopMenu.Contains(actualTopMenn);
           Assert.That(result, Is.True);

        }

        [Test]
        public void SecondTest()
        {
            // Verify that first "Book a Demo" button present or not 
            bool btnpresent;
            ArrayList topMenu = new ArrayList();



            IList<IWebElement> elements = driver.FindElements(By.CssSelector(".navbar7_menu-left>a"));
            foreach (IWebElement element in elements)
            {
                topMenu.Add(element.Text);
            }

            // there are 2 hidden elements are available on page that need to be removed
            topMenu.RemoveAt(6);
            topMenu.RemoveAt(5);



            try
            {
                // Verify on Home page
                btnpresent = driver.FindElement(By.LinkText("Book a demo")).Displayed;
                Assert.That(btnpresent, Is.True);

                //Verify on each page
                foreach (string ele1 in topMenu)
                {
                    IWebElement menu = driver.FindElement(By.LinkText(ele1));
                    menu.Click();
                    bool btnpresentpages = driver.FindElement(By.LinkText("Book a demo")).Displayed;
                    Assert.That(btnpresentpages, Is.True);

                }

            }
            catch (Exception ex)
            {
                TestContext.WriteLine("Book a Solt button not available on page");
            }
        }

        [Test]
        public void ThirdTest()
        {
            IWebElement ContactMenu = driver.FindElement(By.LinkText("Contact"));
            ContactMenu.Click();

            IWebElement emailTxtBox = driver.FindElement(By.Name("email"));
            emailTxtBox.SendKeys("jay@gmail.com");

            IWebElement fnameTxtBox = driver.FindElement(By.Name("firstname"));
            fnameTxtBox.SendKeys("jay");

            IWebElement lnameTxtBox = driver.FindElement(By.Name("lastname"));
            lnameTxtBox.SendKeys("Prajapati");

            IWebElement cnameTxtBox = driver.FindElement(By.Name("company"));
            cnameTxtBox.SendKeys("Tech Tab Pvt Ltd");

            IWebElement messageTypedropdown = driver.FindElement(By.Name("message_type"));
            SelectElement s = new SelectElement(messageTypedropdown);
            s.SelectByText("Marketing");

            IWebElement agreeChkBox = driver.FindElement(By.Name("384607520"));
            agreeChkBox.Click();
            
            IWebElement submitbtn = driver.FindElement(By.CssSelector("form[name='Form']>div>button"));
            submitbtn.Click();


            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.
                ElementIsVisible(By.CssSelector("form[name='Form']>div:last-of-type")));


            string expErrorMessage = "Form Submission Failed";
            string actErrorMessage = driver.FindElement(By.CssSelector("form[name='Form']>div:last-of-type")).Text;

            Assert.AreEqual(expErrorMessage, actErrorMessage);
        }


        [TearDown]
        public void StopBrowser()
        {
            driver.Close();
        }

    }
}
