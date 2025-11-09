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
using SimpleQuizApp.Services;

namespace SimpleQuizApp.ViewModels.Components;

public partial class QuizFormViewModel : ViewModelBase
{
    private readonly Quiz? _quiz;
    [ObservableProperty] private string _viewHeader;
    [ObservableProperty] private string _viewDescription;
    [ObservableProperty] private string _title;
    [ObservableProperty] private string _category;
    [ObservableProperty] private string _description;
    [ObservableProperty] private ImageUploadViewModel _coverImageUpload;
    [ObservableProperty] private string _maxQuestionsErrorMessage;
    [ObservableProperty] private string _quizErrorMessage;
    [ObservableProperty] private string _questionsErrorMessage;

    [ObservableProperty]
    private ObservableCollection<QuestionCardViewModel> _questionCards =
        new();

    [ObservableProperty] private bool _quizErrorVisible;
    [ObservableProperty] private bool _isMaxQuestionsErrorVisible;
    [ObservableProperty] private bool _isQuestionsErrorVisible;

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

            CoverImageUpload = new(q.ImageName);
        }
        else
        {
            ViewHeader = "Skapa Quiz";

            var childVm =
                new QuestionCardViewModel(EditQuestionCommand,
                    RemoveQuestionCommand);
            QuestionCards.Add(childVm);
            CoverImageUpload = new();
        }

        ViewDescription = (q != null ? "Redigera" : "Skapa") +
                          " ditt Quiz steg för steg. Läg till mellan 3-10 frågor. Se över ditt Quiz innan du skapar och publicerar det.";
    }

    [RelayCommand]
    public async Task AddQuestion()
    {
        MaxQuestionsErrorMessage =
            ManageQuizService.HasMaxQuestionsMessage(QuestionCards);
        
        if (!string.IsNullOrWhiteSpace(MaxQuestionsErrorMessage))
        {
            IsMaxQuestionsErrorVisible = true;
            await Task.Delay(1200);
            IsMaxQuestionsErrorVisible = false;
            return;
        }

        foreach (var questionCard in QuestionCards)
        {
            questionCard.IsExpanded = false;
        }

        QuestionCards.Add(new QuestionCardViewModel(
            EditQuestionCommand,
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
        QuizErrorMessage =
            ManageQuizService.QuizErrorMessage(Title, QuestionCards);
        
        if (!string.IsNullOrWhiteSpace(QuizErrorMessage))
        {
            QuizErrorVisible = true;
            await Task.Delay(1200);
            QuizErrorVisible = false;
            return;
        }

        CoverImageUpload.SaveIfPresent();
        var coverImageName = CoverImageUpload.ImageName;


        var questions =
            ManageQuizService.MapToQuestions(QuestionCards, Category);

        bool updateQuiz =
            _quiz != null && await FileService.QuizExists(_quiz.Id);

        if (updateQuiz)
        {
            var updatedQuiz = await ManageQuizService.UpdateQuiz(
                _quiz.Id,
                Title,
                Category,
                Description,
                coverImageName,
                questions);

            if (updatedQuiz != null)
                Main.NavigateTo(new QuizViewModel(updatedQuiz, Main));
        }
        else
        {
            await ManageQuizService.SaveQuizAsync(
                Title,
                Category,
                Description,
                coverImageName,
                questions);

            Main.NavigateTo(new CreateQuizConfirmationViewModel(
                Title,
                CoverImageUpload.ImageSrc,
                Main));
        }
    }

    [RelayCommand]
    public async Task DeleteQuiz()
    {
        if (_quiz == null) return;

        await FileService.DeleteFromJsonFile(_quiz.Id);

        Main.NavigateTo(new HomeViewModel(Main));
    }
}