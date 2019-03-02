using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApp.ImportAction
{
    public class ImportAction
    {
        private string fileDirectory;    /* Diretório em que o arquivo csv se encontra */
        private string fileName;         /* Nome do arquivo csv a ser lido */
        private List<String[]> fileData; /* Matriz que armazena os dados contidos no arquivo csv */

        /*Coleta o diretório e o nome do arquivo*/
        public void ShowImportationMenu()
        {
            Console.Clear();
            Console.WriteLine("Importação da base");
            Console.WriteLine("qual é o diretório do arquivo CSV que deverá ser importado?");
            this.fileDirectory = Console.ReadLine();
            Console.Clear();
            Console.WriteLine("Importação de novas cotações");
            Console.WriteLine("qual é o nome do arquivo CSV que deverá ser importado?");
            this.fileName = Console.ReadLine();
            Console.Clear();
        }

        /*Faz a leitura do arquivo csv e retorna uma matriz com as informações*/
        private List<String[]> ReadCSVfile(String directory, String fileName)
        {
            using (StreamReader file = new StreamReader(directory + "\\" + fileName + ".csv"))
            {
                var data = new List<String[]>();
                var countRow = 1;
                Console.WriteLine("Lendo arquivo");
                while (!file.EndOfStream)
                {
                    string line = file.ReadLine();
                    string[] values = line.Split(';');
                    data.Add(values);
                }
                Console.Clear();
                Console.WriteLine("Leitura do arquivo csv '" + fileName + "' concluída!");
                Console.ReadKey();
                Console.Clear();
                return data;
            }
        }

        /* Realiza a leitura do arquivo e guarda os valores */
        public void ReadFile()
        {
            this.fileData = this.ReadCSVfile(this.FileDirectory, this.FileName);
        }

        protected string FileDirectory { get => fileDirectory; set => fileDirectory = value; }
        protected string FileName { get => fileName; set => fileName = value; }
        public List<string[]> FileData { get => fileData; set => fileData = value; }
    }
}
