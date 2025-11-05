using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using SimpleQuizApp.Models;
using System.IO;
using System.Linq;
using Avalonia.Media.Imaging;
using Avalonia.Threading;

namespace SimpleQuizApp.Servises;

public static class FileService
{
    private static string _appData =
        Environment.GetFolderPath(Environment.SpecialFolder
            .LocalApplicationData);


    private static string _JsonFolder =
        Path.Combine(_appData, "SimpleQuizApp/Json");

    private static string _QuizzesFile =
        Path.Combine(_JsonFolder, "Quizzes.json");

    private static string _imagesFolder =
        Path.Combine(_appData, "SimpleQuizApp/Assets/QuizImages");

    public static async Task WriteJsonFile(List<Quiz> quizzes)
    {
        if (!Directory.Exists(_JsonFolder))
        {
            Directory.CreateDirectory(_JsonFolder);
        }

        string json = JsonSerializer.Serialize(quizzes,
            new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(_QuizzesFile, json);
    }


    public static async Task WriteJsonFile(Quiz quiz)
    {
        List<Quiz> quizzes = new();

        if (File.Exists(_QuizzesFile))
        {
            quizzes = await ReadJsonFile();
        }

        quizzes.Add(quiz);
        await WriteJsonFile(quizzes);
    }


    public static async Task<List<Quiz>> ReadJsonFile()
    {
        if (!File.Exists(_QuizzesFile))
            return new List<Quiz>();

        string json = await File.ReadAllTextAsync(_QuizzesFile);
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
            existingQuiz.CoverImageName = coverImageFileName;
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
            Directory.CreateDirectory(_imagesFolder);
            string destPath = Path.Combine(_imagesFolder, fileName);
            File.Copy(tempPath, destPath, overwrite: true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Image save failed: {ex.Message}");
        }
    }

    public static async Task<(Bitmap? src, bool hasImage)> GetImageAsync(
        string? imgName)
    {
        string path = Path.Combine(_imagesFolder, imgName ?? string.Empty);

        if (string.IsNullOrEmpty(imgName) || !File.Exists(path))
            return (null, false);

        try
        {
            var bitmap = await Task.Run(() => new Bitmap(path));
            return (bitmap, true);
        }
        catch
        {
            return (null, false);
        }
    }
}