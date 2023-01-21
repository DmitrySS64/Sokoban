using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Sokoban.MVVM.ViewModel;

namespace Sokoban.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для GameGridView.xaml
    /// </summary>
    public partial class GameGridView : Window
    {
        private readonly ImageSource[] tileImages = new ImageSource[]
        {
            new BitmapImage(new Uri("/Image/Map/none.png", UriKind.Relative)),
            new BitmapImage(new Uri("/Image/Map/Top external/LFRB.png", UriKind.Relative)),
            new BitmapImage(new Uri("/Image/Map/Floor/1.png", UriKind.Relative)),
            new BitmapImage(new Uri("/Image/Map/PlaceForBox.png", UriKind.Relative)),
            new BitmapImage(new Uri("/Image/Map/Boxs/box.png", UriKind.Relative)),
            new BitmapImage(new Uri("/Image/Map/Boxs/red box.png", UriKind.Relative)),
            new BitmapImage(new Uri("/Image/Map/Boxs/greenBox.png", UriKind.Relative)),
            new BitmapImage(new Uri("/Image/Player.png", UriKind.Relative))
        };

        private Image[,] imageControls;

        bool win;

        MapViewModel map;
        GameState gameState;
        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        int Timer
        {
            get { return _timer; }
            set
            {
                _timer = value;
                TimerBox.Text = _timer.ToString();
            }
        }
        int _timer;

        int CountMove
        {
            get { return _countMove; }
            set
            {
                if (value < 0)
                    value = 0;
                _countMove = value;
                MoveBox.Text = _countMove.ToString();
            }
        }
        int _countMove;
        string Path;

        public GameGridView(string path)
        {
            InitializeComponent();
            Path = path;
            Start(path);
            this.Show();

            KeyDown += Window_KeyDown;

            Timer = 0;
            CountMove = 0;
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            Timer++;
        }

        int cellSize = 64;

        private Image[,] SetupGameCanvas(MapViewModel grid)
        {
            imageControls = new Image[grid.Width, grid.Height];
            

            for (int r = 0; r < grid.Width; r++)
            {
                for (int c = 0; c < grid.Height; c++ )
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize
                    };

                    imageControls[r, c] = imageControl;

                    //Grid.SetColumn(imageControls[r, c], c);
                    //Grid.SetRow(imageControls[r, c], r);
                    //GameGrid.Children.Add(imageControls[r, c]);
                    Canvas.SetTop(imageControl, c * cellSize);
                    Canvas.SetLeft(imageControl, r * cellSize);
                    GameCanvas.Children.Add(imageControls[r, c]);
                }
            }
            return imageControls;
        }

        private void DrawGrid(GameState map)
        {
            MapViewModel grid = map.Model();
            for (int r = 0; r < grid.Width; r++)
            {
                for (int c = 0; c < grid.Height; c++)
                {
                    int id = grid.GetCell(r,c);
                    imageControls[r, c].Source = tileImages[id];
                }
            }
        }

        private void Draw(GameState map)
        {
            DrawGrid(map);
        }

        private void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            Draw(gameState);
        }

        private void Start(string path)
        {
            map = new MapViewModel(path);
            gameState = new GameState(map);
            GameCanvas.Width = map.Width * cellSize;
            GameCanvas.Height = map.Height * cellSize;
            imageControls = SetupGameCanvas(map);
        }

        private void Update()
        {
            Draw(gameState);
            if (gameState.WinGame)
            {
                Win();
                win = true;
            }
        }

        private void Win()
        {
            dispatcherTimer.Stop();
            KeyDown -= Window_KeyDown;
            EndGame.Visibility = Visibility.Visible;
            UserScoreViewModel userScore = new UserScoreViewModel();
            userScore.PathToGame = Path;
            userScore.Score = CountMove;
            userScore.Timer = this.Timer;
            VictoryView.Show(userScore);

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key.ToString())
            {
                case "Up": 
                case "W":
                    gameState.Move(new Vector2(0, -1));
                    CountMove++;
                    Update();
                    break;
                case "Down":
                case "S":
                    gameState.Move(new Vector2(0, 1));
                    CountMove++;
                    Update();
                    break;
                case "Right":
                case "D":
                    gameState.Move(new Vector2(1, 0));
                    CountMove++;
                    Update();
                    break;
                case "Left":
                case "A":
                    gameState.Move(new Vector2(-1, 0));
                    CountMove++;
                    Update();
                    break;
                case "Z":
                    gameState.ReturnMove();
                    CountMove--;
                    Update();
                    break;
                case "R":
                    gameState.ReturnGame();
                    Timer = 0;
                    CountMove = 0;
                    Update();
                    break;
            }
        }

        private void ReturnClick(object sender, RoutedEventArgs e)
        {
            gameState.ReturnMove();
            CountMove--;
            Update();
        }

        private void ReturnGameClick(object sender, RoutedEventArgs e)
        {
            if (win)
            {
                EndGame.Visibility = Visibility.Hidden;
                KeyDown += Window_KeyDown;

            }
            gameState.ReturnGame();
            Timer = 0;
            CountMove = 0;
            Update();
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            gameState.SaveGame();
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
