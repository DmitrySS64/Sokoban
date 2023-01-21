using System.ComponentModel;


namespace Sokoban.MVVM.ViewModel.Core
{
    interface IControlView
    {
        INotifyPropertyChanged ViewModel { get; set; }
    }
}
