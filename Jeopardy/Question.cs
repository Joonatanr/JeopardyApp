using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeopardy
{
    public class QuestionField
    {
        public String Name { get; set; }

        public Question[] questionArray = new Question[5];

        public QuestionField(String name)
        {
            this.Name = name;

            for (int x = 0; x < 5; x++)
            {
                questionArray[x] = new Question();
            }
        }
    }

    public class Question
    {
        public String QuestionString    { get; set; }
        public String AnswerString      { get; set; }

        public Boolean isAnswered { get; set; } = false;

        public int Score { get; set; } = 0;

        public Question()
        {
            QuestionString = "blank";
            AnswerString = "blank";
        }

        public Question(string q, string a)
        {
            this.QuestionString = q;
            this.AnswerString = a;
        }
    }

    class QuestionPicture : Question
    {
        private Image img;

        public QuestionPicture(string q, string a, Image img) : base(q, a)
        {
            this.img = img;
        }

        public Image getImage()
        {
            return img;
        }
    }
}
