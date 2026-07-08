using System;
using System.Collections.Generic;
using System.Text;

namespace EmissaoCND.src.Models
{
    public class ResultadoDownloadCNDModel
    {
        public bool Sucesso {  get; set; }
        public bool Erro { get; set; }
        public string StatusExcel { get ; set; }
    }
}
