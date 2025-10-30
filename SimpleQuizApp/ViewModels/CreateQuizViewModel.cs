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
using SimpleQuizApp.ViewModels.Components;

namespace SimpleQuizApp.ViewModels;

public partial class CreateQuizViewModel : ViewModelBase
{
    [ObservableProperty] private string _title;
    [ObservableProperty] private string _category;
    [ObservableProperty] private string _description;
    [ObservableProperty] private string _tempCoverImagePath;
    [ObservableProperty] private string _coverImageFileName;

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
    [ObservableProperty] private bool _hasCoverImage;

    public CreateQuizViewModel(MainWindowViewModel main) : base(main)
    {
        var childVm =
            new QuestionCardViewModel(EditQuestionCommand,
                RemoveQuestionCommand);
        QuestionCards.Add(childVm);
    }

    [RelayCommand]
    public async Task SelectCoverImage()
    {
        var dlg = new OpenFileDialog();

        dlg.Filters.Add(new FileDialogFilter
            { Name = "Images", Extensions = { "png", "jpg", "jpeg" } });
        dlg.AllowMultiple = false;

        var window =
            App.Current.ApplicationLifetime is
                IClassicDesktopStyleApplicationLifetime
                desktop
                ? desktop.MainWindow
                : null;

        var res = await dlg.ShowAsync(window);

        if (res != null && res.Length > 0)
        {
            TempCoverImagePath = res[0];
            CoverImageFileName = Path.GetFileName(res[0]);
            HasCoverImage = true;
        }
    }

    [RelayCommand]
    public async Task RemoveCoverImage()
    {
        TempCoverImagePath = default;
        CoverImageFileName = default;
        HasCoverImage = false;
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


        if (!string.IsNullOrWhiteSpace(CoverImageFileName))
        {
            FileService.SaveImage(CoverImageFileName, TempCoverImagePath);
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
            q.CorrectOption,
            new List<string>() { q.Option1, q.Option2, q.Option3 })
        ).ToList();

        await FileService.WriteJsonFile(new Quiz(Title, Category, Description, CoverImageFileName,
            questions));
        
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