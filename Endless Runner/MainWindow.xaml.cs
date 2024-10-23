using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Endless_Runner_WPF_MOO_ICT
{
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();

        Rect playerHitBox;
        Rect groundHitBox;
        Rect obstacleHitBox;
        Rect rocketHitBox; // Hitbox do foguete

        bool jumping; // Controle para salto
        bool crouching; // Controle para abaixar

        int force = 10; // Força de salto (ajustado para pular mais rápido, mas não mais alto)
        int speed = 5; // Velocidade do personagem no eixo Y
        int gameSpeed = 12; // Velocidade base para a movimentação dos elementos no eixo X

        Random rnd = new Random();

        bool gameOver;

        double spriteIndex = 0;

        ImageBrush playerSprite = new ImageBrush();
        ImageBrush backgroundSprite = new ImageBrush();
        ImageBrush obstacleSprite = new ImageBrush();
        ImageBrush rocketSprite = new ImageBrush(); // Sprite do foguete

        int[] obstaclePosition = { 320, 310, 300, 305, 315 };

        int score = 0;
        double originalHeight; // Armazena a altura original do player

        public MainWindow()
        {
            InitializeComponent();

            MyCanvas.Focus();

            gameTimer.Tick += GameEngine;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);

            backgroundSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/background.gif"));

            background.Fill = backgroundSprite;
            background2.Fill = backgroundSprite;

            StartGame();
        }

        private void GameEngine(object sender, EventArgs e)
        {
            // Atualização para mover o background
            Canvas.SetLeft(background, Canvas.GetLeft(background) - gameSpeed);
            Canvas.SetLeft(background2, Canvas.GetLeft(background2) - gameSpeed);

            if (Canvas.GetLeft(background) < -1262)
            {
                Canvas.SetLeft(background, Canvas.GetLeft(background2) + background2.Width);
            }

            if (Canvas.GetLeft(background2) < -1262)
            {
                Canvas.SetLeft(background2, Canvas.GetLeft(background) + background.Width);
            }

            // Controle do player e dos obstáculos
            Canvas.SetTop(player, Canvas.GetTop(player) + speed);
            Canvas.SetLeft(obstacle, Canvas.GetLeft(obstacle) - gameSpeed);
            Canvas.SetLeft(rocket, Canvas.GetLeft(rocket) - (gameSpeed + 5)); // Velocidade do foguete é um pouco maior

            scoreText.Content = "Score: " + score;

            // Ajuste para diminuir a área de colisão do boneco
            playerHitBox = new Rect(
                Canvas.GetLeft(player) + 10,
                Canvas.GetTop(player) + 10,
                player.Width - 40,
                player.Height - 20
            );

            // Outras hitboxes
            obstacleHitBox = new Rect(Canvas.GetLeft(obstacle), Canvas.GetTop(obstacle), obstacle.Width, obstacle.Height);
            rocketHitBox = new Rect(Canvas.GetLeft(rocket), Canvas.GetTop(rocket), rocket.Width, rocket.Height);
            groundHitBox = new Rect(Canvas.GetLeft(ground), Canvas.GetTop(ground), ground.Width, ground.Height);

            // Verificar colisão com o solo
            if (playerHitBox.IntersectsWith(groundHitBox))
            {
                speed = 0;
                Canvas.SetTop(player, Canvas.GetTop(ground) - player.Height);
                jumping = false; // Reseta o salto após tocar o solo
                crouching = false; // Reseta abaixar após tocar o solo

                spriteIndex += .5;

                if (spriteIndex > 8)
                {
                    spriteIndex = 1;
                }

                RunSprite(spriteIndex);
            }

            // Lógica de salto
            if (jumping)
            {
                speed = -30; // Velocidade ajustada para um pulo mais rápido, sem ser mais alto
                force -= 2;
            }
            else if (!crouching) // Não acelera se estiver abaixando
            {
                speed = 12;
            }

            if (force < 0)
            {
                jumping = false;
            }

            // Reposicionar obstáculos e foguetes
            if (Canvas.GetLeft(obstacle) < -50)
            {
                Canvas.SetLeft(obstacle, 950);
                Canvas.SetTop(obstacle, obstaclePosition[rnd.Next(0, obstaclePosition.Length)]);
                score += 1;
                IncrementGameSpeed(); // Aumenta a velocidade conforme o score
            }

            if (Canvas.GetLeft(rocket) < -50)
            {
                Canvas.SetLeft(rocket, 950);
                Canvas.SetTop(rocket, rnd.Next(150, 300)); // Posição aleatória do foguete
            }

            if (playerHitBox.IntersectsWith(obstacleHitBox) || playerHitBox.IntersectsWith(rocketHitBox))
            {
                gameOver = true;
                gameTimer.Stop();
            }

            if (gameOver)
            {
                obstacle.Stroke = Brushes.Black;
                obstacle.StrokeThickness = 1;

                player.Stroke = Brushes.Red;
                player.StrokeThickness = 1;

                scoreText.Content = "Score: " + score + " Press Enter to play again!!";
            }
            else
            {
                player.StrokeThickness = 0;
                obstacle.StrokeThickness = 0;
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && gameOver)
            {
                StartGame();
            }

            // Controle para abaixar ao pressionar seta para baixo
            if (e.Key == Key.Down)
            {
                crouching = true;
                player.Height = originalHeight / 2; // Reduz a altura para dar efeito de abaixar
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            // Controle de pulo ao pressionar "Espaço"
            if (e.Key == Key.Space && !jumping)
            {
                jumping = true;
                force = 10; // Força do pulo ajustada
                speed = -15; // Velocidade para um pulo mais rápido
                playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_02.gif"));
            }

            // Se a seta para baixo for solta, ele para de abaixar
            if (e.Key == Key.Down)
            {
                crouching = false;
                player.Height = originalHeight; // Restaura a altura original
            }
        }

        private void StartGame()
        {
            Canvas.SetLeft(background, 0);
            Canvas.SetLeft(background2, 1262);

            Canvas.SetLeft(player, 110);
            Canvas.SetTop(player, 140);

            // Ajuste para diminuir um pouco o tamanho do boneco
            player.Width = 90; // Largura reduzida em 10% (de 100 para 90)
            player.Height = 135; // Altura reduzida em 10% (de 150 para 135)

            Canvas.SetLeft(obstacle, 950);
            Canvas.SetTop(obstacle, 310);

            Canvas.SetLeft(rocket, 950);
            Canvas.SetTop(rocket, rnd.Next(150, 300)); // Posição inicial aleatória do foguete

            RunSprite(1);

            obstacleSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/obstacle.png"));
            rocketSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/foguete.png"));

            obstacle.Fill = obstacleSprite;
            rocket.Fill = rocketSprite;

            jumping = false;
            crouching = false;
            gameOver = false;
            score = 0;
            gameSpeed = 12; // Velocidade inicial
            originalHeight = player.Height; // Armazena a altura original do jogador

            scoreText.Content = "Score: " + score;

            gameTimer.Start();
        }

        private void RunSprite(double i)
        {
            switch (i)
            {
                case 1:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_01.gif"));
                    break;
                case 2:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_02.gif"));
                    break;
                case 3:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_03.gif"));
                    break;
                case 4:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_04.gif"));
                    break;
                case 5:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_05.gif"));
                    break;
                case 6:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_06.gif"));
                    break;
                case 7:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_07.gif"));
                    break;
                case 8:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_08.gif"));
                    break;
            }

            player.Fill = playerSprite;
        }

        private void IncrementGameSpeed()
        {
            // Aumenta a velocidade do jogo conforme o score vai subindo
            if (score % 5 == 0)
            {
                gameSpeed += 1; // Incremento na velocidade a cada 5 pontos
            }
        }
    }
}
