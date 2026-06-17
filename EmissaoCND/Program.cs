using System;
using EmissaoCND.src.ConfigService;
using EmissaoCND.src.Utils;

namespace EmissaoCND
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                LogWriter logWriter = new LogWriter();
                logWriter.WriteLog("-----------------------------------IniciandoExecução-----------------------------------");
                Console.WriteLine("ROBÔ EM EXECUÇÃO");

                var configService = new ConfigService();
                var config = configService.Ler();

                var exitCode = Microsoft.Playwright.Program.Main(new[] { "install", "chromium" });

                if(exitCode != 0)
                {
                    throw new Exception("Falha ao instalar o chromium do playwright");
                }
                Robot robot = new Robot(config);
                await robot.Executar();

                Console.WriteLine("FIM DA EXECUÇÃO");
                logWriter.WriteLog("-----------------------------------FimExecução-----------------------------------");

            }
            catch (Exception ex)
            {
                
            }
        }
    }
}