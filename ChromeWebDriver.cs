using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

namespace WebScrapingSelenium {
    public class ChromeWebDriver {

        private IWebDriver _driver;

        public void AbrirChromeDriver() {
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();            
            _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
            _driver.Navigate().GoToUrl("C:/Users/Felipe/Documents/Teste/portal.html");            
            //_driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);
        }

        public void EnviarRequisicaoPesquisa(string valor) {

            IWebElement query = _driver.FindElement(By.Id("_mask_1"));

            query.Clear();

            // Enter something to search for
            query.SendKeys(valor);

            //System.Threading.Thread.Sleep(1000);

            // Now submit the form. WebDriver will find the form for us from the element
            //query.Submit();
        }

        public IList<IWebElement> ListaResultadoPesquisa() {

            IWebElement resultado;

            // Wait for the page to load, timeout after 10 seconds
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            wait.Until(d => d.FindElement(By.Id("innerHeaderClienteTable")));

            resultado = _driver.FindElement(By.Id("innerHeaderClienteTable"));

            return resultado.FindElements(By.TagName("div"));
        }

        public void FecharChromeDriver() {
            _driver.Quit();
        }
    }
}
