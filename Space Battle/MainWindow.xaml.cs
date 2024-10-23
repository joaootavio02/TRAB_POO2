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

namespace Space_battle_shooter_WPF_MOO_ICT
{
    public partial class MainWindow : Window
    {
        // Timer principal que controla o loop do jogo
        DispatcherTimer gameTimer = new DispatcherTimer();

        // Variáveis de controle para movimento e disparos automáticos
        bool moveLeft, moveRight;

        // Lista para armazenar objetos que devem ser removidos do jogo
        List<Rectangle> itemRemover = new List<Rectangle>();

        // Gerador de números aleatórios para criar inimigos e power-ups
        Random rand = new Random();

        // Variáveis para controle do jogo e comportamento dos inimigos
        int enemySpriteCounter = 0;
        int enemyCounter = 100;
        int playerSpeed = 10;
        int limit = 50;
        int score = 0;
        int damage = 0;
        int enemySpeed = 10;

        // Hitbox do jogador para verificar colisões
        Rect playerHitBox;

        public MainWindow()
        {
            InitializeComponent();

            // Configura o timer para controlar o loop do jogo e inicia
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Tick += GameLoop;
            gameTimer.Start();

            // Foco no Canvas para capturar as teclas pressionadas
            MyCanvas.Focus();

            // Configuração do background do jogo
            ImageBrush bg = new ImageBrush();
            bg.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/purple.png"));
            bg.TileMode = TileMode.Tile;
            bg.Viewport = new Rect(0, 0, 0.15, 0.15);
            bg.ViewportUnits = BrushMappingMode.RelativeToBoundingBox;
            MyCanvas.Background = bg;

            // Configuração da imagem do jogador
            ImageBrush playerImage = new ImageBrush();
            playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/player.png"));
            player.Fill = playerImage;
        }

        private void GameLoop(object sender, EventArgs e)
        {
            // Atualiza a hitbox do jogador para verificar colisões
            playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);

            // Reduz o contador de inimigos para criar novos
            enemyCounter -= 1;

            // Atualiza as informações de pontuação e dano na interface
            scoreText.Content = "Score: " + score;
            damageText.Content = "Damage: " + damage;

            // Cria novos inimigos e power-ups de vida quando o contador chega a zero
            if (enemyCounter < 0)
            {
                MakeEnemies();
                MakeHealthPowerUp(); // Possibilidade de aparecer um power-up de vida
                enemyCounter = limit;
            }

            // Controle de movimento do jogador
            if (moveLeft == true && Canvas.GetLeft(player) > 0)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - playerSpeed);
            }
            if (moveRight == true && Canvas.GetLeft(player) + 90 < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + playerSpeed);
            }

            // Loop que percorre todos os objetos na tela e controla comportamento
            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                // Controle dos projéteis (balas)
                if ((string)x.Tag == "bullet")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) - 20);
                    Rect bulletHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    // Remove balas que saem da tela
                    if (Canvas.GetTop(x) < 10)
                    {
                        itemRemover.Add(x);
                    }

                    // Verifica colisões de balas com inimigos
                    foreach (var y in MyCanvas.Children.OfType<Rectangle>())
                    {
                        if ((string)y.Tag == "enemy")
                        {
                            Rect enemyHit = new Rect(Canvas.GetLeft(y), Canvas.GetTop(y), y.Width, y.Height);

                            if (bulletHitBox.IntersectsWith(enemyHit))
                            {
                                itemRemover.Add(x); // Remove a bala
                                itemRemover.Add(y); // Remove o inimigo
                                score++; // Aumenta a pontuação
                            }
                        }
                    }
                }

                // Controle dos inimigos na tela
                if ((string)x.Tag == "enemy")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + enemySpeed);

                    // Remove inimigos que saem da tela
                    if (Canvas.GetTop(x) > 750)
                    {
                        itemRemover.Add(x);
                        damage += 10; // Penaliza o jogador com dano
                    }

                    Rect enemyHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    // Verifica colisão com o jogador
                    if (playerHitBox.IntersectsWith(enemyHitBox))
                    {
                        damage += 5; // Dano ao jogador se colidir com o inimigo
                        itemRemover.Add(x); // Remove o inimigo
                    }
                }

                // Controle do power-up de vida na tela
                if ((string)x.Tag == "health")
                {
                    Canvas.SetTop(x, Canvas.GetTop(x) + 5); // Move o power-up para baixo
                    Rect healthHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    // Verifica se o jogador coletou o power-up de vida
                    if (playerHitBox.IntersectsWith(healthHitBox))
                    {
                        damage = Math.Max(damage - 20, 0); // Recupera 20 pontos de vida, mas não vai abaixo de 0
                        itemRemover.Add(x); // Remove o power-up
                    }

                    // Remove power-ups que saem da tela
                    if (Canvas.GetTop(x) > 750)
                    {
                        itemRemover.Add(x);
                    }
                }
            }

            // Remove objetos que foram adicionados para remoção
            foreach (Rectangle i in itemRemover)
            {
                MyCanvas.Children.Remove(i);
            }

            // Aumenta a velocidade dos inimigos conforme a pontuação aumenta
            if (score > 5)
            {
                limit = 20;
                enemySpeed = 15;
            }

            // Se o jogador acumular 100 de dano, o jogo termina
            if (damage >= 100)
            {
                gameTimer.Stop();
                damageText.Content = "Damage: 100";
                damageText.Foreground = Brushes.Red;
                MessageBox.Show("Captain, you have destroyed " + score + " Alien Ships" + Environment.NewLine + "Press OK to close.", "Game Over");
                Application.Current.Shutdown();
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            // Controle de movimento e disparo manual
            if (e.Key == Key.Left)
            {
                moveLeft = true;
            }
            if (e.Key == Key.Right)
            {
                moveRight = true;
            }
            if (e.Key == Key.Space)
            {
                CreateBullet(); // Disparo manual
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            // Controle para parar movimento ao soltar tecla
            if (e.Key == Key.Left)
            {
                moveLeft = false;
            }
            if (e.Key == Key.Right)
            {
                moveRight = false;
            }
        }

        // Método para criar projéteis
        private void CreateBullet()
        {
            Rectangle newBullet = new Rectangle
            {
                Tag = "bullet",
                Height = 20,
                Width = 5,
                Fill = Brushes.White,
                Stroke = Brushes.Red
            };

            Canvas.SetLeft(newBullet, Canvas.GetLeft(player) + player.Width / 2);
            Canvas.SetTop(newBullet, Canvas.GetTop(player) - newBullet.Height);
            MyCanvas.Children.Add(newBullet);
        }

        // Método para criar inimigos
        private void MakeEnemies()
        {
            ImageBrush enemySprite = new ImageBrush();
            enemySpriteCounter = rand.Next(1, 5);

            // Seleciona uma imagem aleatória para os inimigos
            switch (enemySpriteCounter)
            {
                case 1:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/1.png"));
                    break;
                case 2:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/2.png"));
                    break;
                case 3:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/3.png"));
                    break;
                case 4:
                    enemySprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/4.png"));
                    break;
            }

            // Cria um novo inimigo
            Rectangle newEnemy = new Rectangle
            {
                Tag = "enemy",
                Height = 50,
                Width = 56,
                Fill = enemySprite
            };

            Canvas.SetTop(newEnemy, -100);
            Canvas.SetLeft(newEnemy, rand.Next(30, 430));
            MyCanvas.Children.Add(newEnemy);
        }

        // Método para criar power-ups de vida
        private void MakeHealthPowerUp()
        {
            // Criando o power-up de vida com imagem 5
            if (rand.Next(1, 6) == 5) // Apenas uma chance de gerar
            {
                ImageBrush healthSprite = new ImageBrush();
                healthSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/5.png"));

                Rectangle newHealth = new Rectangle
                {
                    Tag = "health",
                    Height = 50,
                    Width = 50,
                    Fill = healthSprite
                };

                Canvas.SetTop(newHealth, -100);
                Canvas.SetLeft(newHealth, rand.Next(30, 430));
                MyCanvas.Children.Add(newHealth);
            }
        }
    }
}
