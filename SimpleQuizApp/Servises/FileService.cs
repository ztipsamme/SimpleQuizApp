using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Avalonia.Controls.Shapes;
using SimpleQuizApp.Models;
using Path = Avalonia.Controls.Shapes.Path;

namespace SimpleQuizApp.Servises;

public static class FileService
{
    private static readonly string Path = "./quizzes.json";

    public static async Task WriteJsonFile(List<Quiz> quizzes)
    {
        string json = JsonSerializer.Serialize(quizzes,
            new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(Path, json);
    }


    public static async Task WriteJsonFile(Quiz quiz)
    {
        List<Quiz> quizzes = new();
        
        if (File.Exists(Path))
        {
            quizzes = await ReadJsonFile();
        }

        quizzes.Add(quiz);
        await WriteJsonFile(quizzes);
    }


    public static async Task<List<Quiz>> ReadJsonFile()
    {
        string json = await File.ReadAllTextAsync(Path);
        var quizzes = JsonSerializer.Deserialize<List<Quiz>>(json) ??
                      new List<Quiz>();
        return quizzes;
    }
}