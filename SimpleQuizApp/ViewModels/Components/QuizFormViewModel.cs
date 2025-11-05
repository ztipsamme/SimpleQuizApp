using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimpleQuizApp.Models;
using SimpleQuizApp.Servises;

namespace SimpleQuizApp.ViewModels.Components;

public partial class QuizFormViewModel : ViewModelBase
{
    private readonly Quiz? _quiz;

    [ObservableProperty] private ImageUploadViewModel _coverImageUpload = new();
    [ObservableProperty] private string _viewHeader;
    [ObservableProperty] private string _viewDescription;
    [ObservableProperty] private string _title;
    [ObservableProperty] private string _category;
    [ObservableProperty] private string _description;

    [ObservableProperty]
    private ObservableCollection<QuestionCardViewModel> _questionCards =
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

    public QuizFormViewModel(MainWindowViewModel main, Quiz? q = null) :
        base(main)
    {
        if (q != null)
        {
            _quiz = q;

            ViewHeader = "Redigera Quiz";
            Title = q.Title;
            Category = q.Category;
            Description = q.Description;

            foreach (Question question in q.Questions)
            {
                var qCard = new QuestionCardViewModel(question,
                    EditQuestionCommand,
                    RemoveQuestionCommand);

                QuestionCards.Add(qCard);
            }

            CoverImageUpload = new(q.CoverImageName);
        }
        else
        {
            ViewHeader = "Skapa Quiz";

            var childVm =
                new QuestionCardViewModel(EditQuestionCommand,
                    RemoveQuestionCommand);
            QuestionCards.Add(childVm);
        }

        ViewDescription = (q != null ? "Redigera" : "Skapa") +
                          " ditt Quiz steg för steg. Läg till mellan 3-10 frågor. Se över ditt Quiz innan du skapar och publicerar det.";
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

        QuestionCards.Add(new QuestionCardViewModel(EditQuestionCommand,
            RemoveQuestionCommand));
    }

    [RelayCommand]
    private void EditQuestion(QuestionCardViewModel question)
    {
        foreach (var q in QuestionCards)
            q.IsExpanded = false;

        question.IsExpanded = true;
    }

    [RelayCommand]
    private void RemoveQuestion(QuestionCardViewModel question)
    {
        QuestionCards.Remove(question);
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

        CoverImageUpload.SaveIfPresent();
        var coverImageFileName = CoverImageUpload.ImageFileName;

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

            q.QuestionImageUpload.SaveIfPresent();
        }

        var questions = QuestionCards.Select(q => new Question(q.Statement,
            q.CorrectOption,
            new List<string>() { q.Option1, q.Option2, q.Option3 },
            q.QuestionImageUpload.ImageFileName)
        ).ToList();

        if (_quiz != null && await FileService.QuizExists(_quiz.Id))
        {
            await FileService.UpdateJsonFile(_quiz.Id, Title, Category,
                Description,
                coverImageFileName, questions);
            Main.NavigateTo(new QuizViewModel(_quiz.Id, Main));
        }
        else
        {
            await FileService.WriteJsonFile(new Quiz(Title, Category,
                Description,
                coverImageFileName,
                questions));
            Main.NavigateTo(new HomeViewModel(Main));
        }
    }

    [RelayCommand]
    public async Task DeleteQuiz()
    {
        if (_quiz == null) return;

        await FileService.DeleteFromJsonFile(_quiz.Id);

        Main.NavigateTo(new HomeViewModel(Main));
    }

    private async Task ShowErrorTemporarilyAsync(Action showAction,
        Action hideAction)
    {
        showAction();
        await Task.Delay(1200);
        hideAction();
    }
}