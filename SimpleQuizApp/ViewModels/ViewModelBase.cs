using CommunityToolkit.Mvvm.ComponentModel;

namespace SimpleQuizApp.ViewModels;

public class ViewModelBase : ObservableObject
{
    protected MainWindowViewModel Main { get; }

    protected ViewModelBase(MainWindowViewModel main)
    {
        Main = main;
    }

}