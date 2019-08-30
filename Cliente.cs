using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScrapingSelenium {
    
    public class Cliente {
        public string Linha { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Plano { get; set; }
        public string Status { get; set; }
        public List<Fatura> Faturas { get; set; } = new List<Fatura>();
    }    
}
