using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimpleQuizApp.Models;
using SimpleQuizApp.ViewModels.Components;

namespace SimpleQuizApp.ViewModels;

public partial class CreateQuizViewModel : ViewModelBase
{
    [ObservableProperty] private string _title;

    public ObservableCollection<QuestionCardViewModel> QuestionCards { get; } =
        new();

    [ObservableProperty] private string _isValidErrorMessage =
        "Måste fylla i alla obligatoriska fält";

    [ObservableProperty] private string _allQuestionFieldsErrorMessage =
        "Måste fylla i alla obligatoriska frågefält";

    [ObservableProperty]
    private string _maxQuestionsErrorMessage = "Max 10 frågor";

    [ObservableProperty]
    private string _minQuestionsErrorMessage = "Måste finnas minst 3 frågor";

    [ObservableProperty] private bool _isAllFieldsErrorVisible;
    [ObservableProperty] private bool _isAllQuestionFieldsErrorVisible;
    [ObservableProperty] private bool _isMaxQuestionsVisible;
    [ObservableProperty] private bool _isMinQuestionsErrorVisible;

    public CreateQuizViewModel()
    {
        var childVM = new QuestionCardViewModel
        {
            IsExpanded = true,
            EditCommand = EditQuestionCommand,
        };
        QuestionCards.Add(childVM);
    }

    [RelayCommand]
    public async Task AddQuestion()
    {
        if (QuestionCards.Count == 10)
        {
            await ShowErrorTemporarilyAsync(
                () => IsMaxQuestionsVisible = true,
                () => IsMaxQuestionsVisible = false);
            return;
        }

        foreach (var questionCard in QuestionCards)
        {
            questionCard.IsExpanded = false;
        }

        QuestionCards.Add(new QuestionCardViewModel { IsExpanded = true, EditCommand = editQuestionCommand});
    }

    [RelayCommand]
    private void EditQuestion(QuestionCardViewModel question)
    {
        foreach (var q in QuestionCards)
            q.IsExpanded = false;

        question.IsExpanded = true;
    }

    [RelayCommand]
    public async Task SaveQuiz()
    {
        if (QuestionCards.Count < 3)
        {
            await ShowErrorTemporarilyAsync(
                () => IsMinQuestionsErrorVisible = true,
                () => IsMinQuestionsErrorVisible = false);
            return;
        }

        if (string.IsNullOrWhiteSpace(Title))
        {
            await ShowErrorTemporarilyAsync(
                () => IsAllFieldsErrorVisible = true,
                () => IsAllFieldsErrorVisible = false);
            return;
        }

        foreach (var q in QuestionCards)
        {
            string[] question =
                [q.Statement, q.CorrectOption, q.Option1, q.Option2, q.Option3];

            foreach (string s in question)
            {
                if (string.IsNullOrWhiteSpace(s))
                {
                    await ShowErrorTemporarilyAsync(
                        () => IsAllFieldsErrorVisible = true,
                        () => IsAllFieldsErrorVisible = false);
                    return;
                }
            }
        }

        var questions = QuestionCards.Select(q => new Question(q.Statement,
            q.CorrectOption, q.Option1, q.Option2, q.Option3)
        ).ToList();

        var quiz = new Quiz(Title, questions);
    }

    private async Task ShowErrorTemporarilyAsync(Action showAction,
        Action hideAction)
    {
        showAction();
        await Task.Delay(1200);
        hideAction();
    }
}