using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizWebApp.Models.QuizViewModels
{
    public class QuizViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool HasQuestions { get; set; }
    }
}
