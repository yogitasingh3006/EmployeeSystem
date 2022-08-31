using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using TechTalk.SpecFlow;

namespace SpecFlowProject1.StepDefinitions
{
    [Binding]
    public class TestStepDefinitions
    {
        IWebDriver driver = new ChromeDriver();

        [Given(@"I navigate to URL")]
        public void GivenINavigateToURL()
        {
            driver.Navigate().GoToUrl("https://demo.seleniumeasy.com/");
        }

        [When(@"I click to input form")]
        public void WhenIClickToInputForm()
        {
            driver.FindElement(By.XPath("//*[@id='navbar-brand-centered']/ul[1]/li[1]/a")).Click();
        }

        [When(@"I click to Simple demo form")]
        public void WhenIClickToSimpleDemoForm()
        {
            driver.FindElement(By.XPath("//*[@id='navbar- brand-centered']/ul[1]/li[1]/ul/li[1]/a")).Click();
        }


        [Then(@"I can see Simple demo form")]
        public void ThenICanSeeSimpleDemoForm()
        {
            IWebElement headingTextEle = driver.FindElement(By.XPath("//*[@id='easycont']/div/div[2]/div[1]/div[1]"));
            String text = headingTextEle.Text;
            Console.WriteLine("Text: " + text);
            Assert.IsTrue(text.Contains("Single"));
        }
       

    }
}
