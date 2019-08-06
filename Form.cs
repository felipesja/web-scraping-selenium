using OfficeOpenXml;
using OfficeOpenXml.Style;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebScrapingSelenium {
    public partial class Form : System.Windows.Forms.Form {
        public Form() {
            InitializeComponent();                        
        }

        private void Form_Load(object sender, EventArgs e) {
            textBox.Text = "Selecione a planilha excel para carregar." + Environment.NewLine;
            btnStart.Enabled = false;
        }

        private string filePath = string.Empty;

        private void BtnStart_Click(object sender, EventArgs e) {

            textBox.AppendText("Iniciando Robo." + Environment.NewLine);
            
            FileInfo planilhaExcel = new FileInfo(filePath);

            textBox.AppendText("Abrindo o Browser controlado." + Environment.NewLine);

            try {

                this.TopMost = true;

                using (IWebDriver driver = new ChromeDriver(Environment.CurrentDirectory)) {

                    //driver.Manage().Window.Maximize();
                    driver.Navigate().GoToUrl("https://www.qualoperadora.net");

                    try {
                        using (ExcelPackage package = new ExcelPackage(planilhaExcel)) {
                            
                            while (Utils.IsFileOpen(filePath)) {
                                MessageBox.Show("Verifique se a planilha está aberta", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }

                            // get the first worksheet in the workbook
                            ExcelWorksheet worksheet = package.Workbook.Worksheets[1];

                            textBox.AppendText("Total de " + worksheet.Dimension.Rows + " telefones para pesquisar." + Environment.NewLine);

                            pbForm.Maximum = worksheet.Dimension.Rows;
                            pbForm.Step = 1;

                            ExcelWorksheet worksheet2 = package.Workbook.Worksheets.Add("ResultadoConsulta");

                            worksheet2.Cells[1, 1].Value = "Telefone";
                            worksheet2.Cells[1, 2].Value = "Operadora";
                            worksheet2.Cells[1, 3].Value = "Estado";
                            worksheet2.Cells[1, 4].Value = "TipoPlano";

                            IWebElement query = null;
                            IWebElement resultado = null;
                            IList<IWebElement> listaDados = null;                           
                            int newRow = 0;

                            for (int row = 1; row <= worksheet.Dimension.Rows; row++) {

                                int col = 1;

                                query = driver.FindElement(By.Name("telefone"));

                                query.Clear();
                                // Enter something to search for
                                query.SendKeys(worksheet.Cells[row, col].Value.ToString());

                                System.Threading.Thread.Sleep(1000);

                                // Now submit the form. WebDriver will find the form for us from the element
                                query.Submit();

                                // Wait for the page to load, timeout after 10 seconds
                                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                                wait.Until(d => d.FindElement(By.Id("resultado")));

                                resultado = driver.FindElement(By.Id("resultado"));

                                listaDados = resultado.FindElements(By.TagName("span"));

                                textBox.AppendText("Operadora do telefone " + worksheet.Cells[row, col].Value.ToString() + " é " + listaDados[0].Text + Environment.NewLine);

                                worksheet2.Cells[worksheet2.Dimension.Rows + 1, 1].Value = worksheet.Cells[row, 1].Value;

                                newRow = worksheet2.Dimension.Rows + 1;

                                worksheet2.Cells[newRow, 1].Value = worksheet.Cells[row, col].Value;
                                worksheet2.Cells[newRow, 2].Value = listaDados[0].Text;
                                worksheet2.Cells[newRow, 3].Value = listaDados[1].Text;

                                if (row % 10 == 0) {
                                    package.Save();
                                }

                                textBox.ScrollToCaret();
                                pbForm.PerformStep();
                            }

                            worksheet2.Cells.AutoFitColumns(0);
                            worksheet2.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            package.Save();

                            textBox.AppendText("Telefones processados com sucesso.");
                        }
                    }
                    catch (Exception ex) {
                        MessageBox.Show("Ocorreu um problema inesperado: \n\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Application.Exit();
                    }
                    
                    driver.Quit();
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Não foi possível iniciar o ChromeDriver.exe: \n\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }        

        private void BtnFileOpen_Click(object sender, EventArgs e) {

            using (OpenFileDialog openFileDialog = new OpenFileDialog()) {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Planilhas (*.xlsx;*.xls)|*.xls*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK) {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                    textBox.AppendText("Planilha Carregada." + Environment.NewLine);
                    btnStart.Enabled = true;
                }
            }          
        }        
    }
}
