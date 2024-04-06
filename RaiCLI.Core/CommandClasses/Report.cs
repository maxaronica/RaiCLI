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
        //IExcelGen _excelGen;
        public Report(IServiceProvider sp)
        {
        //    _excelGen =sp.GetService<IExcelGen>()!;  

        }
        public void Invoke(string[] args)
        {
            List<DipendentiDto> ListaDip = new List<DipendentiDto>();
            ListaDip.Add(new DipendentiDto()
            {
                Cognome = "Rossi",
                Nome = "Massimo",
                DataAssunzione = new DateTime(2020, 1, 1),
                IsGiornalista=true
            });
            ListaDip.Add(new DipendentiDto()
            {
                Cognome = "Verdi",
                Nome = "Roberto",
                DataAssunzione = new DateTime(2010, 1, 1),
                IsGiornalista = true
            });
            ListaDip.Add(new DipendentiDto()
            {
                Cognome = "Bianchi",
                Nome = "Gino",
                DataAssunzione = new DateTime(2000, 1, 1),
                IsGiornalista = false
            });
                byte[] buff =
                 new ExcelBuilder()
                .WithSheetName("Dipendenti")
                .WithSheetTitle("DIPENDENTI")
                .WithSheetSubTitle("Al 5/4/2024")
                .Build(ListaDip);

            byte[] buff2 =
                new ExcelBuilder()
               .WithSheetName("Dipendenti2")
               .WithSheetTitle("DIPENDENTI2")
               .WithSheetSubTitle("Al 5/4/2024 2 ")
               .WithExistingFile(buff)
               .Build(ListaDip);

            System.IO.File.WriteAllBytes("c:\\users\\massi\\desktop\\t.xlsx", buff2);
        }

        public string Usage()
        {
            return "Report: Crea file excel da lista di oggetti";
        }
    }
}
