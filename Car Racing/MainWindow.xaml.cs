using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Car_Racing_Game_WPF_MOO_ICT
{
    public partial class MainWindow : Window
    {
        // Timer que controla o loop do jogo
        DispatcherTimer gameTimer = new DispatcherTimer();

        // Lista para armazenar objetos que serão removidos do jogo (ex.: estrelas coletadas)
        List<Rectangle> itemRemover = new List<Rectangle>();

        // Gerador de números aleatórios para movimentação e escolha de objetos
        Random rand = new Random();

        // Imagens do carro do jogador e da estrela (power-up)
        ImageBrush playerImage = new ImageBrush();
        ImageBrush starImage = new ImageBrush();

        // Hitbox do jogador para verificar colisões
        Rect playerHitBox;

        // Variáveis de configuração para a jogabilidade
        int speed = 8; // Velocidade inicial dos carros inimigos e marcas na estrada
        int playerSpeed = 10; // Velocidade de movimentação do jogador
        int carNum; // Número para selecionar o tipo de carro aleatoriamente
        int starCounter = 30; // Controla o tempo para o aparecimento de novas estrelas
        int powerModeCounter = 200; // Tempo de duração do modo de poder (quando ativo)
        double score; // Contabiliza o tempo de sobrevivência do jogador
        double i = 1; // Variável para controle de animação no modo de poder

        // Variáveis booleanas para controlar o estado do jogo e ações do jogador
        bool moveLeft, moveRight, gameOver, powerMode;

        // Faixas horizontais disponíveis para os carros
        int[] lanes = { 50, 150, 250, 350 };

        public MainWindow()
        {
            InitializeComponent();

            // Configura para o Canvas receber o foco e capturar as teclas de movimento
            myCanvas.Focus();

            // Configura o timer para chamar GameLoop a cada 20ms
            gameTimer.Tick += GameLoop;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);

            // Inicia o jogo
            StartGame();
        }

        private void GameLoop(object sender, EventArgs e)
        {
            // Incrementa a pontuação conforme o tempo passa
            score += .05;

            // Aumenta a velocidade dos carros conforme a pontuação aumenta
            if (score > 20 && score < 40) speed = 10;
            else if (score >= 40 && score < 60) speed = 12;
            else if (score >= 60) speed = 14;

            // Atualiza o texto de pontuação na tela
            scoreText.Content = "Survived " + score.ToString("#.#") + " Seconds";

            // Atualiza a posição da hitbox do jogador
            playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);

            // Verifica se as teclas de movimento estão pressionadas para mover o carro
            if (moveLeft == true && Canvas.GetLeft(player) > 0)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - playerSpeed);
            }
            if (moveRight == true && Canvas.GetLeft(player) + 90 < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + playerSpeed);
            }

            // Gera uma nova estrela quando o contador chega a zero
            if (starCounter < 1)
            {
                MakeStar(); // Função que cria a estrela
                starCounter = rand.Next(600, 900); // Reinicia o contador para o próximo aparecimento
            }

            // Loop que verifica todos os objetos no Canvas (marcas, carros e estrelas)
            foreach (var x in myCanvas.Children.OfType<Rectangle>())
            {
                // Move as marcas da estrada para baixo para criar efeito de movimento
                if ((string)x.Tag == "roadMarks")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + speed);
                    if (Canvas.GetTop(x) > 510)
                    {
                        Canvas.SetTop(x, -152); // Reposiciona no topo quando sai da tela
                    }
                }

                // Controla a movimentação dos carros inimigos
                if ((string)x.Tag == "Car")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + speed);
                    if (Canvas.GetTop(x) > 500)
                    {
                        ChangeCars(x); // Troca o carro quando sai da tela
                    }

                    // Cria uma hitbox para verificar colisões entre o carro e o jogador
                    Rect carHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);
                    if (playerHitBox.IntersectsWith(carHitBox))
                    {
                        if (powerMode == true)
                        {
                            ChangeCars(x); // No modo de poder, troca o carro ao colidir
                        }
                        else
                        {
                            // Se não estiver no modo de poder, encerra o jogo ao colidir
                            gameTimer.Stop(); // Para o jogo
                            scoreText.Content += " Game Over! Press Enter to replay.";
                            gameOver = true; // Marca o estado de fim de jogo
                        }
                    }
                }

                // Controla o comportamento das estrelas no jogo
                if ((string)x.Tag == "star")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + 5);
                    Rect starHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (playerHitBox.IntersectsWith(starHitBox))
                    {
                        itemRemover.Add(x); // Marca a estrela para ser removida
                        powerMode = true; // Ativa o modo de poder
                        powerModeCounter = 200; // Define a duração do modo de poder
                    }

                    if (Canvas.GetTop(x) > 400)
                    {
                        itemRemover.Add(x); // Remove estrelas que saem da tela
                    }
                }
            }

            // Verifica se o modo de poder está ativo e diminui seu tempo de duração
            if (powerMode == true)
            {
                powerModeCounter -= 1;
                PowerUp(); // Aplica efeitos visuais do modo de poder
                if (powerModeCounter < 1)
                {
                    powerMode = false; // Desativa o modo de poder quando o tempo acaba
                }
            }
            else
            {
                // Se o modo de poder não está ativo, volta o visual do jogador ao normal
                playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/playerImage.png"));
                myCanvas.Background = Brushes.Gray;
            }

            // Remove objetos que foram adicionados à lista para remoção
            foreach (Rectangle y in itemRemover)
            {
                myCanvas.Children.Remove(y);
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            // Verifica se as teclas de seta são pressionadas para mover o carro
            if (e.Key == Key.Left)
            {
                moveLeft = true;
            }
            if (e.Key == Key.Right)
            {
                moveRight = true;
            }
        }

        private void OnKeyUP(object sender, KeyEventArgs e)
        {
            // Verifica se as teclas de seta são soltas para parar de mover o carro
            if (e.Key == Key.Left)
            {
                moveLeft = false;
            }
            if (e.Key == Key.Right)
            {
                moveRight = false;
            }

            // Permite reiniciar o jogo se o jogador pressionar "Enter" após game over
            if (e.Key == Key.Enter && gameOver == true)
            {
                StartGame();
            }
        }

        private void StartGame()
        {
            // Reinicia as variáveis do jogo para começar uma nova partida
            speed = 8; // Velocidade inicial
            gameTimer.Start();

            moveLeft = false;
            moveRight = false;
            gameOver = false;
            powerMode = false;
            score = 0;
            scoreText.Content = "Survived: 0 Seconds";

            // Carrega as imagens do jogador e da estrela
            playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/playerImage.png"));
            starImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/star.png"));
            player.Fill = playerImage;
            myCanvas.Background = Brushes.Gray;

            // Reposiciona os carros e remove estrelas antigas ao iniciar o jogo
            foreach (var x in myCanvas.Children.OfType<Rectangle>())
            {
                if ((string)x.Tag == "Car")
                {
                    Canvas.SetTop(x, (rand.Next(100, 400) * -1));
                    ChangeCars(x); // Ajusta a posição para evitar sobreposição
                }

                if ((string)x.Tag == "star")
                {
                    itemRemover.Add(x);
                }
            }
            itemRemover.Clear(); // Limpa lista de remoção
        }

        private void ChangeCars(Rectangle car)
        {
            // Seleciona uma imagem de carro aleatória
            carNum = rand.Next(1, 6);
            ImageBrush carImage = new ImageBrush();

            // Define a imagem de acordo com o número gerado
            switch (carNum)
            {
                case 1:
                    carImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/car1.png"));
                    break;
                case 2:
                    carImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/car2.png"));
                    break;
                case 3:
                    carImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/car3.png"));
                    break;
                case 4:
                    carImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/car4.png"));
                    break;
                case 5:
                    carImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/car5.png"));
                    break;
                case 6:
                    carImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/car6.png"));
                    break;
            }

            // Escolhe uma faixa aleatória e posiciona o carro
            int randomLane = lanes[rand.Next(lanes.Length)];
            Canvas.SetLeft(car, randomLane);
            Canvas.SetTop(car, (rand.Next(100, 400) * -1));
            car.Fill = carImage;
        }

        private void PowerUp()
        {
            // Controla a animação e efeitos visuais do modo de poder
            i += .5;
            if (i > 4)
            {
                i = 1;
            }

            // Troca a imagem do jogador enquanto o modo de poder está ativo
            switch (i)
            {
                case 1:
                    playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/powermode1.png"));
                    break;
                case 2:
                    playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/powermode2.png"));
                    break;
                case 3:
                    playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/powermode3.png"));
                    break;
                case 4:
                    playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/powermode4.png"));
                    break;
            }

            myCanvas.Background = Brushes.LightCoral; // Muda o fundo para destacar o modo de poder
        }

        private void MakeStar()
        {
            // Cria uma nova estrela no jogo
            Rectangle newStar = new Rectangle
            {
                Height = 50,
                Width = 50,
                Tag = "star",
                Fill = starImage
            };

            // Define posição inicial da estrela de forma aleatória
            Canvas.SetLeft(newStar, rand.Next(0, 430));
            Canvas.SetTop(newStar, (rand.Next(100, 400) * -1));
            myCanvas.Children.Add(newStar);
        }
    }
}
