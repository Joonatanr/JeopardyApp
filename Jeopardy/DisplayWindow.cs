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
    public partial class DisplayWindow : UserControl
    {
        private enum WindowState 
        {
            STATE_CLOSED,
            STATE_QUESTION,
            STATE_ANSWER
        } ;

        private WindowState state = WindowState.STATE_CLOSED;

        public DisplayWindow()
        {
            InitializeComponent();
            this.BackColor = Color.Transparent;
        }

        public void setActive(bool mode)
        {
            if (mode)
            {
                state = WindowState.STATE_QUESTION;
            }
            panel1.Invalidate();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            string msg = null;

            if(state == WindowState.STATE_CLOSED)
            {
                return;
            }

            Graphics gfx = panel1.CreateGraphics();
            gfx.FillRectangle(new SolidBrush(Color.Blue), new Rectangle(new Point(0, 0), this.Size));

            if (state == WindowState.STATE_QUESTION)
            {
                msg = "Question";
            }
            else if(state == WindowState.STATE_ANSWER)
            {
                msg = "Answer";
            }

            if (msg != null)
            {
                Font font = new Font("Tahoma", 14);

                Size textSize = TextRenderer.MeasureText(msg, font);
                gfx.DrawString(msg, font, Brushes.White, new Rectangle(new Point(0, 0), textSize));
            }
        }

        private void panel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(state == WindowState.STATE_QUESTION)
            {
                state = WindowState.STATE_ANSWER;
                panel1.Invalidate();
            }
            else if(state == WindowState.STATE_ANSWER)
            {
                state = WindowState.STATE_CLOSED;
                panel1.Invalidate();
            }
        }
    }
}
