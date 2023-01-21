using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sokoban.MVVM.Model;

namespace Sokoban.MVVM.ViewModel
{
    public class UserScoreViewModel
    {
        private UserScore user;

        public string UserName
        {
            get { return user.UserName; }
            set { user.UserName = value; }
        }

        public int Score
        {
            get { return user.CountSteps; }
            set { user.CountSteps = value; }
        }

        public int Timer
        {
            get { return user.Time; }
            set { user.Time = value; }
        }

        public string PathToGame
        {
            get { return user.path; }
            set { user.path = value; }
        }

        string _shortPath;
        public string ShortPath
        {
            get
            {
                string[] path =  PathToGame.Split(new string[] { "xx" }, StringSplitOptions.None);
                _shortPath = path.Last();
                return _shortPath;
            }
        }


        public UserScore Model()
        {
            return user;
        }

        public UserScoreViewModel()
        {
            user = new UserScore();
        }

        public UserScoreViewModel(UserScore u)
        {
            user = u;
        }
    }
}
