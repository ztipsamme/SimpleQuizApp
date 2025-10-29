using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using SimpleQuizApp.Models;
using SimpleQuizApp.Servises;
using SimpleQuizApp.ViewModels.Components;
using SimpleQuizApp.Views.Components;

namespace SimpleQuizApp.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<QuizCardViewModel> _quizCards = new();

    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private bool _hasQuizzes;

    public HomeViewModel()
    {
        _ = LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        IsLoading = true;
        HasQuizzes = false;

        var data = await FileService.ReadJsonFile();

        foreach (var q in data)
            QuizCards.Add(new QuizCardViewModel(q.Title, q.Questions));

        if (QuizCards.Count > 0)
        {
            HasQuizzes = true;
        }

        IsLoading = false;
    }
}