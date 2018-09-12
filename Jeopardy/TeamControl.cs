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
    public partial class TeamControl : UserControl
    {
        private int score = 0;

        public delegate int ScoreIncreaseHandler();

        public ScoreIncreaseHandler ScoreHandler;


        private String _teamName = null;
        public String TeamName
        {
            get { return _teamName; }
            set { _teamName = value; labelTeamName.Text = _teamName; }
        }

        public TeamControl()
        {
            InitializeComponent();

            /*
            if(TeamName != null)
            {
                labelTeamName.Text = TeamName;
            }
            */
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ScoreHandler != null)
            {
                int pointIncrease = ScoreHandler();
                score += pointIncrease;
                numericUpDown1.Value = score;
            }
        }
    }
}
