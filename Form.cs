using OfficeOpenXml;
using OfficeOpenXml.Style;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly ChromeWebDriver cwDriver = new ChromeWebDriver();

        private void BtnStart_Click(object sender, EventArgs e) {

            textBox.AppendText("Inicializando Robo." + Environment.NewLine);

            try {

                FileInfo planilhaExcel = new FileInfo(filePath);

                textBox.AppendText("Inicializando o Browser controlado." + Environment.NewLine);

                using (ExcelPackage package = new ExcelPackage(planilhaExcel)) {

                    while (Utils.IsFileOpen(filePath)) {
                        MessageBox.Show("Verifique se a planilha está aberta", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                    // get the first worksheet in the workbook
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];

                    textBox.AppendText("Total de " + worksheet.Dimension.Rows + " telefones para pesquisar." + Environment.NewLine);

                    progressBar.Maximum = worksheet.Dimension.Rows;
                    progressBar.Step = 1;

                    ExcelWorksheet worksheet2 = package.Workbook.Worksheets.Add("ResultadoConsulta");

                    worksheet2.Cells[1, 1].Value = "Nome";
                    worksheet2.Cells[1, 2].Value = "CPF";
                    worksheet2.Cells[1, 3].Value = "Linha";
                    worksheet2.Cells[1, 4].Value = "Plano";
                    worksheet2.Cells[1, 5].Value = "Status";

                    IList<IWebElement> listaDados = null;
                    int newRow = 0;

                    for (int row = 1; row <= worksheet.Dimension.Rows; row++) {

                        int col = 1;

                        cwDriver.EnviarRequisicaoPesquisa(worksheet.Cells[row, col].Value.ToString());

                        try {

                            listaDados = cwDriver.ListaResultadoPesquisa();

                            textBox.AppendText("Proprietário do telefone " + worksheet.Cells[row, col].Value.ToString() + " é " + listaDados[0].Text + Environment.NewLine);                            

                            newRow = worksheet2.Dimension.Rows + 1;

                            worksheet2.Cells[newRow, 1].Value = listaDados[0].Text; //NOME
                            worksheet2.Cells[newRow, 2].Value = listaDados[7].Text; //CPF
                            worksheet2.Cells[newRow, 3].Value = listaDados[21].Text; //Linha
                            worksheet2.Cells[newRow, 4].Value = listaDados[25].Text; //Plano
                            worksheet2.Cells[newRow, 5].Value = listaDados[27].Text; //Status

                            if (row % 10 == 0) {
                                package.Save();
                            }
                        }
                        catch {
                            textBox.AppendText("Timeout.\n");
                        }

                        textBox.ScrollToCaret();
                        progressBar.PerformStep();
                    }

                    worksheet2.Cells.AutoFitColumns(0);
                    worksheet2.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    package.Save();

                    textBox.AppendText("Telefones processados com sucesso.");
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Ocorreu um problema inesperado: \n\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox.AppendText("Ocorreu um problema inesperado: \n");
                textBox.AppendText(ex.Message + Environment.NewLine);
            }

            cwDriver.FecharChromeDriver();
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

                    cwDriver.AbrirChromeDriver();
                }
            }

            TopMost = true;
        }
    }
}
