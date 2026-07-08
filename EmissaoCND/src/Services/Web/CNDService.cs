using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using EmissaoCND.src.Models;
using Microsoft.Playwright;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
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
                //cnpj = new string(cnpj.Where(char.IsDigit).ToArray());
                using var playwright = await Playwright.CreateAsync();

                var perfilChrome = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "EmissaoCND",
                    "ChromeProfile"
                );

                var context = await playwright.Chromium.LaunchPersistentContextAsync(
                    perfilChrome,
                    new()
                    {
                        Channel = "chrome",
                        Headless = false,
                        AcceptDownloads = true,
                        SlowMo = 500,
                        Locale = "pt-BR",
                        TimezoneId = "America/Sao_Paulo"
                    }
                );

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
