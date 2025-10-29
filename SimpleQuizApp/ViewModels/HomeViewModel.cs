using System.Collections.Generic;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using SimpleQuizApp.Models;
using SimpleQuizApp.Servises;

namespace SimpleQuizApp.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    [ObservableProperty]
    private List<Quiz> _quizzes = new();

    public HomeViewModel()
    {
        _ = LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        var data = await FileService.ReadJsonFile();
        Quizzes.AddRange(data);
    }
}