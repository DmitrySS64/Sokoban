using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Sokoban.MVVM.Model;

namespace Sokoban.MVVM.ViewModel
{
    public class MapViewModel
    {
        private Map map;

        private Vector2 player;

        public int Width
        {
            get { return map.width; }
            private set { map.width = value; }
        }

        public int Height
        {
            get { return map.height; }
            private set { map.height = value; }
        }

        public CellType[,] Cells
        {
            get { return map.cells; }
            set { map.cells = value; }
        }

        public int GetCell(int i, int j)
        {
            CellType cell = Cells[i, j];
            switch (cell)
            {
                case CellType.None:
                    return 0;
                case CellType.Wall:
                    return 1;
                case CellType.Floor:
                    return 2;
                case CellType.PlaceForBox:
                    return 3;
                case CellType.Box:
                    return 4;
                case CellType.RedBox:
                    return 5;
                case CellType.GreenBox:
                    return 6;
                case CellType.Player:
                    return 7;

            }
            return 0;
        }

        public void SetCell(int i, int j, int N)
        {

            switch (N)
            {
                case 0:
                    Cells[i, j] = CellType.None;
                    break;
                case 1:
                    Cells[i, j] = CellType.Wall;
                    break;
                case 2:
                    Cells[i, j] = CellType.Floor;
                    break;
                case 3:
                    Cells[i, j] = CellType.PlaceForBox;
                    break;
                case 4:
                    Cells[i, j] = CellType.Box;
                    break;
                case 5:
                    Cells[i, j] = CellType.RedBox;
                    break;
                case 6:
                    Cells[i, j] = CellType.GreenBox;
                    break;
                case 7:
                    Cells[i, j] = CellType.Player;
                    break;
            }
        }

        public int GetNeedScore()
        {
            int count = 0;
            foreach (var i in Cells)
            {
                if (i == CellType.PlaceForBox || i == CellType.GreenBox)
                    count++;
            }
            return count;
        }

        public int GetGreenBoxs()
        {
            int count = 0;
            foreach (var i in Cells)
            {
                if (i == CellType.GreenBox)
                    count++;
            }
            return count;
        }

        public Vector2 GetPlacePlayer()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (GetCell(j,i) == 7)
                    {
                        return new Vector2(j, i);
                    }
                }

            }
            return new Vector2(0, 0);
        }

        public Map Model()
        {
            return map;
        }

        public MapViewModel()
        {
            map = new Map();
            Width = 50;
            Height = 50;
            Cells = new CellType[Width, Height];
        }

        public MapViewModel(int width, int height)
        {
            map = new Map();
            Width = width;
            Height = height;
            Cells = new CellType[Width, Height];
        }

        public MapViewModel(Map _map)
        {
            map = _map;
        }

        string _line;
        string line
        {
            get
            {
                return _line;
            }
            set
            {
                _line = value;
            }
        }

        public MapViewModel(string path)
        {
            map = new Map();
            using (StreamReader reader = new StreamReader(path))
            {
                line = reader.ReadLine();
                int width = Convert.ToInt32(line);
                line = reader.ReadLine();
                int height = Convert.ToInt32(line);
                Width = width;
                Height = height;
                Cells = new CellType[Width, Height];

                for (int y = 0; y < height; y++)
                {
                    line = reader.ReadLine();
                    char[] a = line.ToCharArray();
                    
                    for (int x = 0; x < width; x++)
                    {
                        int N = (int)Char.GetNumericValue(a[x]);
                        SetCell(x, y, N);
                    }
                }
            }
        }
    }
}
