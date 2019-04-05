using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace habermanvariavel
{
    class Program
    {
        /* globals */
        public static List<String[]> fileData;             /* Matriz que armazena os dados contidos no arquivo csv */
        public static List<type> types = new List<type>(); /* Lista que guarda os tipos */
        public static List<Base> lista = new List<Base>();
        public static int tamanhoTotal;
        public static List<Base> z1 = new List<Base>();
        public static List<Base> z2 = new List<Base>();
        public static List<Base> z3 = new List<Base>();


        static void Main(string[] args)
        {


            fileData = ReadCSVfile();

            for (var i = 0; i < fileData.Count(); i++)
            {
                Base db = new Base(converteDecimal(fileData[i][0]), converteDecimal(fileData[i][1]), converteDecimal(fileData[i][2]), fileData[i][3]);

                var w = 0;
                foreach (var j in types)
                {
                    if (j.name == db.nome)
                    {
                        w++;
                    }
                }

                if (w == 0)
                {
                    var h = new type(db.nome);
                    types.Add(h);
                }

                lista.Add(db);
            }

            tamanhoTotal = lista.Count();

            /* Pega o de presença de percentual de cada tipo */
            for (var i = 0; i < types.Count(); i++)
            {
                types[i].contagem = lista.Count(j => j.nome == types[i].name);
                types[i].percentualPresenca = types[i].contagem * 100 / tamanhoTotal;
            }

            int z3size = tamanhoTotal / 2;
            for (var i = 0; i < z3size; i++)
            {
                var random = getRandomNumber();
                var element = lista[random];

                var quant = z3.Where(j => j.nome == element.nome && element.testez3 != true).Count();
                var percent = quant * 100 / z3size;
                var typepercent = types.Where(j => j.name == element.nome).First().percentualPresenca;
                if (percent < typepercent)
                {
                    z3.Add(element);
                    lista.RemoveAt(random);
                    Console.WriteLine(lista.Count());
                }
                else
                {
                    i -= 1;
                    lista[random].testez3 = true;
                }
            }

            int z2size = tamanhoTotal / 4;
            for (var i = 0; i < z2size; i++)
            {

                var random = getRandomNumber();
                var element = lista[random];
                if (z2.Where(j => j.nome == element.nome && element.testez2 != true).Count() * 100 / z2size < types.Where(j => j.name == element.nome).First().percentualPresenca)
                {
                    z2.Add(element);
                    lista.RemoveAt(random);
                }
                else
                {
                    i -= 1;
                    lista[random].testez2 = true;
                }
            }

            while (lista.Count() > 0)
            {
                var random = getRandomNumber();
                z1.Add(lista[random]);
                lista.RemoveAt(random);
            }

            var maiorPercentual = 0;
            var melhorK = 1;

            /* Verifica várias vezes para achar o melhor knn */
            for (var k = 1; k < z1.Count() / 2; k += 2)
            {

                var distance = getDistance(z1, z2); /*pega a distância entre z1 e z2*/
                var acerto = 0;

                for (var i = 0; i < distance.Count(); i++)
                {

                    getRecurrence(ref distance, i, k);

                    /* Pega o nome mais recorrente e atribui ao nome encontrado */
                    var high = types.OrderByDescending(m => m.contagem).Take(1);
                    z1[i].nomeEncontrado = high.First().name;

                    /* Reset na contagem de todos os tipos */
                    resetRecurrence();

                    /* Compara nome encontrado com nome real e soma em acerto caso seja o mesmo valor */
                    if (z1[i].nomeEncontrado == z1[i].nome)
                    {
                        acerto++;
                    }

                }

                /* Calcula o percentual de acerto */
                var percentual = (acerto * 100) / z1.Count();
                Console.WriteLine($"acerto com k: {k} foi de : = {percentual}%");

                /* Atualiza o maior percentual de acertos usado para definir o knn */
                if (maiorPercentual < percentual)
                {
                    melhorK = k;
                    maiorPercentual = percentual;
                }
            }
            Console.WriteLine($"melhor porcentagem de acerto: {maiorPercentual}% com k = {melhorK}");
            //Console.ReadKey();
            Console.Clear();


            /**
             * Realiza trocas trocas
             */

            var d2 = getDistance(z1, z2); /*pega a distância entre z1 e z2*/
            var acerto2 = 0;
            int tentativa = 0;
            while (maiorPercentual < 99 && tentativa < 50)
            {

                for (var i = 0; i < d2.Count(); i++)
                {

                    getRecurrence(ref d2, i, melhorK);

                    /* Pega o nome mais recorrente e atribui ao nome encontrado */
                    var high2 = types.OrderByDescending(m => m.contagem).Take(1);
                    z1[i].nomeEncontrado = high2.First().name;

                    /* Reset na contagem de todos os tipos */
                    resetRecurrence();

                    /* Realiza troca com z2 */
                    if (z1[i].nomeEncontrado != z1[i].nome)
                    {
                        var z2trocaList = z2.Where(j => j.nome == z1[i].nome && j.trocado == false).ToList();
                        if (z2trocaList.Count() > 0)
                        {
                            Random rnd = new Random();
                            var random2 = rnd.Next(0, z2trocaList.Count());
                            var z2troca = z2trocaList[random2];
                            z1[i].trocado = true;
                            z2.Add(z1[i]);
                            z1.Remove(z1[i]);
                            z1.Add(z2troca);
                            z2.Remove(z2troca);
                        }
                    }
                    else
                    {
                        acerto2++;
                    }
                }

                /* reseta as trocas */
                foreach (var z in z2)
                {
                    z.trocado = false;
                }
                var p = (acerto2 * 100) / z1.Count();
                if (p > maiorPercentual)
                {
                    maiorPercentual = p;
                }
                acerto2 = 0;
                tentativa++;

                d2 = getDistance(z1, z2); /*pega a distância entre z1 e z2*/
            }


            /*Compara z1 com z3*/

            var d3 = getDistance(z3, z1); /*pega a distância entre z3 e z1*/
            var acerto1 = 0;

            for (var i = 0; i < d3.Count(); i++)
            {

                getRecurrence(ref d3, i, melhorK);

                /* Pega o nome mais recorrente e atribui ao nome encontrado */
                var high = types.OrderByDescending(m => m.contagem).Take(1);
                z3[i].nomeEncontrado = high.First().name;

                /* Reset na contagem de todos os tipos */
                resetRecurrence();

                Console.WriteLine(z3[i].nomeEncontrado + " | vs | " + z3[i].nome);

                /* Compara nome encontrado com nome real e soma em acerto caso seja o mesmo valor */
                if (z3[i].nomeEncontrado == z3[i].nome)
                {
                    acerto1++;
                }

            }

            /* Calcula o percentual de acerto */
            var percentual1 = (acerto1 * 100) / z3.Count();
            Console.WriteLine($"percentual= {percentual1}");
            Console.ReadKey();

        }

        static void resetRecurrence()
        {
            foreach (var n in types)
            {
                n.contagem = 0;
            }
        }

        static int getRandomNumber()
        {
            Random rnd = new Random();
            return rnd.Next(0, lista.Count());
        }

        static double converteDecimal(string value)
        {
            return double.Parse(value);
        }

        static List<String[]> ReadCSVfile()
        {
            using (StreamReader file = new StreamReader(@"D:\Github\Classificador\database\basehaberman.txt"))
            {
                var data = new List<String[]>();
                while (!file.EndOfStream)
                {
                    string line = file.ReadLine();
                    string[] values = line.Split(',');
                    data.Add(values);
                }
                return data;
            }
        }

        static List<List<Distance>> getDistance(List<Base> zx, List<Base> zy)
        {
            List<List<Distance>> k = new List<List<Distance>>();

            /*percorre zx*/
            for (var i = 0; i < zx.Count(); i++)
            {
                List<Distance> amostra = new List<Distance>();
                /*percorre zy*/
                for (var j = 0; j < zy.Count(); j++)
                {
                    var distance = Math.Sqrt(Math.Pow((zx[i].var1 - zy[j].var1), 2) + Math.Pow((zx[i].var2 - zy[j].var2), 2) + Math.Pow((zx[i].var3 - zy[j].var3), 2));
                    amostra.Add(new Distance(distance, zy[j].nome));
                }
                k.Add(amostra);
            }
            return k;
        }

        static void getRecurrence(ref List<List<Distance>> d, int i, int k)
        {
            d[i] = d[i].OrderBy(j => j.distance).ToList(); /* Ordena por proximidade */
            d[i] = d[i].Take(k).ToList(); /*Pega a quantida do knn*/


            /* Realiza a contagem de recorrencia de cada tipo */
            for (var j = 0; j < d[i].Count(); j++)
            {
                foreach (var m in types)
                {
                    if (m.name == d[i][j].name)
                    {
                        m.contagem++;
                        break;
                    }
                }
            }
        }

    }

    public class Base
    {
        public double var1;
        public double var2;
        public double var3;
        public string nome;
        public string nomeEncontrado;
        public bool trocado = false;
        public bool testez3 = false;
        public bool testez2 = false;

        public Base(double var1, double var2, double var3, string nome)
        {
            this.nome = nome;
            this.var1 = var1;
            this.var2 = var2;
            this.var3 = var3;
        }
    }

    public class type
    {
        public string name;
        public int contagem = 0;
        public decimal percentualPresenca = 0;

        public type(string a)
        {
            this.name = a;
        }
    }

    public class Distance
    {
        public double distance;
        public string name;

        public Distance(double distance, string name)
        {
            this.distance = distance;
            this.name = name;
        }
    }
}
