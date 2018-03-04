using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuizWebApp.Models.QuizViewModels
{
    public class CreateQuestionViewModel
    {
        [Required]
        public int QuizId { get; set; }
        
        public string QuizName { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Answer 1")]
        public string Answer1 { get; set; }
        public bool CorrectAnswer1 { get; set; }

        [Required]
        [Display(Name = "Answer 2")]
        public string Answer2 { get; set; }
        public bool CorrectAnswer2 { get; set; }

        [Required]
        [Display(Name = "Answer 3")]
        public string Answer3 { get; set; }
        public bool CorrectAnswer3 { get; set; }

        [Required]
        [Display(Name = "Answer 4")]
        public string Answer4{ get; set; }
        public bool CorrectAnswer4 { get; set; }
    }
}
