using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using SimpleQuizApp.Models;
using SimpleQuizApp.ViewModels.Components;

namespace SimpleQuizApp.Services;

public static class ManageQuizService
{
    public static string HasMaxQuestionsMessage(
        ObservableCollection<QuestionCardViewModel> questionCards) =>
        questionCards.Count == 10 ? "Max 10 frågor." : string.Empty;


    private static bool EmptyQuestionMessage(
        ObservableCollection<QuestionCardViewModel> questionCards)
    {
        foreach (var q in questionCards)
        {
            string[] fields =
                [q.Statement, q.CorrectOption, q.Option1, q.Option2, q.Option3];

            if (fields.Any(string.IsNullOrWhiteSpace))
                return true;

            q.QuestionImageUpload.SaveIfPresent();
        }

        return false;
    }

    public static string QuizErrorMessage(string title,
        ObservableCollection<QuestionCardViewModel> questionCards)
    {
        if (string.IsNullOrWhiteSpace(title) || EmptyQuestionMessage(questionCards))
            return "Måste fylla i alla obligatoriska fält.";

        if (questionCards.Count < 3)
            return "Måste finnas minst 3 frågor.";

        return String.Empty;
    }

    public static List<Question>
        MapToQuestions(
            IEnumerable<QuestionCardViewModel> cards,
            string category) => cards
        .Select(q => new Question(
            q.Statement,
            category,
            q.CorrectOption,
            new List<string>() { q.Option1, q.Option2, q.Option3 },
            q.QuestionImageUpload.ImageName)
        ).ToList();

    public static async Task SaveQuizAsync(
        string title,
        string category,
        string description,
        string coverImageName,
        List<Question> questions)
        => await FileService.WriteJsonFile(new Quiz(
            title,
            category,
            description,
            coverImageName,
            questions));


    public static async Task<Quiz?> UpdateQuiz(
        Guid id,
        string title,
        string category,
        string description,
        string coverImageName,
        List<Question> questions)
    {
        await FileService.UpdateJsonFile(id,
            title,
            category,
            description,
            coverImageName,
            questions);

        var updatedQuizzes = await FileService.ReadJsonFile();

        return updatedQuizzes.FirstOrDefault(q => q.Id == id);
    }
}