using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimpleQuizApp.Models;
using SimpleQuizApp.Servises;
using SimpleQuizApp.ViewModels.Components;

namespace SimpleQuizApp.ViewModels;

public partial class PlayQuizViewModel : ViewModelBase
{
    private Random _rnd = new();
    private Quiz _quiz;
    private int _currentQuestionIdx = 0;
    private int _correctAnswers;

    public string Header { get; private set; }
    public string Title { get; }
    
    public ObservableCollection<Question> Questions { get; }

    [ObservableProperty]
    private ObservableCollection<QuizOptionButtonViewModel> _optionButtons;
    [ObservableProperty] private Bitmap _imageSrc;
    [ObservableProperty] private bool _hasImage;
    [ObservableProperty] private Question _currentQuestion;
    [ObservableProperty] private string _selectedOption;

    public PlayQuizViewModel(Quiz q, MainWindowViewModel main) : base(main)
    {
        _quiz = q;
        Title = q.Title;
        Questions =
            new ObservableCollection<Question>(q.Questions
                .OrderBy(_ => _rnd.Next()));
        OptionButtons = new ObservableCollection<QuizOptionButtonViewModel>();

        InitializeCurrentQuestion(Questions.First());
    }

    private void InitializeCurrentQuestion(Question? q = null)
    {
        Header = $"Fråga {_currentQuestionIdx + 1}";

        CurrentQuestion = q ?? Questions[_currentQuestionIdx];
        CurrentQuestion.Options.Add(CurrentQuestion.CorrectOption);

        List<string> options =
            CurrentQuestion.Options.OrderBy(_ => _rnd.Next()).ToList();

        OptionButtons.Clear();
        for (int i = 0; i < options.Count; i++)
        {
            OptionButtons.Add(new QuizOptionButtonViewModel(
                i + 1,
                CurrentQuestion.Options[i],
                this,
                Main
            ));
        }

        _ = LoadImageAsync(CurrentQuestion.ImageFileName);
    }
    
    private async Task LoadImageAsync(string imgName)
    {
        var (src, hasImage) = await FileService.GetImageAsync(imgName);
        ImageSrc = src;
        HasImage = hasImage;
    }

    [RelayCommand]
    public async Task SelectedAnswer()
    {
        if (CurrentQuestion.IsRightAnswer(SelectedOption)) _correctAnswers++;
        
        await Task.Delay(1200);

        NextQuestion();
    }

    [RelayCommand]
    public void NextQuestion()
    {
        SelectedOption = default;

        if (_currentQuestionIdx == Questions.Count - 1)
        {
            ShowQuizResult();
            return;
        }

        _currentQuestionIdx++;
        InitializeCurrentQuestion();
    }

    public void ShowQuizResult()
    {
        Main.NavigateTo(
            new PlayQuizResultViewModel(_quiz, _correctAnswers, Main));
    }
}