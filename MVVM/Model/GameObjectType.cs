using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban.MVVM.Model
{
    public enum GameObjectType
    {
        None = 0,
        Box = 1,
        RedBox = 2,
        Gold = 3,    //метка для коробок
        Player = 4
    }
}
