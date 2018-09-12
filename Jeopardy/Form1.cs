using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jeopardy
{
    public partial class Form1 : Form
    {
        private DisplayWindow displayWindow1;

        public Form1()
        {
            InitializeComponent();

            displayWindow1 = new DisplayWindow();
            displayWindow1.Location = new Point(100, 20);
            displayWindow1.Size = new Size(200, 200);
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            teamControl1.ScoreHandler = new TeamControl.ScoreIncreaseHandler(scoreHandler);
            teamControl2.ScoreHandler = new TeamControl.ScoreIncreaseHandler(scoreHandler);
            teamControl3.ScoreHandler = new TeamControl.ScoreIncreaseHandler(scoreHandler);
        }

        private void ShowQuestion(Question q)
        {
            displayWindow1.Location = new Point(0, 0);
            displayWindow1.Size = this.Size;
            textBoxCurrentScore.Text = q.Score.ToString();
            displayWindow1.setActive(q);

            //TODO : Should block answering until points have been given to a team.
            foreach(UserControl field in groupBox1.Controls)
            {
                if (field is UserControlQuestionField)
                {
                    UserControlQuestionField qField = field as UserControlQuestionField;
                    qField.setBlock(true);
                }
            }
        }


        private int scoreHandler()
        {
            int res = 0;
            string str = textBoxCurrentScore.Text;

            if (!int.TryParse(str, out res))
            {
                res = 0;
            }

            /* This will need invoke... */
            textBoxCurrentScore.Text = "0";

            /* Unblock the question buttons... */
            foreach (UserControl field in groupBox1.Controls)
            {
                if (field is UserControlQuestionField)
                {
                    UserControlQuestionField qField = field as UserControlQuestionField;
                    qField.setBlock(false);
                }
            }

            return res;
        }


        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Parser myParser = new Parser();
                List<QuestionField> parsedQuestions;

                bool res = myParser.LoadQuestionsFromFile(openFileDialog1.FileName, out parsedQuestions);

                foreach (QuestionField q in parsedQuestions)
                {
                    Console.WriteLine("Field : " + q.Name);
                }

                if (!res)
                {
                    MessageBox.Show("Failed to open file");
                    return;
                }

                /* Lets populate the game area... */
                groupBox1.Controls.Clear();

                int offset = 0;
                int yPos = 40;

                foreach (QuestionField field in parsedQuestions)
                {
                    UserControlQuestionField item = new UserControlQuestionField(field);
                    Point location = new Point(offset, yPos);
                    item.Location = location;
                    item.QuestionHandler = new UserControlQuestionField.QuestionPressedHandler(ShowQuestion);
                    groupBox1.Controls.Add(item);

                    offset += item.Size.Width + 1;

                }

                //This needs to be done at the end...
                groupBox1.Controls.Add(displayWindow1);
                groupBox1.Invalidate();
            }
        }
    }
}
