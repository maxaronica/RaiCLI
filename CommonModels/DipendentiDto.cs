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
        public string Cognome {  get; set; }
        public string Nome {  get; set; }


        [DateFormat("dd/MM/yyyy")]
        public DateTime DataAssunzione { get; set; }
                
    }
}
