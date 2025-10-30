using CommunityToolkit.Mvvm.ComponentModel;
using SimpleQuizApp.Models;

namespace SimpleQuizApp.ViewModels;

public partial class PlayQuizViewModel : ViewModelBase
{
    [ObservableProperty] private string _title;

    public PlayQuizViewModel(Quiz q, MainWindowViewModel main) : base(main)
    {
        Title = q.Title;
    }
}