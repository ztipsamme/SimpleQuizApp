using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimpleQuizApp.Models;

namespace SimpleQuizApp.ViewModels;

public partial class CreateQuizViewModel : ViewModelBase
{
    [ObservableProperty] private string _title;
    [ObservableProperty] private string _statement;
    [ObservableProperty] private string _correctOption;
    [ObservableProperty] private string _option1;
    [ObservableProperty] private string _option2;
    [ObservableProperty] private string _option3;

    [ObservableProperty]
    private string _allFieldsErrorMessage = "Måste fylla i alla obligatoriska fält";

    [ObservableProperty]
    private string _allQuestionFieldsErrorMessage = "Måste fylla i alla obligatoriska frågefält";

    [ObservableProperty]
    private string _maxQuestionsErrorMessage = "Max 10 frågor";

    [ObservableProperty]
    private string _minQuestionsErrorMessage = "Måste finnas minst 3 frågor";

    private bool IsAllQuestionFieldsFilled =>
        !new[] { Statement, CorrectOption, Option1, Option2, Option3 }
            .Any(string.IsNullOrWhiteSpace);

    private bool IsAllFieldsFilled => IsAllQuestionFieldsFilled &&
                                      string.IsNullOrWhiteSpace(Title);

    [ObservableProperty] private bool _isAllFieldsErrorVisible;
    [ObservableProperty] private bool _isAllQuestionFieldsErrorVisible;
    [ObservableProperty] private bool _isMaxQuestionsVisible;
    [ObservableProperty] private bool _isMinQuestionsErrorVisible;

    public ObservableCollection<Question> Questions { get; set; } = new();

    [RelayCommand]
    public async Task AddQuestion()
    {
        if (Questions.Count == 10)
        {
            await ShowErrorTemporarilyAsync(
                () => IsMaxQuestionsVisible = true,
                () => IsMaxQuestionsVisible = false);
            return;
        }

        if (!IsAllQuestionFieldsFilled)
        {
            await ShowErrorTemporarilyAsync(
                () => IsAllQuestionFieldsErrorVisible = true,
                () => IsAllQuestionFieldsErrorVisible = false);
            return;
        }

        Questions.Add(new Question(Statement, CorrectOption, Option1, Option2,
            Option3));
        ClearFields();
    }

    [RelayCommand]
    public void RemoveQuestion(Question question)
    {
        Questions.Remove(question);
    }

    [RelayCommand]
    public async Task SaveQuiz()
    {
        if (Questions.Count < 3)
        {
            await ShowErrorTemporarilyAsync(
                () => IsMinQuestionsErrorVisible = true,
                () => IsMinQuestionsErrorVisible = false);
            return;
        }

        if (IsAllFieldsFilled)
        {
            await ShowErrorTemporarilyAsync(
                () => IsAllFieldsErrorVisible = true,
                () => IsAllFieldsErrorVisible = false);
            return;
        }

        var quiz = new Quiz(Title, Questions);
    }

    private void ClearFields()
    {
        Statement = "";
        CorrectOption = "";
        Option1 = "";
        Option2 = "";
        Option3 = "";
    }

    private async Task ShowErrorTemporarilyAsync(Action showAction,
        Action hideAction)
    {
        showAction();
        await Task.Delay(1200);
        hideAction();
    }
}