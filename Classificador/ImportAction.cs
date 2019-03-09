using Classificador;
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
        List<Iris> lista = new List<Iris>();
        List<Iris> z1 = new List<Iris>();
        List<Iris> z2 = new List<Iris>();
        List<Iris> z3 = new List<Iris>();
        List<int> elementos = new List<int>();
        int tamanhoTotal;

        /*Coleta o diretório e o nome do arquivo*/
        public void ShowImportationMenu()
        {
            Console.Clear();
            Console.WriteLine("Importação da base");
            Console.WriteLine("qual é o diretório do arquivo CSV que deverá ser importado?");
            //this.fileDirectory = Console.ReadLine();
            this.fileDirectory = "D:\\Github\\Classificador\\Classificador\\database";
            Console.Clear();
            Console.WriteLine("Importação de novas cotações");
            Console.WriteLine("qual é o nome do arquivo CSV que deverá ser importado?");
            //this.fileName = Console.ReadLine();
            this.fileName = "iris";
            Console.Clear();
        }

        /*Faz a leitura do arquivo csv e retorna uma matriz com as informações*/
        private List<String[]> ReadCSVfile(String directory, String fileName)
        {
            using (StreamReader file = new StreamReader(directory + "\\" + fileName + ".txt"))
            {
                var data = new List<String[]>();
                var countRow = 1;
                Console.WriteLine("Lendo arquivo");
                while (!file.EndOfStream)
                {
                    string line = file.ReadLine();
                    string[] values = line.Split(',');
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

        public void classificarIris()
        {

            for(var i = 0; i < this.fileData.Count(); i++)
            {
                Iris iris = new Iris(converteDecimal( this.fileData[i][0]), converteDecimal(this.fileData[i][1]), converteDecimal(this.fileData[i][2]), converteDecimal(this.fileData[i][3]), this.fileData[i][4]);
                this.lista.Add(iris);

            }

            tamanhoTotal = this.lista.Count();

            
            
            for (var i = 0; i < tamanhoTotal / 2; i++)
            {
                this.z3.Add(this.lista[this.getRandomNumber()]);
            }

            for (var i = 0; i < (tamanhoTotal / 2) /2; i++)
            {
                this.z2.Add(this.lista[this.getRandomNumber()]);
            }

            while(this.elementos.Count() < this.tamanhoTotal)
            {
                this.z1.Add(this.lista[this.getRandomNumber()]);
            }

            /*encontra o z (tamanho da amostra)*/
            for(var z = 0; z < tamanhoTotal / 4; z++)
            {
                /*percorre z1*/
                for (var i = 0; i < this.z1.Count(); i++)
                {
                    /*percorre z2*/
                    for (var j = 0; j < this.z2.Count(); j++)
                    {
                        var teste = Math.Sqrt(Math.Pow((this.z1[i].var1 - this.z2[j].var1), 2) + Math.Pow((this.z1[i].var2 - this.z2[j].var2), 2) + Math.Pow((this.z1[i].var3 - this.z2[j].var3), 2) + Math.Pow((this.z1[i].var4 - this.z2[j].var4), 2));
                    }
                }
            } 

        }

        public int getRandomNumber()
        {
            Random rnd = new Random();
            int elemento;
            do
            {
                elemento = rnd.Next(0, this.tamanhoTotal);
            } while (elementos.Contains(elemento));
            elementos.Add(elemento);
            return elemento;
        }

        public double converteDecimal( string value )
        {
            return double.Parse(value);
        }

        

        protected string FileDirectory { get => fileDirectory; set => fileDirectory = value; }
        protected string FileName { get => fileName; set => fileName = value; }
        public List<string[]> FileData { get => fileData; set => fileData = value; }
    }
}
