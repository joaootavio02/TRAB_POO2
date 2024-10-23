using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trab.Model
{
    namespace trab.Models
    {
        public class ContaCorrente : Conta
        {
            public ContaCorrente(int numero, Cliente titular) : base(numero, titular) { }

            public override void Sacar(double valor)
            {
                if (Saldo >= valor)
                {
                    Saldo -= valor;
                }
                else
                {
                    throw new System.Exception("Saldo insuficiente!");
                }
            }
        }
    }

}
