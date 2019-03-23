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
        List<type> types = new List<type>();
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


                var w = 0;
                foreach (var j in this.types)
                {
                    if(j.name == iris.nome)
                    {
                        w++;
                    }
                }

                if (w == 0)
                {
                    var h = new type(iris.nome);
                    this.types.Add(h);
                }

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

            var maiorPercentual = 0;
            var melhorK = 1;


            for (var g = 1; g < this.z1.Count() / 4; g++)
            {

                var k = this.getDistance(this.z1, this.z2);
                var acerto = 0;

                for (var i = 0; i < this.z1.Count(); i++)
                {
                    k[i] = k[i].OrderBy(j => j.distance).ToList(); /* Ordena por proximidade */
                    k[i] = k[i].Take(g).ToList();

                    for (var j = 0; j < k[i].Count(); j++)
                    {
                        foreach (var m in this.types)
                        {
                            if (m.name == k[i][j].name)
                            {
                                m.contagem++;
                                break;
                            }
                        }
                    }

                    var high = this.types.OrderByDescending(m => m.contagem).Take(1);
                    this.z1[i].nomeEncontrado = high.First().name;

                    foreach (var n in this.types)
                    {
                        n.contagem = 0;
                    }

                    //Console.WriteLine(this.z1[i].nomeEncontrado + " | vs | " + this.z1[i].nome);

                    if (this.z1[i].nomeEncontrado == this.z1[i].nome)
                    {
                        acerto++;
                    }

                }
                var percentual = (acerto * 100) / this.z1.Count();
                Console.WriteLine(percentual);
                if( maiorPercentual < percentual)
                {
                    melhorK = g;
                    maiorPercentual = percentual;
                }
            }
            Console.WriteLine($"melhor porcentagem de acerto: {maiorPercentual}% com k = {melhorK}");
            Console.ReadKey();
        }

        private List<List<Distance>> getDistance(List<Iris> zx, List<Iris> zy)
        {
            List<List<Distance>> k = new List<List<Distance>>();

            /*percorre zx*/
            for (var i = 0; i < zx.Count(); i++)
            {
                List<Distance> amostra = new List<Distance>();
                /*percorre zy*/
                for (var j = 0; j < zy.Count(); j++)
                {
                    var distance = Math.Sqrt(Math.Pow((zx[i].var1 - zy[j].var1), 2) + Math.Pow((zx[i].var2 - zy[j].var2), 2) + Math.Pow((zx[i].var3 - zy[j].var3), 2) + Math.Pow((zx[i].var4 - zy[j].var4), 2));
                    amostra.Add(new Distance(distance, zy[j].nome));
                }
                k.Add(amostra);
            }
            return k;
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
