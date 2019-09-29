using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;

namespace WebScrapingSelenium {
    public class WebDriver {

        private IWebDriver _driver;
        private string linhaAnterior = "Waiting";

        public void AbrirWebDriver() {
            
            var options = new FirefoxOptions {
                AcceptInsecureCertificates = true,
                BrowserExecutableLocation = "C:/Program Files/Mozilla Firefox/firefox.exe"
            };

            _driver = new FirefoxDriver(options);            

            _driver.Manage().Window.Maximize();
            _driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
            _driver.Navigate().GoToUrl(ConfigurationManager.AppSettings["url"]);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(double.Parse(ConfigurationManager.AppSettings["timeout"]));                  
        }

        public void EnviarRequisicaoPesquisa(string valor) {

            IWebElement searchField = _driver.FindElement(By.Id("_mask_1"));
            searchField.Clear();
            searchField.SendKeys(valor);

            IWebElement buttonSearch = _driver.FindElement(By.Id("buscarButton"));

            Actions actions = new Actions(_driver);
            actions.MoveToElement(buttonSearch).Click().Perform();            
        }

        public Cliente ObterDadosCliente(int tipoRobo) {
            if (tipoRobo == 0) {
                return ObterDadosClienteMailing();
            }
            else {
                return ObterDadosClienteFatura();
            }
        }

        private Cliente ObterDadosClienteMailing() {

            IList<IWebElement> resultado;
            
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(double.Parse(ConfigurationManager.AppSettings["timeout"])));

            wait.Until(d => d.FindElement(By.Id("innerHeaderClienteTable")).FindElements(By.TagName("div"))[21].Text != linhaAnterior);

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.FrameToBeAvailableAndSwitchToIt("MAIN_TAB_RESUMO_CLIENTE.window"));            

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.FrameToBeAvailableAndSwitchToIt("idTabResumo.window"));

            _driver.SwitchTo().DefaultContent();
            Thread.Sleep(1000);

            resultado = _driver.FindElement(By.Id("innerHeaderClienteTable")).FindElements(By.TagName("div"));

            linhaAnterior = resultado[21].Text;

            if (String.IsNullOrEmpty(linhaAnterior)) {
                linhaAnterior = "Waiting";
            }

            Cliente cliente = new Cliente();
            cliente.Linha = resultado[21].Text;
            cliente.Nome = resultado[0].Text;
            cliente.CPF = resultado[7].Text;
            cliente.Plano = resultado[25].Text;
            cliente.Status = resultado[27].Text;

            return cliente;
        }

        private Cliente ObterDadosClienteFatura() {

            IList<IWebElement> resultado;

            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(double.Parse(ConfigurationManager.AppSettings["timeout"])));

            wait.Until(d => d.FindElement(By.Id("innerHeaderClienteTable")).FindElements(By.TagName("div"))[21].Text != linhaAnterior);

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.FrameToBeAvailableAndSwitchToIt("MAIN_TAB_RESUMO_CLIENTE.window"));           

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("_consultaclientedynatabs_idtabcontas")));

            Thread.Sleep(1500);

            var tabContas = _driver.FindElement(By.Id("_consultaclientedynatabs_idtabcontas"));
            Actions actions = new Actions(_driver);
            actions.MoveToElement(tabContas).Click().Perform();            

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.FrameToBeAvailableAndSwitchToIt("idTabContas.window"));

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("buscaContasBuscarButton")));

            Thread.Sleep(1500);
            
            var btnBuscarContas = _driver.FindElement(By.Id("buscaContasBuscarButton"));
            Actions actions2 = new Actions(_driver);
            actions2.MoveToElement(btnBuscarContas).Click().Perform();

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("imageBuscarContas")));

            _driver.FindElement(By.Id("imageBuscarContas")).Click();

            var rbGridFatura = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.ClassName("gwt-RadioButton")));            
            
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(rbGridFatura.FindElement(By.TagName("input"))));            

            resultado = _driver.FindElement(By.Id("_crux_gridFatura"))
                               .FindElements(By.TagName("td"));

            Cliente cliente = new Cliente();
            
            for (int i = 31; i < resultado.Count; i += 10) {
                Fatura fatura = new Fatura();
                fatura.DataVencimento = resultado[i].Text;
                fatura.Valor = resultado[i + 3].Text;
                fatura.Status = resultado[i + 4].Text;
                cliente.Faturas.Add(fatura);
            }

            _driver.SwitchTo().DefaultContent();
            Thread.Sleep(1000);

            resultado = _driver.FindElement(By.Id("innerHeaderClienteTable")).FindElements(By.TagName("div"));

            linhaAnterior = resultado[21].Text;

            if (String.IsNullOrEmpty(linhaAnterior)) {
                linhaAnterior = "Waiting";
            }

            cliente.Linha = resultado[21].Text;
            cliente.Nome = resultado[0].Text;
            cliente.CPF = resultado[7].Text;
            cliente.Plano = resultado[25].Text;
            cliente.Status = resultado[27].Text;

            return cliente;
        }

        public void RetirarAlertasPopUp(string numeroLinha, ref string mensagem) {

            IWebElement elementErro;

            _driver.SwitchTo().DefaultContent();

            if (_driver.PageSource.Contains("crux-MessageBox")) {
                try {
                    mensagem = _driver.FindElement(By.ClassName("crux-MessageBox")).FindElement(By.ClassName("message")).Text;
                }
                catch (Exception e) {
                    mensagem = e.Message;
                }

                if (_driver.PageSource.Contains("_crux_msgbox_ok_n_o_foi_poss_vel_localizar_nenhum_registro_correspondente_ao_crit_rio_de_pesquisa_in")) {
                    elementErro = _driver.FindElement(By.Id("_crux_msgbox_ok_n_o_foi_poss_vel_localizar_nenhum_registro_correspondente_ao_crit_rio_de_pesquisa_in"));
                    elementErro.Click();
                }
                else if (_driver.PageSource.Contains("Ok")) {
                    elementErro = _driver.FindElement(By.XPath("//button[text() = 'Ok']"));
                    elementErro.Click();
                }                                
            }
            else if (_driver.PageSource.Contains("crux-ErrorMessageBox")) {
                try {
                    mensagem = _driver.FindElement(By.ClassName("crux-ErrorMessageBox")).FindElement(By.ClassName("message")).Text;
                }
                catch (Exception e) {
                    mensagem = e.Message;
                }

                if (_driver.FindElement(By.XPath("/html/body/div[4]/div/table/tbody/tr[2]/td[2]/div/table/tbody/tr[1]/td/div")).Text == "Email inativo.") {
                    elementErro = _driver.FindElement(By.XPath("/html/body/div[4]/div/table/tbody/tr[2]/td[2]/div/table/tbody/tr[2]/td/table/tbody/tr/td/button"));
                    elementErro.Click();
                }
                else if (_driver.FindElement(By.XPath("/html/body/div[4]/div/table/tbody/tr[2]/td[2]/div/table/tbody/tr[1]/td/div")).Text
                    == "Erro inesperado ao acessar o serviço de enablement ConsultarMVNOAssociadoALinha do MVNE. Entre em contato com a Central de Serviços.") {
                    elementErro = _driver.FindElement(By.XPath("/html/body/div[4]/div/table/tbody/tr[2]/td[2]/div/table/tbody/tr[2]/td/table/tbody/tr/td/button"));
                    elementErro.Click();
                }
                else if (_driver.PageSource.Contains("Ok")) {
                    elementErro = _driver.FindElement(By.XPath("//button[text() = 'Ok']"));
                    elementErro.Click();
                }               
            }
            else if (_driver.PageSource.Contains("crux-Confirm")) {
                try {
                    mensagem = _driver.FindElement(By.ClassName("crux-ErrorMessageBox")).FindElement(By.ClassName("message")).Text;
                }
                catch (Exception e) {
                    mensagem = e.Message;
                }
                if (_driver.FindElement(By.XPath("/html/body/div[4]/div/table/tbody/tr[1]/td[2]/div/div")).Text == "CONFIRMAÇÃO") {
                    if (_driver.PageSource.Contains("_crux_confirm_cancel_a_linha_") && _driver.PageSource.Contains("_est__barrado_e_n_o_possui_cadastro_associado__deseja_realiz")) {
                        elementErro = _driver.FindElement(By.Id("_crux_confirm_cancel_a_linha_" + numeroLinha + "_est__barrado_e_n_o_possui_cadastro_associado__deseja_realiz"));
                        elementErro.Click();
                    }
                    else if (_driver.PageSource.Contains("_crux_confirm_cancel_a_linha_") && _driver.PageSource.Contains("_est__desprogramado_e_n_o_possui_cadastro_associado__deseja_")) {
                        elementErro = _driver.FindElement(By.Id("_crux_confirm_cancel_a_linha_" + numeroLinha + "_est__desprogramado_e_n_o_possui_cadastro_associado__deseja_"));
                        elementErro.Click();
                    }
                    else if (_driver.PageSource.Contains("_crux_confirm_cancel_a_linha_") && _driver.PageSource.Contains("_est__cancelado_e_n_o_possui_cadastro_associado__deseja_real")) {
                        elementErro = _driver.FindElement(By.Id("_crux_confirm_cancel_a_linha_" + numeroLinha + "_est__cancelado_e_n_o_possui_cadastro_associado__deseja_real"));
                        elementErro.Click();
                    }
                    else if (_driver.PageSource.Contains("_crux_confirm_cancel_a_linha_") && _driver.PageSource.Contains("_est__pr__ativo_e_n_o_possui_cadastro_associado__deseja_real")) {
                        elementErro = _driver.FindElement(By.Id("_crux_confirm_cancel_a_linha_" + numeroLinha + "_est__pr__ativo_e_n_o_possui_cadastro_associado__deseja_real"));
                        elementErro.Click();
                    }
                }
                else if (_driver.PageSource.Contains("Não")) {
                    elementErro = _driver.FindElement(By.XPath("//button[text() = 'Não']"));
                    elementErro.Click();
                }
                else if (_driver.PageSource.Contains("Ok")) {
                    elementErro = _driver.FindElement(By.XPath("//button[text() = 'Ok']"));
                    elementErro.Click();
                }                              
            }
            else if (_driver.PageSource.Contains("crux-Popup")) {
                if (_driver.PageSource.Contains("frame")) {
                    elementErro = _driver.FindElement(By.ClassName("frame"));
                    string idFrame = elementErro.GetAttribute("Id");
                    _driver.SwitchTo().Frame(_driver.FindElement(By.Id(idFrame)));

                    if (_driver.PageSource.Contains("_crux_tableAlerta")) {
                        elementErro = _driver.FindElement(By.Id("_crux_tableAlerta")).FindElement(By.Id("btnOk"));

                        try {
                            mensagem = _driver.FindElement(By.Id("_crux_tableAlerta")).Text;
                        }
                        catch (Exception e) {
                            mensagem = e.Message;
                        }

                        elementErro.Click();                        
                    }
                    else if (_driver.PageSource.Contains("Ok")) {
                        elementErro = _driver.FindElement(By.XPath("//button[text() = 'Ok']"));
                        elementErro.Click();
                    }
                    _driver.SwitchTo().DefaultContent();
                    Thread.Sleep(1000);
                }
            }
        }

        public void FecharChromeDriver() {
            _driver.Quit();
        }
    }
}
