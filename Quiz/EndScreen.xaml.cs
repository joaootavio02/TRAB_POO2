using System;
using System.Windows;

namespace Quiz_Game_WPF_MOO_ICT
{
    public partial class EndScreen : Window
    {
        public EndScreen(string resultMessage)
        {
            InitializeComponent();
            resultText.Text = resultMessage;
        }

        private void RestartGame_Click(object sender, RoutedEventArgs e)
        {
            // Fechar a tela final e iniciar um novo jogo
            this.Close();
        }
    }
}
