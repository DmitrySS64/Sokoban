using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Sokoban.MVVM.ViewModel.Core;
using System.IO;
using Microsoft.Win32;
using Sokoban.MVVM.View;

namespace Sokoban.MVVM.ViewModel
{
    public class LevelEditorViewModel
    {
        public static MapViewModel map;

        public LevelEditorViewModel()
        {
            map = new MapViewModel();
        }


        #region Save, Load
        public ICommand SaveCommand
        {
            get
            {
                if (_SaveCommand == null)
                {
                    _SaveCommand = new RelayCommand(x =>
                    {
                        SaveFileDialog saveFileDialog = new SaveFileDialog();
                        saveFileDialog.Filter = "Text File|*.txt";
                        saveFileDialog.ShowDialog();
                        if (saveFileDialog.FileName != "" && map != null)
                        {
                            FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create);
                            string path = saveFileDialog.FileName;
                            Save(path);
                            fs.Close();
                        }
                    });
                }
                return _SaveCommand;
            }
        }
        private ICommand _SaveCommand;

        private void Save(string path)
        {
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                int width = map.Width;
                int height = map.Height;

                writer.WriteLineAsync(width + "/n" + height);
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

        public ICommand LoadCommand
        {
            get
            {
                if (_LoadCommand == null)
                {
                    _LoadCommand = new RelayCommand(x =>
                    {
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        openFileDialog.Filter = "Text File|*.txt";
                        openFileDialog.ShowDialog();
                        if (openFileDialog.FileName != "")
                        {
                            string path = openFileDialog.FileName;
                            map = new MapViewModel(path);
                        }
                    });
                }
                return _LoadCommand;
            }
        }
        private ICommand _LoadCommand;

        #endregion

        public ICommand NewLVL
        {
            get
            {
                if (_NewLVL == null)
                {
                    _NewLVL = new RelayCommand(x =>
                    {
                        int[] sizeMap = new int[2];
                        sizeMap = NewLewelView.Show();
                        if (sizeMap[0] == 0 || sizeMap[1] == 0)
                            return;

                    });
                }
                return _NewLVL;
            }
        }
        private ICommand _NewLVL;

    }

}
