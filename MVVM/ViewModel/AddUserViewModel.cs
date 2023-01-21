using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;
using Sokoban.MVVM.ViewModel.Core;
using Sokoban.MVVM.Model;
using System.Windows;

namespace Sokoban.MVVM.ViewModel
{
    class AddUserViewModel: ObservateObject
    {
        public AddUserViewModel()
        {
            Users = new ObservableCollection<UserScoreViewModel>();
            SortUsers = new ObservableCollection<UserScoreViewModel>();
            LoadActiveDictionary.Execute(null);
        }

        public ObservableCollection<UserScoreViewModel> Users { get; set; }
        public ObservableCollection<UserScoreViewModel> SortUsers { get; set; }

        private string _user;
        public string User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }

        public void AddUser(UserScoreViewModel user)
        {
            Users.Add(user);
            SortTop.Execute(null);
        }

        private ICommand SortTop
        {
            get
            {
                if (_sortTop == null)
                {
                    _sortTop = new RelayCommand(x =>
                    {
                        if (Users.Count < 2)
                        {
                            SortUsers = Users;
                            SaveActiveDictionary.Execute(null);
                            return;
                        }

                        int Count = Users.Count;
                        for (int i = 0; i < Count; i++)
                        {
                            var min = Users.First();
                            foreach (var user in Users)
                            {
                                if (min.Score > user.Score)
                                {
                                    min = user;
                                }
                            }
                            SortUsers.Add(min);
                            Users.Remove(min);
                        }
                        SaveActiveDictionary.Execute(null);

                    });
                }
                return _sortTop;
            }
        }
        private ICommand _sortTop;

        string path = "D:/vs/Sokoban/bin/Debug/Top.xml";

        private ICommand SaveActiveDictionary
        {
            get
            {
                if (_setActiveDictionary == null)
                {
                    _setActiveDictionary = new RelayCommand(x =>
                    {
                        FileStream fs = new FileStream(path, FileMode.Create);
                        var users = from model in SortUsers where (model.UserName != null) select model.Model();
                        XmlSerializer xml = new XmlSerializer(typeof(List<UserScore>));
                        xml.Serialize(fs, users.ToList());
                        fs.Close();
                    });
                }
                return _setActiveDictionary;
            }
        }
        private ICommand _setActiveDictionary;


        private ICommand LoadActiveDictionary
        {
            get
            {
                if (_loadActiveDictionary == null)
                {
                    _loadActiveDictionary = new RelayCommand(x =>
                    {

                        FileStream fs = new FileStream(path, FileMode.Open);
                        XmlSerializer xml = new XmlSerializer(typeof(List<UserScore>));
                        var models = (IEnumerable<UserScore>)xml.Deserialize(fs);
                        fs.Close();
                        foreach (var model in models)
                        {
                            Users.Add(new UserScoreViewModel(model));
                        }

                    });
                }
                return _loadActiveDictionary;
            }
        }
        private ICommand _loadActiveDictionary;
    }
}
