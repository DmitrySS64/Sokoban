using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sokoban.MVVM.Model
{
    public class UserScore
    {
        public string UserName;
        public int CountSteps;
        public int Time;
        public string path { get; set; }
    }
}
