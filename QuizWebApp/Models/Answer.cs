using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizWebApp.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public bool Correct { get; set; }
        public string Text { get; set; }
    }
}
