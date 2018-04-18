using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TreeTest
{
    public class QuestionList
    {
        public List<QuestionTypes> Questions { get; set; }

        public QuestionList()
        { Questions = new List<QuestionTypes>(); }
    }
}
