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
        private QuestionField myField = null;

        public UserControlQuestionField()
        {
            InitializeComponent();
        }

        public UserControlQuestionField(QuestionField field) : this () 
        {
            myField = field;
            labelCategory.Text = myField.Name;
        }
    }
}
