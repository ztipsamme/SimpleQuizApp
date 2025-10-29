using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using SimpleQuizApp.Models;
using System.IO;

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

    public static void SaveImage(string fileName, string tempPath)
    {
        Directory.CreateDirectory(_imagesFolder);

        string destPath = Path.Combine(_imagesFolder, fileName);
        File.Copy(tempPath, destPath, overwrite: true);
    }

    public static string? GetImageSrc(string fileName)
    {
        string fullImagePath = Path.Combine(_imagesFolder, fileName);

        return fullImagePath;
    }
}