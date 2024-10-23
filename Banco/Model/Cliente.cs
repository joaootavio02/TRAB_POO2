using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trab.Model
{
    namespace trab.Models
    {
        public class Cliente
        {
            public string Nome { get; set; }
            public string CPF { get; set; }

            public Cliente(string nome, string cpf)
            {
                Nome = nome;
                CPF = cpf;
            }
        }
    }

}
