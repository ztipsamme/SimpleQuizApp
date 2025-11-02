using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimpleQuizApp.Models;
using SimpleQuizApp.Servises;

namespace SimpleQuizApp.ViewModels.Components;

public partial class QuizCardViewModel : ViewModelBase
{
    private Quiz _quiz;
    [ObservableProperty] private string _title;
    [ObservableProperty] private string? _coverImageName;
    [ObservableProperty] private Bitmap _coverImageSrc;
    [ObservableProperty] private ObservableCollection<Question> _questions;

    [ObservableProperty] private bool _hasImage;

    public QuizCardViewModel(Quiz q, MainWindowViewModel main) : base(main)
    {
        _quiz = q;
        Title = q.Title;
        Questions = new ObservableCollection<Question>(q.Questions);
        CoverImageName = q.CoverImageName;

        _ = LoadImageAsync(CoverImageName);
    }
    
    private async Task LoadImageAsync(string imgName)
    {
        var (src, hasImage) = await FileService.GetImageAsync(imgName);
        CoverImageSrc = src;
        HasImage = hasImage;
    }

    [RelayCommand]
    public void OpenQuizView()
    {
        Main.NavigateTo(new QuizViewModel(_quiz.Id, Main));
    }
}