using System;
using System.Collections.Generic;
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
using Microsoft.Win32;
using Sokoban.MVVM.ViewModel;

namespace Sokoban.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для LevelEditor.xaml
    /// </summary>
    public partial class LevelEditor : Window
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
        private Button[,] AllButtons;

        MapViewModel map = LevelEditorViewModel.map;
        int CellSize = 25;

        public LevelEditor()
        {
            InitializeComponent();
            Show();
        }

        private void NewLVLClick(object sender, RoutedEventArgs e)
        {
            int[] sizeMap = new int[2];
            sizeMap = NewLewelView.Show();
            if (sizeMap[0] == 0 || sizeMap[1] == 0)
                return;
            map = new MapViewModel(sizeMap[0], sizeMap[1]);
            for (int i = 0; i < map.Width; i++)
                for (int j=0;j<map.Height;j++)
                {
                    map.SetCell(i, j, 0);
                }
            UpdateMapSize();
        }

        private void UpdateMapSize()
        {
            GameGrid.Children.Clear();
            GameGrid.Width = map.Width * CellSize;
            GameGrid.Height = map.Height * CellSize;
            for (int i = 0; i < map.Width; i++)
            {
                GameGrid.ColumnDefinitions.Add(new ColumnDefinition()
                    {
                        Width = new GridLength(CellSize)
                    });
            }
            for (int i = 0; i < map.Height; i++)
            {
                GameGrid.RowDefinitions.Add(new RowDefinition()
                {
                    Height = new GridLength(CellSize)
                });
            }
            imageControls = SetupGameCanvas(map);
            Update();
        }


        private Image[,] SetupGameCanvas(MapViewModel grid)
        {
            imageControls = new Image[grid.Width, grid.Height];
            AllButtons = new Button[grid.Width, grid.Height];

            for (int c = 0; c < grid.Width; c++)
            {
                for (int r = 0; r < grid.Height; r++)
                {
                    Image imageControl = new Image
                    {
                        Width = CellSize,
                        Height = CellSize
                    };
                    Button button = new Button();
                    button.Width = CellSize;
                    button.Height = CellSize;
                    button.Click += Button_Click;

                    imageControls[c, r] = imageControl;


                    button.Background = Brushes.Transparent;

                    AllButtons[c, r] = button;

                    Grid.SetColumn(imageControls[c, r], c);
                    Grid.SetRow(imageControls[c, r], r);
                    Grid.SetColumn(AllButtons[c, r], c);
                    Grid.SetRow(AllButtons[c, r], r);

                    GameGrid.Children.Add(imageControls[c, r]);
                    GameGrid.Children.Add(AllButtons[c, r]);
                }
            }
            return imageControls;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Vector2 Pos = GetPos(sender);
            if (Pos == null)
            {
                return;
            }
            int r = (int)Pos.Y;
            int c = (int)Pos.X;
            int id = 0;
            if (NoneRB.IsChecked.Value)
            {
                id = 0;

            }
            if (WallRB.IsChecked.Value)
                id = 1;
            if (FloorRB.IsChecked.Value)
                id = 2;
            if (PointRB.IsChecked.Value)
                id = 3;
            if (BoxRB.IsChecked.Value)
                id = 4;
            if (RedBoxRB.IsChecked.Value)
                id = 5;
            if (GreenBoxRB.IsChecked.Value)
                id = 6;
            if (PlayerRB.IsChecked.Value)
                id = 7;

            map.SetCell(c, r, id);
            imageControls[c, r].Source = tileImages[id];
        }

        private void Update()
        {
            DrawGrid(map);
        }

        private void DrawGrid(MapViewModel m)
        {
            for (int r = 0; r < m.Width; r++)
            {
                for (int c = 0; c < m.Height; c++)
                {
                    int id = m.GetCell(r, c);
                    imageControls[r, c].Source = tileImages[id];
                }
            }
        }

        private Vector2 GetPos(object btn)
        {
            for (int i = 0; i < map.Width; i++)
            {
                for(int j = 0; j < map.Height; j++)
                {
                    if (btn == AllButtons[i, j])
                    {
                        return new Vector2(i, j);
                    }
                }
            }
            return new Vector2();
        }

        


        #region Save, Load
        private void SaveClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text File|*.txt";
            saveFileDialog.ShowDialog();
            if (saveFileDialog.FileName != "" && map != null)
            {
                FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create);
                string path = saveFileDialog.FileName;
                
                fs.Close();

                Save(path);
            }
        }

        private void Save(string path)
        {
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                int width = map.Width;
                int height = map.Height;

                writer.WriteLineAsync(width.ToString());
                writer.WriteLineAsync(height.ToString());
                for (int j = 0; j < height; j++)
                {
                    for (int i = 0; i < width; i++)
                    {
                        int N = map.GetCell(i, j);
                        writer.WriteAsync(N.ToString());
                    }
                    writer.WriteLineAsync("");
                }
            }
        }

        private void LoadClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text File|*.txt";
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName != "")
            {
                string path = openFileDialog.FileName;
                map = new MapViewModel(path);
            }
            UpdateMapSize();
        }
        #endregion
    }
}
