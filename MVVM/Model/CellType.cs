using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban.MVVM.Model
{
    public enum CellType
    {
        None = 0,
        Wall = 1,
        Floor = 2,
        PlaceForBox = 3,
        Box = 4,
        RedBox = 5,
        GreenBox = 6,
        Player = 7
    }
}
