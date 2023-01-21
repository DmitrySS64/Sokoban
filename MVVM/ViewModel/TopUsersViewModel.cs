using Sokoban.MVVM.Model;
using Sokoban.MVVM.ViewModel.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;

namespace Sokoban.MVVM.ViewModel
{
    class TopUsersViewModel
    {
        public TopUsersViewModel()
        {
            Users = new ObservableCollection<UserScoreViewModel>();
            LoadActiveDictionary.Execute(null);
        }

        public ObservableCollection<UserScoreViewModel> Users { get; set; }

        private ICommand LoadActiveDictionary
        {
            get
            {
                if (_loadActiveDictionary == null)
                {
                    _loadActiveDictionary = new RelayCommand(x =>
                    {
                        int i = 0;
                        string path = "D:/vs/Sokoban/bin/Debug/Top.xml";
                        FileStream fs = new FileStream(path, FileMode.Open);
                        XmlSerializer xml = new XmlSerializer(typeof(List<UserScore>));
                        var models = (IEnumerable<UserScore>)xml.Deserialize(fs);
                        fs.Close();
                        foreach (var model in models)
                        {
                            i++;
                            Users.Add(new UserScoreViewModel(model));
                            if(i == 100)
                            {
                                break;
                            }
                        }

                    });
                }
                return _loadActiveDictionary;
            }
        }
        private ICommand _loadActiveDictionary;

    }
}
