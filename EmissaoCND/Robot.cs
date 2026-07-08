using DocumentFormat.OpenXml.Office.CustomUI;
using EmissaoCND.src.Models;
using EmissaoCND.src.Services.Excel;
using EmissaoCND.src.Services.Web;
using EmissaoCND.src.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmissaoCND
{
    public class Robot
    {
        private readonly ConfigModel _config;

        LogWriter logWriter = new LogWriter();

        public Robot(ConfigModel config)
        {
            _config = config;
        }
        public async Task Executar()
        {
            try
            {
                logWriter.WriteLog("LEITURA - INICIADA");
                var excelService = new ExcelReaderService();
                CNDService cndService = new CNDService(_config);

                var lista = excelService.Ler(_config.CaminhoExcel);
                
                foreach(var item in lista)
                {
                    logWriter.WriteLog($"Processando item {item.NomeEmpresa}_{item.Cnpj}");
                    var resultado = await cndService.DownloadCND(item.Cnpj,item.NomeEmpresa);
                    //excelWriterService.EscreverStatus(_config.CaminhoExcel, item.LinhaExcel, resultado.StatusExcel);
                }

            }
            catch
            {

            }
        }
        
    }
}
