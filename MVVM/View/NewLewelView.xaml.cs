using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Sokoban.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для NewLewelView.xaml
    /// </summary>
    public partial class NewLewelView : Window
    {
        bool confirmed;

        public NewLewelView()
        {
            InitializeComponent();
        }

        public static int[] Show()
        {
            NewLewelView windows = new NewLewelView();
            windows.Width.Text = 10.ToString();
            windows.Height.Text = 10.ToString();
            windows.ShowDialog();

            int resultA;
            int resultB;
            int[] sizeMap;

            if (!windows.confirmed)
                return new int[] {0,0};

            if (!Int32.TryParse(windows.Width.Text, out resultA) || resultA <= 0 || !Int32.TryParse(windows.Height.Text, out resultB) || resultB <= 0)
            {
                MessageBox.Show("Wrong input");
                sizeMap = new int[2] { 0, 0 };
                
                return sizeMap;
            }

            sizeMap = new int[2] { resultA, resultB };
            return sizeMap;
        }

        private void CancelClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AddClicked(object sender, RoutedEventArgs e)
        {
            confirmed = true;
            Close();
        }
    }
}
