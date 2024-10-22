using System.Collections.Generic;
using System.Linq;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Quiz_Game_WPF_MOO_ICT
{
    public partial class MainWindow : Window
    {
        List<int> questionNumbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        int qNum = 0;
        int i;
        int score;
        int correctAnswers = 0;
        int incorrectAnswers = 0;
        DispatcherTimer timer = new DispatcherTimer();
        int timeLeft = 10;
        TimeSpan totalTimeSpent = TimeSpan.Zero; // Tempo total gasto

        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(1); // Intervalo de 1 segundo
            timer.Tick += Timer_Tick; // Associar o evento de tick
            StartGame();
            NextQuestion();
        }

        private void checkAnswer(object sender, RoutedEventArgs e)
        {
            Button senderButton = sender as Button;
            timer.Stop(); // Parar o temporizador quando o jogador responder

            if (senderButton.Tag.ToString() == "1")
            {
                score++;
                correctAnswers++;
                senderButton.Background = Brushes.Green; // Feedback imediato de resposta correta
            }
            else
            {
                incorrectAnswers++;
                senderButton.Background = Brushes.Red; // Feedback imediato de resposta incorreta
            }

            qNum++;
            scoreText.Content = "Pontuação: " + score;
            NextQuestion();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timeLeft--;
            totalTimeSpent = totalTimeSpent.Add(TimeSpan.FromSeconds(1)); // Aumentar o tempo total gasto
            timerText.Content = "Tempo restante: " + timeLeft + "s";

            if (timeLeft <= 0)
            {
                timer.Stop();
                MessageBox.Show("Tempo esgotado!");
                qNum++;
                NextQuestion();
            }
        }

        private void StartTimer()
        {
            timeLeft = 10; // Resetar o temporizador para 10 segundos
            timer.Start(); // Iniciar o temporizador
        }

        private void NextQuestion()
        {
            if (qNum >= questionNumbers.Count)
            {
                ShowEndScreen();
                return;
            }

            ResetButtons();

            StartTimer();

            // Carregar a próxima pergunta
            i = questionNumbers[qNum];

            switch (i)
            {
                case 1:
                    txtQuestion.Text = "Qual o nome verdadeiro do Superman?";
                    ans1.Content = "Bruce Wayne";
                    ans2.Content = "Clark Kent"; // Correta
                    ans3.Content = "Barry Allen";
                    ans4.Content = "Arthur Curry";
                    ans2.Tag = "1"; // Resposta correta
                    qImage.Source = new BitmapImage(new Uri("pack://application:,,,/images/1.jpg"));
                    break;

                case 2:
                    txtQuestion.Text = "Qual herói é conhecido como o Cavaleiro das Trevas?";
                    ans1.Content = "Superman";
                    ans2.Content = "Lanterna Verde";
                    ans3.Content = "Batman"; // Correta
                    ans4.Content = "Aquaman";
                    ans3.Tag = "1"; // Resposta correta
                    qImage.Source = new BitmapImage(new Uri("pack://application:,,,/images/2.jpg"));
                    break;

                case 3:
                    txtQuestion.Text = "Qual a identidade secreta do Flash?";
                    ans1.Content = "Oliver Queen";
                    ans2.Content = "Bruce Wayne";
                    ans3.Content = "Barry Allen"; // Correta
                    ans4.Content = "Hal Jordan";
                    ans3.Tag = "1"; // Resposta correta
                    qImage.Source = new BitmapImage(new Uri("pack://application:,,,/images/3.jpg"));
                    break;

                case 4:
                    txtQuestion.Text = "Qual o nome da ilha natal da Mulher-Maravilha?";
                    ans1.Content = "Amazônia";
                    ans2.Content = "Atlantis";
                    ans3.Content = "Themyscira"; // Correta
                    ans4.Content = "Metrópolis";
                    ans3.Tag = "1"; // Resposta correta
                    qImage.Source = new BitmapImage(new Uri("pack://application:,,,/images/4.jpg"));
                    break;

                case 5:
                    txtQuestion.Text = "Quem é o rei de Atlantis no universo DC?";
                    ans1.Content = "Aquaman"; // Correta
                    ans2.Content = "Lanterna Verde";
                    ans3.Content = "Flash";
                    ans4.Content = "Superman";
                    ans1.Tag = "1"; // Resposta correta
                    qImage.Source = new BitmapImage(new Uri("pack://application:,,,/images/5.jpg"));
                    break;

                case 6:
                    txtQuestion.Text = "Quem é a prima do Superman?";
                    ans1.Content = "Supergirl"; // Correta
                    ans2.Content = "Mulher-Maravilha";
                    ans3.Content = "Batgirl";
                    ans4.Content = "Zatanna";
                    ans1.Tag = "1"; // Resposta correta
                    qImage.Source = new BitmapImage(new Uri("pack://application:,,,/images/6.jpg"));
                    break;

                case 7:
                    txtQuestion.Text = "Quem é conhecido como o Arqueiro Verde?";
                    ans1.Content = "Bruce Wayne";
                    ans2.Content = "Oliver Queen"; // Correta
                    ans3.Content = "Clark Kent";
                    ans4.Content = "Barry Allen";
                    ans2.Tag = "1"; // Resposta correta
                    qImage.Source = new BitmapImage(new Uri("pack://application:,,,/images/7.jpg"));
                    break;

                case 8:
                    txtQuestion.Text = "Qual o anel que o Lanterna Verde usa?";
                    ans1.Content = "Anel de Kryptonita";
                    ans2.Content = "Anel de poder"; // Correta
                    ans3.Content = "Anel de fogo";
                    ans4.Content = "Anel de luz";
                    ans2.Tag = "1"; // Resposta correta
                    qImage.Source = new BitmapImage(new Uri("pack://application:,,,/images/8.jpg"));
                    break;

                case 9:
                    txtQuestion.Text = "Qual o planeta natal do Superman?";
                    ans1.Content = "Terra";
                    ans2.Content = "Marte";
                    ans3.Content = "Krypton"; // Correta
                    ans4.Content = "Apokolips";
                    ans3.Tag = "1"; // Resposta correta
                    qImage.Source = new BitmapImage(new Uri("pack://application:,,,/images/9.jpg"));
                    break;

                case 10:
                    txtQuestion.Text = "Quem é o vilão mais icônico do Batman?";
                    ans1.Content = "Charada";
                    ans2.Content = "Duas-Caras";
                    ans3.Content = "Coringa"; // Correta
                    ans4.Content = "Pinguim";
                    ans3.Tag = "1"; // Resposta correta
                    qImage.Source = new BitmapImage(new Uri("pack://application:,,,/images/10.jpg"));
                    break;
            }
        }

        private void ResetButtons()
        {
            // Resetar propriedades dos botões diretamente
            ans1.Tag = "0";
            ans2.Tag = "0";
            ans3.Tag = "0";
            ans4.Tag = "0";

            ans1.Background = Brushes.DarkSalmon;
            ans2.Background = Brushes.DarkSalmon;
            ans3.Background = Brushes.DarkSalmon;
            ans4.Background = Brushes.DarkSalmon;
        }

        private void RestartGame()
        {
            score = 0;
            correctAnswers = 0;
            incorrectAnswers = 0;
            qNum = 0;
            totalTimeSpent = TimeSpan.Zero;
            StartGame();
        }

        private void StartGame()
        {
            var randomList = questionNumbers.OrderBy(a => Guid.NewGuid()).ToList();
            questionNumbers = randomList;
            questionOrder.Content = null;

            for (int i = 0; i < questionNumbers.Count; i++)
            {
                questionOrder.Content += " " + questionNumbers[i].ToString();
            }
        }

        private void ShowEndScreen()
        {
            timer.Stop();

            // Exibir tela de fim com resumo de resultados
            string resultMessage = $"Jogo Concluído!\n" +
                                   $"Respostas Corretas: {correctAnswers}\n" +
                                   $"Respostas Incorretas: {incorrectAnswers}\n" +
                                   $"Tempo Total Gasto: {totalTimeSpent.Minutes} minutos e {totalTimeSpent.Seconds} segundos";

            EndScreen endScreen = new EndScreen(resultMessage);
            endScreen.ShowDialog(); // Mostrar a tela de fim

            // Não reiniciar automaticamente - aguarda o jogador clicar no botão na tela final
            RestartGame();
        }
    }
}
