using QuizWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizWebApp.Data
{
    public interface IRepository
    {
        IEnumerable<Quiz> GetQuizesList();
        Quiz GetQuiz(int id);
        void SaveQuiz(Quiz newQuiz);
        void AddQuestion(int quizId, Question newQuestion);
    }
}
