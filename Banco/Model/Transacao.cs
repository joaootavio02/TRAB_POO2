using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trab.Model
{
    namespace trab.Models
    {
        public class Transacao
        {
            public Conta ContaOrigem { get; set; }
            public Conta ContaDestino { get; set; }
            public double Valor { get; set; }

            public Transacao(Conta contaOrigem, Conta contaDestino, double valor)
            {
                ContaOrigem = contaOrigem;
                ContaDestino = contaDestino;
                Valor = valor;
            }

            public void ExecutarTransferencia()
            {
                ContaOrigem.Sacar(Valor);
                ContaDestino.Depositar(Valor);
            }
        }
    }

}
