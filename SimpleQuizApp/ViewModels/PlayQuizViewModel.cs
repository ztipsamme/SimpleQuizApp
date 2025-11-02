using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimpleQuizApp.Models;
using SimpleQuizApp.ViewModels.Components;

namespace SimpleQuizApp.ViewModels;

public partial class PlayQuizViewModel : ViewModelBase
{
    private Random _rnd = new();
    private int _currentQuestionIdx = 0;
    private Quiz _quiz;

    [ObservableProperty] private string _header;
    [ObservableProperty] private string _title;
    [ObservableProperty] private ObservableCollection<Question> _questions;

    [ObservableProperty]
    private ObservableCollection<QuizOptionButtonViewModel> _optionButtons;

    [ObservableProperty] private Question _currentQuestion;
    [ObservableProperty] private int _correctAnswers;
    [ObservableProperty] private bool _isChecked;
    [ObservableProperty] private string _selectedOption;
    [ObservableProperty] private bool _isAnswered;

    public PlayQuizViewModel(Quiz q, MainWindowViewModel main) : base(main)
    {
        _quiz = q;
        Title = q.Title;
        Questions =
            new ObservableCollection<Question>(q.Questions
                .OrderBy(_ => _rnd.Next())
                .Select(x =>
                {
                    x.Options = x.Options.OrderBy(_ => _rnd.Next()).ToList();
                    return x;
                }));
        OptionButtons = new ObservableCollection<QuizOptionButtonViewModel>();

        InitializeCurrentQuestion(Questions.First());
    }

    private void InitializeCurrentQuestion(Question? q = null)
    {
        Header = $"Fråga {_currentQuestionIdx + 1}";

        IsChecked = false;
        IsAnswered = false;
        CurrentQuestion = q ?? Questions[_currentQuestionIdx];

        OptionButtons.Clear();
        for (int i = 0; i < CurrentQuestion.Options.Count; i++)
        {
            OptionButtons.Add(new QuizOptionButtonViewModel(
                i + 1,
                CurrentQuestion.Options[i],
                this,
                Main
            ));
        }
    }

    [RelayCommand]
    public async Task SelectedAnswer()
    {
        if (CurrentQuestion.IsRightAnswer(SelectedOption)) CorrectAnswers++;

        IsAnswered = true;

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
        Main.NavigateTo(new PlayQuizResultViewModel(_quiz, CorrectAnswers, Main));
        Console.WriteLine("Quiz Result");
    }
}