using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.Input;
using SimpleQuizApp.Models;
using SimpleQuizApp.Servises;

namespace SimpleQuizApp.ViewModels;

public partial class PlayQuizResultViewModel : ViewModelBase
{
    public Quiz Quiz { get; }
    public string Message { get; }
    public int CorrectAnswers { get; }
    public int WrongAnswers { get; }
    
    public Bitmap CoverImageSrc { get; private set; }
    public bool HasImage { get; private set; }
    

    public PlayQuizResultViewModel(Quiz q, int correctAnswers,
        MainWindowViewModel main) : base(main)
    {
        int totalQuestions = q.Questions.Count;
        Quiz = q;
        CorrectAnswers = correctAnswers;
        WrongAnswers = totalQuestions - correctAnswers;
        Message = correctAnswers switch
        {
            _ when correctAnswers == totalQuestions =>
                "Fantastiskt! Du fick alla rätt – imponerande!",
            _ when correctAnswers >= totalQuestions * 0.75 =>
                "Bra jobbat! Du fick nästan alla rätt!",
            _ when correctAnswers >= totalQuestions * 0.25 =>
                "Inte illa! Du fick ungefär hälften rätt.",
            _ when correctAnswers > 0 =>
                "Bra försök! Några rätt blev det i alla fall.",
            0 => "Ajdå, ingen rätt den här gången. Prova igen!",
            _ => "Resultatet kunde inte beräknas."
        };

        _ = LoadImageAsync(q.CoverImageName);
    }

    private async Task LoadImageAsync(string imgName)
    {
        var (src, hasImage) = await FileService.GetImageAsync(imgName);
        CoverImageSrc = src;
        HasImage = hasImage;
    }

    [RelayCommand]
    public void GoHome()
    {
        Main.NavigateTo(
            new HomeViewModel(Main));
    }
}