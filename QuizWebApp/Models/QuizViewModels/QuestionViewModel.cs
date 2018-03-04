using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuizWebApp.Models.QuizViewModels
{
    public class QuestionViewModel
    {
        public int QuizId { get; set; }
        public float CorrectAnswers { get; set; }
        public int CurrentQuestion { get; set; }
        public string Title { get; set; }
        public IEnumerable<string> Answers { get; set; }
        public bool IsLastQuestion { get; set; }

        [Required(ErrorMessage ="Please chose tha answer!")]
        public IEnumerable<string> SubmitedAnswers { get; set; }
    }
}
