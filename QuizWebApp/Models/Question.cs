using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuizWebApp.Models
{
    public class Question
    {

        public int Id { get; set; }
        public int QuizId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public IEnumerable<Answer> Answers { get; set; }
    }
}
