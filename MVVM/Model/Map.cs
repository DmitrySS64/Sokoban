﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban.MVVM.Model
{
    public class Map
    {
        public int width;
        public int height;
        public CellType[,] cells;
    }
}
