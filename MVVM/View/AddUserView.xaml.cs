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
    /// Логика взаимодействия для AddUserView.xaml
    /// </summary>
    public partial class AddUserView : Window
    {
        public AddUserView()
        {
            InitializeComponent();
            AddUserVM = new AddUserViewModel();
        }

        private static UserScoreViewModel User;
        static AddUserView window;
        AddUserViewModel AddUserVM;

        public static void Show(UserScoreViewModel user)
        {
            User = user;
            window = new AddUserView();
            window.ShowDialog();

            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string user = window.UserTextBox.Text;
            User.UserName = user;
            AddUserVM.AddUser(User);
            window.Close();
        }
    }
}
