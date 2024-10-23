using System.Windows;
using trab.Controller;

namespace trab
{
    public partial class MainWindow : Window
    {
        private BancoController bancoController;

        public MainWindow()
        {
            InitializeComponent();
            bancoController = new BancoController();
        }

        private void btnAbrirCriarConta_Click(object sender, RoutedEventArgs e)
        {
            CriarContaWindow criarContaWindow = new CriarContaWindow(bancoController);
            criarContaWindow.ShowDialog();  // Abre a janela de criação de contas
        }

        private void btnAbrirTransacao_Click(object sender, RoutedEventArgs e)
        {
            TransacaoWindow transacaoWindow = new TransacaoWindow(bancoController);
            transacaoWindow.ShowDialog();  // Abre a janela de transações
        }
    }
}
