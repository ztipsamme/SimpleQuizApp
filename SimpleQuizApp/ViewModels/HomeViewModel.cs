using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using SimpleQuizApp.Services;
using SimpleQuizApp.ViewModels.Components;

namespace SimpleQuizApp.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private bool _hasQuizzes;
    [ObservableProperty]
    private ObservableCollection<CategoryGroup> _quizCardGroups =
        new();

    public record CategoryGroup(
        string Category,
        ObservableCollection<QuizCardViewModel> QuizCard);


    public HomeViewModel(MainWindowViewModel main) : base(main)
    {
        _ = GetQuizCardGroups();
    }

    private async Task GetQuizCardGroups()
    {
        try
        {
            IsLoading = true;
            HasQuizzes = false;

            var quizzes =
                await QuizService.GetQuizzesGroupedByCategoryAsync();
            HasQuizzes = quizzes.Any();

            var result = new ObservableCollection<CategoryGroup>();

            foreach (var categoryGroup in quizzes)
            {
                var categoryQuizWithRandomQuestions = await QuizService
                    .GetCategoryQuizWithRandomQuestionsAsync(categoryGroup
                        .Category);

                if (categoryQuizWithRandomQuestions != null)
                    categoryGroup.Quizzes.Insert(0,
                        categoryQuizWithRandomQuestions);

                var quizCardVMs = categoryGroup.Quizzes
                    .Select(q => new QuizCardViewModel(q, Main))
                    .ToList();

                result.Add(new CategoryGroup(categoryGroup.Category,
                    new ObservableCollection<QuizCardViewModel>(quizCardVMs)));
            }

            QuizCardGroups = result;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error when reading quiz: " + e.Message);
        }
        finally
        {
            IsLoading = false;
        }
    }
}