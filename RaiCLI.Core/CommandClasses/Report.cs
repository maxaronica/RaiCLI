using CommonModels;
using ExcelGenerator;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaiCLI.Core.CommandClasses
{
    public class Report : IRaiCLI
    {
        IExcelGen _excelGen;
        public Report(IServiceProvider sp)
        {
            _excelGen =sp.GetService<IExcelGen>();  

        }
        public void Invoke(string[] args)
        {
            List<DipendentiDto> ListaDip = new List<DipendentiDto>();
            ListaDip.Add(new DipendentiDto()
            {
                Cognome = "Rossi",
                Nome = "Massimo",
                DataAssunzione = new DateTime(2020, 1, 1)
            });
            ListaDip.Add(new DipendentiDto()
            {
                Cognome = "Verdi",
                Nome = "Roberto",
                DataAssunzione = new DateTime(2010, 1, 1)
            });
            ListaDip.Add(new DipendentiDto()
            {
                Cognome = "Bianchi",
                Nome = "Gino",
                DataAssunzione = new DateTime(2000, 1, 1)
            });
            byte[] buff = _excelGen.Create(ListaDip, "Dipendenti");
            System.IO.File.WriteAllBytes("c:\\users\\massi\\desktop\\t.xlsx", buff);
        }

        public string Usage()
        {
            return "Report: Crea file excel da lista di oggetti";
        }
    }
}
