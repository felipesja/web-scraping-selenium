# Webscraping com Selenium
Projeto para criação de um Robô utilizando técnica de scraping para automatizar recuperação de dados de clientes e faturas em um Sistema Web de uma operadora.

## Build With 
.NET Framework com C# 

Projeto em WindowsForms para melhor feedback e usabilidade do usuário. 

Pacote [Selenium WebDriver](https://www.nuget.org/packages/Selenium.WebDriver/) para automatizar a interação entre o navegador e o código c#

Inicialmente utilizando [ChromeDriver](https://chromedriver.chromium.org/downloads) para permitir testes e automatizações no Chrome. 

Posteriormente o ChromeDriver estava inconsistente para consultas com muito javascript envolvido. Mudar para o driver do [Firefox](https://github.com/mozilla/geckodriver) resolveu o problema perdendo um pouco em performance.

Tratamento de log em arquivo utilizando [Serilog](https://serilog.net/).

## Getting Started
O robô realiza a leitura em uma planilha excel com número de linhas telefonicas e gera uma segunda planilha com o resultado esperado pela consulta. 

Para manipulação da planilha foi utilizado o pacote [EPPlus](https://github.com/JanKallman/EPPlus).