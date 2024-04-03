using CommonModels.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonModels
{
    public  class DipendentiDto
    {
        [ColumnName("Cognome dipendente")]
        public string Cognome {  get; set; }

        [ColumnName("Nome dipendente")]
        public string Nome {  get; set; }


        [DateFormat("dd/MM/yyyy")]
        public DateTime DataAssunzione { get; set; }

        [TrueFalse("Si_No")]
        public bool IsGiornalista {  get; set; }
                
    }
}
