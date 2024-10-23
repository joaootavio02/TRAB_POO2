using System;
using System.Windows;
using System.Windows.Controls;
using trab.Controller;

namespace trab
{
    public partial class TransacaoWindow : Window
    {
        private BancoController bancoController;

        public TransacaoWindow(BancoController controller)
        {
            InitializeComponent();
            bancoController = controller;
            SetInitialPlaceholders();
        }

        private void btnDepositar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int numeroConta = int.Parse(txtNumeroConta.Text);
                double valor = double.Parse(txtValor.Text);
                bancoController.Depositar(numeroConta, valor);
                MessageBox.Show("Depósito realizado com sucesso!");
                AtualizarSaldo(numeroConta);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void btnSacar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int numeroConta = int.Parse(txtNumeroConta.Text);
                double valor = double.Parse(txtValor.Text);
                bancoController.Sacar(numeroConta, valor);
                MessageBox.Show("Saque realizado com sucesso!");
                AtualizarSaldo(numeroConta);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void btnTransferir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int contaOrigem = int.Parse(txtContaOrigem.Text);
                int contaDestino = int.Parse(txtContaDestino.Text);
                double valor = double.Parse(txtValor.Text);
                bancoController.Transferir(contaOrigem, contaDestino, valor);
                MessageBox.Show("Transferência realizada com sucesso!");
                AtualizarSaldo(contaOrigem);
                AtualizarSaldo(contaDestino);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}");
            }
        }

        private void AtualizarSaldo(int numeroConta)
        {
            var conta = bancoController.BuscarConta(numeroConta);
            if (conta != null)
            {
                txtSaldoInfo.Text = $"Saldo: R$ {conta.Saldo:F2}";
            }
        }

        // Métodos para simular comportamento de Placeholder
        private void ClearPlaceholder(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && (textBox.Text == "Insira o número da conta" ||
                                    textBox.Text == "Insira o valor" ||
                                    textBox.Text == "Insira a conta origem" ||
                                    textBox.Text == "Insira a conta destino"))
            {
                textBox.Text = "";
                textBox.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void SetPlaceholder(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null && string.IsNullOrWhiteSpace(textBox.Text))
            {
                if (textBox.Name == "txtNumeroConta")
                    textBox.Text = "Insira o número da conta";
                else if (textBox.Name == "txtValor")
                    textBox.Text = "Insira o valor";
                else if (textBox.Name == "txtContaOrigem")
                    textBox.Text = "Insira a conta origem";
                else if (textBox.Name == "txtContaDestino")
                    textBox.Text = "Insira a conta destino";

                textBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        // Define os placeholders iniciais ao abrir a janela
        private void SetInitialPlaceholders()
        {
            txtNumeroConta.Text = "Insira o número da conta";
            txtValor.Text = "Insira o valor";
            txtContaOrigem.Text = "Insira a conta origem";
            txtContaDestino.Text = "Insira a conta destino";

            txtNumeroConta.Foreground = System.Windows.Media.Brushes.Gray;
            txtValor.Foreground = System.Windows.Media.Brushes.Gray;
            txtContaOrigem.Foreground = System.Windows.Media.Brushes.Gray;
            txtContaDestino.Foreground = System.Windows.Media.Brushes.Gray;
        }
    }
}
