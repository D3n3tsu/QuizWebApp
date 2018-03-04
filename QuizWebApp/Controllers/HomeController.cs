using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizWebApp.Data;
using QuizWebApp.Extensions;
using QuizWebApp.Models;
using QuizWebApp.Models.QuizViewModels;

namespace QuizWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepository _repo;

        public HomeController(IRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            var quizes = _repo.GetQuizesList();
            return View(quizes);
        }

        public IActionResult Question(int quizId)
        {
            var quiz = _repo.GetQuiz(quizId);
            if(quiz.Questions == null || quiz.Questions.Count() == 0)
            {
                return RedirectToAction(nameof(Error));
            }
            var firstQuestion = quiz.Questions.First();
            var answers = 
                firstQuestion.Answers.Select(a => a.Text).ToArray();
            answers.Shuffle();
            var vm = new QuestionViewModel
            {
                Title = firstQuestion.Title,
                IsLastQuestion = quiz.Questions.Count() == 1,
                Answers = answers,
                QuizId = quizId,
                CorrectAnswers = 0,
                CurrentQuestion = 0
            };
            
            return View(vm);
        }

        [HttpPost]
        public IActionResult Question(QuestionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var lQuiz = _repo.GetQuiz(model.QuizId);
                if (lQuiz.Questions == null || lQuiz.Questions.Count() == 0)
                {
                    return RedirectToAction(nameof(Error));
                }
                var question = lQuiz.Questions.ElementAt(model.CurrentQuestion);
                var lAnswers = question.Answers.Select(a => a.Text).ToArray();
                lAnswers.Shuffle();
                var lVm = new QuestionViewModel
                {
                    Title = question.Title,
                    IsLastQuestion = lQuiz.Questions.Count() == 1,
                    Answers = lAnswers,
                    QuizId = model.QuizId,
                    CorrectAnswers = model.CorrectAnswers,
                    CurrentQuestion = model.CurrentQuestion
                };
                return View(lVm);
            }
            var quiz = _repo.GetQuiz(model.QuizId);
            if (quiz.Questions == null || quiz.Questions.Count() == 0)
            {
                return RedirectToAction(nameof(Error));
            }
            var submittedQuestion = quiz.Questions.ElementAt(model.CurrentQuestion);
            model.CorrectAnswers += RateAnswer(model.SubmitedAnswers, submittedQuestion.Answers);
            
            if (quiz.Questions.Count() <= model.CurrentQuestion + 1)
            {
                return RedirectToAction(nameof(QuizResult), 
                    new { score = $"{model.CorrectAnswers:F2} / {quiz.Questions.Count()}" });
            }
            var nextQuestion = quiz.Questions.ElementAt(model.CurrentQuestion + 1);
            var answers = nextQuestion.Answers.Select(a => a.Text).ToArray();
            answers.Shuffle();
            var vm = new QuestionViewModel
            {
                Title = nextQuestion.Title,
                IsLastQuestion = quiz.Questions.Count() == model.CurrentQuestion + 2,
                Answers = answers,
                CorrectAnswers = model.CorrectAnswers,
                CurrentQuestion = model.CurrentQuestion + 1,
                QuizId = model.QuizId
            };

            return View(vm);
        }

        public IActionResult Quiz(int id)
        {
            var quiz = _repo.GetQuiz(id);
            if(quiz == null)
            {
                return RedirectToAction(nameof(Error));
            }
            var questionList = new List<QuestionViewModel>();
            QuizViewModel model = new QuizViewModel
            {
                Id = quiz.Id,
                Title = quiz.Title,
                HasQuestions = quiz.Questions == null ? false : quiz.Questions.Count() > 0
            };
            return View(model);
        }
        
        public IActionResult QuizResult(string score)
        {
            return View((object)score);
        }

        [Authorize]
        public IActionResult CreateQuiz()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateQuiz(CreateQuizViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            List<Question> questions = new List<Question>();
            Quiz newQuiz = new Quiz
            {
                Title = model.Title,
                Description = model.Description
            };
            _repo.SaveQuiz(newQuiz);

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public IActionResult AddQuestion(int quizId)
        {
            var quiz = _repo.GetQuiz(quizId);
            if(quiz == null)
            {
                return RedirectToAction(nameof(Error));
            }
            string quizName = quiz.Title;
            var newModel = new CreateQuestionViewModel
            {
                QuizId = quizId,
                QuizName = quizName
            };
            return View(newModel);
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddQuestion(CreateQuestionViewModel question)
        {
            if (!ModelState.IsValid)
            {
                return View(question);
            }
            var answers = new List<Answer>
            {
                new Answer{ Text = question.Answer1, Correct = question.CorrectAnswer1},
                new Answer{ Text = question.Answer2, Correct = question.CorrectAnswer2},
                new Answer{ Text = question.Answer3, Correct = question.CorrectAnswer3},
                new Answer{ Text = question.Answer4, Correct = question.CorrectAnswer4}
            };
            var newQuestion = new Question
            {
                Title = question.Title,
                QuizId = question.QuizId,
                Answers = answers
            };
            _repo.AddQuestion(question.QuizId, newQuestion);

            return RedirectToAction(nameof(AddQuestion), new { quizId = question.QuizId });
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private float RateAnswer(IEnumerable<string> submittedAnswers, IEnumerable<Answer> realAnswers)
        {
            float correctAnswersSubmitted = 0;
            foreach (var sa in submittedAnswers)
            {
                correctAnswersSubmitted += realAnswers.Single(ra => ra.Text == sa).Correct ? 1 : 0;
            };
            float correctAnswersOverall = realAnswers.Where(a => a.Correct).ToArray().Count();
            float correctness = correctAnswersSubmitted / correctAnswersOverall;
            float chance = correctAnswersSubmitted / submittedAnswers.Count();
            return correctness * chance;
        }
    }
}
