using System;
using System.Collections.Generic;
using System.Linq;
using trab.Model;
using trab.Model.trab.Models;

namespace trab.Controller
{
    public class BancoController
    {
        private List<Conta> contas = new List<Conta>();

        public void AdicionarConta(Conta conta)
        {
            contas.Add(conta);
        }

        public Conta BuscarConta(int numero)
        {
            return contas.FirstOrDefault(c => c.Numero == numero);
        }

        public void Depositar(int numeroConta, double valor)
        {
            var conta = BuscarConta(numeroConta);
            if (conta != null)
            {
                conta.Depositar(valor);
            }
            else
            {
                throw new Exception("Conta não encontrada!");
            }
        }

        public void Sacar(int numeroConta, double valor)
        {
            var conta = BuscarConta(numeroConta);
            if (conta != null)
            {
                conta.Sacar(valor);
            }
            else
            {
                throw new Exception("Conta não encontrada!");
            }
        }

        public void Transferir(int contaOrigem, int contaDestino, double valor)
        {
            var origem = BuscarConta(contaOrigem);
            var destino = BuscarConta(contaDestino);

            if (origem != null && destino != null)
            {
                var transacao = new Transacao(origem, destino, valor);
                transacao.ExecutarTransferencia();
            }
            else
            {
                throw new Exception("Conta origem ou destino não encontrada!");
            }
        }
    }
}
