using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trab.Model
{
    namespace trab.Models
    {
        public class Poupanca : Conta
        {
            public double TaxaJuros { get; private set; }

            public Poupanca(int numero, Cliente titular, double taxaJuros) : base(numero, titular)
            {
                TaxaJuros = taxaJuros;
            }

            public void AplicarRendimento()
            {
                Saldo += Saldo * TaxaJuros;
            }

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
