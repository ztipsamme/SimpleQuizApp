using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimpleQuizApp.Models;
using SimpleQuizApp.Services;
using SimpleQuizApp.ViewModels.Components;

namespace SimpleQuizApp.ViewModels;

public partial class PlayQuizViewModel : ViewModelBase
{
    private PlayQuizService _session;

    public string Title { get; }
    public int Count { get; }
    [ObservableProperty] private string _header;
    [ObservableProperty]
    private ObservableCollection<QuizOptionButtonViewModel> _optionButtons = new();

    [ObservableProperty] private Bitmap? _imageSrc;
    [ObservableProperty] private bool _hasImage;
    [ObservableProperty] private Question _currentQuestion;
    [ObservableProperty] private string _selectedOption;

    public PlayQuizViewModel(Quiz quiz, MainWindowViewModel main) : base(main)
    {
        Title = quiz.Title;
        Count    = quiz.Questions.Count;
        _session = new PlayQuizService(quiz);
        LoadCurrentQuestion();
    }

    private void LoadCurrentQuestion()
    {
        CurrentQuestion = _session.CurrentQuestion;
        Header = $"Fråga {_session.CurrentIndex + 1}";
        OptionButtons.Clear();
        foreach (var option in _session.GetShuffledOptions())
        {
            OptionButtons.Add(new QuizOptionButtonViewModel(
                OptionButtons.Count + 1,
                option,
                this,
                Main
            ));
        }

        _ = LoadImageAsync(CurrentQuestion.ImageName);
    }

    private async Task LoadImageAsync(string imgName)
    {
        ImageSrc = await FileService.GetImageAsync(imgName);
        HasImage = ImageSrc != null;
    }

    [RelayCommand]
    public async Task SelectedAnswer()
    {
        _session.SubmitAnswer(SelectedOption);
        await Task.Delay(1200);

        if (_session.HasMoreQuestions)
        {
            _session.MoveToNextQuestion();
            LoadCurrentQuestion();
        }
        else
        {
            // Show Quiz Result
            Main.NavigateTo(
                new PlayQuizResultViewModel(_session.Quiz,
                    _session.CorrectAnswers, Main));
        }
    }
}