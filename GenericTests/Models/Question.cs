using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericTests
{
    public class Question
    {
        public Question()
        { Answers = new List<Answer>(); }

        public int QuestionTypeId { get; set; }

        public int QuestionId { get; set; }

        public string QuestionDescription { get; set; }

        public List<Answer> Answers { get; set; }
    }
}
