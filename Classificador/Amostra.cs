using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classificador
{
    public class Amostra
    {
        public float proximidade;
        public String nome;

        public Amostra(string nome, float proximidade)
        {
            this.proximidade = proximidade;
            this.nome = nome;
        }
    }
}
