using OfficeOpenXml;
using OpenQA.Selenium;
using System;
using System.Threading;
using System.Windows.Forms;
using Serilog;

namespace WebScrapingSelenium {
    public partial class Form : System.Windows.Forms.Form {
        public Form() {
            InitializeComponent();
        }        

        private void Form_Load(object sender, EventArgs e) {            
            textBox.Text = $"{DateTime.Now} - Selecione a planilha excel para carregar. {Environment.NewLine}";
            btnStartMailing.Enabled = false;
            btnStartFatura.Enabled = false;            
        }

        private string _filePath;
        private readonly WebDriver wDriver = new WebDriver();

        private void BtnStart_Click(object sender, EventArgs e) {
            DesabilitarButtons();
            textBox.AppendText("-------------------------------------------------------------\n");
            textBox.AppendText($"{DateTime.Now} - Inicializando Robo para Mailing. {Environment.NewLine}");
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File($"logRobo_{DateTime.Now.ToString("ddMMyyyy_HHmmss")}")
                .CreateLogger();
            Log.Information("Inicializando Robo para Mailing.");
            ProcessarRobo(0);
        }

        private void BtnStartFatura_Click(object sender, EventArgs e) {            
            DesabilitarButtons();
            textBox.AppendText("-------------------------------------------------------------\n");
            textBox.AppendText($"{DateTime.Now} - Inicializando Robo para Fatura. {Environment.NewLine}");
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File($"logRobo_{DateTime.Now.ToString("ddMMyyyy_HHmmss")}")
                .CreateLogger();
            Log.Information("Inicializando Robo para Fatura.");
            ProcessarRobo(1);
        }

        private void BtnFileOpen_Click(object sender, EventArgs e) {

            using (OpenFileDialog openFileDialog = new OpenFileDialog()) {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "Planilhas (*.xlsx;*.xls)|*.xls*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK) {
                    //Get the path of specified file
                    _filePath = openFileDialog.FileName;
                    textBox.AppendText(DateTime.Now + " - Planilha Carregada." + Environment.NewLine);
                    HabilitarButtons();

                    wDriver.AbrirWebDriver();
                }
            }
            TopMost = true;
        }

        private void ProcessarRobo(int tipoRobo) {
            //0 - Mailing 1 - Fatura

            string numeroLinha = null;
            int linhasProcessadas = 0;
            double taxaInfoProcessadas;
            Planilha planilhaExcel = new Planilha(_filePath);            

            try {

                ExcelWorksheet worksheetLinha = planilhaExcel.ObterExcelWorksheet(1);

                textBox.AppendText($"{DateTime.Now} - Total de {worksheetLinha.Dimension.Rows} Linhas para consultar. {Environment.NewLine}");
                Log.Debug($"Total de {worksheetLinha.Dimension.Rows} Linhas para consultar.");
                progressBar.Maximum = worksheetLinha.Dimension.Rows;
                progressBar.Value = 0;

                taxaInfoProcessadas = Math.Round(worksheetLinha.Dimension.Rows * 0.10);

                planilhaExcel.CriarExcelWorkbookResultado(tipoRobo);
                
                int col = 1;
                string mensagem = "Ok";

                for (int row = 1; row <= worksheetLinha.Dimension.Rows; row++) {
                    
                    numeroLinha = worksheetLinha.Cells[row, col].Value.ToString();
                    Cliente cliente = new Cliente();

                    if (numeroLinha.Length == 11) {

                        wDriver.EnviarRequisicaoPesquisa(numeroLinha);
                        Thread.Sleep(3000);

                        try {                           
                            cliente = wDriver.ObterDadosCliente(tipoRobo);                            
                            linhasProcessadas++;
                            mensagem = "Ok";
                        }
                        catch (WebDriverTimeoutException ex) {                            
                            Log.Error(ex, $"Não foi possível consultar a Linha: {numeroLinha}. Necessário verificar manualmente.");
                            mensagem = "Excedeu o tempo de espera para consulta. Necessário reprocessar.";
                        }
                        catch (Exception ex) {
                            textBox.AppendText($"{DateTime.Now} - Erro inesperado na Linha {numeroLinha}. Verificar posteriormente arquivo de Log.\n");                            
                            Log.Error(ex, $"Erro inesperado na Linha {numeroLinha}: ");
                            mensagem = "Erro inesperado na Linha. Verificar posteriormente arquivo de Log.";
                        }
                        finally {                            
                            wDriver.RetirarAlertasPopUp(numeroLinha, ref mensagem);
                            if (String.IsNullOrEmpty(cliente.Linha)) {
                                cliente.Linha = numeroLinha;
                            }
                            planilhaExcel.InserirDadosCliente(cliente, mensagem, tipoRobo);
                        }
                    }
                    else {
                        textBox.AppendText($"{DateTime.Now} - Número de linha inválido: {numeroLinha}. Necessário verificar manualmente.\n");
                        Log.Debug($"Número de linha inválido: {numeroLinha}. Necessário verificar manualmente.");
                    }

                    textBox.ScrollToCaret();

                    if (row < progressBar.Maximum) {
                        progressBar.Value = row + 1; //bug fix 
                        progressBar.Value = row;
                    }
                    else {
                        progressBar.Value = row;
                    }

                    if (row % taxaInfoProcessadas == 0) {
                        textBox.AppendText($"{DateTime.Now} - {row} Linhas processadas.\n");
                    }

                }

                planilhaExcel.SalvarPlanilha();
            }
            catch (Exception ex) {
                MessageBox.Show("Ocorreu um problema inesperado: \n\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox.AppendText($"{DateTime.Now} - Ocorreu um problema inesperado na linha {numeroLinha}: \n");                
                Log.Error(ex, $"Ocorreu um problema inesperado na linha {numeroLinha}. Processo abortado.");
                progressBar.Value = progressBar.Maximum;
            }
            //cwDriver.FecharChromeDriver();
            textBox.AppendText($"{DateTime.Now} - Total de {linhasProcessadas} Linhas válidas processadas.\n");
            Log.Information($"Total de {linhasProcessadas} Linhas válidas processadas.");
            HabilitarButtons();
            Log.CloseAndFlush();
        }

        private void HabilitarButtons() {
            btnStartMailing.Enabled = true;
            btnStartFatura.Enabled = true;
            btnFileOpen.Enabled = true;
        }

        private void DesabilitarButtons() {
            btnStartMailing.Enabled = false;
            btnStartFatura.Enabled = false;
            btnFileOpen.Enabled = false;
        }        
    }
}
