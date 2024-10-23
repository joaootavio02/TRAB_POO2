using System;
using System.Windows;
using System.Windows.Controls;
using trab.Controller;
using trab.Model;
using trab.Model.trab.Models;

namespace trab
{
    public partial class CriarContaWindow : Window
    {
        private BancoController bancoController;

        public CriarContaWindow(BancoController controller)
        {
            InitializeComponent();
            bancoController = controller;
        }

        private void btnCriarConta_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validação dos campos
                string nome = txtNomeCliente.Text;
                string cpf = txtCPFCliente.Text;
                if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(cpf))
                {
                    MessageBox.Show("Erro: Todos os campos devem ser preenchidos.");
                    return;
                }

                // Validação do número da conta
                if (!int.TryParse(txtNumeroNovaConta.Text, out int numeroConta))
                {
                    MessageBox.Show("Erro: O número da conta deve ser um valor numérico válido.");
                    return;
                }

                // Verifica se já existe uma conta com o mesmo número
                if (bancoController.BuscarConta(numeroConta) != null)
                {
                    MessageBox.Show("Erro: Já existe uma conta com esse número!");
                    return;
                }

                // Criação do cliente
                Cliente cliente = new Cliente(nome, cpf);

                // Verifica o tipo de conta selecionado
                if (cmbTipoConta.SelectedItem is ComboBoxItem tipoConta)
                {
                    if (tipoConta.Content.ToString().Contains("Corrente"))
                    {
                        ContaCorrente contaCorrente = new ContaCorrente(numeroConta, cliente);
                        bancoController.AdicionarConta(contaCorrente);
                    }
                    else if (tipoConta.Content.ToString().Contains("Poupança"))
                    {
                        Poupanca poupanca = new Poupanca(numeroConta, cliente, 0.05); // Exemplo com taxa de juros de 5%
                        bancoController.AdicionarConta(poupanca);
                    }
                    else
                    {
                        MessageBox.Show("Erro: Selecione um tipo de conta válido.");
                        return;
                    }
                }

                MessageBox.Show("Conta criada com sucesso!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao criar conta: {ex.Message}");
            }
        }

        // Método para fechar a janela ao clicar no botão "Cancelar"
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
