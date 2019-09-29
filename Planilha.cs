using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.IO;
using System.Windows.Forms;

namespace WebScrapingSelenium {
    public class Planilha {

        private readonly ExcelPackage package;
        private ExcelPackage packageResultado;
        private ExcelWorksheet worksheetResultado;
        private int newRow = 0;

        public Planilha(string filePath) {

            FileInfo planilhaExcel = new FileInfo(filePath);

            package = new ExcelPackage(planilhaExcel);

            while (Utils.IsFileOpen(filePath)) {
                MessageBox.Show("Verifique se a planilha está aberta!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public ExcelWorksheet ObterExcelWorksheet(int index) {
            return package.Workbook.Worksheets[index];
        }

        public void CriarExcelWorkbookResultado(int tipoRobo, string filePathResult) {
            packageResultado = new ExcelPackage();
            worksheetResultado = packageResultado.Workbook.Worksheets.Add("Resultado");

            if (tipoRobo == 0) {
                worksheetResultado.Cells[1, 1].Value = "Linha";
                worksheetResultado.Cells[1, 2].Value = "Nome";
                worksheetResultado.Cells[1, 3].Value = "CPF";
                worksheetResultado.Cells[1, 4].Value = "Plano";
                worksheetResultado.Cells[1, 5].Value = "Status";
                worksheetResultado.Cells[1, 5].Value = "Response";
            }
            else {
                worksheetResultado.Cells[1, 1].Value = "TELEFONE";
                worksheetResultado.Cells[1, 2].Value = "STATUS_TELEFONE";
                worksheetResultado.Cells[1, 3].Value = "PLANO_TELEFONE";
                worksheetResultado.Cells[1, 4].Value = "RESPONSE";
                worksheetResultado.Cells[1, 5].Value = "VALOR_CONTA1";
                worksheetResultado.Cells[1, 6].Value = "DATA_VENCIMENTO_CONTA1";
                worksheetResultado.Cells[1, 7].Value = "STATUS_CONTA1";
                worksheetResultado.Cells[1, 8].Value = "VALOR_CONTA2";
                worksheetResultado.Cells[1, 9].Value = "DATA_VENCIMENTO_CONTA2";
                worksheetResultado.Cells[1, 10].Value = "STATUS_CONTA2";
                worksheetResultado.Cells[1, 11].Value = "VALOR_CONTA3";
                worksheetResultado.Cells[1, 12].Value = "DATA_VENCIMENTO_CONTA3";
                worksheetResultado.Cells[1, 13].Value = "STATUS_CONTA3";
            }

            packageResultado.SaveAs(new FileInfo($"{filePathResult}//Resultado_{DateTime.Now.ToString("ddMMyyyy_HHmmss")}.xlsx"));
        }

        public void InserirDadosCliente(Cliente cliente, string mensagem, int tipoRobo) {
            newRow = worksheetResultado.Dimension.Rows + 1;
            int colDadosFatura = 5;
            int qtdContaTotal = 0;

            if (tipoRobo == 0) {
                worksheetResultado.Cells[newRow, 1].Value = cliente.Linha; 
                worksheetResultado.Cells[newRow, 2].Value = cliente.Nome;  
                worksheetResultado.Cells[newRow, 3].Value = cliente.CPF;  
                worksheetResultado.Cells[newRow, 4].Value = cliente.Plano; 
                worksheetResultado.Cells[newRow, 5].Value = cliente.Status;
                worksheetResultado.Cells[newRow, 6].Value = mensagem; 
            }
            else {
                worksheetResultado.Cells[newRow, 1].Value = cliente.Linha;
                worksheetResultado.Cells[newRow, 2].Value = cliente.Status;
                worksheetResultado.Cells[newRow, 3].Value = cliente.Plano;
                worksheetResultado.Cells[newRow, 4].Value = mensagem;

                foreach (Fatura fatura in cliente.Faturas) {
                    if (qtdContaTotal == 3) {
                        break;
                    }
                    else {
                        worksheetResultado.Cells[newRow, colDadosFatura].Value = fatura.Valor;
                        colDadosFatura++;
                        worksheetResultado.Cells[newRow, colDadosFatura].Value = fatura.DataVencimento;
                        colDadosFatura++;
                        worksheetResultado.Cells[newRow, colDadosFatura].Value = fatura.Status;
                        colDadosFatura++;
                        qtdContaTotal++;
                    }
                }
            }

            packageResultado.Save();
        }

        public void SalvarPlanilha() {
            worksheetResultado.Cells.AutoFitColumns(0);
            worksheetResultado.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            packageResultado.Save();
            package.Dispose();
        }
    }
}