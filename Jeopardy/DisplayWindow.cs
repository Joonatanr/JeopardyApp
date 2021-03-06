﻿using System;
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
    public partial class DisplayWindow : UserControl
    {
        private enum WindowState 
        {
            STATE_CLOSED,
            STATE_QUESTION,
            STATE_ANSWER
        } ;

        private WindowState state = WindowState.STATE_CLOSED;
        private Question question;

        public DisplayWindow()
        {
            InitializeComponent();
            this.BackColor = Color.Transparent;

            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }

        public void setActive(Question q)
        {
            this.question = q;
            state = WindowState.STATE_QUESTION;
            this.BringToFront();
            this.BackColor = Color.Blue;
            MessageLabel.ForeColor = Color.White;
            MessageLabel.Text = q.QuestionString;

            if (q is QuestionPicture)
            {
                QuestionPicture qp = q as QuestionPicture;

                pictureBox1.Left = (this.ClientSize.Width - pictureBox1.Width) / 2;
                pictureBox1.Image = qp.getImage();
                pictureBox1.Show();
            }
            else
            {
                pictureBox1.Hide();
            }

            MessageLabel.Left = (this.ClientSize.Width - MessageLabel.Width) / 2;
            MessageLabel.Top = (this.ClientSize.Height - MessageLabel.Height) / 2;

            panel1.Invalidate();
        }


        private void panel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(state == WindowState.STATE_QUESTION)
            {
                state = WindowState.STATE_ANSWER;
                MessageLabel.Text = question.AnswerString;
                panel1.Invalidate();
            }
            else if(state == WindowState.STATE_ANSWER)
            {
                state = WindowState.STATE_CLOSED;
                this.BackColor = Color.Transparent;
                MessageLabel.Text = "";
                pictureBox1.Hide();
                SendToBack();
                panel1.Invalidate();
            }
        }

        private void MessageLabel_TextChanged(object sender, EventArgs e)
        {
            int textWidth = TextRenderer.MeasureText(MessageLabel.Text, MessageLabel.Font).Width;
            /* Recenter the control */
            MessageLabel.Left = (this.Width / 2) - (textWidth / 2);
        }
    }
}
