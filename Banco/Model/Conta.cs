using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trab.Model
{
    namespace trab.Models
    {
        public abstract class Conta
        {
            public int Numero { get; set; }
            public double Saldo { get; protected set; }
            public Cliente Titular { get; set; }

            public Conta(int numero, Cliente titular)
            {
                Numero = numero;
                Titular = titular;
                Saldo = 0.0;
            }

            public abstract void Sacar(double valor);

            public void Depositar(double valor)
            {
                Saldo += valor;
            }
        }
    }


}
