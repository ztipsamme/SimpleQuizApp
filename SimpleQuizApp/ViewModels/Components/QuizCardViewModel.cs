using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using SimpleQuizApp.Models;

namespace SimpleQuizApp.ViewModels.Components;

public partial class QuizCardViewModel: ViewModelBase
{
    [ObservableProperty] private string _title;
    [ObservableProperty] private ObservableCollection<Question> _questions;

    public QuizCardViewModel(string title, List<Question> questions)
    {
        Title= title;
        Questions = new ObservableCollection<Question>(questions);
    }
}