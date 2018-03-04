using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace QuizWebApp.Data
{
    public class SqlRepository : IRepository
    {
        private readonly ApplicationDbContext _context;

        public SqlRepository(ApplicationDbContext ctx)
        {
            _context = ctx;
        }

        public void AddQuestion(int quizId, Question newQuestion)
        {
            var quiz = GetQuiz(quizId);
            if (quiz.Questions == null)
            {
                quiz.Questions = new List<Question>();               
            }
            _context.Questions.Add(newQuestion);
            foreach (var answer in newQuestion.Answers)
            {
                _context.Answers.Add(answer);
                newQuestion.Answers.Append(answer);
            }
            quiz.Questions.Append(newQuestion);
            _context.SaveChanges();
        }

        public Quiz GetQuiz(int id)
        {
            return _context.Quizes
                .Include(q => q.Questions)
                .ThenInclude(qw => qw.Answers)
                .SingleOrDefault(row => row.Id == id);
        }

        public IEnumerable<Quiz> GetQuizesList()
        {
            return _context.Quizes.Include(q => q.Questions);
        }

        public void SaveQuiz(Quiz newQuiz)
        {
            _context.Quizes.Add(newQuiz);
            _context.SaveChanges();
        }
    }
}
