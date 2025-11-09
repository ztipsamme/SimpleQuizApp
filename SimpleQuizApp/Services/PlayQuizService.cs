using System;
using System.Collections.Generic;
using System.Linq;
using SimpleQuizApp.Models;

namespace SimpleQuizApp.Services;

public class PlayQuizService
{
    private Random _rnd = new();
    private readonly List<Question> _questions;
    private int _currentQuestionIdx = 0;
    private int _correctAnswers = 0;

    public Quiz Quiz { get; }
    public Question CurrentQuestion => _questions[_currentQuestionIdx];
    public int CurrentIndex => _currentQuestionIdx;
    public int CorrectAnswers => _correctAnswers;
    public bool HasMoreQuestions => _currentQuestionIdx < _questions.Count - 1;

    public PlayQuizService(Quiz quiz)
    {
        Quiz = quiz;
        // Shuffle questions
        _questions = quiz.Questions.OrderBy(_ => _rnd.Next()).ToList();
    }

    public List<string> GetShuffledOptions()
    {
        var options = CurrentQuestion.Options.ToList();
        if (!options.Contains(CurrentQuestion.CorrectOption))
            options.Add(CurrentQuestion.CorrectOption);
        return options.OrderBy(_ => _rnd.Next()).ToList();
    }

    public void SubmitAnswer(string selectedOption)
    {
        if (CurrentQuestion.IsRightAnswer(selectedOption))
            _correctAnswers++;
    }

    public void MoveToNextQuestion()
    {
        if (HasMoreQuestions)
            _currentQuestionIdx++;
    }
}
