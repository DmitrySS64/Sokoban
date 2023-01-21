using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sokoban.MVVM.ViewModel.Core;
using System.Windows.Input;
using System.Windows;
using Sokoban.MVVM.View;
using Microsoft.Win32;

namespace Sokoban.MVVM.ViewModel
{
    class HomeViewModel
    {
        private ICommand _StartGame;
        private ICommand _ChooseCharacter;
        private ICommand _LevelEditor;
        private ICommand _AboutGame;
        private ICommand _exit;


        public ICommand StartGame
        {
            get
            {
                if (_StartGame == null)
                {
                    _StartGame = new RelayCommand(x =>
                    {
                        string path;

                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        openFileDialog.Filter = "Text File|*.txt";
                        openFileDialog.ShowDialog();
                        if (openFileDialog.FileName != "")
                        {
                            path = openFileDialog.FileName;
                            GameGridView game = new GameGridView(path);
                        }

                    });
                }
                return _StartGame;
            }
        }

        public ICommand TopPlayers
        {
            get
            {
                if (_ChooseCharacter == null)
                {
                    _ChooseCharacter = new RelayCommand(x =>
                    {
                        TopUsersView.Show();
                    });
                }
                return _ChooseCharacter;
            }
        }

        public ICommand LevelEditor
        {
            get
            {
                if (_LevelEditor == null)
                {
                    _LevelEditor = new RelayCommand(x =>
                    {
                        LevelEditor window = new LevelEditor();
                    });
                }
                return _LevelEditor;
            }
        }

        public ICommand AboutGame
        {
            get
            {
                if (_AboutGame == null)
                {
                    _AboutGame = new RelayCommand(x =>
                    {

                    });
                }
                return _AboutGame;
            }
        }

        public ICommand Exit
        {
            get
            {
                if (_exit == null)
                {
                    _exit = new RelayCommand(x =>
                    {
                        Application.Current.MainWindow.Close();
                    });
                }
                return _exit;
            }
        }





        public HomeViewModel()
        {

        }
    }
}
