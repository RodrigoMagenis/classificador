using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classificador
{
    public class Iris
    {
        public double var1;
        public double var2;
        public double var3;
        public double var4;
        public string nome;
        public string nomeEncontrado;

        public Iris(double var1, double var2, double var3, double var4, string nome)
        {
            this.var1 = var1;
            this.var2 = var2;
            this.var3 = var3;
            this.var4 = var4;
            this.nome = nome;
        }
    }
}
