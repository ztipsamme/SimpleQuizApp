using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SimpleQuizApp.ViewModels;

public partial class CreateQuizConfirmationViewModel : ViewModelBase
{
    public string Title { get; }
    public Bitmap CoverImageSrc { get; }
    [ObservableProperty] private bool _hasCoverImage;

    public CreateQuizConfirmationViewModel(string title, Bitmap coverImageSrc,
        MainWindowViewModel main) : base(main)
    {
        Title = title;
        CoverImageSrc = coverImageSrc;
        HasCoverImage = coverImageSrc != null;
    }

    [RelayCommand]
    public void GoHome()
    {
        Main.NavigateTo(
            new HomeViewModel(Main));
    }
}