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
using Sokoban.MVVM.ViewModel;

namespace Sokoban.MVVM.View
{
    /// <summary>
    /// Логика взаимодействия для VictoryView.xaml
    /// </summary>
    public partial class VictoryView : Window
    {
        public VictoryView()
        {
            InitializeComponent();
        }

        private static UserScoreViewModel User;

        public static void Show(UserScoreViewModel user)
        {
            VictoryView window = new VictoryView();
            User = user;
            int score = User.Score;
            window.CountStep.Text = score.ToString();
            window.Time.Text = user.Timer.ToString();
            window.ShowDialog();
        }

        private void AddUserClicked(object sender, RoutedEventArgs e)
        {
            AddUserView.Show(User);
        }
    }
}
