using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jeopardy
{
    public partial class UserControlQuestionField : UserControl
    {

        public delegate void QuestionPressedHandler(Question q);
        public QuestionPressedHandler QuestionHandler;

        private QuestionField myField = null;
        private bool isBlocked = false;

        private Font usedFont;

        public UserControlQuestionField()
        {
            InitializeComponent();
            usedFont = labelCategory.Font;
        }

        public UserControlQuestionField(QuestionField field) : this () 
        {
            myField = field;
            labelCategory.Text = myField.Name;
        }

        public void setBlock(bool mode)
        {
            isBlocked = mode;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (isBlocked)
            {
                MessageBox.Show("Allocate points first.");
            }
            else
            {
                myField.questionArray[0].Score = 100;
                QuestionHandler?.Invoke(myField.questionArray[0]);
                this.groupBox1.Controls.Remove(button1);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (isBlocked)
            {
                MessageBox.Show("Allocate points first.");
            }
            else
            {
                myField.questionArray[1].Score = 200;
                QuestionHandler?.Invoke(myField.questionArray[1]);
                this.groupBox1.Controls.Remove(button2);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (isBlocked)
            {
                MessageBox.Show("Allocate points first.");
            }
            else
            {
                myField.questionArray[2].Score = 300;
                QuestionHandler?.Invoke(myField.questionArray[2]);
                this.groupBox1.Controls.Remove(button3);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (isBlocked)
            {
                MessageBox.Show("Allocate points first.");
            }
            else
            {
                myField.questionArray[3].Score = 400;
                QuestionHandler?.Invoke(myField.questionArray[3]);
                this.groupBox1.Controls.Remove(button4);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (isBlocked)
            {
                MessageBox.Show("Allocate points first.");
            }
            else
            {
                myField.questionArray[4].Score = 500;
                QuestionHandler?.Invoke(myField.questionArray[4]);
                this.groupBox1.Controls.Remove(button5);
            }
        }

        private void labelCategory_TextChanged(object sender, EventArgs e)
        {
            RecalculateTextFont();
        }

        private void groupBox1_SizeChanged(object sender, EventArgs e)
        {
            RecalculateTextFont();
        }

        private void RecalculateTextFont()
        {
            String currentStr = labelCategory.Text;
            Size strSize = TextRenderer.MeasureText(currentStr, usedFont);
            float currentFontSize = usedFont.SizeInPoints;
            Font recalculatedFont = usedFont;
            

            while (strSize.Width >= labelCategory.Width)
            {
                currentFontSize -= 0.25f;
                recalculatedFont = new Font(usedFont.FontFamily, currentFontSize);
                strSize = TextRenderer.MeasureText(currentStr, recalculatedFont);
            }

            labelCategory.Font = recalculatedFont;

        }


    }
}
