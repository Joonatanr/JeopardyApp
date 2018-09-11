using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeopardy
{
    class QuestionField
    {
        public String Name { get; set; }

        private Question[] questionArray = new Question[5];

        public QuestionField(String name)
        {
            this.Name = name;

            for (int x = 0; x < 5; x++)
            {
                questionArray[x] = new Question();
            }
        }
    }

    class Question
    {
        public String QuestionString { get; set; }
        public String AnswerString { get; set; }

        public Boolean isAnswered { get; set; } = false; 

        public Question()
        {

        }

        public Question(string q, string a)
        {
            this.QuestionString = q;
            this.AnswerString = a;
        }
    }
}
