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
                cnpj = new string(cnpj.Where(char.IsDigit).ToArray());
                using var playwright = await Playwright.CreateAsync();
                await using var browser = await playwright.Chromium.LaunchAsync(new()
                {
                    Headless = _config.Headless,
                });
                var context = await browser.NewContextAsync();

                var page = await context.NewPageAsync();
                await page.GotoAsync(_config.UrlPortal);
                await page.GetByRole(AriaRole.Option, new() { Name = "Pessoa Jurídica" }).ClickAsync();
                await page.GetByRole(AriaRole.Textbox, new() { Name = "CNPJ" }).FillAsync(cnpj);
                await page.GetByRole(AriaRole.Button, new() { Name = "Emitir Certidão" }).ClickAsync();

            }
            catch
            {

            }


        }


    }
}
