using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pj1
{
    public partial class Form3 : Form
    {
        Form2 f2;
        public Form3(Form2 f)
        {
            InitializeComponent();
            f2 = f;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
