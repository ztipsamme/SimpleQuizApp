using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using SimpleQuizApp.Models;

namespace SimpleQuizApp.Services;

public static class FileService
{
    private static string _appData =
        Environment.GetFolderPath(Environment.SpecialFolder
            .LocalApplicationData);

    private static readonly string DefaultQuizzesPath =
        Path.Combine(AppContext.BaseDirectory, "Data", "defaultQuizzes.json");

    private static readonly string DefaultImagesPath =
        Path.Combine(AppContext.BaseDirectory, "Assets", "DefaultQuizImages");

    private static string _jsonFolder =
        Path.Combine(_appData, "SimpleQuizApp/Json");

    private static string _quizzesFile =
        Path.Combine(_jsonFolder, "Quizzes.json");

    private static string _imagesFolder =
        Path.Combine(_appData, "SimpleQuizApp/Images");


    public static async Task InitialiseQuizzes()
    {
        bool hasData = File.Exists(_quizzesFile);
        if (hasData) return;

        Directory.CreateDirectory(_jsonFolder);
        Directory.CreateDirectory(_imagesFolder);

        var quizzes = await ReadJsonFile(DefaultQuizzesPath);
        await WriteJsonFile(quizzes);

        foreach (var quiz in quizzes)
        {
            if (quiz.ImageName != null)
            {
                var src = Path.Combine(DefaultImagesPath, quiz.ImageName);
                SaveImage(quiz.ImageName, src);
            }
        }
    }

    public static async Task WriteJsonFile(List<Quiz> quizzes)
    {
        string json = JsonSerializer.Serialize(quizzes,
            new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(_quizzesFile, json);
    }


    public static async Task WriteJsonFile(Quiz quiz)
    {
        List<Quiz> quizzes = new();

        if (File.Exists(_quizzesFile))
        {
            quizzes = await ReadJsonFile();
        }

        quizzes.Add(quiz);
        await WriteJsonFile(quizzes);
    }


    public static async Task<List<Quiz>> ReadJsonFile(string? file = null)
    {
        file = file ?? _quizzesFile;

        if (!File.Exists(file))
            return new List<Quiz>();

        string json = await File.ReadAllTextAsync(file);
        var quizzes = JsonSerializer.Deserialize<List<Quiz>>(json) ??
                      new List<Quiz>();
        return quizzes;
    }

    public static async Task<bool> QuizExists(Guid id)
    {
        var quizzes = await ReadJsonFile();

        return quizzes.Where(x => x.Id == id).Any();
    }

    public static async Task<Quiz?> GetQuiz(Guid id)
    {
        List<Quiz> quizzes = await ReadJsonFile();
        Quiz? quiz = quizzes.FirstOrDefault(x => x.Id == id);

        return quiz;
    }

    public static async Task UpdateJsonFile(Guid id, string title,
        string category,
        string description,
        string coverImageFileName,
        List<Question> questions)
    {
        var quizzes = await ReadJsonFile();
        Quiz? existingQuiz = quizzes.Find(x => x.Id == id);

        if (existingQuiz == null)
        {
            Console.WriteLine("No Quiz found");
        }
        else
        {
            existingQuiz.Title = title;
            existingQuiz.Category = category;
            existingQuiz.Description = description;
            existingQuiz.ImageName = coverImageFileName;
            existingQuiz.Questions = questions;

            await WriteJsonFile(quizzes);
        }
    }

    public static async Task DeleteFromJsonFile(Guid id)
    {
        var quizzes = await ReadJsonFile();
        Quiz? existingQuiz = quizzes.Find(x => x.Id == id);

        if (existingQuiz == null)
        {
            Console.WriteLine("No Quiz found");
        }
        else
        {
            quizzes.Remove(existingQuiz);
            await WriteJsonFile(quizzes);
        }
    }

    public static void SaveImage(string fileName, string tempPath)
    {
        try
        {
            string destPath = Path.Combine(_imagesFolder, fileName);
            File.Copy(tempPath, destPath, overwrite: true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Image save failed: {ex.Message}");
        }
    }

    public static async Task<Bitmap?> GetImageAsync(
        string? imageName)
    {
        string path = Path.Combine(_imagesFolder, imageName ?? string.Empty);

        if (string.IsNullOrEmpty(imageName) || !File.Exists(path))
            return null;

        try
        {
            var bitmap = await Task.Run(() => new Bitmap(path));
            return bitmap;
        }
        catch
        {
            return null;
        }
    }
}