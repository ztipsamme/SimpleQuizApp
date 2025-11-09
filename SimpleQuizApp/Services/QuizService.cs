using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleQuizApp.Models;

namespace SimpleQuizApp.Services;

public class QuizService
{
    private static Random _rnd = new();
    public record CategoryGroup(
        string Category,
        List<Quiz> Quizzes);

    public static async Task<Quiz?>
        LoadQuizAsync(Guid id)
    {
        var quiz = await FileService.GetQuiz(id);
        if (quiz == null) return null;
        
        // Could maybe shuffle questions here instead?

        return quiz;
    }
    

    public static async Task<List<CategoryGroup>>
        GetQuizzesGroupedByCategoryAsync()
    {
        var quiz = await FileService.ReadJsonFile();
        
        List<CategoryGroup> groups = quiz
            .Where(q => !string.IsNullOrWhiteSpace(q.Category))
            .GroupBy(q => q.Category)
            .OrderBy(g => g.Key)
            .Select(g => new CategoryGroup(
                g.Key, g.ToList())).ToList();

        return groups;
    }
    
    private static Quiz? CreateCategoryQuizWithRandomQuestions(
        List<Quiz> quizzes, string category)
    {
        if (quizzes.Count < 2)
            return null;
        
        var randomQuestions = quizzes
            .SelectMany(q => q.Questions)
            .OrderBy(_ => _rnd.Next())
            .Take(10).ToList();
        int count = randomQuestions.Count;

        Quiz quiz = new(
            $"{count} frågor om {category}",
            category,
            $"{count} random frågor att besvara ur kategorin {category}.", default,
            randomQuestions
        );

        return quiz;
    }

    public static async Task<Quiz?>
        GetCategoryQuizWithRandomQuestionsAsync(string category)
    {
        var groups = await GetQuizzesGroupedByCategoryAsync();

        var group = groups.FirstOrDefault(g => g.Category == category)?.Quizzes;
        
        if (group == null || group.Count == 0) return null;
        
        return CreateCategoryQuizWithRandomQuestions(group, category);
    }
}