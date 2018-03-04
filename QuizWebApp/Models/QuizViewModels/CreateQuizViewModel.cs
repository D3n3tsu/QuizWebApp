using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuizWebApp.Models.QuizViewModels
{
    public class CreateQuizViewModel
    {
        [Required]
        public string Title { get; set; }

        public IEnumerable<CreateQuestionViewModel> Questions { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
