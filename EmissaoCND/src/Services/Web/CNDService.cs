using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using EmissaoCND.src.Models;
using Microsoft.Playwright;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EmissaoCND.src.Services.Web
{
    public class CNDService
    {
        private readonly ConfigModel _config;
        public CNDService(ConfigModel config)
        {
            _config = config;
        }
        
        public async Task<ResultadoDownloadCNDModel> DownloadCND(string cnpj, string nomeEmpresa)
        {
            try
            {
                using var playwright = await Playwright.CreateAsync();
                string chromePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe";

                Process.Start(new ProcessStartInfo
                {
                    FileName = chromePath,
                    Arguments = "--remote-debugging-port=9222 --user-data-dir=\"" +
                Environment.ExpandEnvironmentVariables("%LOCALAPPDATA%\\EmissaoCND\\ChromeDebug") + "\"",
                    UseShellExecute = true
                });

                // Aguarda alguns segundos
                await Task.Delay(3000);

                var browser = await playwright.Chromium.ConnectOverCDPAsync("http://127.0.0.1:9222");
                var context = browser.Contexts.FirstOrDefault()
                    ?? throw new Exception("Nenhum contexto do Chrome foi encontrado na porta 9222.");

                var page = context.Pages.FirstOrDefault() ?? await context.NewPageAsync();
                await page.GotoAsync(_config.UrlPortal);
                await page.GetByRole(AriaRole.Option, new() { Name = "Pessoa Jurídica" }).ClickAsync();
                Thread.Sleep(1000);
                await page.GetByRole(AriaRole.Textbox, new() { Name = "CNPJ" }).FillAsync(cnpj);
                Thread.Sleep(1000);
                await page.GetByRole(AriaRole.Button, new() { Name = "Emitir Certidão" }).ClickAsync();
                //var download = await page.RunAndWaitForDownloadAsync(async () =>
                //{
                //await page.GetByRole(AriaRole.Button, new() { Name = "Emitir Certidão" }).ClickAsync();
                //});
                Thread.Sleep(3000);
                var fileName = $"{nomeEmpresa}_{cnpj}.pdf";
                Directory.CreateDirectory(_config.CaminhoPastaTemp);
                //await download.SaveAsAsync(Path.Combine(_config.CaminhoPastaTemp, fileName));
                return new ResultadoDownloadCNDModel
                {
                    Sucesso = true,
                    Erro = false,
                    StatusExcel = "Download realizado"
                };
            }
            catch
            {
                return null;
            }


        }


    }
}
