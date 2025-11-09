using System;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimpleQuizApp.Models;
using SimpleQuizApp.Services;

namespace SimpleQuizApp.ViewModels.Components;

public partial class QuizCardViewModel : ViewModelBase
{
    [ObservableProperty] private Quiz _quiz;
    [ObservableProperty] private Bitmap? _imageSrc;
    [ObservableProperty] private bool _hasImage;
    
    public QuizCardViewModel(Quiz q, MainWindowViewModel main) : base(main)
    {
        _quiz = q;
        _ = LoadImageAsync(_quiz.ImageName);
    }
    
    private async Task LoadImageAsync(string imageName)
    {
        ImageSrc = await ImageService.LoadAsync(imageName);
        HasImage = ImageSrc != null;
    }
    
    [RelayCommand]
    public void OpenQuizView()
    {
        Main.NavigateTo(new QuizViewModel(Quiz, Main));
    }
}