using FinanceApp.ImportAction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classificador
{
    class Program
    {
        static void Main(string[] args)
        {
            ImportAction importAction = new ImportAction();

            importAction.ShowImportationMenu();
            importAction.ReadFile();
            importAction.classificarIris();
        }
    }
}
